using System;

namespace VentasApp.Desktop.ViewModels.DTOs;

public class VentaCardDto
{
    public int Id { get; set; }

    public string Codigo { get; set; } = "";

    public DateTime Fecha { get; set; }

    public string EstadoVenta { get; set; } = "";

    public string EstadoPago { get; set; } = "";

    public string? Cliente { get; set; }

    // 🔥 AHORA EL DETALLE ES PARTE DEL ESTADO
    public VentaDetalleDto Detalle { get; set; } = new();

    // ===============================
    // PROPIEDADES CALCULADAS
    // ===============================

    public decimal Total => Detalle.Total;

    public decimal Restante => Detalle.Restante;

    //public DateTime? FechaEstimada => Detalle.FechaEstimada;

    public bool TieneDeuda => Restante > 0;
}
