namespace VentasApp.Application.DTOs.Producto;

public class ListadoProductoDto
{
    public int Id{get;set;}
    public string Nombre{get;set;} = null!;
    public decimal PrecioVenta{get;set;}
}