using VentasApp.Domain.Base;

namespace VentasApp.Domain.Modelo.Pago;

public class MedioPago : Entidad
{
    public string Nombre { get; private set; } = null!;
    public bool TieneRecargo { get; private set; }
    public bool Activo{get;private set;}

    protected MedioPago(){}
    public MedioPago(string nombre, bool recargo)
    {
        CambiarNombre(nombre);
        this.TieneRecargo = recargo;
        this.Activo = true;
    }

    public void Desactivar()
    {
        this.Activo = false;
    }

    public void CambiarNombre(string nombre)
    {
        if (string.IsNullOrWhiteSpace(nombre))
        {
            throw new ExcepcionDominio("El nombre del Medio de Pago es obligatorio");
        }
        this.Nombre = nombre.Trim();
    }

}
