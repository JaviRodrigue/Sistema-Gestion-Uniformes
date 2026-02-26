using Xunit;

using VentasApp.Domain.Modelo.Venta;
using VentasApp.Domain.Base;

public class DetalleVentaTest
{
    [Fact]
    public void crearDetalleVenta_valido_crearCorrectamente()
    {
        int itemId= 1;
        int cantidad = 2;
        decimal precio = 100;

        var detalle = new DetalleVenta(itemId, cantidad, precio);

        // Assert
        Assert.Equal(itemId, detalle.IdItemVendible);
        Assert.Equal(cantidad, detalle.Cantidad);
        Assert.Equal(precio, detalle.PrecioUnitario);
        Assert.Equal(200, detalle.SubTotal);
    }

     [Theory]
    [InlineData(0)]
    [InlineData(-1)]
    public void CrearDetalleVenta_CantidadInvalida_LanzaExcepcion(int cantidad)
    {
        // Act & Assert
        Assert.Throws<ExcepcionDominio>(() =>
            new DetalleVenta(1, cantidad, 100)
        );
    }

    [Theory]
    [InlineData(0)]
    [InlineData(-50)]
    public void CrearDetalleVenta_PrecioInvalido_LanzaExcepcion(decimal precio)
    {
        Assert.Throws<ExcepcionDominio>(() =>
            new DetalleVenta(1, 1, precio)
        );
    }

    [Fact]
    public void ModificarCantidad_Valida_ActualizaCorrectamente()
    {
        var detalle = new DetalleVenta(1, 1, 100);

        detalle.ModificarCantidad(3);

        Assert.Equal(3, detalle.Cantidad);
        Assert.Equal(300, detalle.SubTotal);
    }

    [Fact]
    public void ModificarPrecio_Valido_ActualizaCorrectamente()
    {
        var detalle = new DetalleVenta(1, 2, 100);

        detalle.ModificarPrecio(150);

        Assert.Equal(150, detalle.PrecioUnitario);
        Assert.Equal(300, detalle.SubTotal);
    }

    [Fact]
    public void ModificarCantidad_Invalida_LanzaExcepcion()
    {
        var detalle = new DetalleVenta(1, 1, 100);

        Assert.Throws<ExcepcionDominio>(() =>
            detalle.ModificarCantidad(0)
        );
    }

}

