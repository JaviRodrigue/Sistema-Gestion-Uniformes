using VentasApp.Domain.Base;

namespace VentasApp.Domain.Modelo.Cliente;

public class Cliente : Entidad
{
    public long? DNI { get; private set; }
    public string? Nombre { get; private set; }
    public DateTime FechaAlta { get; private set; }

    protected Cliente(){}

    public Cliente(string? nombre, long? dni)
    {
        this.Nombre = nombre;
        this.DNI = dni;
        this.FechaAlta = DateTime.Now;
    }
}
