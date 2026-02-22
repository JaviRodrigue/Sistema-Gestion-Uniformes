using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Windows;
using VentasApp.Desktop.Services;

namespace VentasApp.Desktop.ViewModels.Config;

public partial class ConfigViewModel : ObservableObject
{
    private readonly ThemeService _themeService;
    private readonly AppSettingsService _settings;

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

    public ConfigViewModel(ThemeService themeService, AppSettingsService settings)
    {
        _themeService = themeService;
        _settings = settings;
        _modoOscuroActivo = settings.ModoOscuro;
        _stockMinimoLibreria = settings.StockMinimoLibreria.ToString();
        _stockMinimoUniforme = settings.StockMinimoUniforme.ToString();
    }

    [RelayCommand]
    private void GuardarConfig()
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

        _settings.StockMinimoLibreria = libMin;
        _settings.StockMinimoUniforme = uniMin;
        _settings.Guardar();

        MessageBox.Show("Configuración guardada correctamente.", "Listo",
            MessageBoxButton.OK, MessageBoxImage.Information);
    }
}
