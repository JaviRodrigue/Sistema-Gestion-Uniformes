using VentasApp.Domain.Modelo.Pago;

namespace VentasApp.Application.DTOs.Pago;

public class CrearPagoDto
{
    public int IdVenta{get;set;}
    public List<PagoMetodo> Metodos{get;set;} = new();
}