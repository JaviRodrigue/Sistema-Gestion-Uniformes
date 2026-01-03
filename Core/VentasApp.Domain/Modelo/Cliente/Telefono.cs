using VentasApp.Domain.Base;

namespace VentasApp.Domain.Modelo.Cliente;

public class Telefono : Entidad
{
    public int Numero { get; private set; }
    public int IdCliente { get; private set; }

    protected Telefono(){}
    public Telefono(int idCliente, int numero)
    {
        this.Numero = numero;
        this.IdCliente = idCliente;
    }
}
