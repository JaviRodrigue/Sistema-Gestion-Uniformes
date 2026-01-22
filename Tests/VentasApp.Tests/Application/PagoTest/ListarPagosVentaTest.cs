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

        // add a PagoMetodo and set its MedioPago via reflection
        var metodosField = typeof(Pago).GetField("_metodos", BindingFlags.NonPublic | BindingFlags.Instance)!;
        var metodosList = (List<PagoMetodo>)metodosField.GetValue(pago)!;

        var metodo = new PagoMetodo(5, 200m);
        var medio = new MedioPago("Tarjeta Débito", recargo: true);

        var medioPagoBacking = typeof(PagoMetodo).GetField("<MedioPago>k__BackingField", BindingFlags.NonPublic | BindingFlags.Instance)!;
        medioPagoBacking.SetValue(metodo, medio);

        metodosList.Add(metodo);

        var repo = new Mock<IPagoRepository>();
        repo.Setup(r => r.ObtenerPorVenta(42)).ReturnsAsync(new List<Pago> { pago });

        var useCase = new ListarPagosVentaUseCase(repo.Object);
        var result = await useCase.EjecutarAsync(42);

        result.Should().HaveCount(1);
        var dto = result.First();
        dto.IdVenta.Should().Be(42);
        dto.Total.Should().Be(200m);
        dto.Metodos.Should().HaveCount(1);
        dto.Metodos.First().MedioPago.Should().Be("Tarjeta Débito");
        dto.Metodos.First().Monto.Should().Be(200m);
    }

    [Fact]
    public async Task EjecutarAsync_MapsMultiplePaymentsAndMethods_Correctly()
    {
        var pago1 = new Pago(1, false);
        var pago2 = new Pago(1, false);

        var metodosField = typeof(Pago).GetField("_metodos", BindingFlags.NonPublic | BindingFlags.Instance)!;
        var list1 = (List<PagoMetodo>)metodosField.GetValue(pago1)!;
        var list2 = (List<PagoMetodo>)metodosField.GetValue(pago2)!;

        var metodo1 = new PagoMetodo(2, 75m);
        var medio1 = new MedioPago("Efectivo", recargo: false);
        var metodo2 = new PagoMetodo(3, 25m);
        var medio2 = new MedioPago("Transferencia", recargo: false);

        var medioPagoBacking = typeof(PagoMetodo).GetField("<MedioPago>k__BackingField", BindingFlags.NonPublic | BindingFlags.Instance)!;
        medioPagoBacking.SetValue(metodo1, medio1);
        medioPagoBacking.SetValue(metodo2, medio2);

        list1.Add(metodo1);
        list2.Add(metodo2);

        var repo = new Mock<IPagoRepository>();
        repo.Setup(r => r.ObtenerPorVenta(1)).ReturnsAsync(new List<Pago> { pago1, pago2 });

        var useCase = new ListarPagosVentaUseCase(repo.Object);
        var result = await useCase.EjecutarAsync(1);

        result.Should().HaveCount(2);
        result.Sum(r => r.Total).Should().Be(100m);
        result.SelectMany(r => r.Metodos).Should().Contain(m => m.MedioPago == "Efectivo" && m.Monto == 75m);
        result.SelectMany(r => r.Metodos).Should().Contain(m => m.MedioPago == "Transferencia" && m.Monto == 25m);
    }
}
