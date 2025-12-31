using System;

namespace VentasApp.Domain;

public class PagoMetodo
{
	public int IdPagoMetodo { get; set; }
	public int IdPago { get; set; }
	public Pago Pago { get; set; }
    public int IdMedioPago { get; set; }
	public MedioPago MedioPago { get; set; }
    public double Monto { get; set; }

    public PagoMetodo()
	{
	}
}
