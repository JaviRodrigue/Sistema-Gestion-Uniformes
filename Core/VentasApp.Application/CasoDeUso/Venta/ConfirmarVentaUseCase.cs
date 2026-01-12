namespace VentasApp.Application.CasoDeUso.Venta;
using VentasApp.Application.Interfaces.Repositorios;

public class ConfirmarVentaUseCase
{
    private readonly IVentaRepository _ventaRepository;
    private readonly IUnitOfWork _unitOfWork;

    public ConfirmarVentaUseCase(IVentaRepository ventaRepository, IUnitOfWork unit)
    {
        _ventaRepository = ventaRepository;
        _unitOfWork = unit;
    }

    public async Task EjecutarAsync(int id)
    {
        //Verifico que la venta exista
        var venta = await _ventaRepository.ObtenerPorId(id) ?? throw new Exception("La venta no existe");

        venta.Confirmar();
        await _unitOfWork.SaveChanges();
    }
}