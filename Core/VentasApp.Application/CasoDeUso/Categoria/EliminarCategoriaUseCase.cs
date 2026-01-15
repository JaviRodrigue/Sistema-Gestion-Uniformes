namespace VentasApp.Application.CasoDeUso.Categoria;
using VentasApp.Application.Interfaces.Repositorios;

public class EliminarCategoriaUseCase
{
    public readonly ICategoriaRepository _repository;
    public readonly IUnitOfWork _unit;

    public EliminarCategoriaUseCase(ICategoriaRepository repo, IUnitOfWork unit)
    {
        _repository = repo;
        _unit = unit;
    }

    public async Task EjecutarAsync(int idCategoria)
    {
        var categoria = await _repository.ObtenerCategoriaPorId(idCategoria) ?? throw new Exception("Categoria no encontrada");
        categoria.Desactivar();
        await _unit.SaveChanges();
    }
}