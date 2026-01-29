using System.Collections.ObjectModel;
namespace VentasApp.Desktop.ViewModels.DTOs;

public class ProductoCardDto
{
    public int Id { get; set; }
    public string Nombre { get; set; } = string.Empty;
    public string Categoria { get; set; } = string.Empty;
    public decimal Precio { get; set; }
    public int StockTotal { get; set; }
    public string CodigoBarraReferencia { get; set; } = string.Empty;
}