namespace VentasApp.Infrastructure.Persistencia.Configuraciones;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using VentasApp.Domain.Modelo.Productos;

    public class ItemVendibleConfiguracion : IEntityTypeConfiguration<ItemVendible>
    {
        public void Configure(EntityTypeBuilder<ItemVendible> builder)
        {
            // Tabla
            builder.ToTable("ItemVendible");

            // Clave primaria
            builder.HasKey(i => i.Id);

            // Propiedades
            builder.Property(i => i.Nombre)
                   .HasMaxLength(150)
                   .IsRequired();

            builder.Property(i => i.CodigoBarra)
                   .HasMaxLength(255)
                   .IsRequired();

            builder.Property(i => i.Talle)
                   .HasMaxLength(20)
                   .IsRequired(false);

            // Índice único para código de barras
            builder.HasIndex(i => i.CodigoBarra)
                   .IsUnique();

            // Relación con Producto (muchos ItemVendible → un Producto)
            builder.HasOne(i => i.Producto)
                .WithMany()
                .HasForeignKey(i => i.IdProducto)
                .OnDelete(DeleteBehavior.Restrict);


            // Relación 1–1 con Stock (sin navegación en dominio)
       // builder.HasOne<Stock>()
       //        .WithOne()
       //        .HasForeignKey<ItemVendible>(i => i.IdStock)
       //        .OnDelete(DeleteBehavior.Restrict);
        }
    }

       