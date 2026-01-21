using System;

namespace VentasApp.Application.DTOs.Cliente;
public class BuscarClienteDTO
{
    public int? Id { get; set; }
    public string? Dni { get; set; }
    public string? Nombre { get; set; }
    public List<string>? Telefonos { get; set; }
    public DateTime FechaAlta { get; set; }
    
}