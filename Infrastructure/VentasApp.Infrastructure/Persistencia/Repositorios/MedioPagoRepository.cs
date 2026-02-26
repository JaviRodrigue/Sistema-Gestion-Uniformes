using Microsoft.EntityFrameworkCore;
using VentasApp.Application.Interfaces.Repositorios;
using VentasApp.Domain.Modelo.Pago;
using VentasApp.Infrastructure.Persistencia.Contexto;

namespace VentasApp.Infrastructure.Persistencia.Repositorios;

public class MedioPagoRepository : IMedioPagoRepository
{
    private DatabaseContext _context;

    public MedioPagoRepository(DatabaseContext context)
    {
        _context = context;
    }

    public async Task<MedioPago?> ObtenerPorId(int id)
    {
        return await _context.MedioPagos.FindAsync(id);
    }

    public async Task<List<MedioPago>> ObtenerTodos()
    {
        return await _context.MedioPagos.ToListAsync();
    }

    public async Task Agregar(MedioPago medioPago)
    {
        await _context.MedioPagos.AddAsync(medioPago);
    }
}