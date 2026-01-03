using VentasApp.Domain.Base;
namespace VentasApp.Domain.Modelo.Venta;

public class DetalleVenta : Entidad
{
    public int IdItemVendible {get ; private set;}
    public int IdVenta {get ; private set;}
    public int Cantidad {get ; private set;}
    public decimal PrecioUnitario {get; private set;}
    public decimal SubTotal => Cantidad * PrecioUnitario;

    protected DetalleVenta(){}
    public DetalleVenta(int idItem,int idVenta, int cantidad, decimal precioUnitario)
    {
        this.IdItemVendible = idItem;
        this.IdVenta = idVenta;
        this.Cantidad = cantidad;
        this.PrecioUnitario = precioUnitario;
    }

}