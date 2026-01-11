using Microsoft.EntityFrameworkCore;
using VentasApp.Application.Interfaces.Repositorios;
using VentasApp.Domain.Modelo.Producto;
using VentasApp.Infrastructure.Persistencia.Contexto;

namespace VentasApp.Infrastructure.Persistencia.Repositorios;

public class ItemVendibleRepository : IItemVendibleRepository
{
    private DatabaseContext _context;

    public ItemVendibleRepository(DatabaseContext context)
    {
        _context = context;
    }

    public async Task Agregar(ItemVendible item)
    {
        await _context.ItemVendible.AddAsync(item);
    }

    public async Task<ItemVendible?> ObtenerItem(int id)
    {
        return await _context.ItemVendible
                            .FirstOrDefaultAsync(i => i.Id == id);
    }

    public async Task<List<ItemVendible>> ListarItem(int IdProducto)
    {
        return await _context.ItemVendible
                .Where(i => i.IdProducto == IdProducto)
                .ToListAsync();
    }

    public async Task<ItemVendible?> ObtenerItemPorCodigoBarra(string codigo)
    {
        return await _context.ItemVendible
                    .FirstOrDefaultAsync(i => i.CodigoBarra == codigo);
    }

}