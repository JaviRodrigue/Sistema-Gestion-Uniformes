using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VentasApp.Application.DTOs.ItemVendible;
using VentasApp.Application.DTOs.Pago;

namespace VentasApp.Application.DTOs.Venta;


public class VentaDetalleDto
{
    public int Id { get; set; }
    public string Codigo { get; set; } = "";
    public string Cliente { get; set; } = "";
    public int IdCliente { get; set; }
    public decimal Total { get; set; }
    public decimal Restante { get; set; }

    public List<VentaItemDto> Items { get; set; } = new();
    public List<PagoDto> Pagos { get; set; } = new();
}
