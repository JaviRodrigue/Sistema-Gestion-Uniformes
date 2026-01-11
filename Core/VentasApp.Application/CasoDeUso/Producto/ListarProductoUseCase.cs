using VentasApp.Application.Interfaces.Repositorios;
using VentasApp.Application.DTOs.Producto;
namespace VentasApp.Application.CasoDeUso.Producto;

public class ListarProductoUseCase
{
    public readonly IProductoRepository _repository;

    public ListarProductoUseCase(IProductoRepository repo)
    {
        _repository = repo;
    }

    public async  Task<List<ListadoProductoDto>> EjecutarAsync()
    {
        var categorias = await _repository.ListarProductos();

        return categorias.Where(p => p.Activo)
                        .Select(p => new ListadoProductoDto
                        {
                            Id = p.Id,
                            Nombre = p.Nombre,
                            PrecioVenta = p.PrecioVenta
                        }).ToList();
    }
}