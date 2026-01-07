using VentasApp.Application.Interfaces.Repositorios;

namespace VentasApp.Application.CasoDeUso.DetalleVenta;

public class EliminarDetalleVentaUseCase
{
    private readonly IVentaRepository _repo;
    private readonly IUnitOfWork _unitOfWork;

    public EliminarDetalleVentaUseCase(IVentaRepository repo, IUnitOfWork unit)
    {
        _repo = repo;
        _unitOfWork = unit;
    }

    public async Task Ejecutar(int idVenta, int idDetalle)
    {
        var venta = await _repo.ObtenerPorId(idVenta) ?? throw new Exception("No se encontro Venta");
        venta.EliminarDetalle(idDetalle);
        await _unitOfWork.SaveChanges();
    }
}