namespace VentasApp.Application.CasoDeUso.Categoria;

using System.Data.Common;
using VentasApp.Application.DTOs.Categoria;
using VentasApp.Application.Interfaces.Repositorios;

public class ObtenerCategoriaUseCase
{
    public readonly ICategoriaRepository _repository;

    public ObtenerCategoriaUseCase(ICategoriaRepository repo)
    {
        _repository = repo;
    }

    public async Task<CategoriaDto?> EjecutarAsync(int idCategoria)
    {
        var categoria = await _repository.ObtenerCategoriaPorId(idCategoria);
        //verifico si se encontro la categoria
        if(categoria == null) return null;

        //Mapeando el Dto Categoria
        return new CategoriaDto
        {
            Id = categoria.Id,
            Nombre = categoria.Nombre,
            Activa = categoria.Activa
        };
    }
}