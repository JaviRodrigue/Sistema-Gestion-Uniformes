using System;
using VentasApp.Application.DTOs.Cliente;
using VentasApp.Application.Interfaces.Repositorios;

namespace VentasApp.Application.CasoDeUso.Cliente;

public class ActualizarClienteCasoDeUso
{
    public readonly IClienteRepository _repository;
    public readonly IUnitOfWork _unit;

    public ActualizarClienteCasoDeUso(IClienteRepository repo, IUnitOfWork unit)
    {
        _repository = repo;
        _unit = unit;
    }

    public async Task EjecutarAsync(int idCliente, ActualizarClienteDto dto)
    {
        var cliente = await _repository.ObtenerClientePorId(idCliente)
            ?? throw new Exception("No se encontró el cliente");

        if (!string.IsNullOrWhiteSpace(dto.Nombre))
            cliente.CambiarNombre(dto.Nombre);

        if (!string.IsNullOrWhiteSpace(dto.Dni))
            cliente.CambiarDni(dto.Dni);

        if (dto.Telefonos != null)
            cliente.ReemplazarTelefonos(dto.Telefonos);

        await _repository.Actualizar(cliente);
        await _unit.SaveChanges();
    }
}