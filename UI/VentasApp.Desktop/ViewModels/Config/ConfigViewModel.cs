using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Windows;
using VentasApp.Desktop.Services;
using Microsoft.Extensions.DependencyInjection;
using VentasApp.Infrastructure.Persistencia.Contexto;
using System.Linq;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.Messaging;
using VentasApp.Desktop.Messages;

namespace VentasApp.Desktop.ViewModels.Config;

public partial class ConfigViewModel : ObservableObject
{
    private readonly ThemeService _themeService;
    private readonly AppSettingsService _settings;
    private readonly System.IServiceProvider _serviceProvider;

    private bool _modoOscuroActivo;
    public bool ModoOscuroActivo
    {
        get => _modoOscuroActivo;
        set
        {
            if (SetProperty(ref _modoOscuroActivo, value))
            {
                _themeService.AplicarTema(value);
                _settings.ModoOscuro = value;
            }
        }
    }

    [ObservableProperty] private string _stockMinimoLibreria = string.Empty;
    [ObservableProperty] private string _stockMinimoUniforme = string.Empty;

    public ConfigViewModel(ThemeService themeService, AppSettingsService settings, System.IServiceProvider serviceProvider)
    {
        _themeService = themeService;
        _settings = settings;
        _serviceProvider = serviceProvider;
        _modoOscuroActivo = settings.ModoOscuro;
        _stockMinimoLibreria = settings.StockMinimoLibreria.ToString();
        _stockMinimoUniforme = settings.StockMinimoUniforme.ToString();
    }

    [RelayCommand]
    private async Task GuardarConfig()
    {
        if (!int.TryParse(_stockMinimoLibreria, out var libMin) || libMin < 0)
        {
            MessageBox.Show("Stock mínimo de Librería inválido.", "Validación",
                MessageBoxButton.OK, MessageBoxImage.Warning);
            return;
        }
        if (!int.TryParse(_stockMinimoUniforme, out var uniMin) || uniMin < 0)
        {
            MessageBox.Show("Stock mínimo de Uniformes inválido.", "Validación",
                MessageBoxButton.OK, MessageBoxImage.Warning);
            return;
        }

        bool changed = _settings.StockMinimoLibreria != libMin || _settings.StockMinimoUniforme != uniMin;

        _settings.StockMinimoLibreria = libMin;
        _settings.StockMinimoUniforme = uniMin;
        _settings.Guardar();

        if (changed)
        {
            var result = MessageBox.Show("¿Desea aplicar estos nuevos valores de stock mínimo a todos los productos existentes?", 
                "Actualizar productos", MessageBoxButton.YesNo, MessageBoxImage.Question);
                
            if (result == MessageBoxResult.Yes)
            {
                await ActualizarStockMinimoGlobal(libMin, uniMin);
            }
        }

        MessageBox.Show("Configuración guardada correctamente.", "Listo",
            MessageBoxButton.OK, MessageBoxImage.Information);
    }

    private async Task ActualizarStockMinimoGlobal(int libMin, int uniMin)
    {
        try
        {
            using var scope = _serviceProvider.CreateScope();
            var db = scope.ServiceProvider.GetRequiredService<DatabaseContext>();
            
            var stocks = db.Stock.ToList();
            var items = db.ItemVendible.ToList();
            var productos = db.Producto.ToList();
            
            int actualizados = 0;
            
            foreach (var stock in stocks)
            {
                var item = items.FirstOrDefault(i => i.Id == stock.IdItemVendible);
                if (item != null)
                {
                    var producto = productos.FirstOrDefault(p => p.Id == item.IdProducto);
                    if (producto != null)
                    {
                        // 1 = Uniforme, 2 = Libreria
                        int nuevoMinimo = producto.IdCategoria == 1 ? uniMin : libMin;
                        if (stock.StockMinimo != nuevoMinimo)
                        {
                            stock.CambiarStockMinimo(nuevoMinimo);
                            actualizados++;
                        }
                    }
                }
            }
            
            if (actualizados > 0)
            {
                await db.SaveChangesAsync();
                WeakReferenceMessenger.Default.Send(new StockChangedMessage());
                MessageBox.Show($"Se actualizaron {actualizados} productos con el nuevo stock mínimo.", "Actualización completada", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }
        catch (System.Exception ex)
        {
            MessageBox.Show($"Error al actualizar productos: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }
}
