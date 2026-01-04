using System;

namespace VentasApp.Application.DTOs.Cliente;
public class BuscarClienteDTO
{
    public int? Id { get; set; }
    public long? Dni { get; set; }
    public string? Telefono { get; set; }
    public string? Nombre { get; set; }
}