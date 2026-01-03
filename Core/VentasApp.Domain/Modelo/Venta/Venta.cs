using VentasApp.Domain.Base;
using VentasApp.Domain.Enum;
namespace VentasApp.Domain.Modelo.Venta;

public class Venta : Entidad
{
    public DateTime FechaVenta { get; private set; }
    public bool TipoVenta { get; private set; }
    public decimal MontoTotal { get; private set; }
    public decimal MontoPagado { get; private set; }
    public decimal SaldoPendiente => MontoTotal - MontoPagado;
    public int IdEstado { get; private set; }
    public EstadoVenta Estado { get; private set; }

    protected Venta() { }

    public Venta(bool tipoVenta, decimal montoTotal, decimal montoPagado, int idEstado)
    {
        this.TipoVenta = tipoVenta;
        this.MontoPagado = montoPagado;
        this.MontoTotal = montoTotal;
        this.IdEstado = idEstado;
        this.Estado = EstadoVenta.SinPagar;
    }

    public void Confirmar()
    {
        if(this.MontoTotal <= 0)
        {
            throw new ExcepcionDominio("La venta no tiene detalle");
        }
        this.Estado = EstadoVenta.Confirmada;
    }

    public void RegistrarPago(decimal monto)
    {
        if(monto <= 0)
        {
            throw new ExcepcionDominio("El monto debe ser mayor a 0");
        }
        
        this.MontoPagado += monto;
        if(this.MontoPagado >= this.MontoTotal)
        {
            this.Estado = EstadoVenta.Pagada;
        }
    }
}
