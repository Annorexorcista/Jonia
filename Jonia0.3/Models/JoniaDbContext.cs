using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace Jonia0._3.Models;

public partial class JoniaDbContext : DbContext
{
    public JoniaDbContext()
    {
    }

    public JoniaDbContext(DbContextOptions<JoniaDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Abono> Abonos { get; set; }

    public virtual DbSet<Cliente> Clientes { get; set; }

    public virtual DbSet<DetalleReservaPaquete> DetalleReservaPaquetes { get; set; }

    public virtual DbSet<DetalleReservaServicio> DetalleReservaServicios { get; set; }

    public virtual DbSet<Estado> Estados { get; set; }

    public virtual DbSet<Habitacione> Habitaciones { get; set; }

    public virtual DbSet<MetodoPago> MetodoPagos { get; set; }

    public virtual DbSet<Paquete> Paquetes { get; set; }

    public virtual DbSet<PaquetesServicio> PaquetesServicios { get; set; }

    public virtual DbSet<Permiso> Permisos { get; set; }

    public virtual DbSet<Reserva> Reservas { get; set; }

    public virtual DbSet<Rol> Rols { get; set; }

    public virtual DbSet<RolPermiso> RolPermisos { get; set; }

    public virtual DbSet<Servicio> Servicios { get; set; }

    public virtual DbSet<TipoDocumento> TipoDocumentos { get; set; }

    public virtual DbSet<TipoHabitacion> TipoHabitacions { get; set; }

    public virtual DbSet<TipoServicio> TipoServicios { get; set; }

    public virtual DbSet<Usuario> Usuarios { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Server=ALAN;Initial Catalog=Jonia_DB;integrated security=True; TrustServerCertificate=True");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Abono>(entity =>
        {
            entity.HasKey(e => e.IdAbono).HasName("PK__abonos__1E6B958311F2EBCB");

            entity.ToTable("abonos");

            entity.Property(e => e.IdAbono).HasColumnName("id_abono");
            entity.Property(e => e.Estado).HasColumnName("estado");
            entity.Property(e => e.FechaRegistro).HasColumnName("fecha_registro");
            entity.Property(e => e.IdReserva).HasColumnName("id_reserva");
            entity.Property(e => e.Iva).HasColumnName("iva");
            entity.Property(e => e.Porcentaje).HasColumnName("porcentaje");
            entity.Property(e => e.SubtotalAbonado)
                .HasColumnType("money")
                .HasColumnName("subtotal_abonado");
            entity.Property(e => e.TotalAbonado)
                .HasColumnType("money")
                .HasColumnName("total_abonado");
            entity.Property(e => e.TotalPendiente)
                .HasColumnType("money")
                .HasColumnName("total_pendiente");
            entity.Property(e => e.ValorDeuda)
                .HasColumnType("money")
                .HasColumnName("valor_deuda");

            entity.HasOne(d => d.IdReservaNavigation).WithMany(p => p.Abonos)
                .HasForeignKey(d => d.IdReserva)
                .HasConstraintName("FK__abonos__id_reser__6D0D32F4");
        });

        modelBuilder.Entity<Cliente>(entity =>
        {
            entity.HasKey(e => e.NroDocumento).HasName("PK__clientes__761A4C479F33C808");

            entity.ToTable("clientes");

            entity.Property(e => e.NroDocumento)
                .ValueGeneratedNever()
                .HasColumnName("nro_documento");
            entity.Property(e => e.Apellido)
                .HasMaxLength(40)
                .IsUnicode(false)
                .HasColumnName("apellido");
            entity.Property(e => e.Celular)
                .HasMaxLength(10)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("celular");
            entity.Property(e => e.Confirmado).HasColumnName("confirmado");
            entity.Property(e => e.ConfirmarClave)
                .HasMaxLength(222)
                .IsUnicode(false)
                .HasColumnName("confirmarclave");
            entity.Property(e => e.Contrasena)
                .HasMaxLength(222)
                .IsUnicode(false)
                .HasColumnName("contrasena");
            entity.Property(e => e.Correo)
                .HasMaxLength(40)
                .IsUnicode(false)
                .HasColumnName("correo");
            entity.Property(e => e.Direccion)
                .HasMaxLength(80)
                .IsUnicode(false)
                .HasColumnName("direccion");
            entity.Property(e => e.Estado).HasColumnName("estado");
            entity.Property(e => e.FechaNacimiento).HasColumnName("fecha_nacimiento");
            entity.Property(e => e.IdRol)
                .HasDefaultValue(2)
                .HasColumnName("id_rol");
            entity.Property(e => e.Nombre)
                .HasMaxLength(40)
                .IsUnicode(false)
                .HasColumnName("nombre");
            entity.Property(e => e.Restablecer).HasColumnName("restablecer");
            entity.Property(e => e.TipoDocumento).HasColumnName("tipo_documento");
            entity.Property(e => e.Token)
                .HasMaxLength(222)
                .IsUnicode(false)
                .HasColumnName("token");

            entity.HasOne(d => d.IdRolNavigation).WithMany(p => p.Clientes)
                .HasForeignKey(d => d.IdRol)
                .HasConstraintName("FK__clientes__id_rol__46E78A0C");

            entity.HasOne(d => d.TipoDocumentoNavigation).WithMany(p => p.Clientes)
                .HasForeignKey(d => d.TipoDocumento)
                .HasConstraintName("FK__clientes__tipo_d__45F365D3");
        });

        modelBuilder.Entity<DetalleReservaPaquete>(entity =>
        {
            entity.HasKey(e => e.IdDetalle).HasName("PK__detalle___4F1332DEAEDB2EE6");

            entity.ToTable("detalle_reserva_paquete");

            entity.Property(e => e.IdDetalle)
                .ValueGeneratedNever()
                .HasColumnName("id_detalle");
            entity.Property(e => e.Cantidad).HasColumnName("cantidad");
            entity.Property(e => e.IdPaquete).HasColumnName("id_paquete");
            entity.Property(e => e.Precio)
                .HasColumnType("money")
                .HasColumnName("precio");

            entity.HasOne(d => d.IdDetalleNavigation).WithOne(p => p.DetalleReservaPaquete)
                .HasForeignKey<DetalleReservaPaquete>(d => d.IdDetalle)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__detalle_r__id_de__693CA210");

            entity.HasOne(d => d.IdPaqueteNavigation).WithMany(p => p.DetalleReservaPaquetes)
                .HasForeignKey(d => d.IdPaquete)
                .HasConstraintName("FK__detalle_r__id_pa__6A30C649");
        });

        modelBuilder.Entity<DetalleReservaServicio>(entity =>
        {
            entity.HasKey(e => e.IdRs).HasName("PK__detalle___0148530BD5BD9DD9");

            entity.ToTable("detalle_reserva_servicio");

            entity.Property(e => e.IdRs).HasColumnName("id_rs");
            entity.Property(e => e.Cantidad).HasColumnName("cantidad");
            entity.Property(e => e.IdReserva).HasColumnName("id_reserva");
            entity.Property(e => e.IdServicio).HasColumnName("id_servicio");
            entity.Property(e => e.Precio)
                .HasColumnType("money")
                .HasColumnName("precio");

            entity.HasOne(d => d.IdReservaNavigation).WithMany(p => p.DetalleReservaServicios)
                .HasForeignKey(d => d.IdReserva)
                .HasConstraintName("FK__detalle_r__id_re__619B8048");

            entity.HasOne(d => d.IdServicioNavigation).WithMany(p => p.DetalleReservaServicios)
                .HasForeignKey(d => d.IdServicio)
                .HasConstraintName("FK__detalle_r__id_se__628FA481");
        });

        modelBuilder.Entity<Estado>(entity =>
        {
            entity.HasKey(e => e.IdEstado).HasName("PK__estados__86989FB294AE84B4");

            entity.ToTable("estados");

            entity.Property(e => e.IdEstado).HasColumnName("id_estado");
            entity.Property(e => e.Nombre)
                .HasMaxLength(40)
                .IsUnicode(false)
                .HasColumnName("nombre");
        });

        modelBuilder.Entity<Habitacione>(entity =>
        {
            entity.HasKey(e => e.IdHabitacion).HasName("PK__habitaci__773F28F3B890E00A");

            entity.ToTable("habitaciones");

            entity.Property(e => e.IdHabitacion).HasColumnName("id_habitacion");
            entity.Property(e => e.Descripcion)
                .HasMaxLength(444)
                .IsUnicode(false)
                .HasColumnName("descripcion");
            entity.Property(e => e.Estado).HasColumnName("estado");
            entity.Property(e => e.IdTipo).HasColumnName("id_tipo");
            entity.Property(e => e.Nombre)
                .HasMaxLength(40)
                .IsUnicode(false)
                .HasColumnName("nombre");
            entity.Property(e => e.Precio)
                .HasColumnType("money")
                .HasColumnName("precio");

            entity.HasOne(d => d.IdTipoNavigation).WithMany(p => p.Habitaciones)
                .HasForeignKey(d => d.IdTipo)
                .HasConstraintName("FK__habitacio__id_ti__4CA06362");
        });

        modelBuilder.Entity<MetodoPago>(entity =>
        {
            entity.HasKey(e => e.IdMp).HasName("PK__metodo_p__014987E08CF9766F");

            entity.ToTable("metodo_pago");

            entity.Property(e => e.IdMp).HasColumnName("id_MP");
            entity.Property(e => e.Nombre)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("nombre");
        });

        modelBuilder.Entity<Paquete>(entity =>
        {
            entity.HasKey(e => e.IdPaquete).HasName("PK__paquetes__609C3BCB16D17E06");

            entity.ToTable("paquetes");

            entity.Property(e => e.IdPaquete).HasColumnName("id_paquete");
            entity.Property(e => e.Descripcion)
                .HasMaxLength(444)
                .IsUnicode(false)
                .HasColumnName("descripcion");
            entity.Property(e => e.Estado).HasColumnName("estado");
            entity.Property(e => e.IdHabitacion).HasColumnName("id_habitacion");
            entity.Property(e => e.Nombre)
                .HasMaxLength(40)
                .IsUnicode(false)
                .HasColumnName("nombre");
            entity.Property(e => e.Precio)
                .HasColumnType("money")
                .HasColumnName("precio");

            entity.HasOne(d => d.IdHabitacionNavigation).WithMany(p => p.Paquetes)
                .HasForeignKey(d => d.IdHabitacion)
                .HasConstraintName("FK__paquetes__id_hab__4F7CD00D");
        });

        modelBuilder.Entity<PaquetesServicio>(entity =>
        {
            entity.HasKey(e => e.IdPs).HasName("PK__paquetes__0148A349D41E8757");

            entity.ToTable("paquetes_servicios");

            entity.Property(e => e.IdPs).HasColumnName("id_ps");
            entity.Property(e => e.IdPaquete).HasColumnName("id_paquete");
            entity.Property(e => e.IdServicio).HasColumnName("id_servicio");
            entity.Property(e => e.Precio)
                .HasColumnType("money")
                .HasColumnName("precio");

            entity.HasOne(d => d.IdPaqueteNavigation).WithMany(p => p.PaquetesServicios)
                .HasForeignKey(d => d.IdPaquete)
                .HasConstraintName("FK__paquetes___id_pa__656C112C");

            entity.HasOne(d => d.IdServicioNavigation).WithMany(p => p.PaquetesServicios)
                .HasForeignKey(d => d.IdServicio)
                .HasConstraintName("FK__paquetes___id_se__66603565");
        });

        modelBuilder.Entity<Permiso>(entity =>
        {
            entity.HasKey(e => e.IdPermiso).HasName("PK__permisos__228F224F50D47F23");

            entity.ToTable("permisos");

            entity.Property(e => e.IdPermiso).HasColumnName("id_permiso");
            entity.Property(e => e.Nombre)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("nombre");
        });

        modelBuilder.Entity<Reserva>(entity =>
        {
            entity.HasKey(e => e.IdReserva).HasName("PK__reserva__423CBE5D1CC69B79");

            entity.ToTable("reserva");

            entity.Property(e => e.IdReserva).HasColumnName("id_reserva");
            entity.Property(e => e.Estado)
                .HasDefaultValue(1)
                .HasColumnName("estado");
            entity.Property(e => e.FechaEntrada).HasColumnName("fecha_entrada");
            entity.Property(e => e.FechaRegistro).HasColumnName("fecha_registro");
            entity.Property(e => e.FechaSalida).HasColumnName("fecha_salida");
            entity.Property(e => e.HoraLlegada).HasColumnName("hora_llegada");
            entity.Property(e => e.HoraSalida).HasColumnName("hora_salida");
            entity.Property(e => e.Informacion)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("informacion");
            entity.Property(e => e.Iva).HasColumnName("iva");
            entity.Property(e => e.MetodoPago).HasColumnName("metodo_pago");
            entity.Property(e => e.NroDocumentoCliente).HasColumnName("nro_documento_cliente");
            entity.Property(e => e.NroDocumentoTrabajador).HasColumnName("nro_documento_trabajador");
            entity.Property(e => e.NumeroAdultos).HasColumnName("numero_adultos");
            entity.Property(e => e.NumeroNinos).HasColumnName("numero_ninos");
            entity.Property(e => e.Subtotal)
                .HasColumnType("money")
                .HasColumnName("subtotal");
            entity.Property(e => e.Total)
                .HasColumnType("money")
                .HasColumnName("total");

            entity.HasOne(d => d.EstadoNavigation).WithMany(p => p.Reservas)
                .HasForeignKey(d => d.Estado)
                .HasConstraintName("FK__reserva__estado__59063A47");

            entity.HasOne(d => d.MetodoPagoNavigation).WithMany(p => p.Reservas)
                .HasForeignKey(d => d.MetodoPago)
                .HasConstraintName("FK__reserva__metodo___5812160E");

            entity.HasOne(d => d.NroDocumentoClienteNavigation).WithMany(p => p.Reservas)
                .HasForeignKey(d => d.NroDocumentoCliente)
                .HasConstraintName("FK__reserva__nro_doc__5629CD9C");

            entity.HasOne(d => d.NroDocumentoTrabajadorNavigation).WithMany(p => p.Reservas)
                .HasForeignKey(d => d.NroDocumentoTrabajador)
                .HasConstraintName("FK__reserva__nro_doc__571DF1D5");
        });

        modelBuilder.Entity<Rol>(entity =>
        {
            entity.HasKey(e => e.IdRol).HasName("PK__rol__6ABCB5E0A8AE7FF6");

            entity.ToTable("rol");

            entity.Property(e => e.IdRol).HasColumnName("id_rol");
            entity.Property(e => e.Estado).HasColumnName("estado");
            entity.Property(e => e.Nombre)
                .HasMaxLength(40)
                .HasColumnName("nombre");
        });

        modelBuilder.Entity<RolPermiso>(entity =>
        {
            entity.HasKey(e => e.IdRd).HasName("PK__rol_perm__0148533A493298E7");

            entity.ToTable("rol_permisos");

            entity.Property(e => e.IdRd).HasColumnName("id_rd");
            entity.Property(e => e.IdPermiso).HasColumnName("id_permiso");
            entity.Property(e => e.IdRol).HasColumnName("id_rol");

            entity.HasOne(d => d.IdPermisoNavigation).WithMany(p => p.RolPermisos)
                .HasForeignKey(d => d.IdPermiso)
                .HasConstraintName("FK__rol_permi__id_pe__3C69FB99");

            entity.HasOne(d => d.IdRolNavigation).WithMany(p => p.RolPermisos)
                .HasForeignKey(d => d.IdRol)
                .HasConstraintName("FK__rol_permi__id_ro__3B75D760");
        });

        modelBuilder.Entity<Servicio>(entity =>
        {
            entity.HasKey(e => e.IdServicio).HasName("PK__servicio__6FD07FDC7ED74742");

            entity.ToTable("servicios");

            entity.Property(e => e.IdServicio).HasColumnName("id_servicio");
            entity.Property(e => e.Descripcion)
                .HasMaxLength(444)
                .IsUnicode(false)
                .HasColumnName("descripcion");
            entity.Property(e => e.Estado).HasColumnName("estado");
            entity.Property(e => e.Nombre)
                .HasMaxLength(40)
                .IsUnicode(false)
                .HasColumnName("nombre");
            entity.Property(e => e.Precio)
                .HasColumnType("money")
                .HasColumnName("precio");
            entity.Property(e => e.TipoServicio).HasColumnName("tipo_servicio");

            entity.HasOne(d => d.TipoServicioNavigation).WithMany(p => p.Servicios)
                .HasForeignKey(d => d.TipoServicio)
                .HasConstraintName("FK__servicios__tipo___5EBF139D");
        });

        modelBuilder.Entity<TipoDocumento>(entity =>
        {
            entity.HasKey(e => e.IdTd).HasName("PK__tipo_doc__01495F3FF4E64E31");

            entity.ToTable("tipo_documento");

            entity.Property(e => e.IdTd).HasColumnName("id_TD");
            entity.Property(e => e.Nombre)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("nombre");
        });

        modelBuilder.Entity<TipoHabitacion>(entity =>
        {
            entity.HasKey(e => e.IdTipo).HasName("PK__tipo_hab__CF901089CF89AC84");

            entity.ToTable("tipo_habitacion");

            entity.Property(e => e.IdTipo).HasColumnName("id_tipo");
            entity.Property(e => e.Estado).HasColumnName("estado");
            entity.Property(e => e.Nombre)
                .HasMaxLength(40)
                .IsUnicode(false)
                .HasColumnName("nombre");
            entity.Property(e => e.NroPersonas).HasColumnName("nro_personas");
        });

        modelBuilder.Entity<TipoServicio>(entity =>
        {
            entity.HasKey(e => e.IdTs).HasName("PK__tipo_ser__01495CCCFE88D91A");

            entity.ToTable("tipo_servicio");

            entity.Property(e => e.IdTs).HasColumnName("id_TS");
            entity.Property(e => e.Nombre)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("nombre");
        });

        modelBuilder.Entity<Usuario>(entity =>
        {
            entity.HasKey(e => e.NroDocumento).HasName("PK__usuarios__761A4C47FDD89008");

            entity.ToTable("usuarios");

            entity.Property(e => e.NroDocumento)
                .ValueGeneratedNever()
                .HasColumnName("nro_documento");
            entity.Property(e => e.Apellido)
                .HasMaxLength(40)
                .IsUnicode(false)
                .HasColumnName("apellido");
            entity.Property(e => e.Celular)
                .HasMaxLength(10)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("celular");
            entity.Property(e => e.Confirmado).HasColumnName("confirmado");
            entity.Property(e => e.Confirmarclave)
                .HasMaxLength(222)
                .IsUnicode(false)
                .HasColumnName("confirmarclave");
            entity.Property(e => e.Contrasena)
                .HasMaxLength(222)
                .IsUnicode(false)
                .HasColumnName("contrasena");
            entity.Property(e => e.Correo)
                .HasMaxLength(40)
                .IsUnicode(false)
                .HasColumnName("correo");
            entity.Property(e => e.Estado).HasColumnName("estado");
            entity.Property(e => e.FechaNacimiento).HasColumnName("fecha_nacimiento");
            entity.Property(e => e.IdRol)
                .HasDefaultValue(1)
                .HasColumnName("id_rol");
            entity.Property(e => e.Nombre)
                .HasMaxLength(40)
                .IsUnicode(false)
                .HasColumnName("nombre");
            entity.Property(e => e.Restablecer).HasColumnName("restablecer");
            entity.Property(e => e.TipoDocumento).HasColumnName("tipo_documento");
            entity.Property(e => e.Token)
                .HasMaxLength(222)
                .IsUnicode(false)
                .HasColumnName("token");

            entity.HasOne(d => d.IdRolNavigation).WithMany(p => p.Usuarios)
                .HasForeignKey(d => d.IdRol)
                .HasConstraintName("FK__usuarios__id_rol__4222D4EF");

            entity.HasOne(d => d.TipoDocumentoNavigation).WithMany(p => p.Usuarios)
                .HasForeignKey(d => d.TipoDocumento)
                .HasConstraintName("FK__usuarios__tipo_d__412EB0B6");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
