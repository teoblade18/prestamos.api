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

    public virtual DbSet<Usuario> Usuarios { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {

    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Abono>(entity =>
        {
            entity.HasKey(e => e.IdAbono).HasName("PK__Abonos__C43BE6C47368F54D");

            entity.Property(e => e.IdAbono).HasColumnName("idAbono");
            entity.Property(e => e.Fecha).HasColumnName("fecha");
            entity.Property(e => e.IdPrestamo).HasColumnName("idPrestamo");
            entity.Property(e => e.Valor)
                .HasColumnType("numeric(18, 0)")
                .HasColumnName("valor");

            entity.HasOne(d => d.oPrestamo).WithMany(p => p.Abonos)
                .HasForeignKey(d => d.IdPrestamo)
                .HasConstraintName("FK__Abonos__idPresta__73BA3083");
        });

        modelBuilder.Entity<Cliente>(entity =>
        {
            entity.HasKey(e => e.IdCliente).HasName("PK__Clientes__885457EE7C676459");

            entity.Property(e => e.IdCliente).HasColumnName("idCliente");
            entity.Property(e => e.Cedula)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("cedula");
            entity.Property(e => e.IdPrestamista)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("idPrestamista");
            entity.Property(e => e.Nombre)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("nombre");
            entity.Property(e => e.Puntaje)
                .HasColumnType("numeric(18, 0)")
                .HasColumnName("puntaje");

            entity.HasOne(d => d.oPrestamista).WithMany(p => p.Clientes)
                .HasForeignKey(d => d.IdPrestamista)
                .HasConstraintName("FK__Clientes__idPres__778AC167");
        });

        modelBuilder.Entity<Interes>(entity =>
        {
            entity.HasKey(e => e.IdInteres).HasName("PK__Interese__650CDE95A8CBE292");

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
                .HasConstraintName("FK__Intereses__idPre__74AE54BC");
        });

        modelBuilder.Entity<Prestamista>(entity =>
        {
            entity.HasKey(e => e.IdPrestamista).HasName("PK__Prestami__65ECA1985D3E5379");

            entity.Property(e => e.IdPrestamista)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("idPrestamista");
            entity.Property(e => e.Capital)
                .HasColumnType("numeric(18, 0)")
                .HasColumnName("capital");
            entity.Property(e => e.IdUsuario).HasColumnName("idUsuario");
            entity.Property(e => e.Nombre)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("nombre");
            entity.Property(e => e.NumeroCuenta)
                .IsUnicode(false)
                .HasColumnName("numeroCuenta");

            entity.HasOne(d => d.oUsuario).WithMany(p => p.Prestamista)
                .HasForeignKey(d => d.IdUsuario)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Prestamis__idUsu__787EE5A0");
        });

        modelBuilder.Entity<Prestamo>(entity =>
        {
            entity.HasKey(e => e.IdPrestamo).HasName("PK__Prestamo__A4876C133CD500F3");

            entity.Property(e => e.IdPrestamo).HasColumnName("idPrestamo");
            entity.Property(e => e.DiaCorte)
                .HasColumnType("numeric(18, 0)")
                .HasColumnName("diaCorte");
            entity.Property(e => e.FechaInicial).HasColumnName("fechaInicial");
            entity.Property(e => e.FechaPago).HasColumnName("fechaPago");
            entity.Property(e => e.IdCliente).HasColumnName("idCliente");
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
                .HasForeignKey(d => d.IdCliente)
                .HasConstraintName("FK__Prestamos__idCli__75A278F5");

            entity.HasOne(d => d.oPrestamista).WithMany(p => p.Prestamos)
                .HasForeignKey(d => d.IdPrestamista)
                .HasConstraintName("FK__Prestamos__idPre__76969D2E");
        });

        modelBuilder.Entity<Usuario>(entity =>
        {
            entity.HasKey(e => e.IdUsuario).HasName("PK__Usuarios__645723A659447E68");

            entity.Property(e => e.IdUsuario).HasColumnName("idUsuario");
            entity.Property(e => e.Contraseña).HasColumnName("contraseña");
            entity.Property(e => e.NombreUsuario).HasColumnName("nombreUsuario");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
