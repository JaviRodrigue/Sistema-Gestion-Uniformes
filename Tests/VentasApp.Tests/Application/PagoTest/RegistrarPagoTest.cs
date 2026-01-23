using FluentAssertions;
using Moq;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Threading.Tasks;
using VentasApp.Application.CasoDeUso.Pago;
using VentasApp.Application.DTOs.Pago;
using VentasApp.Application.Interfaces.Repositorios;
using VentasApp.Domain.Modelo.Pago;
using VentasApp.Domain.Modelo.Venta;
using VentasApp.Domain.Enum;
using Xunit;

namespace VentasApp.Tests.Application.PagoTest;

public class RegistrarPagoTest
{
    [Fact]
    public async Task EjecutarAsync_WhenVentaNotFound_Throws()
    {
        var repoPago = new Mock<IPagoRepository>();
        var repoVenta = new Mock<IVentaRepository>();
        var repoMedio = new Mock<IMedioPagoRepository>();
        var unit = new Mock<IUnitOfWork>();

        repoVenta.Setup(r => r.ObtenerPorId(It.IsAny<int>())).ReturnsAsync((Venta?)null);

        var useCase = new RegistrarPagoUseCase(repoPago.Object, repoVenta.Object, repoMedio.Object, unit.Object);

        var dto = new CrearPagoDto { IdVenta = 1, EsSenia = false, Metodos = new List<PagoMetodo>() };

        await useCase.Invoking(u => u.EjecutarAsync(dto)).Should().ThrowAsync<Exception>().WithMessage("No se encontro la venta");
    }

    [Fact]
    public async Task EjecutarAsync_WhenVentaCancelled_Throws()
    {
        var repoPago = new Mock<IPagoRepository>();
        var repoVenta = new Mock<IVentaRepository>();
        var repoMedio = new Mock<IMedioPagoRepository>();
        var unit = new Mock<IUnitOfWork>();

        var venta = new Venta(TipoVenta.Presencial);
        venta.AnularVenta();

        repoVenta.Setup(r => r.ObtenerPorId(It.IsAny<int>())).ReturnsAsync(venta);

        var useCase = new RegistrarPagoUseCase(repoPago.Object, repoVenta.Object, repoMedio.Object, unit.Object);

        var dto = new CrearPagoDto { IdVenta = 1, EsSenia = false, Metodos = new List<PagoMetodo>() };

        await useCase.Invoking(u => u.EjecutarAsync(dto)).Should().ThrowAsync<Exception>().WithMessage("No se puede agregar un pago a una venta cancelada");
    }

    [Fact]
    public async Task EjecutarAsync_WhenMedioPagoInvalid_Throws()
    {
        var repoPago = new Mock<IPagoRepository>();
        var repoVenta = new Mock<IVentaRepository>();
        var repoMedio = new Mock<IMedioPagoRepository>();
        var unit = new Mock<IUnitOfWork>();

        var venta = new Venta(TipoVenta.Presencial);
        venta.AgregarDetalle(1, 1, 100m);

        repoVenta.Setup(r => r.ObtenerPorId(It.IsAny<int>())).ReturnsAsync(venta);
        repoPago.Setup(r => r.ObtenerPorVenta(It.IsAny<int>())).ReturnsAsync(new List<Pago>());
        repoMedio.Setup(r => r.ObtenerPorId(It.IsAny<int>())).ReturnsAsync((MedioPago?)null);

        var useCase = new RegistrarPagoUseCase(repoPago.Object, repoVenta.Object, repoMedio.Object, unit.Object);

        var metodo = new PagoMetodo(5, 50m);
        var dto = new CrearPagoDto { IdVenta = venta.Id, EsSenia = false, Metodos = new List<PagoMetodo> { metodo } };

        await useCase.Invoking(u => u.EjecutarAsync(dto)).Should().ThrowAsync<Exception>().WithMessage("Medio de pago invalido");
    }

    [Fact]
    public async Task EjecutarAsync_WhenPaymentExceedsTotal_Throws()
    {
        var repoPago = new Mock<IPagoRepository>();
        var repoVenta = new Mock<IVentaRepository>();
        var repoMedio = new Mock<IMedioPagoRepository>();
        var unit = new Mock<IUnitOfWork>();

        // 1. Crear Venta de 100
        var venta = new Venta(TipoVenta.Presencial);
        venta.AgregarDetalle(1, 1, 100m); // MontoTotal = 100

        // 2. Crear Pago Previo de 80 USANDO AGREGARPAGO (NO REFLECTION)
        // Esto asegura que pagoPrevio.Total sea 80
        var pagoPrevio = new Pago(venta.Id, false);
        pagoPrevio.AgregarPago(1, 80m);

        repoVenta.Setup(r => r.ObtenerPorId(venta.Id)).ReturnsAsync(venta);
        repoPago.Setup(r => r.ObtenerPorVenta(venta.Id)).ReturnsAsync(new List<Pago> { pagoPrevio });

        // 3. Mockear el MedioPago para el NUEVO pago que vamos a intentar hacer
        var medio = new MedioPago("Tarjeta", true);

        // Usamos reflection SOLO para setear el ID del MedioPago simulado (porque Id tiene private set)
        var propId = medio.GetType().GetProperty("Id", BindingFlags.Public | BindingFlags.Instance | BindingFlags.NonPublic);
        propId!.SetValue(medio, 2);

        repoMedio.Setup(r => r.ObtenerPorId(2)).ReturnsAsync(medio);

        var useCase = new RegistrarPagoUseCase(repoPago.Object, repoVenta.Object, repoMedio.Object, unit.Object);

        // 4. Intentar pagar 30 más.
        // Previo (80) + Nuevo (30) = 110. 
        // 110 > 100 (Total Venta) -> EXCEPCIÓN
        var nuevoMetodo = new PagoMetodo(2, 30m);
        var dto = new CrearPagoDto { IdVenta = venta.Id, EsSenia = false, Metodos = new List<PagoMetodo> { nuevoMetodo } };

        await useCase.Invoking(u => u.EjecutarAsync(dto))
                     .Should().ThrowAsync<Exception>()
                     .WithMessage("El pago supera el monto total de la venta");
    }

    [Fact]
    public async Task EjecutarAsync_WhenValid_AddsPagoAndSaves()
    {
        var repoPago = new Mock<IPagoRepository>();
        var repoVenta = new Mock<IVentaRepository>();
        var repoMedio = new Mock<IMedioPagoRepository>();
        var unit = new Mock<IUnitOfWork>();

        var venta = new Venta(TipoVenta.Presencial);
        venta.AgregarDetalle(1, 1, 200m);

        repoVenta.Setup(r => r.ObtenerPorId(venta.Id)).ReturnsAsync(venta);
        repoPago.Setup(r => r.ObtenerPorVenta(venta.Id)).ReturnsAsync(new List<Pago>());

        var medio = new MedioPago("Tarjeta", true);
        medio.GetType().GetProperty("Id", BindingFlags.Public | BindingFlags.Instance | BindingFlags.NonPublic)!.SetValue(medio, 5);
        repoMedio.Setup(r => r.ObtenerPorId(5)).ReturnsAsync(medio);

        repoPago.Setup(r => r.Agregar(It.IsAny<Pago>())).Returns(Task.CompletedTask).Verifiable();
        unit.Setup(u => u.SaveChanges()).ReturnsAsync(1).Verifiable();

        var useCase = new RegistrarPagoUseCase(repoPago.Object, repoVenta.Object, repoMedio.Object, unit.Object);

        var metodo = new PagoMetodo(5, 150m);
        var dto = new CrearPagoDto { IdVenta = venta.Id, EsSenia = false, Metodos = new List<PagoMetodo> { metodo } };

        await useCase.EjecutarAsync(dto);

        repoPago.Verify(r => r.Agregar(It.IsAny<Pago>()), Times.Once);
        unit.Verify(u => u.SaveChanges(), Times.Once);
        venta.MontoPagado.Should().Be(150m);
    }
}
