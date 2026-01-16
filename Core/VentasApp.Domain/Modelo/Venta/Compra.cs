using VentasApp.Domain.Base;

namespace VentasApp.Domain.Modelo.Venta;

public class Compra : Entidad
{
    public int IdVenta { get; private set; }
    //public Venta? Venta { get; private set; }
    public int IdCliente { get; private set; }
    //public Cliente? Cliente { get; private set; }

    protected Compra(){}
    public Compra(int idVenta, int idCliente)
    {
        this.IdCliente = idCliente;
        this.IdVenta = idVenta;
    }
}
