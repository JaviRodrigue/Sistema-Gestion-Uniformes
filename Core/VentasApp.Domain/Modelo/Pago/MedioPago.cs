using VentasApp.Domain.Base;

namespace VentasApp.Domain.Modelo.Pago;

public class MedioPago : Entidad
{
    public string Nombre { get; set; }
    public bool TieneRecargo { get; set; }

    //protected MedioPago(){}
    public MedioPago(string nombre, bool recargo)
    {
        this.Nombre = nombre;
        this.TieneRecargo = recargo;
    }
}
