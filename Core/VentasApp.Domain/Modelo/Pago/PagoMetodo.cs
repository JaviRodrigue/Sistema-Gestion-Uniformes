using VentasApp.Domain.Base;

namespace VentasApp.Domain.Modelo.Pago;

public class PagoMetodo : Entidad
{
    public int IdPago { get; private set; }
    public Pago? Pago { get; private set; }
    public int IdMedioPago { get; private set; }
    public MedioPago MedioPago { get; private set; } = null!;
    public decimal Monto { get; private set; }

    protected PagoMetodo(){}
    public PagoMetodo(int idMedioPago, decimal monto)
    {
        if(monto <= 0)
        {
            throw new ExcepcionDominio("El monto debe ser mayor a 0");
        }
        
        this.IdMedioPago = idMedioPago;
        this.Monto = monto;
    }

    
}
