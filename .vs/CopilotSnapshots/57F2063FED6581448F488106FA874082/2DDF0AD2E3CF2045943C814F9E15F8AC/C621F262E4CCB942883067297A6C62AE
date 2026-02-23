using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VentasApp.Application.DTOs.Venta;

public class VentaItemDto
{
    public int IdDetalle { get; set; }

    public string Descripcion { get; set; } = "";
    public int Cantidad { get; set; }

    public decimal PrecioUnitario { get; set; }

    public decimal Subtotal => Cantidad * PrecioUnitario;
}
