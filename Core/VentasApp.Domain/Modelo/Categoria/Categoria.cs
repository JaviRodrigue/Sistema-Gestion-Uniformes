using VentasApp.Domain.Base;

namespace VentasApp.Domain.Modelo.Categoria;

public class Categoria : Entidad
{
    //Tuve que poner =null! porque me tiraba advertencia de posibles referencias a null
    public string Nombre {get; private set;} = null!;
    public bool Activa {get; private set;}

    protected Categoria(){}
    public Categoria(string nombre)
    {
        CambiarNombre(nombre);
        this.Activa = true;
    }

    public void CambiarNombre(string nombre)
    {
        if (string.IsNullOrWhiteSpace(nombre))
        {
            throw new ExcepcionDominio("El nombre debe ser obligatorio");
        }
        Nombre = nombre.Trim();
    }

    public void Desactivar()
    {
        this.Activa = false;
    }


}