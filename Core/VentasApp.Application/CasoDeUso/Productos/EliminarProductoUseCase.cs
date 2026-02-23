namespace VentasApp.Application.CasoDeUso.Productos;
using VentasApp.Application.Interfaces.Repositorios;

public class EliminarProductoUseCase
{
    public readonly IProductoRepository _repository;
    public readonly IUnitOfWork _unit;

    public EliminarProductoUseCase(IProductoRepository repo, IUnitOfWork unit)
    {
        _repository = repo;
        _unit = unit;
    }

    public async Task EjecutarAsync(int idProducto)
    {
        //Verifico si el producto existe
        var producto = await _repository.ObtenerProducto(idProducto) ?? throw new Exception("No se encontro el producto");
        //Realizo una eliminacion logica, no fisica
        producto.Desactivar();
        //Guardo los cambios
        await _unit.SaveChanges();
    }
}