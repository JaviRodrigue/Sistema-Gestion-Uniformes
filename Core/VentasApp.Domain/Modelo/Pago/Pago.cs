using VentasApp.Domain.Base;

namespace VentasApp.Domain.Modelo.Pago;

public class Pago : Entidad
{
    public decimal Monto { get; private set; }
    public DateTime FechaPago { get; private set; }
    public bool EsSenia { get; private set; }
    public int IdVenta { get; private set; }
   // public Venta? Venta { get; private set; }
    
    protected Pago(){}
    public Pago(decimal monto, bool senia, int idVenta)
    {
        this.Monto = monto;
        this.EsSenia = senia;
        this.IdVenta = idVenta;
        this.FechaPago = DateTime.Now;
    }
}
