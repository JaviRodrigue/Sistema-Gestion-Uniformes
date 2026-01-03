using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using VentasApp.Domain.Modelo.Pago;

namespace VentasApp.Infrastructure.Persistencia.Configuraciones;
    public class PagoConfiguracion : IEntityTypeConfiguration<Pago>
    {
      public void Configure(EntityTypeBuilder<Pago> builder)
        {
            builder.ToTable("Pago");
            builder.HasKey(e => e.Id);
            builder.Property(p => p.Id)
                  .HasColumnName("id_pago")
                  .ValueGeneratedOnAdd();
            builder.Property(p => p.Monto)
                  .HasColumnName("monto")
                  .HasColumnType("DOUBLE")
                  .IsRequired();
            builder.Property(p => p.FechaPago)
                  .HasColumnName("fecha_pago")
                  .HasColumnType("DATE")
                  .IsRequired();
            builder.Property(p => p.EsSenia)
                  .HasColumnName("es_senia")
                  .HasColumnType("BOOLEAN")
                  .IsRequired();
            builder.Property(p => p.IdVenta)
                  .HasColumnName("id_venta")
                  .IsRequired();
        
        }
    }
