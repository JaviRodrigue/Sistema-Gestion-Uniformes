namespace VentasApp.Domain.Modelo.Producto;
using VentasApp.Domain.Base;


public class ItemVendible : Entidad
{
    public int IdProducto {get; private set;}
    public Producto Producto {get;}
    public string? Nombre {get ; private set ;}
    public string? CodigoBarra{get ; private set;}
    public string? Talle {get; private set;}

    protected ItemVendible(){}

    public ItemVendible(int idProducto,string nombre, string codigo, string? talle)
    {
        this.IdProducto = idProducto;
        CambiarNombre(nombre);
        CambiarCodigoBarras(codigo);
        this.Talle = talle;
    }

    public void CambiarNombre(string nombre)
    {
        if (string.IsNullOrWhiteSpace(nombre))
        {
            throw new ExcepcionDominio("El nombre debe ser obligatorio");
        }
        this.Nombre = nombre.Trim();
        this.Nombre = Nombre.ToLower();
    }

    public void CambiarCodigoBarras(string cod)
    {
        if (string.IsNullOrWhiteSpace(cod))
        {
            throw new ExcepcionDominio("El codigo de barras de ser obligatorio");
        }
        this.CodigoBarra = cod;
    }



}