namespace VentasApp.Domain.Modelo.Producto;
using VentasApp.Domain.Base;


public class ItemVendible : Entidad
{
    public int IdProducto {get; private set;}
    public Producto Producto {get;}

    public int IdStock {get; private set;}

    public string? Nombre {get ; private set ;}
    public string? CodigoBarra{get ; private set;}
    public string? Talle {get; private set;}

    protected ItemVendible(){}

    public ItemVendible(int idProducto,int idStock,string nombre, string codigo, string? talle)
    {
        this.IdProducto = idProducto;
        this.IdStock = idStock;
        this.CodigoBarra = codigo;
        this.Nombre = nombre;
        this.Talle = talle;
    }   

}