using VentasApp.Domain.Base;
namespace VentasApp.Domain.Modelo.Venta;

public class DetalleVenta : Entidad
{
    public int IdItemVendible {get ; private set;}
    public int Cantidad {get ; private set;}
    public decimal PrecioUnitario {get; private set;}
    public decimal SubTotal => Cantidad * PrecioUnitario;

    protected DetalleVenta(){}
    public DetalleVenta(int idItem, int cantidad, decimal precioUnitario)
    {
        if(cantidad <= 0)
        {
            throw new ExcepcionDominio("La cantidad debe ser mayor a cero");
        }
        if(precioUnitario <= 0)
        {
            throw new ExcepcionDominio("El precio unitario debe ser mayor a cero");
        }
        this.IdItemVendible = idItem;
        this.Cantidad = cantidad;
        this.PrecioUnitario = precioUnitario;
    }

    internal void ModificarPrecio(decimal precio)
    {
        if(precio <= 0)
        {
            throw new ExcepcionDominio("El precio debe ser mayor a 0");
        }
        PrecioUnitario = precio;
    }

    internal void ModificarCantidad(int cantidad)
    {
        if(cantidad <= 0)
        {
            throw new ExcepcionDominio("La cantidad debe ser mayor a 0");
        }
        this.Cantidad = cantidad;
    }



}