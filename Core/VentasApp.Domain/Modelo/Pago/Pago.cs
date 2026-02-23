namespace VentasApp.Domain.Modelo.Pago;
using VentasApp.Domain.Base;
using VentasApp.Domain.Modelo.Venta;



public class Pago : Entidad
{
    private List<PagoMetodo> _metodos = new();

    public decimal Total { get; private set; }
    public DateTime FechaPago { get; private set; }
    public int IdVenta { get; private set; }
    public Venta? Venta { get; private set; }

    public bool EsSenia{get;private set;}
    public bool Verificado{get;private set;}
    
    public IReadOnlyCollection<PagoMetodo> Metodos => _metodos.AsReadOnly();
    protected Pago(){}
    public Pago(int idVenta, bool esSenia)
    {
        this.IdVenta = idVenta;
        this.FechaPago = DateTime.Now;
        this.EsSenia = esSenia;
        this.Total = 0;
        this.Verificado = false;
    }   

    public void AgregarPago(int idMedioPago, decimal monto)
    {
        if(monto <= 0)
        {
            throw new ExcepcionDominio("El monto debe ser mayor a cero");
        }
        _metodos.Add(new PagoMetodo(idMedioPago, monto));
        Total += monto;
    }

    public void ValidarPago()
    {
        if (_metodos.Count == 0)
        {
            throw new ExcepcionDominio("El pago debe tener por lo menos un metodo");
        }
        if(Total <= 0)
        {
            throw new ExcepcionDominio("El total del pago debe ser mayor a cero");
        }
        
    }

    public void MarcarComoVerificado()
    {
        this.Verificado = true;
    }

    public void DesmarcarVerificacion()
    {
        this.Verificado = false;
    }

    public void ModificarFecha(DateTime nuevaFecha)
    {
        if (nuevaFecha > DateTime.Now)
        {
            throw new ExcepcionDominio("La fecha del pago no puede ser futura");
        }

        this.FechaPago = nuevaFecha;
    }

}
