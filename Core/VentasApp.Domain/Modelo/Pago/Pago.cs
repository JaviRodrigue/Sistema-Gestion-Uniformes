namespace VentasApp.Domain.Modelo.Pago;
using VentasApp.Domain.Base;
using VentasApp.Domain.Modelo.Venta;


public class Pago : Entidad
{
    public List<PagoMetodo> _metodos = new();

    public decimal Total => _metodos.Sum(m => m.Monto);
    public DateTime FechaPago { get; private set; }
    public int IdVenta { get; private set; }
    public Venta? Venta { get; private set; }
    
    public IReadOnlyCollection<PagoMetodo> Metodos => _metodos.AsReadOnly();
    protected Pago(){}
    public Pago(int idVenta)
    {
        this.IdVenta = idVenta;
        this.FechaPago = DateTime.Now;
    }

    public void AgregarPago(MedioPago medioPago, decimal monto)
    {
        if (!medioPago.Activo)
        {
            throw new ExcepcionDominio("El medio de pago no esta activo");
        }
        if(monto <= 0)
        {
            throw new ExcepcionDominio("El monto debe ser mayor a cero");
        }
        _metodos.Add(new PagoMetodo(medioPago.Id, monto));
    }

    public void ValidarPago()
    {
        if (!_metodos.Any())
        {
            throw new ExcepcionDominio("El pago debe tener por lo menos un metodo");
        }
        
    }


}
