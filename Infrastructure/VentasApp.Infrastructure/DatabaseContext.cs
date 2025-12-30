using System;
using Microsoft.EntityFrameworkCore;
using VentasApp.Domain;

namespace VentasApp.Infrastructure;

public class DatabaseContext: DbContext
{
    public DbSet<Venta> ventas { get; set; }
    public DbSet<Estado> estados { get; set; }


    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlite("data source=Database.sqlite");
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Venta>(entity =>
        {
            entity.ToTable("Venta");
            entity.HasKey(e => e.IdVenta);
            entity.Property(v => v.IdVenta)
              .HasColumnName("id_venta")
              .ValueGeneratedOnAdd();
            entity.Property(v => v.FechaVenta)
              .HasColumnName("fecha_venta")
              .HasColumnType("DATE")
              .IsRequired();
            entity.Property(v => v.TipoVenta)
                  .HasColumnName("tipo_venta")
                  .HasColumnType("BOOLEAN")
                  .IsRequired();
            entity.Property(v => v.MontoTotal)
                  .HasColumnName("monto_total")
                  .HasColumnType("DOUBLE")
                  .IsRequired();
            entity.Property(v => v.MontoPagado)
                  .HasColumnName("monto_pagado")
                  .HasColumnType("DOUBLE")
                  .IsRequired();
            entity.Property(v => v.SaldoPendiente)
                  .HasColumnName("saldo_pendiente")
                  .HasColumnType("DOUBLE")
                  .IsRequired();
            entity.Property(v => v.IdEstado)
                  .HasColumnName("id_estado")
                  .IsRequired();
            entity.HasOne<Estado>()
                  .WithMany()
                  .HasForeignKey(v => v.IdEstado);
        }

        modelBuilder.Entity<Estado>(entity =>
        {
            entity.ToTable("Estado");
            entity.HasKey(e => e.IdEstado);
            entity.Property(e => e.IdEstado)
                  .HasColumnName("id_estado")
                  .ValueGeneratedOnAdd();
            entity.Property(e => e.Nombre)
                  .HasColumnName("nombre")
                  .HasColumnType("VARCHAR(50)")
                  .IsRequired();
        });
    }
