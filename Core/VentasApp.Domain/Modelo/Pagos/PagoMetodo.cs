namespace VentasApp.Domain;

public class PagoMetodo
{
    public int IdPagoMetodo { get; private set; }
    public int IdPago { get; private set; }
    public Pago? Pago { get; private set; }
    public int IdMedioPago { get; private set; }
    public MedioPago? MedioPago { get; private set; }
    public double Monto { get; private set; }

    public PagoMetodo()
    {
    }
}
