namespace VentasApp.Application.CasoDeUso.Stock;

using VentasApp.Application.Interfaces.Repositorios;

public class ObtenerStockDisponibleUseCase
{
    private readonly IStockRepository _stockRepository;

    public ObtenerStockDisponibleUseCase(IStockRepository stockRepository)
    {
        _stockRepository = stockRepository;
    }

    public async Task<int?> EjecutarAsync(int idItemVendible)
    {
        var stock = await _stockRepository.ObtenerPorItemVendible(idItemVendible);
        return stock?.CantidadDisponible;
    }
}
