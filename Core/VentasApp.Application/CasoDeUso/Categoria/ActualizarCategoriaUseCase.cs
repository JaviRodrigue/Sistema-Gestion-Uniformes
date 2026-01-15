namespace VentasApp.Application.CasoDeUso.Categoria;

using VentasApp.Application.DTOs.Categoria;
using VentasApp.Application.Interfaces.Repositorios;


public class ActualizarCategoriaUseCase
{
    public readonly ICategoriaRepository _repository;
    public readonly IUnitOfWork _unit;

    public ActualizarCategoriaUseCase(ICategoriaRepository repo, IUnitOfWork unit)
    {
        _repository = repo;
        _unit = unit;
    }

    public async Task EjecutarAsync(int idCategoria, ActualizarCategoriaDto dto)
    {
        var categoria = await _repository.ObtenerCategoriaPorId(idCategoria) ?? throw new Exception("No se encontro la categoria");
        categoria.CambiarNombre(dto.Nombre);
        await _unit.SaveChanges();
    }
}