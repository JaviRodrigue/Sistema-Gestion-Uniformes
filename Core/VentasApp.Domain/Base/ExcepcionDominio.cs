namespace VentasApp.Domain.Base;

//EJEMPLO DE USO
//public void CambiarPrecio(decimal precio)
//{
//    if (precio <= 0)
//        throw new ExcepcionDominio("El precio debe ser mayor a cero");
//    Precio = precio;
//}


//Exception: Es la base de todas las excepsiones de .NET
public class ExcepcionDominio : Exception
{

    //El constructor queda vacio, ya que se utiliza de la Clase padre "Exception"
    public ExcepcionDominio(string mensaje) : base(mensaje)
    {
        
    }

}