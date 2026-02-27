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

    // Backing fields used when Detalle no tiene items (cargado desde listado)
    private decimal _total;
    private decimal _restante;

    public decimal Total
    {
        get => (Detalle?.Items != null && Detalle.Items.Any()) ? Detalle.Total : _total;
        set => _total = value;
    }

    public decimal Restante
    {
        get => (Detalle?.Pagos != null && Detalle.Items != null && Detalle.Items.Any()) ? Detalle.Restante : _restante;
        set => _restante = value;
    }

    public DateTime? FechaEstimada => Detalle.FechaEstimada;

    public bool TieneDeuda => Restante > 0;

    public bool EstaPagada => Restante <= 0.01m;
    
    public bool TodosLosPagosVerificados { get; set; }
}
