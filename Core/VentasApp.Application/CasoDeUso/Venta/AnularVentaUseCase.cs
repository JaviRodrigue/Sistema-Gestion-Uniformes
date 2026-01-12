namespace VentasApp.Application.CasoDeUso.Venta;
using VentasApp.Application.Interfaces.Repositorios;


public class AnularVentaUseCase
{
    private readonly IVentaRepository _ventaRepository;
    private readonly IUnitOfWork _unitOfWork;

    public AnularVentaUseCase(IVentaRepository ventaRepository, IUnitOfWork unit)
    {
        _ventaRepository = ventaRepository;
        _unitOfWork = unit;
    }

    public async Task EjecutarAsync(int idVenta)
    {
        var venta = await _ventaRepository.ObtenerPorId(idVenta) ?? throw new Exception("La venta no existe");
        venta.AnularVenta();
        await _unitOfWork.SaveChanges();
    }
}