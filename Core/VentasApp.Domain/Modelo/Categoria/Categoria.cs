using VentasApp.Domain.Base;

namespace VentasApp.Domain.Modelo.Categoria;

public class Categoria : Entidad
{
    public string Nombre {get; private set;}

    public Categoria(string nombre)
    {
        this.Nombre = nombre;
    }
}