using System.Collections.Generic;

namespace VentasApp.Application.DTOs.Cliente;

public class CrearClienteDto
{
    public string Nombre { get; set; } = string.Empty;
    public string? Instagram { get; set; }
    public List<string>? Telefonos { get; set; }
}