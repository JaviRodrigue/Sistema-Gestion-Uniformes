namespace VentasApp.Application.DTOs.Pago;

public class PagoMetodoDetalleDto
{
    public int IdMedioPago { get; set; }
    public string MedioPago{get;set;} = null!;
    public decimal Monto{get;set;}
}