using FluentAssertions;
using VentasApp.Domain.Modelo.Categoria;
using VentasApp.Domain.Base;
using Xunit;

namespace VentasApp.Domain.Tests.Categorias;

public class CategoriaTests
{
    // =============================
    // CREACIÓN
    // =============================

    [Fact]
    public void CrearCategoria_Valida_DeberiaCrearseCorrectamente()
    {
        var categoria = new Categoria("Ropa");

        categoria.Nombre.Should().Be("Ropa");
        categoria.Activa.Should().BeTrue();
    }

    [Fact]
    public void CrearCategoria_SinNombre_DeberiaLanzarExcepcion()
    {
        Action act = () => new Categoria("");

        act.Should().Throw<ExcepcionDominio>()
           .WithMessage("*nombre*");
    }

    [Fact]
    public void CrearCategoria_NombreSoloEspacios_DeberiaLanzarExcepcion()
    {
        Action act = () => new Categoria("   ");

        act.Should().Throw<ExcepcionDominio>()
           .WithMessage("*nombre*");
    }

    // =============================
    // CAMBIAR NOMBRE
    // =============================

    [Fact]
    public void CambiarNombre_Valido_DeberiaActualizarNombre()
    {
        var categoria = CategoriaValida();

        categoria.CambiarNombre("Calzado");

        categoria.Nombre.Should().Be("Calzado");
    }

    [Fact]
    public void CambiarNombre_Vacio_DeberiaLanzarExcepcion()
    {
        var categoria = CategoriaValida();

        Action act = () => categoria.CambiarNombre("");

        act.Should().Throw<ExcepcionDominio>()
           .WithMessage("*nombre*");
    }

    [Fact]
    public void CambiarNombre_SoloEspacios_DeberiaLanzarExcepcion()
    {
        var categoria = CategoriaValida();

        Action act = () => categoria.CambiarNombre("   ");

        act.Should().Throw<ExcepcionDominio>()
           .WithMessage("*nombre*");
    }

    // =============================
    // DESACTIVAR
    // =============================

    [Fact]
    public void DesactivarCategoria_DeberiaMarcarComoInactiva()
    {
        var categoria = CategoriaValida();

        categoria.Desactivar();

        categoria.Activa.Should().BeFalse();
    }

    // =============================
    // HELPERS
    // =============================

    private static Categoria CategoriaValida()
    {
        return new Categoria("Accesorios");
    }
}
