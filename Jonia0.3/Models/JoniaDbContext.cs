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
            entity.HasKey(e => e.IdAbono).HasName("PK__abonos__1E6B9583D4C40A55");

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
                .HasConstraintName("FK__abonos__id_reser__6B24EA82");
        });

        modelBuilder.Entity<Cliente>(entity =>
        {
            entity.HasKey(e => e.NroDocumento).HasName("PK__clientes__761A4C4760526A18");

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
                .HasConstraintName("FK__clientes__id_rol__45F365D3");

            entity.HasOne(d => d.TipoDocumentoNavigation).WithMany(p => p.Clientes)
                .HasForeignKey(d => d.TipoDocumento)
                .HasConstraintName("FK__clientes__tipo_d__44FF419A");
        });

        modelBuilder.Entity<DetalleReservaPaquete>(entity =>
        {
            entity.HasKey(e => e.IdRp).HasName("PK__detalle___0148530E37C3C95A");

            entity.ToTable("detalle_reserva_paquete");

            entity.Property(e => e.IdRp).HasColumnName("id_rp");
            entity.Property(e => e.IdPaquete).HasColumnName("id_paquete");
            entity.Property(e => e.IdReserva).HasColumnName("id_reserva");
            entity.Property(e => e.Precio)
                .HasColumnType("money")
                .HasColumnName("precio");

            entity.HasOne(d => d.IdPaqueteNavigation).WithMany(p => p.DetalleReservaPaquetes)
                .HasForeignKey(d => d.IdPaquete)
                .HasConstraintName("FK__detalle_r__id_pa__68487DD7");

            entity.HasOne(d => d.IdReservaNavigation).WithMany(p => p.DetalleReservaPaquetes)
                .HasForeignKey(d => d.IdReserva)
                .HasConstraintName("FK__detalle_r__id_re__6754599E");
        });

        modelBuilder.Entity<DetalleReservaServicio>(entity =>
        {
            entity.HasKey(e => e.IdRs).HasName("PK__detalle___0148530B54BC6CE5");

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
                .HasConstraintName("FK__detalle_r__id_re__5FB337D6");

            entity.HasOne(d => d.IdServicioNavigation).WithMany(p => p.DetalleReservaServicios)
                .HasForeignKey(d => d.IdServicio)
                .HasConstraintName("FK__detalle_r__id_se__60A75C0F");
        });

        modelBuilder.Entity<Estado>(entity =>
        {
            entity.HasKey(e => e.IdEstado).HasName("PK__estados__86989FB2C685CAE6");

            entity.ToTable("estados");

            entity.Property(e => e.IdEstado).HasColumnName("id_estado");
            entity.Property(e => e.Nombre)
                .HasMaxLength(40)
                .IsUnicode(false)
                .HasColumnName("nombre");
        });

        modelBuilder.Entity<Habitacione>(entity =>
        {
            entity.HasKey(e => e.IdHabitacion).HasName("PK__habitaci__773F28F304B2B725");

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
                .HasConstraintName("FK__habitacio__id_ti__4BAC3F29");
        });

        modelBuilder.Entity<MetodoPago>(entity =>
        {
            entity.HasKey(e => e.IdMp).HasName("PK__metodo_p__014987E0CEDEF0CC");

            entity.ToTable("metodo_pago");

            entity.Property(e => e.IdMp).HasColumnName("id_MP");
            entity.Property(e => e.Nombre)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("nombre");
        });

        modelBuilder.Entity<Paquete>(entity =>
        {
            entity.HasKey(e => e.IdPaquete).HasName("PK__paquetes__609C3BCBF5B4E187");

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
                .HasConstraintName("FK__paquetes__id_hab__4E88ABD4");
        });

        modelBuilder.Entity<PaquetesServicio>(entity =>
        {
            entity.HasKey(e => e.IdPs).HasName("PK__paquetes__0148A3496A746510");

            entity.ToTable("paquetes_servicios");

            entity.Property(e => e.IdPs).HasColumnName("id_ps");
            entity.Property(e => e.IdPaquete).HasColumnName("id_paquete");
            entity.Property(e => e.IdServicio).HasColumnName("id_servicio");
            entity.Property(e => e.Precio)
                .HasColumnType("money")
                .HasColumnName("precio");

            entity.HasOne(d => d.IdPaqueteNavigation).WithMany(p => p.PaquetesServicios)
                .HasForeignKey(d => d.IdPaquete)
                .HasConstraintName("FK__paquetes___id_pa__6383C8BA");

            entity.HasOne(d => d.IdServicioNavigation).WithMany(p => p.PaquetesServicios)
                .HasForeignKey(d => d.IdServicio)
                .HasConstraintName("FK__paquetes___id_se__6477ECF3");
        });

        modelBuilder.Entity<Permiso>(entity =>
        {
            entity.HasKey(e => e.IdPermiso).HasName("PK__permisos__228F224FEF2E2E0B");

            entity.ToTable("permisos");

            entity.Property(e => e.IdPermiso).HasColumnName("id_permiso");
            entity.Property(e => e.Nombre)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("nombre");
        });

        modelBuilder.Entity<Reserva>(entity =>
        {
            entity.HasKey(e => e.IdReserva).HasName("PK__reserva__423CBE5D06F10166");

            entity.ToTable("reserva");

            entity.Property(e => e.IdReserva).HasColumnName("id_reserva");
            entity.Property(e => e.Estado).HasColumnName("estado");
            entity.Property(e => e.FechaEntrada).HasColumnName("fecha_entrada");
            entity.Property(e => e.FechaRegistro).HasColumnName("fecha_registro");
            entity.Property(e => e.FechaSalida).HasColumnName("fecha_salida");
            entity.Property(e => e.Informacion)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("informacion");
            entity.Property(e => e.Iva).HasColumnName("iva");
            entity.Property(e => e.MetodoPago).HasColumnName("metodo_pago");
            entity.Property(e => e.NroDocumentoCliente).HasColumnName("nro_documento_cliente");
            entity.Property(e => e.NroDocumentoTrabajador).HasColumnName("nro_documento_trabajador");
            entity.Property(e => e.NumeroPersonas).HasColumnName("numero_personas");
            entity.Property(e => e.Subtotal)
                .HasColumnType("money")
                .HasColumnName("subtotal");
            entity.Property(e => e.Total)
                .HasColumnType("money")
                .HasColumnName("total");

            entity.HasOne(d => d.EstadoNavigation).WithMany(p => p.Reservas)
                .HasForeignKey(d => d.Estado)
                .HasConstraintName("FK__reserva__estado__5812160E");

            entity.HasOne(d => d.MetodoPagoNavigation).WithMany(p => p.Reservas)
                .HasForeignKey(d => d.MetodoPago)
                .HasConstraintName("FK__reserva__metodo___571DF1D5");

            entity.HasOne(d => d.NroDocumentoClienteNavigation).WithMany(p => p.Reservas)
                .HasForeignKey(d => d.NroDocumentoCliente)
                .HasConstraintName("FK__reserva__nro_doc__5535A963");

            entity.HasOne(d => d.NroDocumentoTrabajadorNavigation).WithMany(p => p.Reservas)
                .HasForeignKey(d => d.NroDocumentoTrabajador)
                .HasConstraintName("FK__reserva__nro_doc__5629CD9C");
        });

        modelBuilder.Entity<Rol>(entity =>
        {
            entity.HasKey(e => e.IdRol).HasName("PK__rol__6ABCB5E00DD571BF");

            entity.ToTable("rol");

            entity.Property(e => e.IdRol).HasColumnName("id_rol");
            entity.Property(e => e.Estado).HasColumnName("estado");
            entity.Property(e => e.Nombre)
                .HasMaxLength(40)
                .HasColumnName("nombre");
        });

        modelBuilder.Entity<RolPermiso>(entity =>
        {
            entity.HasKey(e => e.IdRd).HasName("PK__rol_perm__0148533A6477BB9F");

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
            entity.HasKey(e => e.IdServicio).HasName("PK__servicio__6FD07FDC99FF0288");

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
                .HasConstraintName("FK__servicios__tipo___5CD6CB2B");
        });

        modelBuilder.Entity<TipoDocumento>(entity =>
        {
            entity.HasKey(e => e.IdTd).HasName("PK__tipo_doc__01495F3FED942882");

            entity.ToTable("tipo_documento");

            entity.Property(e => e.IdTd).HasColumnName("id_TD");
            entity.Property(e => e.Nombre)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("nombre");
        });

        modelBuilder.Entity<TipoHabitacion>(entity =>
        {
            entity.HasKey(e => e.IdTipo).HasName("PK__tipo_hab__CF901089D5B1AE19");

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
            entity.HasKey(e => e.IdTs).HasName("PK__tipo_ser__01495CCCEB748F01");

            entity.ToTable("tipo_servicio");

            entity.Property(e => e.IdTs).HasColumnName("id_TS");
            entity.Property(e => e.Estado).HasColumnName("estado");
            entity.Property(e => e.Nombre)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("nombre");
        });

        modelBuilder.Entity<Usuario>(entity =>
        {
            entity.HasKey(e => e.NroDocumento).HasName("PK__usuarios__761A4C472CA012BB");

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
            entity.Property(e => e.IdRol).HasColumnName("id_rol");
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
