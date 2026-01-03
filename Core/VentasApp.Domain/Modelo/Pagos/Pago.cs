namespace VentasApp.Domain;

public class Pago
{
    public int IdPago { get; private set; }
    public double Monto { get; private set; }
    public DateTime FechaPago { get; private set; }
    public bool EsSenia { get; private set; }
    public int IdVenta { get; private set; }
    public Venta? Venta { get; private set; }

    public Pago()
    {
    }
}
