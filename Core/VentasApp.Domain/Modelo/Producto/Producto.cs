namespace VentasApp.Domain.Modelo.Producto;
using VentasApp.Domain.Base;
using VentasApp.Domain.Modelo.Categoria;
public class Producto : Entidad
{
    public int IdCategoria {get; private set;}
    public string Nombre {get; private set;}
    public decimal Costo {get; private set;}
    public decimal PrecioVenta {get; private set;}
    public decimal Ganancia => PrecioVenta - Costo;
    public Categoria Categoria {get;}


    public Producto(int id_categoria,string nombre, decimal costo, decimal precioVenta)
    {
        this.IdCategoria = id_categoria;
        this.Nombre = nombre;
        this.Costo = costo;
        this.PrecioVenta = precioVenta;
    }
}