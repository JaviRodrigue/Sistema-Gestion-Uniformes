using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VentasApp.Desktop.ViewModels.DTOs;

public class VentaDetalleDto
{
    public string Codigo { get; set; } = null!;

    public string Cliente { get; set; } = null!;

    public decimal Total { get; set; }

    public decimal Restante { get; set; }

    public ObservableCollection<VentaItemDto> Items { get; set; } = null!;

    public ObservableCollection<PagoDto> Pagos { get; set; } = null!;
}

