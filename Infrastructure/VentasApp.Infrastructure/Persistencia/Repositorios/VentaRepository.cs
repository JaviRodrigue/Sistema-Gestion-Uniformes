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
        return await _context.Ventas.OrderByDescending(v => v.FechaVenta)
                                    .ToListAsync();
    }

    public Task Actualizar(Venta venta)
    {
        _context.Ventas.Update(venta);
        return Task.CompletedTask;
    }


}