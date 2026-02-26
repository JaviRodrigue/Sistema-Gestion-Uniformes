using System;
using System.Threading.Tasks;
using FluentAssertions;
using Moq;
using VentasApp.Application.CasoDeUso.Telefono;
using VentasApp.Application.Interfaces.Repositorios;
using Xunit;

namespace VentasApp.Tests.Application.TelefonoTest;

public class EliminarTelefonoTest
{
    [Fact]
    public async Task EliminarTelefono_DeberiaDesactivarYGuardar()
    {
        var repoMock = new Mock<ITelefonoRepository>();
        var unitMock = new Mock<IUnitOfWork>();

        repoMock.Setup(r => r.ObtenerTelefonoPorId(1)).ReturnsAsync("123");
        repoMock.Setup(r => r.DesactivarTelefono(1)).Returns(Task.CompletedTask);
        unitMock.Setup(u => u.SaveChanges()).ReturnsAsync(1);

        var useCase = new EliminarTelefonoCasoDeUso(repoMock.Object, unitMock.Object);

        await useCase.EjecutarAsync(1);

        repoMock.Verify(r => r.DesactivarTelefono(1), Times.Once);
        unitMock.Verify(u => u.SaveChanges(), Times.Once);
    }

    [Fact]
    public async Task EliminarTelefono_NoExiste_DeberiaLanzar()
    {
        var repoMock = new Mock<ITelefonoRepository>();
        var unitMock = new Mock<IUnitOfWork>();
        repoMock.Setup(r => r.ObtenerTelefonoPorId(2)).ReturnsAsync((string?)null);

        var useCase = new EliminarTelefonoCasoDeUso(repoMock.Object, unitMock.Object);

        await Assert.ThrowsAsync<Exception>(async () => await useCase.EjecutarAsync(2));
    }
}
