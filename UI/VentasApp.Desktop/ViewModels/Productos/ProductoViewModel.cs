using CommunityToolkit.Mvvm.ComponentModel;
using System.Collections.ObjectModel;
using VentasApp.Desktop.ViewModels.DTOs;

namespace VentasApp.Desktop.ViewModels.Productos
{
    public partial class ProductoViewModel : ObservableObject
    {
        // Esta es la lista que la Vista va a "dibujar"
        [ObservableProperty]
        private ObservableCollection<ProductoCardDto> _productos;

        public ProductoViewModel()
        {
            // Cargar datos FALSOS para probar el diseño
            Productos = new ObservableCollection<ProductoCardDto>
            {
                new ProductoCardDto
                {
                    Id = 1,
                    Nombre = "Chomba Escolar Talle 12",
                    Categoria = "Uniforme", // Esto activará el color Lavanda y el icono de remera
                    Precio = 15000,
                    StockTotal = 50,
                    CodigoBarraReferencia = "779123456789"
                },
                new ProductoCardDto
                {
                    Id = 2,
                    Nombre = "Cuaderno Tapa Dura A4",
                    Categoria = "Librería", // Esto activará el color Azul y el icono de libro
                    Precio = 4500,
                    StockTotal = 120,
                    CodigoBarraReferencia = "LIB-001-A4"
                },
                new ProductoCardDto
                {
                    Id = 3,
                    Nombre = "Pantalón Gimnasia Talle 10",
                    Categoria = "Uniforme",
                    Precio = 18000,
                    StockTotal = 0, // Esto debería poner el badge en Rojo
                    CodigoBarraReferencia = "779987654321"
                }
            };
        }
    }
}