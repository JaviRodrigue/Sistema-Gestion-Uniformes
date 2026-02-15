using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VentasApp.Desktop.ViewModels.DTOs;


public class VentaCardDto
{
    public int Id { get; set; }

    public string Codigo { get; set; }

    public DateTime Fecha { get; set; }

    public decimal Total { get; set; }

    public decimal Restante { get; set; }

    public string EstadoVenta { get; set; }

    public string EstadoPago { get; set; }

    public DateTime? FechaEstimada { get; set; }

    public string? Cliente { get; set; }

    public bool TieneDeuda => Restante > 0;
}
