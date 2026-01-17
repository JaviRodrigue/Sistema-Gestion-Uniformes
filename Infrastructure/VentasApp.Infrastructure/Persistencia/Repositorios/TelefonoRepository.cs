using Microsoft.EntityFrameworkCore;
using System.Reflection;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VentasApp.Application.Interfaces.Repositorios;
using VentasApp.Domain.Modelo.Cliente;
using VentasApp.Infrastructure.Persistencia.Contexto;

namespace VentasApp.Infrastructure.Persistencia.Repositorios;

public class TelefonoRepository : ITelefonoRepository
{
    private readonly DatabaseContext _context;

    public TelefonoRepository(DatabaseContext context)
    {
        _context = context;
    }

    public async Task<List<string>> ObtenerTelefonosPorClienteId(int clienteId)
    {
        return await _context.Telefonos
            .Where(t => t.IdCliente == clienteId && t.Activado)
            .Select(t => t.Numero)
            .ToListAsync();
    }

    public async Task AgregarTelefonos(int clienteId, List<string> telefonos)
    {
        var cliente = await _context.Clientes.FindAsync(clienteId);
        if (cliente == null) throw new Exception("Cliente no encontrado");

        foreach (var num in telefonos)
        {
            if (string.IsNullOrWhiteSpace(num)) continue;
            var tel = new Telefono(cliente, num);
            await _context.Telefonos.AddAsync(tel);
        }
    }

    public async Task EliminarTelefonosPorClienteId(int clienteId)
    {
        var lista = await _context.Telefonos.Where(t => t.IdCliente == clienteId).ToListAsync();
        _context.Telefonos.RemoveRange(lista);
    }

    public async Task<string?> ObtenerTelefonoPorId(int telefonoId)
    {
        var t = await _context.Telefonos.FindAsync(telefonoId);
        return t?.Numero;
    }

    public async Task ActualizarTelefono(int telefonoId, string numero)
    {
        var t = await _context.Telefonos.FindAsync(telefonoId);
        if (t == null) return;

        var prop = typeof(Telefono).GetProperty("Numero", BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public);
        prop?.SetValue(t, numero);
    }

    public async Task EliminarTelefono(int telefonoId)
    {
        var t = await _context.Telefonos.FindAsync(telefonoId);
        if (t == null) return;
        _context.Telefonos.Remove(t);
    }

    public async Task DesactivarTelefono(int telefonoId)
    {
        var t = await _context.Telefonos.FindAsync(telefonoId);
        if (t == null) return;
        t.Desactivar();
    }

    public async Task DesactivarTelefonosPorClienteId(int clienteId)
    {
        var lista = await _context.Telefonos.Where(t => t.IdCliente == clienteId).ToListAsync();
        foreach (var t in lista)
        {
            t.Desactivar();
        }
    }
}
