using System;
using System.Collections.ObjectModel;
using System.Linq;

namespace VentasApp.Desktop.ViewModels.DTOs;

public class VentaDetalleDto
{
    public string Codigo { get; set; } = "";
    public string Cliente { get; set; } = "";

    public DateTime? FechaEstimada { get; set; }

    public ObservableCollection<VentaItemDto> Items { get; set; } = new();
    public ObservableCollection<PagoDto> Pagos { get; set; } = new();

    public decimal Total => Items.Sum(i => i.Subtotal);

    public decimal Pagado => Pagos.Sum(p => p.Monto);

    public decimal Restante => Total - Pagado;
}
