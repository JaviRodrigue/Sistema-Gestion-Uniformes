using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using VentasApp.Domain.Modelo.Pago;

namespace VentasApp.Infrastructure.Persistencia.Configuraciones;

    public class PagoMetodoConfiguracion : IEntityTypeConfiguration<PagoMetodo>
    {
      public void Configure(EntityTypeBuilder<PagoMetodo> builder)
      {
        builder.ToTable("PagoMetodo");
            builder.HasKey(e => e.Id);
            builder.Property(pm => pm.Id)
                  .HasColumnName("id_pago_metodo")
                  .ValueGeneratedOnAdd();
            builder.Property(pm => pm.IdPago)
                  .HasColumnName("id_pago")
                  .IsRequired();
            builder.Property(pm => pm.IdMedioPago)
                  .HasColumnName("id_medio_pago")
                  .IsRequired();
            builder.Property(pm => pm.Monto)
                  .HasColumnName("monto")
                  .HasColumnType("DOUBLE")
                  .IsRequired();

            builder.HasOne(pm => pm.Pago)
                  .WithMany()
                  .HasForeignKey(pm => pm.IdPago);

            builder.HasOne(pm => pm.MedioPago)
                  .WithMany()
                  .HasForeignKey(pm => pm.IdMedioPago);
      }
    }
