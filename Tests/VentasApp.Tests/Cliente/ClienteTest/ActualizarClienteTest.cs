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
using Xunit;

namespace VentasApp.Tests.Cliente.ClienteTest;

public class ActualizarClienteTest
{
    [Fact]
    public async Task ActualizarCliente_test()
    {
        // Arrange
        var repoMock = new Mock<IClienteRepository>();
        var unitMock = new Mock<IUnitOfWork>();

        var clienteExistente = new VentasApp.Domain.Modelo.Cliente.Cliente("Juan", "44048885");
        typeof(VentasApp.Domain.Base.Entidad)
            .GetProperty("Id", BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public)!
            .SetValue(clienteExistente, 1);

        var dtoActualizacion = new ActualizarClienteDto
        {
            Id = 1,
            Nombre = "pedro"
        };

        repoMock.Setup(r => r.ObtenerClientePorId(dtoActualizacion.Id))
                .ReturnsAsync(clienteExistente);
        repoMock.Setup(r => r.Actualizar(It.IsAny<VentasApp.Domain.Modelo.Cliente.Cliente>()))
                .Returns(Task.CompletedTask);
        unitMock.Setup(u => u.SaveChanges()).ReturnsAsync(1);

        var actualizarClienteCasoDeUso = new ActualizarClienteCasoDeUso(repoMock.Object, unitMock.Object);
        

        // Act
        await actualizarClienteCasoDeUso.EjecutarAsync(dtoActualizacion);

        //assert
        repoMock.Verify(r => r.Agregar(It.Is<Domain.Modelo.Cliente.Cliente>(c => c.Nombre == dtoActualizacion.Nombre && c.DNI == clienteExistente.DNI)), Times.Once);
        unitMock.Verify(u => u.SaveChanges(), Times.Once);


    }
}
