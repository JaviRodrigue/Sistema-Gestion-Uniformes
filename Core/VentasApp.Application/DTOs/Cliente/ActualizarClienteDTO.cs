using System.Collections.Generic;

namespace VentasApp.Application.DTOs.Cliente;

public class ActualizarClienteDto
{
    public int Id { get; set; }
    public string? Nombre { get; set; }
    public string? Dni { get; set; }
    public List<string>? Telefonos { get; set; }
}