namespace VentasApp.Application.CasoDeUso.Stocks;
using VentasApp.Application.DTOs.Stocks;
using VentasApp.Application.Interfaces.Repositorios;

public class ObtenerStockUseCase
{
    public readonly IStockRepository _repository;


    public ObtenerStockUseCase(IStockRepository repo)
    {
        _repository = repo;
    }

    public async Task<StockDto?> EjecutarAsync(int idItem)
    {
        var stock = await _repository.ObtenerPorItemVendible(idItem);
        if(stock == null) return null;

        return new StockDto
        {
            Id = stock.Id,
            IdItemVendible = stock.IdItemVendible,
            CantidadDisponible = stock.CantidadDisponible,
            CantidadReservada = stock.CantidadReservada,
            StockMinimo = stock.StockMinimo,
            Activo = stock.Activo
        };
    }
}