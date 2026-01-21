namespace VentasApp.Application.DTOs.Productos;

public class CrearProductoDto
{
    public string Nombre{get;set;} = null!;
    public int IdCategoria{get;set;}
    public decimal Costo{get;set;}
    public decimal PrecioVenta{get;set;}
}