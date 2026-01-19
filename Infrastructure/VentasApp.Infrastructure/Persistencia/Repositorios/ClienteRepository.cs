using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VentasApp.Application.Interfaces.Repositorios;
using VentasApp.Domain.Modelo.Cliente;
using VentasApp.Infrastructure.Persistencia.Contexto;

namespace VentasApp.Infrastructure.Persistencia.Repositorios;

public class ClienteRepository : IClienteRepository
{
    private readonly DatabaseContext _context;

    public ClienteRepository(DatabaseContext context)
    {
        _context = context;
    }

    public async Task Agregar(Cliente cliente)
    {
        await _context.AddAsync(cliente);
    }

    public async Task<Cliente?> ObtenerClientePorId(int id)
    {
        return await _context.Clientes
            .Include(c => c.Telefonos)
            .FirstOrDefaultAsync(c => c.Id == id && c.Activado);
    }

    public async Task<Cliente?> ObtenerClientePorDni(long dni)
    {
        var str = dni.ToString();
        return await _context.Clientes
            .Include(c => c.Telefonos)
            .FirstOrDefaultAsync(c => c.DNI == str && c.Activado);
    }

    public async Task<Cliente?> ObtenerClientePorTelefono(string telefono)
    {
        return await _context.Clientes
            .Include(c => c.Telefonos)
            .FirstOrDefaultAsync(c => c.Activado && c.Telefonos.Any(t => t.Numero == telefono && t.Activado));
    }

    public async Task<List<Cliente>> BuscarPorNombre(string nombre)
    {
        var q = nombre?.Trim().ToLower() ?? string.Empty;
        return await _context.Clientes
            .Where(c => c.Activado && EF.Functions.Like(c.Nombre.ToLower(), $"%{q}%"))
            .Include(c => c.Telefonos)
            .ToListAsync();
    }

    public async Task<List<Cliente>> ListarClientes()
    {
        var clientes = await _context.Clientes
            .Include(c => c.Telefonos)
            .Where(c => c.Activado)
            .ToListAsync();

        foreach (var c in clientes)
        {
            c.Telefonos.RemoveAll(t => !t.Activado);
        }

        return clientes;
    }

    public async Task Actualizar(Cliente cliente)
    {
        _context.Update(cliente);
    }

    public async Task Eliminar(int id)
    {
        var cliente = await _context.Clientes.FindAsync(id);
        if (cliente == null) return;
        _context.Remove(cliente);
    }

    public async Task Desactivar(int id)
    {
        var cliente = await _context.Clientes.Include(c => c.Telefonos).FirstOrDefaultAsync(c => c.Id == id);
        if (cliente == null) return;

        cliente.Desactivar();

        foreach (var t in cliente.Telefonos)
        {
            t.Desactivar();
        }
    }
}
