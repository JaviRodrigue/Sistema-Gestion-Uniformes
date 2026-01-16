using System;
using VentasApp.Application.DTOs.Cliente;
using VentasApp.Application.Interfaces.Repositorios;

namespace VentasApp.Application.CasoDeUso.Telefono;

public class ActualizarTelefonoCasoDeUso
{
    public readonly ITelefonoRepository _repository;
    public readonly IUnitOfWork _unit;

    public ActualizarTelefonoCasoDeUso(ITelefonoRepository repo, IUnitOfWork unit)
    {
        _repository = repo;
        _unit = unit;
    }

    public async Task EjecutarAsync(int idTelefono, string nuevoNumero)
    {
        var existe = await _repository.ObtenerTelefonoPorId(idTelefono)
            ?? throw new Exception("No se encontró el teléfono");

        await _repository.ActualizarTelefono(idTelefono, nuevoNumero);
        await _unit.SaveChanges();
    }
}
