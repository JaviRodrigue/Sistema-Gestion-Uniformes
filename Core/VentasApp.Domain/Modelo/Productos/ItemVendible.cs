namespace VentasApp.Domain.Modelo.Productos;
using VentasApp.Domain.Base;


public class ItemVendible : Entidad
{
    public int IdProducto {get; private set;}
    public Producto Producto {get;} = null!;
    public string Nombre {get ; private set;} = null!;
    public string CodigoBarra{get ; private set;} = null!;
    public string? Talle {get; private set;}
    public bool Activado{get; private set;}
    public Stock? Stock { get; private set; }

    protected ItemVendible(){}

    public ItemVendible(int idProducto,string nombre, string codigo, string? talle)
    {
        this.IdProducto = idProducto;
        CambiarNombre(nombre);
        CambiarCodigoBarras(codigo);
        this.Talle = talle;
        Activado = true;
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

    public void CambiarTalle(string? talle)
    {
        Talle = talle;
    }

    public void Desactivar()
    {
        Activado = false;
    }
}