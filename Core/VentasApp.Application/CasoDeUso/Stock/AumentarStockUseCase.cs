namespace VentasApp.Application.CasoDeUso.Stock;
using VentasApp.Application.Interfaces.Repositorios;
using VentasApp.Application.DTOs.Stock;

public class AumentarStockUseCase
{
    public readonly IStockRepository _repository;
    public readonly IUnitOfWork _unit;

    public AumentarStockUseCase(IStockRepository repo, IUnitOfWork unit)
    {
        _repository = repo;
        _unit = unit;
    }

    public async Task EjecutarAsync(int idItem, ActualizarStockDto dto)
    {
        //Verifico si existe el item
        var stock = await _repository.ObtenerPorItemVendible(idItem) ?? throw new Exception("Stock no econtrado");

        stock.Aumentar(dto.Cantidad);
        //Guardo los cambios hechos
        await _unit.SaveChanges();
    }
}