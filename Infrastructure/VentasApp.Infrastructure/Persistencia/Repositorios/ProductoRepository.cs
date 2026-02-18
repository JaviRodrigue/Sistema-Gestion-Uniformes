using Microsoft.EntityFrameworkCore;
using VentasApp.Application.Interfaces.Repositorios;
using VentasApp.Domain.Modelo.Productos;
using VentasApp.Infrastructure.Persistencia.Contexto;

namespace VentasApp.Infrastructure.Persistencia.Repositorios;

public class ProductoRepository : IProductoRepository
{
    private readonly DatabaseContext _context;

    public ProductoRepository(DatabaseContext context)
    {
        _context = context;
    }

    public async Task Agregar(Producto producto)
    {
        await _context.Producto.AddAsync(producto);
    }

    public async Task<Producto?> ObtenerProducto(int id)
    {
        return await _context.Producto
                            .Include(p => p.ItemsVendibles)
                            .FirstOrDefaultAsync(p => p.Id == id);
    }

    public async Task<List<Producto>> ListarProductos()
    {
        return await _context.Producto
            .Include(p => p.ItemsVendibles)
            .ToListAsync();
    }
}