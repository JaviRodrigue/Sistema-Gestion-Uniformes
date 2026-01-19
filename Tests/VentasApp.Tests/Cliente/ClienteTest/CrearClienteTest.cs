using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using FluentAssertions;
using Moq;
using VentasApp.Domain.Modelo.Cliente;
using VentasApp.Application.CasoDeUso.Cliente;
using VentasApp.Application.DTOs.Cliente;
using VentasApp.Application.Interfaces.Repositorios;
using Xunit;

namespace VentasApp.Tests.Cliente.ClienteTest;

public class CrearClienteTest
{

    [Fact]
    public async Task CrearCliente_Test()
    {
        // Arrange
        var repoMock = new Mock<IClienteRepository>();
        var unitMock = new Mock<IUnitOfWork>();

        repoMock.Setup(r => r.Agregar(It.IsAny<Domain.Modelo.Cliente.Cliente>())).Returns(Task.CompletedTask);
        unitMock.Setup(u => u.SaveChanges()).ReturnsAsync(1);

        var crearClienteCasoDeUso = new CrearClienteCasoDeUso(repoMock.Object, unitMock.Object);
        var dto = new CrearClienteDto
        {
            Nombre = "Juan",
            Dni = "44048885",
            Telefonos = new List<string>{"1234567890"}
        };

        // Act
        await crearClienteCasoDeUso.EjecutarAsync(dto);

        //assert
        repoMock.Verify(r => r.Agregar(It.Is<Domain.Modelo.Cliente.Cliente>(c => c.Nombre == dto.Nombre && c.DNI == dto.Dni)), Times.Once);
        unitMock.Verify(u => u.SaveChanges(), Times.Once);
    }
}
