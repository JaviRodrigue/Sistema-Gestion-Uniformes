using VentasApp.Application.DTOs.Cliente;
using VentasApp.Application.Interfaces.Repositorios;
using VentasApp.Domain.Modelo.Cliente;

namespace VentasApp.Application.CasoDeUso.Cliente;

public class CrearClienteCasoDeUso(IClienteRepository repo, IUnitOfWork unit)
{
    public readonly IClienteRepository _repository=repo;
    public readonly IUnitOfWork _unit=unit;


    public async Task EjecutarAsync(CrearClienteDto dto)
    {
        var cliente = new VentasApp.Domain.Modelo.Cliente.Cliente(dto.Nombre, dto.Instagram);
        if (dto.Telefonos != null && dto.Telefonos.Count != 0)
        {
            cliente.ReemplazarTelefonos(dto.Telefonos);
        }

        await _repository.Agregar(cliente);
        await _unit.SaveChanges();
    }
}