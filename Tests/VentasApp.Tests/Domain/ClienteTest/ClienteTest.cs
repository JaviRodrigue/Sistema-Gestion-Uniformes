using FluentAssertions;
using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;
using VentasApp.Domain.Modelo.Cliente;

namespace VentasApp.Tests.Domain.ClienteTest;

public class ClienteTest
{
    // =============================
    // CREACIÓN
    // =============================

    [Fact]
    public void CrearCliente_Valido_DeberiaInicializarCorrectamente()
    {
        // Arrange
        string nombre = "Juan Perez";
        string instagram = "@juanperez";

        // Act
        var cliente = new Cliente(nombre, instagram);

        // Assert
        cliente.Nombre.Should().Be(nombre);
        cliente.Instagram.Should().Be(instagram);
        cliente.Activado.Should().BeTrue();
        cliente.Telefonos.Should().BeEmpty();
        cliente.FechaAlta.Should().BeCloseTo(DateTime.Now, TimeSpan.FromSeconds(1));
    }

    [Fact]
    public void CrearCliente_ConTelefonos_DeberiaInicializarLista()
    {
        // Arrange
        var telefonos = new List<Telefono>(); // Asumimos que Telefono se puede instanciar o mockear si fuera necesario
                                              // Nota: Como Telefono depende del cliente en tu constructor interno, 
                                              // aquí probamos el constructor que acepta la lista, aunque la lista venga vacía o pre-llenada externamente.

        // Act
        var cliente = new Cliente("Juan", telefonos, "@juan");

        // Assert
        cliente.Telefonos.Should().NotBeNull();
        cliente.Telefonos.Should().BeSameAs(telefonos);
    }

    // =============================
    // CAMBIOS DE PROPIEDADES (Nombre y DNI)
    // =============================

    [Fact]
    public void CambiarNombre_Valido_DeberiaActualizarNombre()
    {
        var cliente = ClienteValido();

        cliente.CambiarNombre("Nuevo Nombre");

        cliente.Nombre.Should().Be("Nuevo Nombre");
    }

    [Theory]
    [InlineData("")]
    [InlineData(" ")]
    [InlineData(null)]
    public void CambiarNombre_Invalido_DeberiaLanzarExcepcion(string? nombreInvalido)
    {
        var cliente = ClienteValido();

        Action act = () => cliente.CambiarNombre(nombreInvalido!);

        act.Should().Throw<ArgumentException>()
            .WithMessage("*Nombre inválido*");
    }

    [Fact]
    public void CambiarInstagram_Valido_DeberiaActualizarInstagram()
    {
        var cliente = ClienteValido();

        cliente.CambiarInstagram("@nuevoinstagram");

        cliente.Instagram.Should().Be("@nuevoinstagram");
    }

    [Fact]
    public void CambiarInstagram_Nulo_DeberiaPermitirlo()
    {
        var cliente = ClienteValido();

        cliente.CambiarInstagram(null);

        cliente.Instagram.Should().BeNull();
    }

    // =============================
    // GESTIÓN DE TELÉFONOS
    // =============================

    [Fact]
    public void AgregarTelefono_Valido_DeberiaAgregarloALaLista()
    {
        var cliente = ClienteValido();
        string numero = "555-1234";

        cliente.AgregarTelefono(numero);

        cliente.Telefonos.Should().HaveCount(1);
        cliente.Telefonos.First().Numero.Should().Be(numero); // Asumiendo que Telefono expone 'Numero'
    }

    [Fact]
    public void AgregarTelefono_Invalido_DeberiaLanzarExcepcion()
    {
        var cliente = ClienteValido();

        Action act = () => cliente.AgregarTelefono("");

        act.Should().Throw<ArgumentException>()
            .WithMessage("*Número inválido*");
    }

    [Fact]
    public void ReemplazarTelefonos_ListaValida_DeberiaReemplazarLosExistentes()
    {
        // Arrange
        var cliente = ClienteValido();
        cliente.AgregarTelefono("111-111"); // Telefono previo
        var nuevosNumeros = new List<string> { "222-222", "333-333" };

        // Act
        cliente.ReemplazarTelefonos(nuevosNumeros);

        // Assert
        cliente.Telefonos.Should().HaveCount(2);
        cliente.Telefonos.Should().Contain(t => t.Numero == "222-222");
        cliente.Telefonos.Should().Contain(t => t.Numero == "333-333");
        cliente.Telefonos.Should().NotContain(t => t.Numero == "111-111");
    }

    [Fact]
    public void ReemplazarTelefonos_ConNulo_DeberiaLimpiarLaLista()
    {
        // Arrange
        var cliente = ClienteValido();
        cliente.AgregarTelefono("111-111");

        // Act
        cliente.ReemplazarTelefonos(null!);

        // Assert
        cliente.Telefonos.Should().BeEmpty();
    }

    [Fact]
    public void ReemplazarTelefonos_ConElementosVacios_DeberiaIgnorarLosVacios()
    {
        // Arrange
        var cliente = ClienteValido();
        var numerosMezclados = new List<string?> { "222-222", "", "   ", null };

        // Act
        cliente.ReemplazarTelefonos(numerosMezclados!);

        // Assert
        cliente.Telefonos.Should().HaveCount(1); // Solo debería agregar el "222-222"
        cliente.Telefonos.First().Numero.Should().Be("222-222");
    }

    // =============================
    // ESTADO (ACTIVAR / DESACTIVAR)
    // =============================

    [Fact]
    public void DesactivarCliente_DeberiaCambiarEstadoAFalso()
    {
        var cliente = ClienteValido(); // Por defecto nace true

        cliente.Desactivar();

        cliente.Activado.Should().BeFalse();
    }

    [Fact]
    public void ActivarCliente_PreviamenteDesactivado_DeberiaCambiarEstadoATrue()
    {
        var cliente = ClienteValido();
        cliente.Desactivar();

        cliente.Activar();

        cliente.Activado.Should().BeTrue();
    }

    // =============================
    // HELPERS
    // =============================

    private static Cliente ClienteValido()
    {
        return new Cliente("Cliente Test", "@clientetest");
    }
}