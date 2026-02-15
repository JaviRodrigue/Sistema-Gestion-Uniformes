using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VentasApp.Desktop.ViewModels.DTOs;


    public class VentaItemDto
{
    public string Producto { get; set; } = null!;
    public decimal PrecioUnitario { get; set; }
    public int Cantidad { get; set; }

    public decimal Subtotal => PrecioUnitario * Cantidad;
}
