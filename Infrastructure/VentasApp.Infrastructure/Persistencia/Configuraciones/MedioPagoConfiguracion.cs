using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using VentasApp.Domain.Modelo.Pago;

namespace VentasApp.Infrastructure.Persistencia.Configuraciones;

    public class MedioPagoConfiguracion : IEntityTypeConfiguration<MedioPago>
    {
      public void Configure(EntityTypeBuilder<MedioPago> builder)
      {
            builder.ToTable("MedioPago");
            builder.HasKey(e => e.Id);

            builder.Property(m => m.Id)
                  .HasColumnName("id_medio_pago")
                  .ValueGeneratedOnAdd();

            builder.Property(m => m.Nombre)
                  .IsRequired()
                  .HasMaxLength(100);

            builder.Property(m => m.TieneRecargo)
                  .HasColumnName("tiene_recargo")
                  .HasColumnType("BOOLEAN")
                  .IsRequired();

            builder.Property(m => m.Nombre)
                  .HasColumnName("nombre")
                  .HasColumnType("VARCHAR(50)")
                  .IsRequired();

            builder.Property(m => m.Activo)
            .IsRequired();

      }
    } 
