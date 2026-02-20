using VentasApp.Application.DTOs.Venta;
using VentasApp.Application.Interfaces.Repositorios;

namespace VentasApp.Application.CasoDeUso.Venta;

public class GuardarVentaCompletaUseCase
{
    private readonly IVentaRepository _ventaRepo;
    private readonly IPagoRepository _pagoRepo;
    private readonly IMedioPagoRepository _medioRepo;
    private readonly IUnitOfWork _unitOfWork;

    public GuardarVentaCompletaUseCase(IVentaRepository ventaRepo, IPagoRepository pagoRepo, IMedioPagoRepository medioRepo, IUnitOfWork unit)
    {
        _ventaRepo = ventaRepo;
        _pagoRepo = pagoRepo;
        _medioRepo = medioRepo;
        _unitOfWork = unit;
    }

    public async Task EjecutarAsync(VentaDetalleDto dto)
    {
        var venta = await _ventaRepo.ObtenerPorId(dto.Id) ?? throw new Exception("Venta no encontrada");

        // Manejo detalles
        var existentes = venta.Detalles.Select(d => d.Id).ToList();

        foreach (var item in dto.Items)
        {
            if (item.IdDetalle == 0)
            {
                venta.AgregarDetalle(item.IdItemVendible, item.Cantidad, item.PrecioUnitario);
            }
            else
            {
                var original = venta.Detalles.FirstOrDefault(d => d.Id == item.IdDetalle);
                if (original is not null)
                {
                    if (original.Cantidad != item.Cantidad || original.PrecioUnitario != item.PrecioUnitario)
                    {
                        venta.ModificarDetalle(item.IdDetalle, item.Cantidad, item.PrecioUnitario);
                    }
                    existentes.Remove(item.IdDetalle);
                }
            }
        }

        foreach (var idToRemove in existentes)
        {
            venta.EliminarDetalle(idToRemove);
        }

        // Manejo pagos
        var pagosPersistidos = await _pagoRepo.ObtenerPorVenta(venta.Id);
        var idsPersistidos = pagosPersistidos.Select(p => p.Id).ToList();
        var idsEnDto = dto.Pagos.Select(p => p.Id).ToList();

        // Agregar nuevos pagos
        foreach (var pagoDto in dto.Pagos.Where(p => p.Id == 0))
        {
            var pago = new VentasApp.Domain.Modelo.Pago.Pago(venta.Id, false);
            foreach (var metodo in pagoDto.Metodos)
            {
                var medio = await _medioRepo.ObtenerPorId(metodo.IdMedioPago) ?? throw new Exception("Medio de pago invalido");
                pago.AgregarPago(medio.Id, metodo.Monto);
            }
            pago.ValidarPago();
            venta.RegistrarPago(pago.Total);
            await _pagoRepo.Agregar(pago);
        }

        // Eliminar pagos removidos
        var idsAEliminar = idsPersistidos.Except(idsEnDto).ToList();
        foreach (var idEl in idsAEliminar)
        {
            // eliminar pago
            await _pagoRepo.Eliminar(idEl);
        }

        // Recalcular monto pagado desde la tabla de pagos para evitar inconsistencias
        var pagosFinales = await _pagoRepo.ObtenerPorVenta(venta.Id);
        var totalPagos = pagosFinales.Sum(p => p.Total);
        venta.RecalcularMontosDesdePagos(totalPagos);

        // Actualizar venta y guardar todo en una sola transaccion
        await _ventaRepo.Actualizar(venta);
        await _unitOfWork.SaveChanges();
    }
}
