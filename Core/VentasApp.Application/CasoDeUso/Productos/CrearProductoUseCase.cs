using VentasApp.Domain.Modelo.Productos;
using VentasApp.Application.Interfaces.Repositorios;
using VentasApp.Application.DTOs.Productos;
namespace VentasApp.Application.CasoDeUso.Productos;

public class CrearProductoUseCase
{
    public readonly IProductoRepository _repository;
    public readonly IUnitOfWork _unit;

    public CrearProductoUseCase(IProductoRepository repo, IUnitOfWork unit)
    {
        _repository = repo;
        _unit = unit;
    }

    public async Task<int> EjecutarAsync(CrearProductoDto dto)
    {
        var producto = new Producto(
            dto.IdCategoria,
            dto.Nombre,
            dto.Costo,
            dto.PrecioVenta
        );

        await _repository.Agregar(producto);
        await _unit.SaveChanges();
        return producto.Id;
    }
}