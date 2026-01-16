namespace VentasApp.Domain.Enum;

//Manejaremos dos tipo de venta
//Una venta por Pedido y una venta por Presencial, el cual es opcional guardar la informacion del cliente
public enum TipoVenta
{
    Pedido = 1,
    Presencial = 2
}