using System;

namespace VentasApp.Domain;

public class MedioPago
{
	public int IdMedioPago { get; set; }
	public string Nombre { get; set; }
	public bool TieneRecargo { get; set; }

    public MedioPago()
	{
	}
}
