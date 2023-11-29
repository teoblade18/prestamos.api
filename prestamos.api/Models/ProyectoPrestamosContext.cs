using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace prestamos.api.Models;

public partial class ProyectoPrestamosContext : DbContext
{
    public ProyectoPrestamosContext()
    {
    }

    public ProyectoPrestamosContext(DbContextOptions<ProyectoPrestamosContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Abono> Abonos { get; set; }

    public virtual DbSet<Cliente> Clientes { get; set; }

    public virtual DbSet<Interes> Intereses { get; set; }

    public virtual DbSet<Prestamista> Prestamistas { get; set; }

    public virtual DbSet<Prestamo> Prestamos { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {

    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Abono>(entity =>
        {
            entity.HasKey(e => e.IdAbono).HasName("PK__Abonos__C43BE6C494CF6DA0");

            entity.Property(e => e.IdAbono).HasColumnName("idAbono");
            entity.Property(e => e.Fecha).HasColumnName("fecha");
            entity.Property(e => e.IdPrestamo).HasColumnName("idPrestamo");
            entity.Property(e => e.Valor)
                .HasColumnType("numeric(18, 0)")
                .HasColumnName("valor");

            entity.HasOne(d => d.oPrestamo).WithMany(p => p.Abonos)
                .HasForeignKey(d => d.IdPrestamo)
                .HasConstraintName("FK__Abonos__idPresta__45F365D3");
        });

        modelBuilder.Entity<Cliente>(entity =>
        {
            entity.HasKey(e => e.Cedula).HasName("PK__Clientes__415B7BE451EEF9DA");

            entity.Property(e => e.Cedula)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("cedula");
            entity.Property(e => e.Nombre)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("nombre");
            entity.Property(e => e.Puntaje)
                .HasColumnType("numeric(18, 0)")
                .HasColumnName("puntaje");
        });

        modelBuilder.Entity<Interes>(entity =>
        {
            entity.HasKey(e => e.IdInteres).HasName("PK__Interese__650CDE9535E1874E");

            entity.Property(e => e.IdInteres).HasColumnName("idInteres");
            entity.Property(e => e.Fecha).HasColumnName("fecha");
            entity.Property(e => e.IdPrestamo).HasColumnName("idPrestamo");
            entity.Property(e => e.Tipo)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("tipo");
            entity.Property(e => e.Valor)
                .HasColumnType("numeric(18, 0)")
                .HasColumnName("valor");

            entity.HasOne(d => d.oPrestamo).WithMany(p => p.Intereses)
                .HasForeignKey(d => d.IdPrestamo)
                .HasConstraintName("FK__Intereses__idPre__4316F928");
        });

        modelBuilder.Entity<Prestamista>(entity =>
        {
            entity.HasKey(e => e.IdPrestamista).HasName("PK__Prestami__65ECA198C09F7809");

            entity.Property(e => e.IdPrestamista)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("idPrestamista");
            entity.Property(e => e.Capital)
                .HasColumnType("numeric(18, 0)")
                .HasColumnName("capital");
            entity.Property(e => e.Nombre)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("nombre");
            entity.Property(e => e.NumeroCuenta)
                .IsUnicode(false)
                .HasColumnName("numeroCuenta");
        });

        modelBuilder.Entity<Prestamo>(entity =>
        {
            entity.HasKey(e => e.IdPrestamo).HasName("PK__Prestamo__A4876C13ADEF6E68");

            entity.Property(e => e.IdPrestamo).HasColumnName("idPrestamo");
            entity.Property(e => e.CedulaCliente)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("cedulaCliente");
            entity.Property(e => e.DiaCorte)
                .HasColumnType("numeric(18, 0)")
                .HasColumnName("diaCorte");
            entity.Property(e => e.FechaInicial).HasColumnName("fechaInicial");
            entity.Property(e => e.FechaPago).HasColumnName("fechaPago");
            entity.Property(e => e.IdPrestamista)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("idPrestamista");
            entity.Property(e => e.MontoInicial)
                .HasColumnType("numeric(18, 0)")
                .HasColumnName("montoInicial");
            entity.Property(e => e.MontoReal)
                .HasColumnType("numeric(18, 0)")
                .HasColumnName("montoReal");
            entity.Property(e => e.Porcentaje)
                .HasColumnType("numeric(18, 0)")
                .HasColumnName("porcentaje");
            entity.Property(e => e.TipoIntereses)
                .HasMaxLength(30)
                .IsUnicode(false)
                .HasColumnName("tipoIntereses");

            entity.HasOne(d => d.oCliente).WithMany(p => p.Prestamos)
                .HasForeignKey(d => d.CedulaCliente)
                .HasConstraintName("FK__Prestamos__cedul__3F466844");

            entity.HasOne(d => d.oPrestamista).WithMany(p => p.Prestamos)
                .HasForeignKey(d => d.IdPrestamista)
                .HasConstraintName("FK__Prestamos__idPre__403A8C7D");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
