namespace VentasApp.Application.DTOs.ItemVendible;

public class CrearItemVendibleDto
{
    public int IdProducto{get;set;}
    public string nombre{get;set;} = null!;
    public string CodigoBarra{get;set;} = null!;
    public string? Talle{get;set;}
}