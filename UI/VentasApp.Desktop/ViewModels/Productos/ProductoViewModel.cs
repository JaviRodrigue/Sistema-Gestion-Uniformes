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

        [ObservableProperty]
        private ObservableCollection<ProductoCardDto> _productos;

        public ProductoViewModel(VentasApp.Application.Interfaces.Repositorios.IProductoRepository productoRepository)
        {
            _productoRepository = productoRepository;
            Productos = new ObservableCollection<ProductoCardDto>();
            CargarProductos();
        }

        [RelayCommand]
        private void AgregarProducto()
        {
            var win = new Views.Productos.AgregarProductoWindow
            {
                Owner = System.Windows.Application.Current.MainWindow
            };
            var ok = win.ShowDialog();
            if (ok == true)
            {
                var nombre = (win.FindName("TxtNombre") as System.Windows.Controls.TextBox)?.Text ?? string.Empty;
                var categoria = (win.FindName("CmbCategoria") as System.Windows.Controls.ComboBox)?.SelectedItem as System.Windows.Controls.ComboBoxItem;
                var precioText = (win.FindName("TxtPrecio") as System.Windows.Controls.TextBox)?.Text ?? "0";
                var stockText = (win.FindName("TxtStock") as System.Windows.Controls.TextBox)?.Text ?? "0";
                var codigoBarra = (win.FindName("TxtCodigoBarra") as System.Windows.Controls.TextBox)?.Text ?? string.Empty;

                decimal.TryParse(precioText, out var precio);
                int.TryParse(stockText, out var stock);

                Productos.Add(new ProductoCardDto
                {
                    Id = Productos.Count + 1,
                    Nombre = nombre,
                    Categoria = categoria?.Content?.ToString() ?? "Uniforme",
                    Precio = precio,
                    StockTotal = stock,
                    CodigoBarraReferencia = codigoBarra
                });
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