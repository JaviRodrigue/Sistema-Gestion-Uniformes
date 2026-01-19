using System;
using VentasApp.Application.Interfaces.Repositorios;

namespace VentasApp.Application.CasoDeUso.Telefono;

public class EliminarTelefonosDeClienteCasoDeUso
{
    public readonly ITelefonoRepository _repository;
    public readonly IUnitOfWork _unit;

    public EliminarTelefonosDeClienteCasoDeUso(ITelefonoRepository repo, IUnitOfWork unit)
    {
        _repository = repo;
        _unit = unit;
    }

    public async Task EjecutarAsync(int idCliente)
    {
        if (_repository is not null)
        {
                await _repository.DesactivarTelefonosPorClienteId(idCliente);
        }

        await _unit.SaveChanges();
    }
}
