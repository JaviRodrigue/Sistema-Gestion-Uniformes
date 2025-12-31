using System;

namespace VentasApp.Domain;

public class Estado
{
    public int IdEstado { get; private set; }
    public string Nombre { get; private set; }

    private Estado() { }
}