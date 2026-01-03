using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using VentasApp.Domain.Modelo.Venta;

namespace VentasApp.Infrastructure.Persistencia.Configuraciones;

    public class DetalleVentaConfiguracion : IEntityTypeConfiguration<DetalleVenta>
    {
        public void Configure(EntityTypeBuilder<DetalleVenta> builder)
        {
            builder.ToTable("DetalleVenta");

            builder.HasKey(d => d.Id);

            builder.Property(d => d.Cantidad)
                   .IsRequired();

            builder.Property(d => d.PrecioUnitario)
                   .HasColumnType("DOUBLE");

            builder.Property(d => d.SubTotal)
                   .HasColumnType("DOUBLE");
            
            builder.Property(d =>d.IdVenta)
                    .IsRequired();
                    
        }
    }

