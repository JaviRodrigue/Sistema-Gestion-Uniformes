using System;

namespace VentasApp.Application.DTOs.Cliente;

public class AgregarTelefonoClienteDTO
{
    public int IdCliente { get; set; }
    public string? Numero { get; set; }
}
