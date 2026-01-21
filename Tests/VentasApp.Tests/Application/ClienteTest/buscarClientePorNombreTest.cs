using FluentAssertions;
using Moq;
using System;
using System.Reflection;
using System.Threading.Tasks;
using VentasApp.Application.CasoDeUso.Cliente;
using VentasApp.Application.DTOs.Cliente;
using VentasApp.Application.Interfaces.Repositorios;
using VentasApp.Domain.Modelo.Cliente;
using Xunit;
using VentasApp.Domain.Base;

namespace VentasApp.Tests.Application.ClienteTest;

public class BuscarClientePorNombreTest
{
    [Fact]
    public async Task BuscarClientePorNombre_test()
    {
        // Arrange
        var repoMock = new Mock<IClienteRepository>();
        var cliente = new Cliente("Juan", "44048885");
        typeof(Entidad)
            .GetProperty("Id", BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public)!
            .SetValue(cliente, 1);

        repoMock.Setup(r => r.BuscarPorNombre("Juan"))
                .ReturnsAsync([cliente]);


        var buscarClientePorNombreCasoDeUso = new BuscarClientePorNombreCasoDeUso(repoMock.Object);

        // Act
        var resultado = await buscarClientePorNombreCasoDeUso.EjecutarAsync("Juan");

        // Assert
        resultado.Should().NotBeNull();
        resultado.Should().HaveCount(1);

        var dto = resultado.First();
        dto.Id.Should().Be(1);
        dto.Nombre.Should().Be("Juan");
        dto.Dni.Should().Be("44048885");
    }

    [Fact]
    public async Task BuscarClientePorNombre_NoEncontrado_DeberiaLanzar()
    {
        var repoMock = new Mock<IClienteRepository>();
        repoMock.Setup(r => r.BuscarPorNombre("x")).ReturnsAsync([]);

        var buscarClientePorNombreCasoDeUso = new BuscarClientePorNombreCasoDeUso(repoMock.Object);

        await Assert.ThrowsAsync<Exception>(async () => await buscarClientePorNombreCasoDeUso.EjecutarAsync("x"));
    }
}
