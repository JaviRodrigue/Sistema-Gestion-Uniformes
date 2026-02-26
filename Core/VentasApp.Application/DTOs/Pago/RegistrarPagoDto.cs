namespace VentasApp.Application.DTOs.Pago;

public class RegistrarPagoDto
{
    public decimal Monto{get;set;}
    public bool EsSenia{get;set;}
    public List<PagoMetodoDto> Metodos{get;set;} = null!;

}