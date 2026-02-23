namespace VentasApp.Application.Interfaces.Repositorios;
using VentasApp.Application.DTOs.Categoria;

public class ListarCategoriaUseCase
{
    public ICategoriaRepository _repository;
    public IUnitOfWork _unit;

    public ListarCategoriaUseCase(ICategoriaRepository repo, IUnitOfWork unit)
    {
        _repository = repo;
        _unit = unit;
    }

    public async Task<List<CategoriaDto>> EjecutarAsync()
    {
        var categorias = await _repository.ObtenerTodasCategorias();

        return categorias.Where(c => c.Activa)
        .Select(c => new CategoriaDto
        {
            Id = c.Id,
            Nombre = c.Nombre
        }).ToList();
    }
}