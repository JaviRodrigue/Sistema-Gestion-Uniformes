using VentasApp.Domain.Base;
using VentasApp.Domain.Enum;
namespace VentasApp.Domain.Modelo.Venta;

public class Venta : Entidad
{
    private readonly List<DetalleVenta> _detalles = new();
    public DateTime FechaVenta { get; private set; }
    public TipoVenta TipoVenta { get; private set; }
    public decimal MontoTotal { get; private set; }
    public decimal MontoPagado { get; private set; }
    public decimal SaldoPendiente => MontoTotal - MontoPagado;
    public EstadoVenta Estado { get; private set; }

    public IReadOnlyCollection<DetalleVenta> Detalles => _detalles.AsReadOnly();

    protected Venta() { }

    public Venta(TipoVenta tipoVenta)
    {
        this.TipoVenta = tipoVenta;
        this.MontoPagado = 0;
        this.MontoTotal = 0;
        this.Estado = EstadoVenta.Pendiente;
        this.FechaVenta = DateTime.Now;
    }

    public void AgregarDetalle(int itemVendible, int cantidad, decimal precioUnitario)
    {
        var detalle = new DetalleVenta(itemVendible,cantidad,precioUnitario);
        _detalles.Add(detalle);
        RecalcularTotal();
    }
    public void Confirmar()
    {
        if(!_detalles.Any())
        {
            throw new ExcepcionDominio("La venta debe tener al menos un item");
        }
        this.Estado = EstadoVenta.Confirmada;
    }

    public void RegistrarPago(decimal monto)
    {
        if(Estado == EstadoVenta.Cancelada)
        {
            throw new ExcepcionDominio("No se puede pagar una venta cancelada");
        }
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

    public void AnularVenta()
    {
        if(this.Estado == EstadoVenta.Pagada)
        {
            throw new ExcepcionDominio("No se puede anular una venta pagada");
        }

        this.Estado = EstadoVenta.Cancelada;
    }

    private void RecalcularTotal()
    {
        this.MontoTotal = _detalles.Sum(d => d.SubTotal);
    }
}
