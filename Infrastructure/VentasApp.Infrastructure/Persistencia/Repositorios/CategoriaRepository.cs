namespace VentasApp.Infrastructure.Persistencia.Repositorios;

using Microsoft.EntityFrameworkCore;
using VentasApp.Application.Interfaces.Repositorios;
using VentasApp.Domain.Modelo.Categoria;
using VentasApp.Infrastructure.Persistencia.Contexto;

public class CategoriaRepository : ICategoriaRepository
{
    private readonly DatabaseContext _context;

    public CategoriaRepository(DatabaseContext context)
    {
        _context = context;
    }

    public async Task Agregar(Categoria categoria)
    {
        await _context.Categoria.AddAsync(categoria);
    }

    public async Task<Categoria?> ObtenerCategoriaPorId(int id)
    {
        return await _context.Categoria.FindAsync(id);
    }

    public async Task<List<Categoria>> ObtenerTodasCategorias()
    {
        return await _context.Categoria.ToListAsync();
    }


}