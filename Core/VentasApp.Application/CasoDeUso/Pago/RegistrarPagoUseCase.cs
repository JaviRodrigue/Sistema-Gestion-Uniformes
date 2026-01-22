namespace VentasApp.Application.CasoDeUso.Pago;

using VentasApp.Application.Interfaces.Repositorios;
using VentasApp.Application.DTOs.Pago;
using VentasApp.Domain.Enum;
using VentasApp.Domain.Modelo.Pago;

public class RegistrarPagoUseCase(IPagoRepository pago, IVentaRepository venta, IMedioPagoRepository medioPago, IUnitOfWork unit)
{
    private readonly IPagoRepository _repositoryPago=pago;
    private readonly IVentaRepository _repositoryVenta=venta;
    private IMedioPagoRepository _repositoryMedioPago=medioPago;
    private IUnitOfWork _unit=unit;


    public async Task EjecutarAsync(CrearPagoDto dto)
    {
        //Verifico si la venta existe
        var venta = await _repositoryVenta.ObtenerPorId(dto.IdVenta) ?? throw new Exception("No se encontro la venta");
        //Verifico el estado de la venta
        if(venta.Estado == EstadoVenta.Cancelada)
        {
            throw new Exception("No se puede agregar un pago a una venta cancelada");
        }

        //Obtengo todos los pagos previos de la venta
        //venta.Id == dto.IdVenta para este punto
        var pagosPrevios = await _repositoryPago.ObtenerPorVenta(venta.Id);

        //Totalizo el total de pagos efectuados a la venta
        var totalPagado = pagosPrevios.Sum(p => p.Total);

        //Llegado a este punto, creo el pago
        var pago = new Pago(dto.IdVenta, dto.EsSenia);

        foreach(var metodo in dto.Metodos)
        {
            var medio = await _repositoryMedioPago.ObtenerPorId(metodo.IdMedioPago) ?? throw new Exception("Medio de pago invalido");
            pago.AgregarPago(medio.Id, metodo.Monto);
        }

        //valido si el pago tiene la menos un metodo de pago
        pago.ValidarPago();

        if(totalPagado + pago.Total > venta.MontoTotal)
        {
            throw new Exception("El pago supera el monto total de la venta");
        }

        venta.RegistrarPago(pago.Total);
        await _repositoryPago.Agregar(pago);
        await _unit.SaveChanges();
    }
}