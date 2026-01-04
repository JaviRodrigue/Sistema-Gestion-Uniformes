namespace VentasApp.Infrastructure.Persistencia.Configuraciones;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using VentasApp.Domain.Modelo.Cliente;


    public class TelefonoConfiguracion : IEntityTypeConfiguration<Telefono>
    {
      public void Configure(EntityTypeBuilder<Telefono> builder)
      {
        builder.ToTable("Telefono");
            builder.HasKey(e => e.Id);
            builder.Property(t => t.Id)
                  .HasColumnName("id_telefono")
                  .ValueGeneratedOnAdd();
            builder.Property(t => t.Numero)
                  .HasColumnName("numero")
                  .HasColumnType("VARCHAR(20)")
                  .IsRequired();
            builder.Property(t => t.IdCliente)
                  .HasColumnName("id_cliente")
                  .IsRequired();
            builder.HasOne(t => t.Cliente)
                  .WithMany(c => c.Telefonos)
                  .HasForeignKey(t => t.IdCliente)
                  .OnDelete(DeleteBehavior.Cascade);

      }
    } 
