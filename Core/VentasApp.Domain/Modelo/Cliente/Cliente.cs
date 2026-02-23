using VentasApp.Domain.Base;

namespace VentasApp.Domain.Modelo.Cliente;

public class Cliente : Entidad
{
    public string? DNI { get; private set; }
    public string? Nombre { get; private set; }
    public DateTime FechaAlta { get; private set; }
    public List<Telefono> Telefonos { get; private set; }
    public bool Activado { get; private set; }

    protected Cliente()
    {
        Telefonos = [];
        Activado = true;
    }

    public Cliente(string nombre, string dni)
    {
        this.Nombre = nombre;
        this.DNI = dni;
        this.FechaAlta = DateTime.Now;
        this.Telefonos = [];
        Activado = true;
    }

    public Cliente(string nombre, string dni, List<Telefono> telefonos)
    {
        this.Nombre = nombre;
        this.DNI = dni;
        this.FechaAlta = DateTime.Now;
        this.Telefonos = telefonos ?? [];
        Activado = true;
    }

    // Mutadores / comportamientos del agregado
    public void CambiarNombre(string nombre)
    {
        if (string.IsNullOrWhiteSpace(nombre)) throw new ArgumentException("Nombre inválido", nameof(nombre));
        Nombre = nombre;
    }

    public void CambiarDni(string dni)
    {
        if (string.IsNullOrWhiteSpace(dni)) throw new ArgumentException("DNI inválido", nameof(dni));
        DNI = dni;
    }

    public void AgregarTelefono(string numero)
    {
        if (string.IsNullOrWhiteSpace(numero)) throw new ArgumentException("Número inválido", nameof(numero));
        Telefonos.Add(new Telefono(this, numero));
    }

    public void ReemplazarTelefonos(IEnumerable<string> numeros)
    {
        Telefonos.Clear();
        if (numeros == null) return;
        foreach (var n in numeros)
        {
            if (!string.IsNullOrWhiteSpace(n))
                Telefonos.Add(new Telefono(this, n));
        }
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