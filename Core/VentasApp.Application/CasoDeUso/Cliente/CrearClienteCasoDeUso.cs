using VentasApp.Application.DTOs.Cliente;
using VentasApp.Application.Interfaces.Repositorios;
using VentasApp.Domain.Modelo.Cliente;

namespace VentasApp.Application.CasoDeUso.Cliente;

public class CrearClienteCasoDeUso
{
    public readonly IClienteRepository _repository;
    public readonly IUnitOfWork _unit;

    public CrearClienteCasoDeUso(IClienteRepository repo, IUnitOfWork unit)
    {
        _repository = repo;
        _unit = unit;
    }

    public async Task EjecutarAsync(CrearClienteDto dto)
    {
        var cliente = new Cliente(dto.Nombre, dto.Dni);
        if (dto.Telefonos != null && dto.Telefonos.Any())
        {
            cliente.ReemplazarTelefonos(dto.Telefonos);
        }

        await _repository.Agregar(cliente);
        await _unit.SaveChanges();
    }
}