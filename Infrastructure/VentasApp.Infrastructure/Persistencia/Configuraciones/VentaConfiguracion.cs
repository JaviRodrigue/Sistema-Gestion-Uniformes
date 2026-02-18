namespace VentasApp.Infrastructure.Persistencia.Configuraciones;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using VentasApp.Domain.Modelo.Venta;

public class VentaConfiguracion : IEntityTypeConfiguration<Venta>
{
    public void Configure(EntityTypeBuilder<Venta> builder)
    {
        builder.ToTable("Venta");

        builder.HasKey(v => v.Id);

        builder.Property(v => v.FechaVenta)
               .IsRequired();

        builder.Property(v => v.MontoTotal)
               .HasColumnType("DOUBLE");

        builder.Property(v => v.MontoPagado)
               .HasColumnType("DOUBLE");

        builder.Property(v => v.SaldoPendiente)
               .HasColumnType("DOUBLE");

        // NO volver a configurar la relación aquí.
        // La relación entre Venta y DetalleVenta se configura en DetalleVentaConfiguracion
        // para usar la propiedad concreta IdVenta en la entidad DetalleVenta.
    }
}