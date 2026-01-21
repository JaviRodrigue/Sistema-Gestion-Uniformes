using VentasApp.Domain.Modelo.Productos;

namespace VentasApp.Application.Interfaces.Repositorios;

public interface IStockRepository
{
    Task Agregar(Stock stock);
    Task<Stock?> ObtenerPorId(int id);
    Task<Stock?>ObtenerPorItemVendible(int idItem);
}