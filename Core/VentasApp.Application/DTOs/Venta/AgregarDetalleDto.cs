namespace VentasApp.Application.DTOs.Venta;

public class AgregarDetalleDto
{
    public int IdItemVendible {get;set;}
    public int Cantidad {get; set;}
    public decimal PrecioUnitario {get; set;}
}