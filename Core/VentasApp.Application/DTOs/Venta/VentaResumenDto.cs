

namespace VentasApp.Application.DTOs.Venta;

public class VentaResumenDto
{
    public int Id { get; set; }
    public DateTime Fecha { get; set; }

    public string Codigo { get; set; } = "";
    public string? Cliente { get; set; }
    public decimal Total { get; set; }
    public decimal Restante { get; set; }
    public string EstadoVenta { get; set; } = "";

    public string EstadoPago { get; set; } = "";
}