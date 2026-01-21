namespace VentasApp.Application.CasoDeUso.Stocks;
using VentasApp.Application.Interfaces.Repositorios;
using VentasApp.Application.DTOs.Stocks;

public class DescontarStockUseCase
{
    public readonly IStockRepository _repository;
    public readonly IUnitOfWork _unit;

    public DescontarStockUseCase(IStockRepository repo, IUnitOfWork unit)
    {
        _repository = repo;
        _unit = unit;
    }

    public async Task EjecutarAsync(int idItem,int cantidad)
    {
        var stock = await _repository.ObtenerPorItemVendible(idItem) ?? throw new Exception("No se encontro stock");
        stock.Descontar(cantidad);
        await _unit.SaveChanges();
    }
}