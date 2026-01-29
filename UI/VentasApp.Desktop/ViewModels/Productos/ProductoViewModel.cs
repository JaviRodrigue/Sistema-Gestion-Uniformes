using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input; // Necesario para los botones
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using VentasApp.Desktop.ViewModels.DTOs;
// using VentasApp.Application.UseCases.Productos; // Descomentar cuando tengas tus interfaces

namespace VentasApp.Desktop.ViewModels.Productos
{
    public partial class ProductoViewModel : ObservableObject
    {
        // Aquí inyectarías tu caso de uso real
        // private readonly IGetAllProductosUseCase _getAllProductosUseCase;

        [ObservableProperty]
        private ObservableCollection<ProductoCardDto> _productos;

        public ProductoViewModel(/* IGetAllProductosUseCase getAllProductosUseCase */)
        {
            // _getAllProductosUseCase = getAllProductosUseCase;
            Productos = new ObservableCollection<ProductoCardDto>();

            // Simulamos la llamada al caso de uso
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

        

        private async void CargarProductos()
        {
            // var listaReal = await _getAllProductosUseCase.ExecuteAsync();
            // Por ahora, usamos los datos Mock:

            await Task.Delay(500); // Simulamos tiempo de carga de DB

            Productos = new ObservableCollection<ProductoCardDto>
            {
                new ProductoCardDto { Id = 1, Nombre = "Chomba Escolar Talle 12", Categoria = "Uniforme", Precio = 15000, StockTotal = 50, CodigoBarraReferencia = "779123456789" },
                new ProductoCardDto { Id = 2, Nombre = "Cuaderno A4 Rayado", Categoria = "Librería", Precio = 4500, StockTotal = 120, CodigoBarraReferencia = "LIB-001" },
                new ProductoCardDto { Id = 3, Nombre = "Pantalón Jogging Talle 10", Categoria = "Uniforme", Precio = 18000, StockTotal = 0, CodigoBarraReferencia = "779987654321" }
            };
        }
    }
}