using VentasApp.Domain.Base;

namespace VentasApp.Domain.Modelo.Categoria;

public class Categoria : Entidad
{
    //Tuve que poner =null! porque me tiraba advertencia de posibles referencias a null
    public string Nombre {get; private set;} = null!;

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