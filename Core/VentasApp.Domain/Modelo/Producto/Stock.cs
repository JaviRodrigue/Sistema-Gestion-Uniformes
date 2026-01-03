using VentasApp.Domain.Base;

namespace VentasApp.Domain.Modelo.Producto;

public class Stock : Entidad
{
    public int Cantidad {get; private set;}

    protected Stock(){}

    public Stock(int cantidad)
    {
        this.Cantidad = cantidad;
    }

    public void Descontar(int cantidad)
    {
        if(cantidad > this.Cantidad)
        {
            throw new ExcepcionDominio("Stock insuficiente");
        }
        this.Cantidad -= cantidad;
    }

    public void Aumentar(int cantidad)
    {
        this.Cantidad += cantidad;
    }
}