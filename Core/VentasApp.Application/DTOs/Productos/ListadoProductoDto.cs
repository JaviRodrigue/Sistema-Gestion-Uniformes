namespace VentasApp.Application.DTOs.Productos;

public class ListadoProductoDto
{
    public int Id{get;set;}
    public string Nombre{get;set;} = null!;
    public string? CodigoBarra{get;set;}
    public decimal PrecioVenta{get;set;}
    public int StockDisponible { get; set; }
}