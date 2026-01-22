using FluentAssertions;
using System;
using VentasApp.Domain.Base;
using VentasApp.Domain.Modelo.Pago;
using Xunit;

namespace VentasApp.Tests.Domain.PagoTest;

public class PagoMetodoTest
{
    [Fact]
    public void Constructor_WithValidParameters_ShouldSetProperties()
    {
        // Arrange
        var idMedioPago = 3;
        var monto = 120.5m;

        // Act
        var pagoMetodo = new PagoMetodo(idMedioPago, monto);

        // Assert
        pagoMetodo.IdMedioPago.Should().Be(idMedioPago);
        pagoMetodo.Monto.Should().Be(monto);
        pagoMetodo.IdPago.Should().Be(0);     // asignado por ORM
        pagoMetodo.Pago.Should().BeNull();    // navegación ORM
    }

    [Theory]
    [InlineData(0)]
    [InlineData(-1)]
    public void Constructor_InvalidIdMedioPago_ShouldThrowExcepcionDominio(int idMedioPago)
    {
        // Act
        Action act = () => new PagoMetodo(idMedioPago, 10m);

        // Assert
        act.Should()
            .Throw<ExcepcionDominio>()
            .WithMessage("Medio de pago invalido");
    }

    [Theory]
    [InlineData(0)]
    [InlineData(-5)]
    public void Constructor_InvalidMonto_ShouldThrowExcepcionDominio(decimal monto)
    {
        // Act
        Action act = () => new PagoMetodo(1, monto);

        // Assert
        act.Should()
            .Throw<ExcepcionDominio>()
            .WithMessage("El monto debe ser mayor a 0");
    }

    [Fact]
    public void Constructor_InvalidMontoAndIdMedioPago_ShouldThrowMontoExceptionFirst()
    {
        // Act
        Action act = () => new PagoMetodo(0, 0);

        // Assert
        act.Should()
            .Throw<ExcepcionDominio>()
            .WithMessage("El monto debe ser mayor a 0");
    }

    [Fact]
    public void Constructor_ShouldAllowVeryLargeMonto()
    {
        // Arrange
        var montoGrande = decimal.MaxValue;

        // Act
        var pagoMetodo = new PagoMetodo(1, montoGrande);

        // Assert
        pagoMetodo.Monto.Should().Be(montoGrande);
    }
}