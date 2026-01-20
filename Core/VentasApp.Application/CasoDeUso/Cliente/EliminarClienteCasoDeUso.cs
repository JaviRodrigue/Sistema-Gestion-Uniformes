using System;
using VentasApp.Application.Interfaces.Repositorios;

namespace VentasApp.Application.CasoDeUso.Cliente;

public class EliminarClienteCasoDeUso(IClienteRepository repo, IUnitOfWork unit)
{
    public readonly IClienteRepository _repository = repo;
    public readonly IUnitOfWork _unit=unit;


    public async Task EjecutarAsync(int idCliente)
    {
        _ = await _repository.ObtenerClientePorId(idCliente)
            ?? throw new Exception("No se encontró el cliente");

        // Eliminación lógica: desactivar el cliente en vez de borrarlo físicamente
        await _repository.Desactivar(idCliente);
        await _unit.SaveChanges();
    }
}