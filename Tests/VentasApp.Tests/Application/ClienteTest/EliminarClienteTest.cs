using FluentAssertions;
using Moq;
using System;
using System.Reflection;
using System.Threading.Tasks;
using VentasApp.Application.CasoDeUso.Cliente;
using VentasApp.Application.DTOs.Cliente;
using VentasApp.Application.Interfaces.Repositorios;
using VentasApp.Domain.Modelo.Cliente;
using VentasApp.Domain.Base;
using Xunit;

namespace VentasApp.Tests.Application.ClienteTest;

public class EliminarClienteTest
{
    [Fact]
    public async Task EliminarCliente_test()
    {
        // Arrange
        var repoMock = new Mock<IClienteRepository>();
        var unitMock = new Mock<IUnitOfWork>();

        var clienteExistente = new Cliente("Juan", "44048885");
        typeof(Entidad)
            .GetProperty("Id", BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public)!
            .SetValue(clienteExistente, 1);
        repoMock.Setup(r => r.ObtenerClientePorId(1))
                .ReturnsAsync(clienteExistente);
        repoMock.Setup(r => r.Desactivar(1)).Returns(Task.CompletedTask);
        unitMock.Setup(u => u.SaveChanges()).ReturnsAsync(1);

        var eliminarClienteCasoUso = new EliminarClienteCasoDeUso(repoMock.Object, unitMock.Object);

        // Act
        await eliminarClienteCasoUso.EjecutarAsync(1);

        // Assert
        repoMock.Verify(r => r.Desactivar(clienteExistente.Id), Times.Once);
        unitMock.Verify(u => u.SaveChanges(), Times.Once);
    }

    [Fact]
    public async Task EliminarCliente_NoExiste_DeberiaLanzar()
    {
        var repoMock = new Mock<IClienteRepository>();
        var unitMock = new Mock<IUnitOfWork>();

        repoMock.Setup(r => r.ObtenerClientePorId(2)).ReturnsAsync((Cliente?)null);

        var eliminarClienteCasoUso = new EliminarClienteCasoDeUso(repoMock.Object, unitMock.Object);

        await Assert.ThrowsAsync<Exception>(async () => await eliminarClienteCasoUso.EjecutarAsync(2));
    }
}

