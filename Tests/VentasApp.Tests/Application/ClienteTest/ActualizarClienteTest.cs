using FluentAssertions;
using Moq;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Threading.Tasks;
using VentasApp.Application.CasoDeUso.Cliente;
using VentasApp.Application.DTOs.Cliente;
using VentasApp.Application.Interfaces.Repositorios;
using VentasApp.Domain.Modelo.Cliente;
using VentasApp.Domain.Base;
using System.Linq;
using Xunit;

namespace VentasApp.Tests.Application.ClienteTest;

public class ActualizarClienteTest
{
    [Fact]
    public async Task ActualizarCliente_DeberiaActualizarYGuardar()
    {
        // Arrange
        var repoMock = new Mock<IClienteRepository>();
        var unitMock = new Mock<IUnitOfWork>();

        var clienteExistente = new Cliente("Juan", "44048885");
        typeof(Entidad)
            .GetProperty("Id", BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public)!
            .SetValue(clienteExistente, 1);

        var dtoActualizacion = new ActualizarClienteDto
        {
            Id = 1,
            Nombre = "pedro"
        };

        repoMock.Setup(r => r.ObtenerClientePorId(dtoActualizacion.Id))
                .ReturnsAsync(clienteExistente);
        repoMock.Setup(r => r.Actualizar(It.IsAny<Cliente>()))
                .Returns(Task.CompletedTask);
        unitMock.Setup(u => u.SaveChanges()).ReturnsAsync(1);

        var actualizarClienteCasoDeUso = new ActualizarClienteCasoDeUso(repoMock.Object, unitMock.Object);
        

        // Act
        await actualizarClienteCasoDeUso.EjecutarAsync(dtoActualizacion);

        //assert: debe haberse llamado a Actualizar
        repoMock.Verify(r => r.Actualizar(It.Is<Cliente>(c => c.Nombre == dtoActualizacion.Nombre && c.DNI == clienteExistente.DNI)), Times.Once);
        unitMock.Verify(u => u.SaveChanges(), Times.Once);


    }

    [Fact]
    public async Task ActualizarCliente_NoExiste_DebeLanzar()
    {
        // Arrange
        var repoMock = new Mock<IClienteRepository>();
        var unitMock = new Mock<IUnitOfWork>();

        var dtoActualizacion = new ActualizarClienteDto
        {
            Id = 99,
            Nombre = "pedro"
        };

        repoMock.Setup(r => r.ObtenerClientePorId(dtoActualizacion.Id)).ReturnsAsync((Cliente?)null);

        var actualizarClienteCasoDeUso = new ActualizarClienteCasoDeUso(repoMock.Object, unitMock.Object);

        // Act & Assert
        await Assert.ThrowsAsync<Exception>(async () => await actualizarClienteCasoDeUso.EjecutarAsync(dtoActualizacion));
    }

    [Fact]
    public async Task ActualizarCliente_CambiarDniYTelefonos_DebeActualizarCampos()
    {
        // Arrange
        var repoMock = new Mock<IClienteRepository>();
        var unitMock = new Mock<IUnitOfWork>();

        var clienteExistente = new Cliente("Juan", "44048885");
        typeof(Entidad)
            .GetProperty("Id", BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public)!
            .SetValue(clienteExistente, 1);

        var dtoActualizacion = new ActualizarClienteDto
        {
            Id = 1,
            Nombre = "Pedro",
            Dni = "99999999",
            Telefonos = ["555", "666"]
        };

        repoMock.Setup(r => r.ObtenerClientePorId(dtoActualizacion.Id)).ReturnsAsync(clienteExistente);
        repoMock.Setup(r => r.Actualizar(It.IsAny<Cliente>())).Returns(Task.CompletedTask);
        unitMock.Setup(u => u.SaveChanges()).ReturnsAsync(1);

        var actualizarClienteCasoDeUso = new ActualizarClienteCasoDeUso(repoMock.Object, unitMock.Object);

        // Act
        await actualizarClienteCasoDeUso.EjecutarAsync(dtoActualizacion);

        // Assert: verificar que se llamó a Actualizar con los cambios aplicados
        repoMock.Verify(r => r.Actualizar(It.Is<Cliente>(c => c.Nombre == dtoActualizacion.Nombre && c.DNI == dtoActualizacion.Dni && c.Telefonos.Any(t => t.Numero == "555") && c.Telefonos.Any(t => t.Numero == "666"))), Times.Once);
        unitMock.Verify(u => u.SaveChanges(), Times.Once);
    }
}
