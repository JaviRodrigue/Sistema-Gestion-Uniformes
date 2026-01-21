using FluentAssertions;
using VentasApp.Domain.Modelo.Productos;
using VentasApp.Domain.Base;
using Xunit;

namespace VentasApp.Domain.Tests.Producto;

public class ItemVendibleTests
{
    // =============================
    // CREACIÓN
    // =============================

    [Fact]
    public void CrearItemVendible_Valido_DeberiaCrearseCorrectamente()
    {
        var item = new ItemVendible(
            idProducto: 1,
            nombre: "Remera Negra",
            codigo: "ABC123",
            talle: "M"
        );

        item.IdProducto.Should().Be(1);
        item.Nombre.Should().Be("remera negra"); // normalizado
        item.CodigoBarra.Should().Be("ABC123");
        item.Talle.Should().Be("M");
    }

    [Fact]
    public void CrearItemVendible_SinNombre_DeberiaLanzarExcepcion()
    {
        Action act = () => new ItemVendible(
            idProducto: 1,
            nombre: "",
            codigo: "ABC123",
            talle: null
        );

        act.Should().Throw<ExcepcionDominio>()
           .WithMessage("*nombre*");
    }

    [Fact]
    public void CrearItemVendible_SinCodigoBarra_DeberiaLanzarExcepcion()
    {
        Action act = () => new ItemVendible(
            idProducto: 1,
            nombre: "Remera",
            codigo: "",
            talle: null
        );

        act.Should().Throw<ExcepcionDominio>()
           .WithMessage("*codigo de barras*");
    }

    [Fact]
    public void CrearItemVendible_TallePuedeSerNull()
    {
        var item = new ItemVendible(
            idProducto: 1,
            nombre: "Remera Blanca",
            codigo: "XYZ789",
            talle: null
        );

        item.Talle.Should().BeNull();
    }

    // =============================
    // CAMBIAR NOMBRE
    // =============================

    [Fact]
    public void CambiarNombre_Valido_DeberiaActualizarNombre()
    {
        var item = ItemValido();

        item.CambiarNombre("Pantalón Azul");

        item.Nombre.Should().Be("pantalón azul");
    }

    [Fact]
    public void CambiarNombre_Vacio_DeberiaLanzarExcepcion()
    {
        var item = ItemValido();

        Action act = () => item.CambiarNombre(" ");

        act.Should().Throw<ExcepcionDominio>()
           .WithMessage("*nombre*");
    }

    // =============================
    // CAMBIAR CÓDIGO DE BARRAS
    // =============================

    [Fact]
    public void CambiarCodigoBarras_Valido_DeberiaActualizarCodigo()
    {
        var item = ItemValido();

        item.CambiarCodigoBarras("NEW123");

        item.CodigoBarra.Should().Be("NEW123");
    }

    [Fact]
    public void CambiarCodigoBarras_Vacio_DeberiaLanzarExcepcion()
    {
        var item = ItemValido();

        Action act = () => item.CambiarCodigoBarras("");

        act.Should().Throw<ExcepcionDominio>()
           .WithMessage("*codigo de barras*");
    }

    // =============================
    // HELPERS
    // =============================

    private static ItemVendible ItemValido()
    {
        return new ItemVendible(
            idProducto: 1,
            nombre: "Item Test",
            codigo: "TEST123",
            talle: "L"
        );
    }
}
