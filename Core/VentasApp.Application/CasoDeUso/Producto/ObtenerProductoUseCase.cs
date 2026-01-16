namespace VentasApp.Application.CasoDeUso.Producto;
using VentasApp.Application.Interfaces.Repositorios;
using VentasApp.Application.DTOs.Producto;

public class ObtenerProductoUseCase
{
    public IProductoRepository _repository;

    public ObtenerProductoUseCase(IProductoRepository repo)
    {
        _repository = repo;
    }

    public async Task<ProductoDto?> EjecutarAsync(int idProducto)
    {
        var producto = await _repository.ObtenerProducto(idProducto);
        if(producto == null) return null;

        //mapeo el dto de productos
        return new ProductoDto{
            Id = producto.Id,
            IdCategoria = producto.IdCategoria,
            Nombre = producto.Nombre,
            Costo = producto.Costo,
            PrecioVenta = producto.PrecioVenta,
            Ganancia = producto.Ganancia,
            Activa = producto.Activo
        };
        ;
    }
}