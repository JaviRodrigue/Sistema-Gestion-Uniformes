using FluentAssertions;
using Moq;
using System;
using System.Reflection;
using System.Threading.Tasks;
using VentasApp.Application.CasoDeUso.Cliente;
using VentasApp.Application.DTOs.Cliente;
using VentasApp.Application.Interfaces.Repositorios;
using Xunit;

namespace VentasApp.Tests.Cliente.ClienteTest;

public class ObtenerClientePorIdTest
{
    [Fact]
    public async Task ObtenerTodosClientes_test()
    {
        // Arrange
        var repoMock = new Mock<IClienteRepository>();
        var cliente = new VentasApp.Domain.Modelo.Cliente.Cliente("Juan", "44048885");
        typeof(VentasApp.Domain.Base.Entidad)
            .GetProperty("Id", BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public)!
            .SetValue(cliente, 1);
        
        repoMock.Setup(r => r.ObtenerClientePorId(cliente.Id))
                .ReturnsAsync(cliente);
        var obtenerClientePorIdCasoDeUso = new ObtenerClientePorIdCasoDeUso(repoMock.Object);

        // Act
        var resultado = await obtenerClientePorIdCasoDeUso.EjecutarAsync(1);

        // Assert
        resultado.Nombre.Should().Be("Juan");
        resultado.Id.Should().Be(1);
    }
}
