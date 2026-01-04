using System;

namespace VentasApp.Application.DTOs.Cliente;

public class CrearClienteDTO
{
    public long? Dni { get; set; }
    public string? Telefono { get; set; }
    public string? Nombre { get; set; }
}
