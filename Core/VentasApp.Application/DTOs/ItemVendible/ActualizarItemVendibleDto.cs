namespace VentasApp.Application.DTOs.ItemVendible;

public class ActualizarItemVendibleDto
{
    public string Nombre{get;set;} = null!;
    public string CodigoBarra{get;set;} = null!;
    public string? Talle{get;set;}
}