using VentasApp.Application.DTOs.Venta;
using VentasApp.Application.Interfaces.Repositorios;

namespace VentasApp.Application.CasoDeUso.DetalleVenta;

public class ModificarDetalleUseCase
{
    private readonly IVentaRepository _repo;
    private readonly IUnitOfWork _unitOfWork;
    
    public ModificarDetalleUseCase(IVentaRepository repo, IUnitOfWork unit)
    {
        _repo = repo;
        _unitOfWork = unit;
    }

    public async Task Ejecutar(int idVenta, int idDetalle, ModificarDetalleVentaDto dto)
    {
        var venta = await _repo.ObtenerPorId(idVenta) ?? throw new Exception("No se encontro la Venta");
        venta.ModificarDetalle(idDetalle,dto.Cantidad,dto.PrecioUnitario);
        await _unitOfWork.SaveChanges();
    }
}