using VentasApp.Application.Interfaces;
using VentasApp.Application.Interfaces.Repositorios;

namespace VentasApp.Application.CasoDeUso.Stocks;

public class ActualizarStockMinimoGlobalUseCase
{
    private readonly IStockRepository _stockRepository;
    private readonly IUnitOfWork _unitOfWork;

    public ActualizarStockMinimoGlobalUseCase(IStockRepository stockRepository, IUnitOfWork unitOfWork)
    {
        _stockRepository = stockRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<int> EjecutarAsync(int libMin, int uniMin)
    {
        var stocks = await _stockRepository.ObtenerTodosConProducto();
        int actualizados = 0;

        foreach (var stock in stocks)
        {
            if (stock.ItemVendible?.Producto != null)
            {
                // 1 = Uniforme, 2 = Libreria
                int nuevoMinimo = stock.ItemVendible.Producto.IdCategoria == 1 ? uniMin : libMin;
                if (stock.StockMinimo != nuevoMinimo)
                {
                    stock.CambiarStockMinimo(nuevoMinimo);
                    await _stockRepository.Actualizar(stock);
                    actualizados++;
                }
            }
        }

        if (actualizados > 0)
        {
            await _unitOfWork.SaveChanges();
        }

        return actualizados;
    }
}
