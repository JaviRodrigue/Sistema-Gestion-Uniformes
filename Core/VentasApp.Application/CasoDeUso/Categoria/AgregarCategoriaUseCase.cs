namespace VentasApp.Application.CasoDeUso.Categoria;

using VentasApp.Application.DTOs.Categoria;
using VentasApp.Application.Interfaces.Repositorios;
using VentasApp.Domain.Modelo.Categoria;

public class AgregarCategoriaUseCase
{
    private readonly ICategoriaRepository _repository;
    private readonly IUnitOfWork _unit;

    public AgregarCategoriaUseCase(ICategoriaRepository repo, IUnitOfWork unit)
    {
        _repository = repo;
        _unit = unit;
    }

    public async Task Ejecutar(CrearCategoriaDto dto)
    {
        var categoria = new Categoria(dto.Nombre);
        await _repository.Agregar(categoria);
        await _unit.SaveChanges();
    }
}