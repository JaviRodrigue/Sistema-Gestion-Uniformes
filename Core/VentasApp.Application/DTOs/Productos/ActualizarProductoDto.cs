namespace VentasApp.Application.DTOs.Productos;

public class ActualizarProductoDto
{
    public int IdCategoria{get;set;}
    public string Nombre{get;set;} = null!;
    public decimal Costo{get;set;}
    public decimal PrecioVenta{get;set;}
}