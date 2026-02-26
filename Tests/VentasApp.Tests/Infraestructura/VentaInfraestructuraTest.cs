using FluentAssertions;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using VentasApp.Domain.Enum;
using VentasApp.Domain.Modelo.Categoria;
using VentasApp.Domain.Modelo.Pago;
using VentasApp.Domain.Modelo.Productos;
using VentasApp.Domain.Modelo.Venta;
using VentasApp.Infrastructure.Persistencia.Contexto;
using VentasApp.Infrastructure.Persistencia.Repositorios;
using Xunit;

namespace VentasApp.Tests.Infrastructure
{
    public class VentaInfraestructuraTest : IDisposable
    {
        private readonly SqliteConnection _connection;
        private readonly DbContextOptions<DatabaseContext> _options;

        public VentaInfraestructuraTest()
        {
            _connection = new SqliteConnection("DataSource=:memory:");
            _connection.Open();
            _options = new DbContextOptionsBuilder<DatabaseContext>()
                .UseSqlite(_connection)
                .Options;

            using var context = new DatabaseContext(_options);
            context.Database.EnsureCreated();
        }

        // ----------------------------------------------------------------------
        // TEST 1: Verificar la base de la pirámide (Productos y Categorías)
        // Si este falla, el problema está en ProductoConfiguracion o ItemVendibleConfiguracion
        // ----------------------------------------------------------------------
        [Fact]
        public async Task Test1_Debe_Crear_Cadena_Categoria_Producto_Item()
        {
            using var context = new DatabaseContext(_options);

            // 1. Categoria
            var cat = new Categoria("Test Cat");
            context.Categoria.Add(cat);
            await context.SaveChangesAsync();

            // 2. Producto (Depende de Categoria)
            var prod = new Producto(cat.Id, "Prod Test", 100, 200);
            context.Producto.Add(prod);
            await context.SaveChangesAsync();

            // 3. Item (Depende de Producto)
            var item = new ItemVendible(prod.Id, "Item Test", "XXX", "M");
            context.ItemVendible.Add(item);
            await context.SaveChangesAsync();

            // Assert
            item.Id.Should().BeGreaterThan(0);
            prod.Id.Should().BeGreaterThan(0);
        }

        // ----------------------------------------------------------------------
        // TEST 2: Verificar Venta y Detalle (Sin Pagos aún)
        // Si este falla, el problema está en VentaRepository o DetalleVenta
        // ----------------------------------------------------------------------
        [Fact]
        public async Task Test2_Debe_Crear_Venta_Con_Detalle()
        {
            // A. PREPARAR DATOS (Seed mínimo necesario)
            int idItem;
            using (var contextSeed = new DatabaseContext(_options))
            {
                var cat = new Categoria("Cat");
                contextSeed.Categoria.Add(cat);
                await contextSeed.SaveChangesAsync();

                var prod = new Producto(cat.Id, "Prod", 100, 200);
                contextSeed.Producto.Add(prod);
                await contextSeed.SaveChangesAsync();

                var item = new ItemVendible(prod.Id, "Item", "CODE", "L");
                contextSeed.ItemVendible.Add(item);
                await contextSeed.SaveChangesAsync();

                idItem = item.Id; // Guardamos ID para usarlo abajo
            }

            // B. PROBAR REPOSITORIO VENTA
            using var contextWrite = new DatabaseContext(_options);
            var ventaRepo = new VentaRepository(contextWrite);

            var venta = new Venta(TipoVenta.Presencial);
            venta.AgregarDetalle(idItem, 1, 200);

            await ventaRepo.Agregar(venta);
            await contextWrite.SaveChangesAsync();

            // Assert
            venta.Id.Should().BeGreaterThan(0);
            venta.Detalles.Should().HaveCount(1);
        }

        // ----------------------------------------------------------------------
        // TEST 3: El Flujo Completo (Lo que intentábamos hacer antes)
        // ----------------------------------------------------------------------
        [Fact]
        public async Task Test3_Completo_Venta_Y_Pago()
        {
            // A. SEED DATA COMPLETO (Item + MedioPago)
            int idItem, idMedioPago;
            using (var contextSeed = new DatabaseContext(_options))
            {
                // Cadena Producto
                var cat = new Categoria("C");
                contextSeed.Categoria.Add(cat);
                await contextSeed.SaveChangesAsync();
                var prod = new Producto(cat.Id, "P", 100, 200);
                contextSeed.Producto.Add(prod);
                await contextSeed.SaveChangesAsync();
                var item = new ItemVendible(prod.Id, "I", "C", "M");
                contextSeed.ItemVendible.Add(item);

                // Medio Pago
                var mp = new MedioPago("Efec", false);
                contextSeed.MedioPagos.Add(mp);

                await contextSeed.SaveChangesAsync();
                idItem = item.Id;
                idMedioPago = mp.Id;
            }

            // B. LOGICA PRINCIPAL
            using var context = new DatabaseContext(_options);
            var ventaRepo = new VentaRepository(context);
            var pagoRepo = new PagoRepository(context);

            // 1. Venta
            var venta = new Venta(TipoVenta.Presencial);
            venta.AgregarDetalle(idItem, 1, 200); // Total 200
            await ventaRepo.Agregar(venta);
            await context.SaveChangesAsync();

            // 2. Pago
            var pago = new Pago(venta.Id, false);
            pago.AgregarPago(idMedioPago, 200);
            await pagoRepo.Agregar(pago);
            await context.SaveChangesAsync();

            // Assert
            pago.Id.Should().BeGreaterThan(0);
        }

        public void Dispose()
        {
            _connection.Close();
            _connection.Dispose();
        }
    }
}