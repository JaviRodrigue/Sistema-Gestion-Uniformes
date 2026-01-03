using Microsoft.EntityFrameworkCore;
using VentasApp.Domain.Modelo.Venta;
using VentasApp.Domain.Modelo.Categoria;
using VentasApp.Domain.Modelo.Pago;
using VentasApp.Domain.Enum;
using VentasApp.Domain.Modelo.Cliente;
using VentasApp.Domain.Modelo.Producto;
using VentasApp.Infrastructure.Persistencia.Configuraciones;


namespace VentasApp.Infrastructure.Persistencia.Contexto;

public class DatabaseContext : DbContext
{
    public DbSet<Venta> Ventas { get; set; }
    public DbSet<DetalleVenta> DetalleVenta { get; set; }
    public DbSet<Compra> Compras { get; set; }
    public DbSet<Cliente> Clientes { get; set; }
    public DbSet<Telefono> Telefonos { get; set; }
    public DbSet<Pago> Pagos { get; set; }
    public DbSet<PagoMetodo> PagoMetodos { get; set; }
    public DbSet<MedioPago> MedioPagos { get; set; }
    public DbSet<Producto> Producto { get; set; }
    public DbSet<ItemVendible> ItemVendible { get; set; }
    public DbSet<Stock> Stock { get; set; }
    public DbSet<Categoria> Categoria { get; set; }
  


    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlite("data source=Database.sqlite");
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfiguration(new VentaConfiguracion());
        modelBuilder.ApplyConfiguration(new DetalleVentaConfiguracion());
        modelBuilder.ApplyConfiguration(new CompraConfiguracion());
        modelBuilder.ApplyConfiguration(new ClienteConfiguracion());
        modelBuilder.ApplyConfiguration(new TelefonoConfiguracion());
        modelBuilder.ApplyConfiguration(new PagoConfiguracion());
        modelBuilder.ApplyConfiguration(new PagoMetodoConfiguracion());
        modelBuilder.ApplyConfiguration(new MedioPagoConfiguracion());
        modelBuilder.ApplyConfiguration(new ProductoConfiguracion());
        modelBuilder.ApplyConfiguration(new ItemVendibleConfiguracion());
        modelBuilder.ApplyConfiguration(new StockConfiguracion());
        modelBuilder.ApplyConfiguration(new CategoriaConfiguracion());
        
    }
}
