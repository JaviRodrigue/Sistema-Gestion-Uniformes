using System.Collections.ObjectModel;
using System.Windows.Input;

namespace VentasApp.Desktop.ViewModels.DTOs;

public class ClienteCardDto
{
    public int Id { get; set; }
    public string Nombre { get; set; }
    public string Instagram { get; set; }
    public string Telefonos { get; set; } // Ej: "11223344 / 55667788" (Concatenados)
    public decimal DeudaTotal { get; set; } // Calculado desde el backend
    public bool TieneDeuda => DeudaTotal > 0;

    // Lista para el "Ver más"
    public ObservableCollection<VentaResumenDto> UltimasCompras { get; set; } = new();

    public ICommand? VerDetalleVentaCommand { get; set; }
}
public class VentaResumenDto

{
    public int Id { get; set; }
    public DateTime Fecha { get; set; }
    public string EstadoVenta { get; set; }
    public decimal Total { get; set; }
}