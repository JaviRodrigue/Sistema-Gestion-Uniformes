using VentasApp.Domain.Modelo.Producto;

namespace VentasApp.Application.Interfaces.Repositorios;

public interface IItemVendibleRepository
{
    Task Agregar(ItemVendible itemVendible);
    Task<ItemVendible?> ObtenerPorId(int id);
    Task<List<ItemVendible>> ObtenerTodas();
}