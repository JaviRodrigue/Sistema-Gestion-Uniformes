using VentasApp.Application.DTOs.DetalleVenta;
using VentasApp.Application.Interfaces.Repositorios;

namespace VentasApp.Application.CasoDeUso.DetalleVenta;

public class AgregarDetalleVentaUseCase
{
    private readonly IVentaRepository _repo;
    private readonly IUnitOfWork _unitOfWork;

    public AgregarDetalleVentaUseCase(IVentaRepository repo, IUnitOfWork unit)
    {
        _repo = repo;
        _unitOfWork = unit;
    }

    public async Task Ejecutar(int idVenta, AgregarDetalleVentaDto dto)
    {
        var venta = await _repo.ObtenerPorId(idVenta) ?? throw new Exception("La venta no existe");
        venta.AgregarDetalle(dto.IdItemVendible,dto.Cantidad,dto.PrecioUnitario);
        await _unitOfWork.SaveChanges();
    }
}