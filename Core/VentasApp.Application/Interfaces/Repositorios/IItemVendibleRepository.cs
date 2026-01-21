using VentasApp.Domain.Modelo.Productos;

namespace VentasApp.Application.Interfaces.Repositorios;

public interface IItemVendibleRepository
{
    Task Agregar(ItemVendible itemVendible);
    Task<ItemVendible?> ObtenerItem(int id);
    Task<ItemVendible?> ObtenerItemPorCodigoBarra(string codigo);
    Task<List<ItemVendible>> ListarItem(int idProducto);
}