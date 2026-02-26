namespace VentasApp.Application.CasoDeUso.Stocks;
using VentasApp.Application.DTOs.Stocks;
using VentasApp.Application.Interfaces.Repositorios;

public class LiberarReservaUseCase
{
    public readonly IStockRepository _repository;
    public readonly IUnitOfWork _unit;

    public LiberarReservaUseCase(IStockRepository repo, IUnitOfWork unit)
    {
        _repository = repo;
        _unit = unit;
    }

    public async Task EjecutarAsync(int idItem, int cantidad)
    {
        var stock = await _repository.ObtenerPorItemVendible(idItem) ?? throw new Exception("Stock no encontrado");
        stock.LiberarReserva(cantidad);
        await _unit.SaveChanges();
   }
}