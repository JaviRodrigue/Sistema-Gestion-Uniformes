namespace VentasApp.Domain;

public class Venta
{
    public int IdVenta { get; private set; }
    public DateTime FechaVenta { get; private set; }
    public bool TipoVenta { get; private set; }
    public double MontoTotal { get; private set; }
    public double MontoPagado { get; private set; }
    public double SaldoPendiente { get; private set; }
    public int IdEstado { get; private set; }
    public Estado? Estado { get; private set; }

    private Venta() { }
}
