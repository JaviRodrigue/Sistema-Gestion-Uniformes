using System;

namespace VentasApp.Domain;

public class Cliente
{
	public int IdCliente; { get; private set; }
	public long DNI; { get; private set; }
    public string Nombre; { get; private set; }
	public DateTime FechaAlta; { get; private set; }

    public Cliente()
	{
	}
}
