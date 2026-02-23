using System.Collections.ObjectModel;
namespace VentasApp.Desktop.ViewModels.DTOs;

public class ProductoCardDto
{
    public int Id { get; set; }
    public string Nombre { get; set; } = string.Empty;
    public string Categoria { get; set; } = string.Empty;
    public decimal Precio { get; set; }
    public int StockTotal { get; set; }
    public int StockMinimo { get; set; }
    public bool BajoStock => StockMinimo > 0 && StockTotal <= StockMinimo;
    public string CodigoBarraReferencia { get; set; } = string.Empty;
    public string? Talle { get; set; }
}