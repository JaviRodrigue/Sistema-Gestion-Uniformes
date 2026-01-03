namespace VentasApp.Domain;

public class Compra
{
    public int IdVenta { get; private set; }
    public Venta? Venta { get; private set; }
    public int IdCliente { get; private set; }
    public Cliente? Cliente { get; private set; }

    public Compra()
    {
    }
}
