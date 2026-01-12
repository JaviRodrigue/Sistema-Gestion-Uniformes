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
        if(Estado != EstadoVenta.Pendiente)
        {
            throw new ExcepcionDominio("Solo se puede agregar items a una venta pendiente");
        }
        var detalle = new DetalleVenta(itemVendible,cantidad,precioUnitario);
        _detalles.Add(detalle);
        RecalcularTotal();
    }
    public void Confirmar()
    {
        if(Estado != EstadoVenta.Pendiente)
        {
            throw new ExcepcionDominio("La venta no puede confirmarse en su estado actual");
        }
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
        if(this.MontoPagado + monto > this.MontoTotal)
        {
            throw new ExcepcionDominio("El monto excede al total de la venta");
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

    public void EliminarDetalle(int idDetalle)
    {
        if(Estado != EstadoVenta.Pendiente)
        {
            throw new ExcepcionDominio("No se puede eliminar un detalle de una venta confirmada");
        }

        var detalle = _detalles.FirstOrDefault(d => d.Id == idDetalle) ?? throw new ExcepcionDominio("Detalle no encontrado");
        _detalles.Remove(detalle);
        RecalcularTotal();
    }

    public void ModificarDetalle(int idDetalle, int cantidad, decimal precio)
    {
        if(Estado != EstadoVenta.Pendiente)
        {
            throw new ExcepcionDominio("No se puede modificar un detalle de una venta confirmada");
        }
        var detalle = _detalles.FirstOrDefault(d => d.Id == idDetalle) ?? throw new ExcepcionDominio("El detalle no existe");
        detalle.ModificarPrecio(precio);
        detalle.ModificarCantidad(cantidad);
        RecalcularTotal();
    }
}
