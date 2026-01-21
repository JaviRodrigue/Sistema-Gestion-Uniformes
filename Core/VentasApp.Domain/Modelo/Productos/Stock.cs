using VentasApp.Domain.Base;

namespace VentasApp.Domain.Modelo.Productos;

public class Stock : Entidad
{
    public int IdItemVendible{get; private set;}
    public ItemVendible ItemVendible{get; private set;} = null!;
    public int CantidadDisponible{get;private set;}
    public int CantidadReservada{get;private set;}
    public int StockMinimo{get;private set;}
    public bool Activo{get; private set;}

    protected Stock(){}

    public Stock(int IdItemVendible, int cantidadInicial, int stockMinimo = 0)
    {
        this.IdItemVendible = IdItemVendible;
        CambiarStockMinimo(stockMinimo);
        CambiarCantidadDisponible(cantidadInicial);
        this.CantidadReservada = 0;
        this.Activo = true;
    }

    public void CambiarStockMinimo(int stockMinimo)
    {
        if(stockMinimo <= 0)
        {
            throw new ExcepcionDominio("El stock minimo debe ser mayor a 0");
        }
        this.StockMinimo = stockMinimo;
    }
    public void CambiarCantidadDisponible(int cant)
    {
        if(cant < 0)
        {
            throw new ExcepcionDominio("La cantidad inicial no puede ser negativa");
        }
        this.CantidadDisponible = cant;
    }
    public void Reservar(int cant)
    {
        if(cant <= 0)
        {
            throw new ExcepcionDominio("La cantidad reservada debe ser mayor a 0");
        }
        if(cant > this.CantidadDisponible)
        {
            throw new ExcepcionDominio("No hay stock suficiente para realizar la reserva");
        }
        this.CantidadDisponible -= cant;
        this.CantidadReservada += cant;
        
    }

    public void Aumentar(int cantidad)
    {
        if(cantidad <= 0)
        {
            throw new ExcepcionDominio("La cantidad debe ser mayor a cero");
        }
        this.CantidadDisponible += cantidad;
    }

    public void Descontar(int cantidad)
    {
        if(cantidad <= 0)
        {
            throw new ExcepcionDominio("La cantidad debe ser mayor a cero");
        }
        if(cantidad > this.CantidadDisponible)
        {
            throw new ExcepcionDominio("Stock insuficiente");
        }
        this.CantidadDisponible -= cantidad;
    }

    public void ConfirmarReserva(int cantidad)
    {
        if(cantidad > this.CantidadReservada)
        {
            throw new ExcepcionDominio("Cantidad reserva invalida");
        }
        this.CantidadReservada -= cantidad;
    }

    public void LiberarReserva(int cant)
    {
        if(cant > this.CantidadReservada)
        {
            throw new ExcepcionDominio("Cantidad de reserva invalida");
        }
        this.CantidadReservada -= cant;
        this.CantidadDisponible += cant;
    }

    public bool BajoStock() => this.CantidadDisponible <= this.StockMinimo;

    public void Desactivar()
    {
        this.Activo = false;
    }


}