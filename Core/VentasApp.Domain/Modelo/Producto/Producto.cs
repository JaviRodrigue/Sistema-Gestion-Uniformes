using VentasApp.Domain.Base;

namespace VentasApp.Domain.Modelo.Producto;

public class Producto : Entidad
{
    public int Id_categoria {get; private set;}
    public string Nombre {get; private set;}
    public decimal Costo {get; private set;}
    public decimal PrecioVenta {get; private set;}
    public decimal Ganancia => PrecioVenta - Costo;

    public Producto(int id_categoria,string nombre, decimal costo, decimal precioVenta)
    {
        this.Id_categoria = id_categoria;
        this.Nombre = nombre;
        this.Costo = costo;
        this.PrecioVenta = precioVenta;
    }
}