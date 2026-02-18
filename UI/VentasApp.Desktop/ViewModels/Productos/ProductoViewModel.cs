using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using VentasApp.Desktop.ViewModels.DTOs;
using System.Linq;

namespace VentasApp.Desktop.ViewModels.Productos
{
    public partial class ProductoViewModel : ObservableObject
    {
        private readonly VentasApp.Application.Interfaces.Repositorios.IProductoRepository _productoRepository;
        private readonly VentasApp.Application.CasoDeUso.Productos.CrearProductoUseCase _crearProductoUseCase;

        [ObservableProperty]
        private ObservableCollection<ProductoCardDto> _productos;

        public ProductoViewModel(
            VentasApp.Application.Interfaces.Repositorios.IProductoRepository productoRepository,
            VentasApp.Application.CasoDeUso.Productos.CrearProductoUseCase crearProductoUseCase)
        {
            _productoRepository = productoRepository;
            _crearProductoUseCase = crearProductoUseCase;
            Productos = new ObservableCollection<ProductoCardDto>();
            CargarProductos();
        }

        [RelayCommand]
        private async void AgregarProducto()
        {
            var win = new Views.Productos.AgregarProductoWindow
            {
                Owner = System.Windows.Application.Current.MainWindow
            };
            var ok = win.ShowDialog();
            if (ok == true)
            {
                var nombre = (win.FindName("TxtNombre") as System.Windows.Controls.TextBox)?.Text ?? string.Empty;
                var categoriaItem = (win.FindName("CmbCategoria") as System.Windows.Controls.ComboBox)?.SelectedItem as System.Windows.Controls.ComboBoxItem;
                var costoText = (win.FindName("TxtCosto") as System.Windows.Controls.TextBox)?.Text ?? "0";
                var precioVentaText = (win.FindName("TxtPrecioVenta") as System.Windows.Controls.TextBox)?.Text ?? "0";

                decimal.TryParse(costoText, out var costo);
                decimal.TryParse(precioVentaText, out var precioVenta);

                // IdCategoria: determinar según selección simple (Uniforme=1, Librería=2) como mínimo viable
                var idCategoria = categoriaItem?.Content?.ToString() switch
                {
                    "Uniforme" => 1,
                    "Librería" => 2,
                    _ => 1
                };

                var dto = new VentasApp.Application.DTOs.Productos.CrearProductoDto
                {
                    IdCategoria = idCategoria,
                    Nombre = nombre,
                    Costo = costo,
                    PrecioVenta = precioVenta
                };

                await _crearProductoUseCase.EjecutarAsync(dto);

                // Recargar desde base de datos
                CargarProductos();
            }
        }

        [RelayCommand]
        private void EditarProducto(ProductoCardDto? producto)
        {
            if (producto is null) return;
            producto.Nombre = producto.Nombre + " (editado)";
            OnPropertyChanged(nameof(Productos));
        }

        [RelayCommand]
        private void EliminarProducto(ProductoCardDto? producto)
        {
            if (producto is null) return;
            if (Productos.Contains(producto))
            {
                Productos.Remove(producto);
            }
        }

        private async void CargarProductos()
        {
            var lista = await _productoRepository.ListarProductos();
            var mapped = lista.Select(p => new ProductoCardDto
            {
                Id = p.Id,
                Nombre = p.Nombre ?? string.Empty,
                Categoria = string.Empty, // no existe Categoria en Producto dominio
                Precio = p.PrecioVenta,
                StockTotal = 0, // no existe StockTotal en Producto dominio
                CodigoBarraReferencia = p.ItemsVendibles?.FirstOrDefault()?.CodigoBarra ?? string.Empty
            });

            Productos = new ObservableCollection<ProductoCardDto>(mapped);
        }
    }
}