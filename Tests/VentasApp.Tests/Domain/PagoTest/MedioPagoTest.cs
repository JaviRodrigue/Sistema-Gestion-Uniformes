using FluentAssertions;
using System;
using VentasApp.Domain.Base;
using VentasApp.Domain.Modelo.Pago;
using Xunit;

namespace VentasApp.Tests.Domain.PagoTest;

public class MedioPagoTest
{
    [Fact]
    public void Constructor_WithValidParameters_ShouldSetProperties()
    {
        // Arrange
        var nombre = "Efectivo";

        // Act
        var medio = new MedioPago(nombre, recargo: false);

        // Assert
        medio.Nombre.Should().Be(nombre);
        medio.TieneRecargo.Should().BeFalse();
        medio.Activo.Should().BeTrue();
    }

    [Fact]
    public void Constructor_ShouldTrimNombre()
    {
        // Arrange
        var nombre = "  Transferencia  ";

        // Act
        var medio = new MedioPago(nombre, true);

        // Assert
        medio.Nombre.Should().Be("Transferencia");
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("   ")]
    public void CambiarNombre_Invalid_ShouldThrowExcepcionDominio(string? nombre)
    {
        // Arrange
        var medio = new MedioPago("Inicial", true);

        // Act
        Action act = () => medio.CambiarNombre(nombre!);

        // Assert
        act.Should()
            .Throw<ExcepcionDominio>()
            .WithMessage("El nombre del Medio de Pago es obligatorio");
    }

    [Fact]
    public void CambiarNombre_Valid_ShouldUpdateNombre()
    {
        // Arrange
        var medio = new MedioPago("Inicial", false);

        // Act
        medio.CambiarNombre("Tarjeta");

        // Assert
        medio.Nombre.Should().Be("Tarjeta");
    }

    [Fact]
    public void Desactivar_ShouldSetActivoFalse()
    {
        // Arrange
        var medio = new MedioPago("Efectivo", false);

        // Act
        medio.Desactivar();

        // Assert
        medio.Activo.Should().BeFalse();
    }

    [Fact]
    public void Desactivar_WhenAlreadyInactive_ShouldRemainInactive()
    {
        // Arrange
        var medio = new MedioPago("Efectivo", false);
        medio.Desactivar();

        // Act
        medio.Desactivar();

        // Assert
        medio.Activo.Should().BeFalse();
    }
}
