using VentasApp.Domain.Modelo.Producto;

namespace VentasApp.Application.Interfaces.Repositorios;

public interface IProductoRepository
{
    Task Agregar(Producto producto);
    Task<Producto?> ObtenerProductoPorId(int id);
    Task<List<Producto>> ObtenerTodosProductos();
}