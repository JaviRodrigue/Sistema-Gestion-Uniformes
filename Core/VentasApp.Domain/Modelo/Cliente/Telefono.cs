using VentasApp.Domain.Base;

namespace VentasApp.Domain.Modelo.Cliente;

public class Telefono : Entidad
{
    public string Numero { get; private set; }
    public int IdCliente { get; private set; }
    public Cliente? Cliente { get; private set; }
    public bool Activado { get; private set; }

    protected Telefono() { }
    // Se mantiene ctor anterior por compatibilidad si fuese necesario
    public Telefono(int idCliente, string numero, Cliente cliente)
    {
        this.Numero = numero;
        this.IdCliente = idCliente;
        this.Cliente = cliente;
        Activado = true;
    }

    // Ctor práctico para crear teléfonos dentro del agregado Cliente
    public Telefono(Cliente cliente, string numero)
    {
        this.Numero = numero;
        this.Cliente = cliente;
        this.IdCliente = cliente.Id;
        Activado = true;
    }

    public void Desactivar()
    {
        Activado = false;
    }

    public void Activar()
    {
        Activado = true;
    }
}
