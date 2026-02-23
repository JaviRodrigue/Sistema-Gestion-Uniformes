using FluentAssertions;
using System;
using VentasApp.Domain.Base;
using VentasApp.Domain.Modelo.Pago;
using Xunit;

namespace VentasApp.Tests.Domain.PagoTest;

public class PagoTest
{
    [Fact]
    public void Constructor_DeberiaInicializarCorrectamente()
    {
        // Arrange
        var idVenta = 10;
        var esSenia = true;

        // Act
        var pago = new Pago(idVenta, esSenia);

        // Assert
        pago.IdVenta.Should().Be(idVenta);
        pago.EsSenia.Should().BeTrue();
        pago.FechaPago.Should().BeCloseTo(DateTime.Now, TimeSpan.FromSeconds(2));
        pago.Total.Should().Be(0);
        pago.Metodos.Should().BeEmpty();
    }

    [Fact]
    public void AgregarPago_DeberiaAgregarMetodoCuandoMontoEsValido()
    {
        // Arrange
        var pago = new Pago(1, false);

        // Act
        pago.AgregarPago(1, 100);

        // Assert
        pago.Metodos.Should().HaveCount(1);
        pago.Total.Should().Be(100);
    }

    [Theory]
    [InlineData(0)]
    [InlineData(-1)]
    [InlineData(-100)]
    public void AgregarPago_DeberiaLanzarExcepcionCuandoMontoEsMenorOIgualACero(decimal monto)
    {
        // Arrange
        var pago = new Pago(1, false);

        // Act
        Action act = () => pago.AgregarPago(1, monto);

        // Assert
        act.Should()
            .Throw<ExcepcionDominio>()
            .WithMessage("El monto debe ser mayor a cero");
    }

    [Fact]
    public void Total_DeberiaSerLaSumaDeTodosLosMetodos()
    {
        // Arrange
        var pago = new Pago(1, false);

        // Act
        pago.AgregarPago(1, 100);
        pago.AgregarPago(2, 50);

        // Assert
        pago.Total.Should().Be(150);
    }

    [Fact]
    public void ValidarPago_DeberiaLanzarExcepcionSiNoTieneMetodos()
    {
        // Arrange
        var pago = new Pago(1, false);

        // Act
        Action act = () => pago.ValidarPago();

        // Assert
        act.Should()
            .Throw<ExcepcionDominio>()
            .WithMessage("El pago debe tener por lo menos un metodo");
    }

    [Fact]
    public void ValidarPago_DeberiaLanzarExcepcionSiTotalEsCero()
    {
        // Arrange
        var pago = new Pago(1, false);

        // Manipulación directa del estado para simular edge case
        // (solo para test de dominio)
        var metodosField = typeof(Pago)
            .GetField("_metodos", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);

        metodosField!.SetValue(pago, new List<PagoMetodo>());

        // Act
        Action act = () => pago.ValidarPago();

        // Assert
        act.Should().Throw<ExcepcionDominio>();
    }

    [Fact]
    public void ValidarPago_NoDeberiaLanzarExcepcionCuandoPagoEsValido()
    {
        // Arrange
        var pago = new Pago(1, false);
        pago.AgregarPago(1, 200);

        // Act
        Action act = () => pago.ValidarPago();

        // Assert
        act.Should().NotThrow();
    }

    [Fact]
    public void Metodos_NoDeberiaPermitirModificacionExterna()
    {
        // Arrange
        var pago = new Pago(1, false);
        pago.AgregarPago(1, 100);

        // Act
        Action act = () =>
        {
            var metodos = (System.Collections.Generic.IList<PagoMetodo>)pago.Metodos;
            metodos!.Add(new PagoMetodo(2, 50));
        };

        // Assert
        act.Should().Throw<NotSupportedException>();
    }
}
