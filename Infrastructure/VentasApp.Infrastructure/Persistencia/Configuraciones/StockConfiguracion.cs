namespace VentasApp.Infrastructure.Persistencia.Configuraciones;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using VentasApp.Domain.Modelo.Productos;
public class StockConfiguracion : IEntityTypeConfiguration<Stock>
{
    public void Configure(EntityTypeBuilder<Stock> builder)
    {
        builder.ToTable("Stock");

        builder.HasKey(s => s.Id);

        builder.Property(s => s.CantidadDisponible).IsRequired();
        builder.Property(s => s.CantidadReservada).IsRequired();
        builder.Property(s => s.StockMinimo).IsRequired();
        builder.Property(s => s.Activo).IsRequired();

        builder.HasOne(s => s.ItemVendible)
            .WithOne()
            .HasForeignKey<Stock>(s => s.IdItemVendible)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
