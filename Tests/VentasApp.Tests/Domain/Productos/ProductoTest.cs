using FluentAssertions;
using VentasApp.Domain.Modelo.Productos;
using VentasApp.Domain.Base;
using Xunit;


namespace VentasApp.Domain.Tests.Productos;

public class ProductoTests
{
    // =============================
    // CREACIÓN
    // =============================

    [Fact]
    public void CrearProducto_Valido_DeberiaInicializarCorrectamente()
    {
        var producto = new Producto(
            id_categoria: 1,
            nombre: "Remera",
            costo: 100,
            precioVenta: 150
        );

        producto.IdCategoria.Should().Be(1);
        producto.Nombre.Should().Be("Remera");
        producto.Costo.Should().Be(100);
        producto.PrecioVenta.Should().Be(150);
        producto.Ganancia.Should().Be(50);
        producto.Activo.Should().BeTrue();
    }

    [Fact]
    public void CrearProducto_SinNombre_DeberiaLanzarExcepcion()
    {
        Action act = () => new Producto(1, "", 100, 150);

        act.Should().Throw<ExcepcionDominio>()
           .WithMessage("*nombre*");
    }

    [Fact]
    public void CrearProducto_CostoInvalido_DeberiaLanzarExcepcion()
    {
        Action act = () => new Producto(1, "Remera", 0, 150);

        act.Should().Throw<ExcepcionDominio>()
           .WithMessage("*costo*");
    }

    [Fact]
    public void CrearProducto_PrecioVentaMenorQueCosto_DeberiaLanzarExcepcion()
    {
        Action act = () => new Producto(1, "Remera", 150, 100);

        act.Should().Throw<ExcepcionDominio>()
           .WithMessage("*precio venta*");
    }

    // =============================
    // CAMBIOS DE PROPIEDADES
    // =============================

    [Fact]
    public void CambiarNombre_Valido_DeberiaActualizarNombre()
    {
        var producto = ProductoValido();

        producto.CambiarNombre("Pantalón");

        producto.Nombre.Should().Be("Pantalón");
    }

    [Fact]
    public void CambiarNombre_Vacio_DeberiaFallar()
    {
        var producto = ProductoValido();

        Action act = () => producto.CambiarNombre(" ");

        act.Should().Throw<ExcepcionDominio>()
           .WithMessage("*nombre*");
    }

    [Fact]
    public void CambiarCosto_Valido_DeberiaActualizarCosto()
    {
        var producto = ProductoValido();

        producto.CambiarCosto(200);

        producto.Costo.Should().Be(200);
    }

    [Fact]
    public void CambiarCosto_Cero_DeberiaFallar()
    {
        var producto = ProductoValido();

        Action act = () => producto.CambiarCosto(0);

        act.Should().Throw<ExcepcionDominio>()
           .WithMessage("*costo*");
    }

    [Fact]
    public void CambiarPrecioVenta_Valido_DeberiaActualizarPrecio()
    {
        var producto = ProductoValido();

        producto.CambiarPrecioVenta(250);

        producto.PrecioVenta.Should().Be(250);
    }

    [Fact]
    public void CambiarPrecioVenta_MenorQueCosto_DeberiaFallar()
    {
        var producto = ProductoValido();

        Action act = () => producto.CambiarPrecioVenta(50);

        act.Should().Throw<ExcepcionDominio>()
           .WithMessage("*precio venta*");
    }

    // =============================
    // ITEMS VENDIBLES
    // =============================

    [Fact]
    public void AgregarItemVendible_Valido_DeberiaAgregarlo()
    {
        var producto = ProductoValido();
        var item = new ItemVendible(
            idProducto: producto.Id,
            nombre: "Remera M",
            codigo: "ABC123",
            talle: "M"
        );

        producto.AgregarItem(item);

        producto.ItemsVendibles.Should().HaveCount(1);
    }

    [Fact]
    public void AgregarItemVendible_CodigoBarraDuplicado_DeberiaFallar()
    {
        var producto = ProductoValido();

        var item1 = new ItemVendible(producto.Id, "Item 1", "ABC123", "M");
        var item2 = new ItemVendible(producto.Id, "Item 2", "ABC123", "L");

        producto.AgregarItem(item1);

        Action act = () => producto.AgregarItem(item2);

        act.Should().Throw<ExcepcionDominio>()
           .WithMessage("*codigo de barra*");
    }

    // =============================
    // ESTADO
    // =============================

    [Fact]
    public void DesactivarProducto_DeberiaCambiarEstado()
    {
        var producto = ProductoValido();

        producto.Desactivar();

        producto.Activo.Should().BeFalse();
    }

    // =============================
    // HELPERS
    // =============================

    private static Producto ProductoValido()
    {
        return new Producto(
            id_categoria: 1,
            nombre: "Producto Test",
            costo: 100,
            precioVenta: 150
        );
    }
}
