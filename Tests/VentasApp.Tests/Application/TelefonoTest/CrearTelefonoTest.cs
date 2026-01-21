using System;
using System.Threading.Tasks;
using FluentAssertions;
using Moq;
using VentasApp.Application.CasoDeUso.Telefono;
using VentasApp.Application.DTOs.Cliente;
using VentasApp.Application.Interfaces.Repositorios;
using Xunit;

namespace VentasApp.Tests.Application.TelefonoTest;

public class CrearTelefonoTest
{
    [Fact]
    public async Task CrearTelefono_DeberiaAgregarTelefonoYGuardar()
    {
        // Arrange
        var repoMock = new Mock<ITelefonoRepository>();
        var unitMock = new Mock<IUnitOfWork>();

        repoMock.Setup(r => r.AgregarTelefonos(1, It.IsAny<System.Collections.Generic.List<string>>())).Returns(Task.CompletedTask);
        unitMock.Setup(u => u.SaveChanges()).ReturnsAsync(1);

        var useCase = new CrearTelefonoCasoDeUso(repoMock.Object, unitMock.Object);
        var dto = new AgregarTelefonoClienteDTO { IdCliente = 1, Numero = "1234567890" };

        // Act
        await useCase.EjecutarAsync(dto);

        // Assert
        repoMock.Verify(r => r.AgregarTelefonos(1, It.Is<System.Collections.Generic.List<string>>(l => l.Contains("1234567890"))), Times.Once);
        unitMock.Verify(u => u.SaveChanges(), Times.Once);
    }

    [Fact]
    public async Task CrearTelefono_NumeroVacio_DeberiaLanzar()
    {
        var repoMock = new Mock<ITelefonoRepository>();
        var unitMock = new Mock<IUnitOfWork>();
        var useCase = new CrearTelefonoCasoDeUso(repoMock.Object, unitMock.Object);
        var dto = new AgregarTelefonoClienteDTO { IdCliente = 1, Numero = " " };

        await Assert.ThrowsAsync<ArgumentException>(async () => await useCase.EjecutarAsync(dto));
    }
}
