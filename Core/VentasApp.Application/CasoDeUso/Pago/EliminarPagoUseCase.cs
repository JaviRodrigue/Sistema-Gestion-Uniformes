using VentasApp.Application.Interfaces.Repositorios;

namespace VentasApp.Application.CasoDeUso.Pago;

public class EliminarPagoUseCase
{
    private readonly IPagoRepository _pagoRepo;
    private readonly IUnitOfWork _unitOfWork;

    public EliminarPagoUseCase(IPagoRepository pagoRepo, IUnitOfWork unitOfWork)
    {
        _pagoRepo = pagoRepo;
        _unitOfWork = unitOfWork;
    }

    public async Task EjecutarAsync(int idPago)
    {
        await _pagoRepo.Eliminar(idPago);
        await _unitOfWork.SaveChanges();
    }
}
