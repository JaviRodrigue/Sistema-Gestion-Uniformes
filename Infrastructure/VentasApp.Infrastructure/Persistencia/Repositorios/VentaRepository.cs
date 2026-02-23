namespace VentasApp.Infrastructure.Persistencia.Repositorios;
using Microsoft.EntityFrameworkCore;
using VentasApp.Application.Interfaces.Repositorios;
using VentasApp.Domain.Modelo.Venta;
using VentasApp.Infrastructure.Persistencia.Contexto;

public class VentaRepository : IVentaRepository
{
    private readonly DatabaseContext _context;

    public VentaRepository(DatabaseContext context)
    {
        _context = context;
    }

    public async Task Agregar(Venta venta)
    {
        await _context.Ventas.AddAsync(venta);
    }

    public async Task<Venta?> ObtenerPorId(int idVenta)
    {
        return await _context.Ventas.Include(v => v.Detalles)
                                    .FirstOrDefaultAsync(v => v.Id == idVenta);
    }

    public async Task<List<Venta>> ObtenerTodas()
    {
        // Use AsNoTracking so callers (UI) get fresh values and EF doesn't return
        // tracked instances from a long-lived DbContext. This helps avoid stale
        // values in desktop scenarios where multiple DbContext instances may exist.
        return await _context.Ventas
                             .AsNoTracking()
                             .OrderByDescending(v => v.FechaVenta)
                             .ToListAsync();
    }

    public Task Actualizar(Venta venta)
    {
        _context.Ventas.Update(venta);
        return Task.CompletedTask;
    }

    public async Task VincularClienteVenta(int idVenta, int idCliente)
    {
        var compraExistente = await _context.Compras.FirstOrDefaultAsync(c => c.IdVenta == idVenta);
        if (compraExistente != null)
        {
            if (compraExistente.IdCliente != idCliente)
            {
                _context.Compras.Remove(compraExistente);
                await _context.Compras.AddAsync(new Compra(idVenta, idCliente));
            }
        }
        else
        {
            await _context.Compras.AddAsync(new Compra(idVenta, idCliente));
        }
    }

    public async Task<List<int>> ObtenerIdsVentasPorClientes(List<int> clientesIds)
    {
        return await _context.Compras
            .Where(c => clientesIds.Contains(c.IdCliente))
            .Select(c => c.IdVenta)
            .Distinct()
            .ToListAsync();
    }
}