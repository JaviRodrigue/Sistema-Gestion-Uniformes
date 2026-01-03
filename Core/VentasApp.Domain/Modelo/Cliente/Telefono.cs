using VentasApp.Domain.Base;

namespace VentasApp.Domain.Modelo.Cliente;

public class Telefono : Entidad
{
    public int Numero { get; private set; }
    public int IdCliente { get; private set; }
    public Cliente Cliente { get; private set; }

    protected Telefono(){}
    public Telefono(int idCliente, int numero, Cliente cliente)
    {
        this.Numero = numero;
        this.IdCliente = idCliente;
        this.IdCliente = cliente.Id;
        this.Cliente = cliente;
    }
}
