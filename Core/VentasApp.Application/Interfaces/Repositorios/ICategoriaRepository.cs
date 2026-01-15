using VentasApp.Domain.Modelo.Categoria;

namespace VentasApp.Application.Interfaces.Repositorios;

public interface ICategoriaRepository
{
    Task Agregar(Categoria categoria);
    Task<Categoria?> ObtenerCategoriaPorId(int idCategoria);
    Task<List<Categoria>> ObtenerTodasCategorias();
}