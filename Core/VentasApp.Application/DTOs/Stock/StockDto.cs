namespace VentasApp.Application.DTOs.Stock;

public class StockDto
{
    public int Id{get;set;}
    public int CantidadDisponible{get;set;}
    public int IdItemVendible{get;set;}
    public int CantidadReservada{get;set;}
    public int StockMinimo{get;set;}
    public bool Activo{get;set;}
}