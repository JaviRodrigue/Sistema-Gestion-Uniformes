namespace VentasApp.Application.DTOs.DetalleVenta;

public class DetalleVentaDto
{
    public int Id{get;set;}
    public int IdItemVendible{get;set;}
    public int Cantidad{get;set;}
    public decimal PrecioUnitario{get;set;}
    public decimal SubTotal{get;set;}

    
}