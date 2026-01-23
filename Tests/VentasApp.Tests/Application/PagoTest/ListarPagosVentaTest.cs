using FluentAssertions;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using VentasApp.Application.CasoDeUso.Pago;
using VentasApp.Application.Interfaces.Repositorios;
using VentasApp.Domain.Modelo.Pago;
using Xunit;

namespace VentasApp.Tests.Application.PagoTest;

public class ListarPagosVentaTest
{
    [Fact]
    public async Task EjecutarAsync_WhenNoPayments_ReturnsEmpty()
    {
        var repo = new Mock<IPagoRepository>();
        repo.Setup(r => r.ObtenerPorVenta(It.IsAny<int>())).ReturnsAsync(new List<Pago>());

        var useCase = new ListarPagosVentaUseCase(repo.Object);

        var result = await useCase.EjecutarAsync(123);

        result.Should().NotBeNull();
        result.Should().BeEmpty();
        repo.Verify(r => r.ObtenerPorVenta(123), Times.Once);
    }

    [Fact]
    public async Task EjecutarAsync_MapsPaymentWithoutMethods_Correctly()
    {
        var pago = new Pago(10, false);

        var repo = new Mock<IPagoRepository>();
        repo.Setup(r => r.ObtenerPorVenta(10)).ReturnsAsync(new List<Pago> { pago });

        var useCase = new ListarPagosVentaUseCase(repo.Object);
        var result = await useCase.EjecutarAsync(10);

        result.Should().HaveCount(1);
        var dto = result.First();
        dto.IdVenta.Should().Be(10);
        dto.Total.Should().Be(0m);
        dto.Metodos.Should().BeEmpty();
        dto.FechaPago.Should().BeCloseTo(pago.FechaPago, TimeSpan.FromSeconds(5));
    }

    [Fact]
    public async Task EjecutarAsync_MapsPaymentWithMethods_Correctly()
    {
        var pago = new Pago(42, false);

        // 1. USAR EL MÉTODO DEL DOMINIO (Calcula Total = 200)
        pago.AgregarPago(5, 200m);

        // 2. SIMULAR LA PROPIEDAD DE NAVEGACIÓN (Para que el DTO pueda leer el Nombre)
        var metodo = pago.Metodos.First();
        var medio = new MedioPago("Tarjeta Débito", recargo: true);

        // Seteamos la propiedad 'MedioPago' usando Reflection (porque tiene private set)
        typeof(PagoMetodo).GetProperty("MedioPago")!.SetValue(metodo, medio);

        var repo = new Mock<IPagoRepository>();
        repo.Setup(r => r.ObtenerPorVenta(42)).ReturnsAsync(new List<Pago> { pago });

        var useCase = new ListarPagosVentaUseCase(repo.Object);
        var result = await useCase.EjecutarAsync(42);

        result.Should().HaveCount(1);
        var dto = result.First();
        dto.IdVenta.Should().Be(42);
        dto.Total.Should().Be(200m); // Ahora sí da 200
        dto.Metodos.Should().HaveCount(1);
        dto.Metodos.First().MedioPago.Should().Be("Tarjeta Débito");
        dto.Metodos.First().Monto.Should().Be(200m);
    }

    [Fact]
    public async Task EjecutarAsync_MapsMultiplePaymentsAndMethods_Correctly()
    {
        // --- PAGO 1 ---
        var pago1 = new Pago(1, false);
        pago1.AgregarPago(2, 75m); // Total se actualiza a 75

        var medio1 = new MedioPago("Efectivo", recargo: false);
        // Setear navegación
        typeof(PagoMetodo).GetProperty("MedioPago")!.SetValue(pago1.Metodos.First(), medio1);


        // --- PAGO 2 ---
        var pago2 = new Pago(1, false);
        pago2.AgregarPago(3, 25m); // Total se actualiza a 25

        var medio2 = new MedioPago("Transferencia", recargo: false);
        // Setear navegación
        typeof(PagoMetodo).GetProperty("MedioPago")!.SetValue(pago2.Metodos.First(), medio2);


        var repo = new Mock<IPagoRepository>();
        repo.Setup(r => r.ObtenerPorVenta(1)).ReturnsAsync(new List<Pago> { pago1, pago2 });

        var useCase = new ListarPagosVentaUseCase(repo.Object);
        var result = await useCase.EjecutarAsync(1);

        result.Should().HaveCount(2);
        // 75 + 25 = 100. Ahora sí coincidirá.
        result.Sum(r => r.Total).Should().Be(100m);

        result.SelectMany(r => r.Metodos).Should().Contain(m => m.MedioPago == "Efectivo" && m.Monto == 75m);
        result.SelectMany(r => r.Metodos).Should().Contain(m => m.MedioPago == "Transferencia" && m.Monto == 25m);
    }
}