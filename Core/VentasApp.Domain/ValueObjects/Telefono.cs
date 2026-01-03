namespace VentasApp.Domain;

public class Telefono
{
    public int IdTelefono { get; private set; }
    public int Numero { get; private set; }
    public int IdCliente { get; private set; }
    public Cliente? Cliente { get; private set; }

    public Telefono()
    {
    }
}
