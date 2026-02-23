using FluentAssertions;
using VentasApp.Domain.Modelo.Productos;
using VentasApp.Domain.Base;
using Xunit;

namespace VentasApp.Domain.Tests.Productos;

public class StockTests
{
    // =============================
    // CREACIÓN
    // =============================

    [Fact]
    public void CrearStock_Valido_DeberiaInicializarCorrectamente()
    {
        var stock = new Stock(IdItemVendible: 1, cantidadInicial: 10, stockMinimo: 2);

        stock.IdItemVendible.Should().Be(1);
        stock.CantidadDisponible.Should().Be(10);
        stock.CantidadReservada.Should().Be(0);
        stock.StockMinimo.Should().Be(2);
        stock.Activo.Should().BeTrue();
    }

    [Fact]
    public void CrearStock_ConCantidadNegativa_DeberiaLanzarExcepcion()
    {
        Action act = () => new Stock(1, -1, 1);

        act.Should().Throw<ExcepcionDominio>()
           .WithMessage("*cantidad inicial*");
    }

    [Fact]
    public void CrearStock_ConStockMinimoInvalido_DeberiaLanzarExcepcion()
    {
        Action act = () => new Stock(1, 10, -1);

        act.Should().Throw<ExcepcionDominio>()
           .WithMessage("*stock minimo*");
    }

    // =============================
    // RESERVA
    // =============================

    [Fact]
    public void ReservarStock_Valido_DeberiaMoverDisponibleAReservado()
    {
        var stock = new Stock(1, 10, 1);

        stock.Reservar(3);

        stock.CantidadDisponible.Should().Be(7);
        stock.CantidadReservada.Should().Be(3);
    }

    [Fact]
    public void Reservar_MayorADisponible_DeberiaFallar()
    {
        var stock = new Stock(1, 5, 1);

        Action act = () => stock.Reservar(6);

        act.Should().Throw<ExcepcionDominio>()
           .WithMessage("*stock suficiente*");
    }

    [Fact]
    public void Reservar_CantidadCero_DeberiaFallar()
    {
        var stock = new Stock(1, 5, 1);

        Action act = () => stock.Reservar(0);

        act.Should().Throw<ExcepcionDominio>()
           .WithMessage("*mayor a 0*");
    }

    // =============================
    // CONFIRMAR RESERVA
    // =============================

    [Fact]
    public void ConfirmarReserva_Valida_DeberiaReducirReservado()
    {
        var stock = new Stock(1, 10, 1);
        stock.Reservar(4);

        stock.ConfirmarReserva(4);

        stock.CantidadReservada.Should().Be(0);
        stock.CantidadDisponible.Should().Be(6);
    }

    [Fact]
    public void ConfirmarReserva_MayorAReservado_DeberiaFallar()
    {
        var stock = new Stock(1, 10, 1);
        stock.Reservar(3);

        Action act = () => stock.ConfirmarReserva(4);

        act.Should().Throw<ExcepcionDominio>()
           .WithMessage("*reserva invalida*");
    }

    // =============================
    // LIBERAR RESERVA
    // =============================

    [Fact]
    public void LiberarReserva_Valida_DeberiaRestaurarDisponible()
    {
        var stock = new Stock(1, 10, 1);
        stock.Reservar(5);

        stock.LiberarReserva(2);

        stock.CantidadDisponible.Should().Be(7);
        stock.CantidadReservada.Should().Be(3);
    }

    [Fact]
    public void LiberarReserva_MayorAReservado_DeberiaFallar()
    {
        var stock = new Stock(1, 10, 1);
        stock.Reservar(2);

        Action act = () => stock.LiberarReserva(3);

        act.Should().Throw<ExcepcionDominio>()
           .WithMessage("*reserva invalida*");
    }

    // =============================
    // DESCONTAR DIRECTO
    // =============================

    [Fact]
    public void DescontarStock_Valido_DeberiaReducirDisponible()
    {
        var stock = new Stock(1, 10, 1);

        stock.Descontar(4);

        stock.CantidadDisponible.Should().Be(6);
    }

    [Fact]
    public void Descontar_MayorADisponible_DeberiaFallar()
    {
        var stock = new Stock(1, 3, 1);

        Action act = () => stock.Descontar(4);

        act.Should().Throw<ExcepcionDominio>()
           .WithMessage("*Stock insuficiente*");
    }

    // =============================
    // AUMENTAR
    // =============================

    [Fact]
    public void AumentarStock_Valido_DeberiaSumarDisponible()
    {
        var stock = new Stock(1, 5, 1);

        stock.Aumentar(3);

        stock.CantidadDisponible.Should().Be(8);
    }

    [Fact]
    public void Aumentar_CantidadNegativa_DeberiaFallar()
    {
        var stock = new Stock(1, 5, 1);

        Action act = () => stock.Aumentar(-1);

        act.Should().Throw<ExcepcionDominio>();
    }

    // =============================
    // BAJO STOCK
    // =============================

    [Fact]
    public void BajoStock_CuandoDisponibleMenorIgualMinimo_DeberiaSerTrue()
    {
        var stock = new Stock(1, 5, 5);

        stock.BajoStock().Should().BeTrue();
    }

    [Fact]
    public void BajoStock_CuandoDisponibleMayorMinimo_DeberiaSerFalse()
    {
        var stock = new Stock(1, 10, 3);

        stock.BajoStock().Should().BeFalse();
    }

    // =============================
    // ESTADO
    // =============================

    [Fact]
    public void DesactivarStock_DeberiaCambiarEstado()
    {
        var stock = new Stock(1, 10, 1);

        stock.Desactivar();

        stock.Activo.Should().BeFalse();
    }
}
