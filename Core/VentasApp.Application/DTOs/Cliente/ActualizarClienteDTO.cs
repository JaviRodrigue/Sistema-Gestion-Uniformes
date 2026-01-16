using System.Collections.Generic;

namespace VentasApp.Application.DTOs.Cliente;

public class ActualizarClienteDto
{
    public string? Nombre { get; set; }
    public string? Dni { get; set; }
    public List<string>? Telefonos { get; set; }
}