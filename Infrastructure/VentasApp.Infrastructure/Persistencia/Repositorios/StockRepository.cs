using Microsoft.EntityFrameworkCore;
using VentasApp.Application.Interfaces.Repositorios;
using VentasApp.Domain.Modelo.Productos;
using VentasApp.Infrastructure.Persistencia.Contexto;

namespace VentasApp.Infrastructure.Persistencia.Repositorios;

public class StockRepository : IStockRepository
{
    private readonly DatabaseContext _context;

    public StockRepository(DatabaseContext context)
    {
        _context = context;
    }

    public async Task Agregar(Stock stock)
    {
        await _context.Stock.AddAsync(stock);
    }

    //Revisar!!
    public async Task<Stock?> ObtenerPorId(int id)
    {
        return await _context.Stock.FindAsync(id);
    }

    public async Task<Stock?> ObtenerPorItemVendible(int idItem)
    {
        return await _context.Stock
                .FirstOrDefaultAsync(s => s.IdItemVendible == idItem);
    }


}