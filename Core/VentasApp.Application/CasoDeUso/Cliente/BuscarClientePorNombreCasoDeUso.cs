using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VentasApp.Application.DTOs.Cliente;
using VentasApp.Application.Interfaces.Repositorios;

namespace VentasApp.Application.CasoDeUso.Cliente;

public class BuscarClientePorNombreCasoDeUso(IClienteRepository repo)
{
    private readonly IClienteRepository _repository=repo;


    public async Task<List<BuscarClienteDTO>> EjecutarAsync(string nombre)
    {
        var clientes = await _repository.BuscarPorNombre(nombre);

        if (clientes == null || clientes.Count == 0)
            throw new Exception("Cliente no encontrado");

        var resultado = clientes.Select(cliente => new BuscarClienteDTO
        {
            Id = cliente.Id,
            Instagram = cliente.Instagram,
            Nombre = cliente.Nombre,
            Telefonos = cliente.Telefonos?.Select(t => t.Numero).ToList(),
            FechaAlta = cliente.FechaAlta
        }).ToList();

        return resultado;
    }
}
