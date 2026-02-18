namespace VentasApp.Application.CasoDeUso.Stocks;
using VentasApp.Application.Interfaces.Repositorios;

public class ActualizarStockMinimoUseCase
{
    public readonly IStockRepository _repository;
    public readonly IUnitOfWork _unit;

    public ActualizarStockMinimoUseCase(IStockRepository repo, IUnitOfWork unit)
    {
        _repository = repo;
        _unit = unit;
    }

    public async Task EjecutarAsync(int idItem, int nuevoMinimo)
    {
        var stock = await _repository.ObtenerPorItemVendible(idItem)
            ?? throw new Exception("Stock no encontrado");

        stock.CambiarStockMinimo(nuevoMinimo);
        await _unit.SaveChanges();
    }
}
