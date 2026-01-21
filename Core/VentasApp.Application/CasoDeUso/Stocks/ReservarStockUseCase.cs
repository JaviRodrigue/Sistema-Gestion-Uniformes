namespace VentasApp.Application.CasoDeUso.Stocks;
using VentasApp.Application.DTOs.Stocks;
using VentasApp.Application.Interfaces.Repositorios;

public class ReservarStockUseCase
{
    public readonly IStockRepository _repository;
    public readonly IUnitOfWork _unit;

    public ReservarStockUseCase(IStockRepository repo, IUnitOfWork unit)
    {
        _repository = repo;
        _unit = unit;
    }

    public async Task EjecutarAsync(int idItem,ReservarStockDto dto)
    {
        var stock = await _repository.ObtenerPorItemVendible(idItem) ?? throw new Exception("No se encontro Stock");
        stock.Reservar(dto.Cantidad);
        await _unit.SaveChanges();
    }
}