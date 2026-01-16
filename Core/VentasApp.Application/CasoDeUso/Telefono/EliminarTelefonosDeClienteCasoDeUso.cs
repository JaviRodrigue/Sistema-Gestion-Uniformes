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
        await _repository.EliminarTelefonosPorClienteId(idCliente);
        await _unit.SaveChanges();
    }
}
