using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using prestamos.api.Models;
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
            entity.HasKey(e => e.IdAbono).HasName("PK__Abonos__C43BE6C4D2F65A17");

            entity.Property(e => e.IdAbono).HasColumnName("idAbono");
            entity.Property(e => e.Fecha).HasColumnName("fecha");
            entity.Property(e => e.IdPrestamo).HasColumnName("idPrestamo");
            entity.Property(e => e.Valor)
                .HasColumnType("numeric(18, 0)")
                .HasColumnName("valor");

            entity.HasOne(d => d.oPrestamo).WithMany(p => p.Abonos)
                .HasForeignKey(d => d.IdPrestamo)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Abonos__idPresta__0C85DE4D");

            entity.ToTable("Abonos", tb => tb.HasTrigger("adiciona_abonos"));
        });

        modelBuilder.Entity<Cliente>(entity =>
        {
            entity.HasKey(e => e.IdCliente).HasName("PK__Clientes__885457EEDBDA17FA");

            entity.Property(e => e.IdCliente).HasColumnName("idCliente");
            entity.Property(e => e.Cedula)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("cedula");
            entity.Property(e => e.IdPrestamista).HasColumnName("idPrestamista");
            entity.Property(e => e.Nombre)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("nombre");
            entity.Property(e => e.Puntaje)
                .HasColumnType("numeric(18, 0)")
                .HasColumnName("puntaje");
            entity.Property(e => e.MaxPrestar)
                .HasColumnType("numeric(18, 0)")
                .HasColumnName("maxPrestar");
            entity.Property(e => e.NumeroCuenta)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("numeroCuenta");

            entity.HasOne(d => d.oPrestamista).WithMany(p => p.Clientes)
                .HasForeignKey(d => d.IdPrestamista)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Clientes__idPres__0D7A0286");
        });

        modelBuilder.Entity<Interes>(entity =>
        {
            entity.HasKey(e => e.IdInteres).HasName("PK__Interese__650CDE951165C3B6");

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
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Intereses__idPre__0E6E26BF");

            entity.ToTable("Intereses", tb => tb.HasTrigger("adiciona_interes"));
        });

        modelBuilder.Entity<Prestamista>(entity =>
        {
            entity.HasKey(e => e.IdPrestamista).HasName("PK__Prestami__65ECA1986F301437");

            entity.Property(e => e.IdPrestamista).HasColumnName("idPrestamista");
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
                .HasConstraintName("FK__Prestamis__idUsu__0F624AF8");
        });

        modelBuilder.Entity<Prestamo>(entity =>
        {
            entity.HasKey(e => e.IdPrestamo).HasName("PK__Prestamo__A4876C133F4CA8EB");

            entity.Property(e => e.IdPrestamo).HasColumnName("idPrestamo");
            entity.Property(e => e.DiaCorte)
                .HasColumnType("numeric(18, 0)")
                .HasColumnName("diaCorte");
            entity.Property(e => e.FechaInicial).HasColumnName("fechaInicial");
            entity.Property(e => e.FechaFinal).HasColumnName("fechaFinal");
            entity.Property(e => e.FechaProximoPago).HasColumnName("fechaProximoPago");
            entity.Property(e => e.FechaPago).HasColumnName("fechaPago");
            entity.Property(e => e.IdCliente).HasColumnName("idCliente");
            entity.Property(e => e.IdPrestamista).HasColumnName("idPrestamista");
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
            entity.Property(e => e.Estado)
                .HasMaxLength(10)
                .HasColumnName("estado")
                .HasDefaultValue("Abonado");

            entity.HasOne(d => d.oCliente).WithMany(p => p.Prestamos)
                .HasForeignKey(d => d.IdCliente)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Prestamos__idCli__10566F31");

            entity.HasOne(d => d.oPrestamista).WithMany(p => p.Prestamos)
                .HasForeignKey(d => d.IdPrestamista)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Prestamos__idPre__114A936A");

            entity.ToTable("Prestamos", tb => tb.HasTrigger("primer_intereres_prestamo"));
        });

        modelBuilder.Entity<Usuario>(entity =>
        {
            entity.HasKey(e => e.IdUsuario).HasName("PK__Usuarios__645723A6E810318B");

            entity.Property(e => e.IdUsuario).HasColumnName("idUsuario");
            entity.Property(e => e.Contraseña).HasColumnName("contraseña");
            entity.Property(e => e.Email).HasColumnName("email");
            entity.Property(e => e.NombreUsuario).HasColumnName("nombreUsuario");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
