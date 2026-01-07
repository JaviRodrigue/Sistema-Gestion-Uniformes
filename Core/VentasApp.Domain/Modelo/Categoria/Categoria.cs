using VentasApp.Domain.Base;

namespace VentasApp.Domain.Modelo.Categoria;

public class Categoria : Entidad
{
    public string Nombre {get; private set;}

    protected Categoria(){}
    public Categoria(string nombre)
    {
        CambiarNombre(nombre);
    }

    public void CambiarNombre(string nombre)
    {
        if (string.IsNullOrWhiteSpace(nombre))
        {
            throw new ExcepcionDominio("El nombre debe ser obligatorio");
        }
        Nombre = nombre.Trim();
        Nombre = Nombre.ToLower();
    }


}