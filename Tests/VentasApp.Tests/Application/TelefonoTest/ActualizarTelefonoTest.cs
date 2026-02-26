using System;
using System.Threading.Tasks;
using FluentAssertions;
using Moq;
using VentasApp.Application.CasoDeUso.Telefono;
using VentasApp.Application.Interfaces.Repositorios;
using Xunit;

namespace VentasApp.Tests.Application.TelefonoTest;

public class ActualizarTelefonoTest
{
    [Fact]
    public async Task ActualizarTelefono_DeberiaActualizarYGuardar()
    {
        var repoMock = new Mock<ITelefonoRepository>();
        var unitMock = new Mock<IUnitOfWork>();

        repoMock.Setup(r => r.ObtenerTelefonoPorId(1)).ReturnsAsync("123");
        repoMock.Setup(r => r.ActualizarTelefono(1, "999")).Returns(Task.CompletedTask);
        unitMock.Setup(u => u.SaveChanges()).ReturnsAsync(1);

        var useCase = new ActualizarTelefonoCasoDeUso(repoMock.Object, unitMock.Object);

        await useCase.EjecutarAsync(1, "999");

        repoMock.Verify(r => r.ActualizarTelefono(1, "999"), Times.Once);
        unitMock.Verify(u => u.SaveChanges(), Times.Once);
    }

    [Fact]
    public async Task ActualizarTelefono_NoExiste_DeberiaLanzar()
    {
        var repoMock = new Mock<ITelefonoRepository>();
        var unitMock = new Mock<IUnitOfWork>();
        repoMock.Setup(r => r.ObtenerTelefonoPorId(2)).ReturnsAsync((string?)null);

        var useCase = new ActualizarTelefonoCasoDeUso(repoMock.Object, unitMock.Object);

        await Assert.ThrowsAsync<Exception>(async () => await useCase.EjecutarAsync(2, "000"));
    }
}
