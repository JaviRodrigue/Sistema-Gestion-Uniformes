namespace VentasApp.Application.DTOs.ItemVendible;

public class ItemVendibleDto
{
    public int Id{get;set;}
    public int IdProducto{get;set;}
    public string Nombre{get;set;} = null!;
    public string CodigoBarra{get;set;} = null!;
    public string? Talle{get;set;}
    public bool Activo{get;set;}
}