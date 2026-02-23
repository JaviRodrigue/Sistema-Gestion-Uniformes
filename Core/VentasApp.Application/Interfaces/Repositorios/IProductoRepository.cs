using VentasApp.Domain.Modelo.Productos;

namespace VentasApp.Application.Interfaces.Repositorios;

public interface IProductoRepository
{
    Task Agregar(Producto producto);
    Task<Producto?> ObtenerProducto(int id);
    Task<List<Producto>> ListarProductos();
}