using System;
using VentasApp.Application.Interfaces.Repositorios;

namespace VentasApp.Application.CasoDeUso.Cliente;

public class EliminarClienteCasoDeUso
{
    public readonly IClienteRepository _repository;
    public readonly IUnitOfWork _unit;

    public EliminarClienteCasoDeUso(IClienteRepository repo, IUnitOfWork unit)
    {
        _repository = repo;
        _unit = unit;
    }

    public async Task EjecutarAsync(int idCliente)
    {
        var existe = await _repository.ObtenerClientePorId(idCliente)
            ?? throw new Exception("No se encontró el cliente");

        await _repository.Eliminar(idCliente);
        await _unit.SaveChanges();
    }
}