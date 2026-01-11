namespace VentasApp.Application.CasoDeUso.Producto;

using VentasApp.Application.Interfaces.Repositorios;
using VentasApp.Application.DTOs.Producto;

public class ActualizarProductoUseCase
{
    public readonly IProductoRepository _repository;
    public readonly IUnitOfWork _unit;

    public ActualizarProductoUseCase(IProductoRepository repo, IUnitOfWork unit)
    {
        _repository = repo;
        _unit = unit;
    }

    public async Task EjecutarAsync(int idProducto, ActualizarProductoDto dto)
    {
        //Verifico si el producto existe
        var producto = await _repository.ObtenerProducto(idProducto) ?? throw new Exception("No se encontro el producto");

        //Realizo el respectivo cambio
        producto.CambiarNombre(dto.Nombre);
        producto.CambiarCosto(dto.Costo);
        producto.CambiarPrecioVenta(dto.PrecioVenta);

        //Guardos los cambios hechos
        await _unit.SaveChanges();
    }
}