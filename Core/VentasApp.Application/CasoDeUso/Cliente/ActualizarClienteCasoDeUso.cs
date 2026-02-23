using System;
using VentasApp.Application.DTOs.Cliente;
using VentasApp.Application.Interfaces.Repositorios;

namespace VentasApp.Application.CasoDeUso.Cliente;

public class ActualizarClienteCasoDeUso(IClienteRepository repo, IUnitOfWork unit)
{
    public readonly IClienteRepository _repository=repo;
    public readonly IUnitOfWork _unit=unit;

    

    public async Task EjecutarAsync(ActualizarClienteDto dto)
    {
        var cliente = await _repository.ObtenerClientePorId(dto.Id)
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