using VentasApp.Application.DTOs.Venta;
using VentasApp.Application.Interfaces.Repositorios;
using System.Linq;

namespace VentasApp.Application.CasoDeUso.DetalleVenta;

public class GuardarDetalleVentaUseCase
{
    private readonly IVentaRepository _ventaRepo;
    private readonly IUnitOfWork _unitOfWork;

    public GuardarDetalleVentaUseCase(IVentaRepository ventaRepo, IUnitOfWork unitOfWork)
    {
        _ventaRepo = ventaRepo;
        _unitOfWork = unitOfWork;
    }

    public async Task EjecutarAsync(int idVenta, VentasApp.Application.DTOs.Venta.VentaDetalleDto dto)
    {
        var venta = await _ventaRepo.ObtenerPorId(idVenta) ?? throw new Exception("Venta no encontrada");

        // Actualizar código de venta si cambió
        if (!string.IsNullOrWhiteSpace(dto.Codigo) && dto.Codigo != venta.CodigoVenta)
        {
            if (await _ventaRepo.ExisteCodigoVenta(dto.Codigo))
            {
                throw new Exception($"El código de venta '{dto.Codigo}' ya existe");
            }
            venta.EstablecerCodigoVenta(dto.Codigo);
        }

        var existentes = venta.Detalles.Select(d => d.Id).ToList();

        // Procesar items enviados desde UI
        foreach (var item in dto.Items)
        {
            if (item.IdDetalle == 0)
            {
                // Agregar directamente al agregado Venta usando el IdItemVendible proporcionado por la UI
                venta.AgregarDetalle(item.IdItemVendible, item.Cantidad, item.PrecioUnitario);
            }
            else
            {
                var idDet = item.IdDetalle;
                var original = venta.Detalles.FirstOrDefault(d => d.Id == idDet);
                if (original is null) continue;

                if (original.Cantidad != item.Cantidad || original.PrecioUnitario != item.PrecioUnitario)
                {
                    venta.ModificarDetalle(idDet, item.Cantidad, item.PrecioUnitario);
                }

                existentes.Remove(idDet);
            }
        }

        // Eliminar los que ya no estan en la lista
        foreach (var idToRemove in existentes)
        {
            venta.EliminarDetalle(idToRemove);
        }

        // Notificar al repositorio que la venta fue actualizada para que EF la rastree
        await _ventaRepo.Actualizar(venta);

        // Procesar pagos: eliminar pagos que no se encuentren en dto (se manejan en usecases externos)

        await _unitOfWork.SaveChanges();
    }
}
