using VentasApp.Application.Interfaces.Repositorios;
using VentasApp.Infrastructure.Persistencia.Contexto;

namespace VentasApp.Infrastructure.Persistencia.Repositorios;

public class UnitOfWork : IUnitOfWork
{
    private readonly DatabaseContext _context;

    public UnitOfWork(DatabaseContext context)
    {
        _context = context;
    }

    public Task<int> SaveChanges()
    {
        return _context.SaveChangesAsync();
    }
}