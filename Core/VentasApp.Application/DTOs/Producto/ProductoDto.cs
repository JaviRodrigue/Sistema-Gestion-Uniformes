namespace VentasApp.Application.DTOs.Producto;

public class ProductoDto
{
    public int Id{get;set;}
    public int IdCategoria{get;set;}
    public string Nombre{get;set;} = null!;
    public decimal Costo{get;set;}
    public decimal PrecioVenta{get;set;}
    public decimal Ganancia{get;set;}
    public bool Activa{get;set;}
}