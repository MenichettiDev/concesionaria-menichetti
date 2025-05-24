using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Pomelo.EntityFrameworkCore.MySql.Scaffolding.Internal;

namespace concesionaria_menichetti.Models;

public partial class ConcesionariaContext : DbContext
{
    public ConcesionariaContext()
    {
    }

    public ConcesionariaContext(DbContextOptions<ConcesionariaContext> options)
        : base(options)
    {
    }

    public virtual DbSet<AccesosPagado> AccesosPagados { get; set; }

    public virtual DbSet<Comentario> Comentarios { get; set; }

    public virtual DbSet<Concesionaria> Concesionarias { get; set; }

    public virtual DbSet<ContratosPlane> ContratosPlanes { get; set; }

    public virtual DbSet<ContratosSuscripcion> ContratosSuscripcions { get; set; }

    public virtual DbSet<Destacado> Destacados { get; set; }

    public virtual DbSet<EmpleadosConcesionarium> EmpleadosConcesionaria { get; set; }

    public virtual DbSet<Favorito> Favoritos { get; set; }

    public virtual DbSet<FotosVehiculo> FotosVehiculos { get; set; }

    public virtual DbSet<Marca> Marcas { get; set; }

    public virtual DbSet<Modelo> Modelos { get; set; }

    public virtual DbSet<Pago> Pagos { get; set; }

    public virtual DbSet<PlanesConcesionarium> PlanesConcesionaria { get; set; }

    public virtual DbSet<Reporte> Reportes { get; set; }

    public virtual DbSet<Suscripcione> Suscripciones { get; set; }

    public virtual DbSet<Usuario> Usuarios { get; set; }

    public virtual DbSet<Vehiculo> Vehiculos { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseMySql("server=localhost;database=concesionaria;user id=root", Microsoft.EntityFrameworkCore.ServerVersion.Parse("8.0.30-mysql"));

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder
            .UseCollation("utf8mb4_0900_ai_ci")
            .HasCharSet("utf8mb4");

        modelBuilder.Entity<AccesosPagado>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("accesos_pagados");

            entity.HasIndex(e => e.UsuarioId, "usuarioId");

            entity.HasIndex(e => e.VehiculoId, "vehiculoId");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Fecha)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("timestamp")
                .HasColumnName("fecha");
            entity.Property(e => e.UsuarioId).HasColumnName("usuarioId");
            entity.Property(e => e.VehiculoId).HasColumnName("vehiculoId");

            entity.HasOne(d => d.Usuario).WithMany(p => p.AccesosPagados)
                .HasForeignKey(d => d.UsuarioId)
                .HasConstraintName("accesos_pagados_ibfk_1");

            entity.HasOne(d => d.Vehiculo).WithMany(p => p.AccesosPagados)
                .HasForeignKey(d => d.VehiculoId)
                .HasConstraintName("accesos_pagados_ibfk_2");
        });

        modelBuilder.Entity<Comentario>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("comentarios");

            entity.HasIndex(e => e.CompradorId, "compradorId");

            entity.HasIndex(e => e.VendedorId, "vendedorId");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Calificacion).HasColumnName("calificacion");
            entity.Property(e => e.CompradorId).HasColumnName("compradorId");
            entity.Property(e => e.Fecha)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("timestamp")
                .HasColumnName("fecha");
            entity.Property(e => e.Texto)
                .HasMaxLength(200)
                .HasColumnName("texto");
            entity.Property(e => e.VendedorId).HasColumnName("vendedorId");

            entity.HasOne(d => d.Comprador).WithMany(p => p.ComentarioCompradors)
                .HasForeignKey(d => d.CompradorId)
                .HasConstraintName("comentarios_ibfk_1");

            entity.HasOne(d => d.Vendedor).WithMany(p => p.ComentarioVendedors)
                .HasForeignKey(d => d.VendedorId)
                .HasConstraintName("comentarios_ibfk_2");
        });

        modelBuilder.Entity<Concesionaria>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("concesionarias");

            entity.HasIndex(e => e.UsuarioId, "usuarioId");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Cuit)
                .HasMaxLength(20)
                .HasColumnName("CUIT");
            entity.Property(e => e.Direccion)
                .HasMaxLength(200)
                .HasColumnName("direccion");
            entity.Property(e => e.Nombre)
                .HasMaxLength(100)
                .HasColumnName("nombre");
            entity.Property(e => e.UsuarioId).HasColumnName("usuarioId");

            entity.HasOne(d => d.Usuario).WithMany(p => p.Concesionaria)
                .HasForeignKey(d => d.UsuarioId)
                .HasConstraintName("concesionarias_ibfk_1");
        });

        modelBuilder.Entity<ContratosPlane>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("contratos_planes");

            entity.HasIndex(e => e.ConcesionariaId, "concesionariaId");

            entity.HasIndex(e => e.PlanId, "planId");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Activo)
                .HasDefaultValueSql("'1'")
                .HasColumnName("activo");
            entity.Property(e => e.ConcesionariaId).HasColumnName("concesionariaId");
            entity.Property(e => e.FechaFin).HasColumnName("fechaFin");
            entity.Property(e => e.FechaInicio).HasColumnName("fechaInicio");
            entity.Property(e => e.PlanId).HasColumnName("planId");

            entity.HasOne(d => d.Concesionaria).WithMany(p => p.ContratosPlanes)
                .HasForeignKey(d => d.ConcesionariaId)
                .HasConstraintName("contratos_planes_ibfk_1");

            entity.HasOne(d => d.Plan).WithMany(p => p.ContratosPlanes)
                .HasForeignKey(d => d.PlanId)
                .HasConstraintName("contratos_planes_ibfk_2");
        });

        modelBuilder.Entity<ContratosSuscripcion>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("contratos_suscripcion");

            entity.HasIndex(e => e.SuscripcionId, "suscripcionId");

            entity.HasIndex(e => e.UsuarioId, "usuarioId");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Activo)
                .HasDefaultValueSql("'1'")
                .HasColumnName("activo");
            entity.Property(e => e.FechaFin).HasColumnName("fechaFin");
            entity.Property(e => e.FechaInicio).HasColumnName("fechaInicio");
            entity.Property(e => e.SuscripcionId).HasColumnName("suscripcionId");
            entity.Property(e => e.UsuarioId).HasColumnName("usuarioId");

            entity.HasOne(d => d.Suscripcion).WithMany(p => p.ContratosSuscripcions)
                .HasForeignKey(d => d.SuscripcionId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("contratos_suscripcion_ibfk_2");

            entity.HasOne(d => d.Usuario).WithMany(p => p.ContratosSuscripcions)
                .HasForeignKey(d => d.UsuarioId)
                .HasConstraintName("contratos_suscripcion_ibfk_1");
        });

        modelBuilder.Entity<Destacado>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("destacados");

            entity.HasIndex(e => e.VehiculoId, "vehiculoId");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.FechaFin)
                .HasColumnType("timestamp")
                .HasColumnName("fechaFin");
            entity.Property(e => e.FechaInicio)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("timestamp")
                .HasColumnName("fechaInicio");
            entity.Property(e => e.VehiculoId).HasColumnName("vehiculoId");

            entity.HasOne(d => d.Vehiculo).WithMany(p => p.Destacados)
                .HasForeignKey(d => d.VehiculoId)
                .HasConstraintName("destacados_ibfk_1");
        });

        modelBuilder.Entity<EmpleadosConcesionarium>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("empleados_concesionaria");

            entity.HasIndex(e => e.ConcesionariaId, "concesionariaId");

            entity.HasIndex(e => e.UsuarioId, "usuarioId");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.ConcesionariaId).HasColumnName("concesionariaId");
            entity.Property(e => e.UsuarioId).HasColumnName("usuarioId");

            entity.HasOne(d => d.Concesionaria).WithMany(p => p.EmpleadosConcesionaria)
                .HasForeignKey(d => d.ConcesionariaId)
                .HasConstraintName("empleados_concesionaria_ibfk_1");

            entity.HasOne(d => d.Usuario).WithMany(p => p.EmpleadosConcesionaria)
                .HasForeignKey(d => d.UsuarioId)
                .HasConstraintName("empleados_concesionaria_ibfk_2");
        });

        modelBuilder.Entity<Favorito>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("favoritos");

            entity.HasIndex(e => e.UsuarioId, "usuarioId");

            entity.HasIndex(e => e.VehiculoId, "vehiculoId");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.UsuarioId).HasColumnName("usuarioId");
            entity.Property(e => e.VehiculoId).HasColumnName("vehiculoId");

            entity.HasOne(d => d.Usuario).WithMany(p => p.Favoritos)
                .HasForeignKey(d => d.UsuarioId)
                .HasConstraintName("favoritos_ibfk_1");

            entity.HasOne(d => d.Vehiculo).WithMany(p => p.Favoritos)
                .HasForeignKey(d => d.VehiculoId)
                .HasConstraintName("favoritos_ibfk_2");
        });

        modelBuilder.Entity<FotosVehiculo>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("fotos_vehiculo");

            entity.HasIndex(e => e.VehiculoId, "vehiculoId");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Descripcion)
                .HasMaxLength(200)
                .HasColumnName("descripcion");
            entity.Property(e => e.Fecha)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("timestamp")
                .HasColumnName("fecha");
            entity.Property(e => e.FotoArchivo)
                .HasMaxLength(400)
                .HasColumnName("foto_archivo");
            entity.Property(e => e.VehiculoId).HasColumnName("vehiculoId");

            entity.HasOne(d => d.Vehiculo).WithMany(p => p.FotosVehiculos)
                .HasForeignKey(d => d.VehiculoId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("fotos_vehiculo_ibfk_1");
        });

        modelBuilder.Entity<Marca>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("marca");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Descripcion)
                .HasMaxLength(20)
                .HasColumnName("descripcion");
        });

        modelBuilder.Entity<Modelo>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("modelo");

            entity.HasIndex(e => e.IdMarca, "idMarca");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Descripcion)
                .HasMaxLength(20)
                .HasColumnName("descripcion");
            entity.Property(e => e.IdMarca).HasColumnName("idMarca");

            entity.HasOne(d => d.Marca).WithMany(p => p.Modelos)
                .HasForeignKey(d => d.IdMarca)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("modelo_ibfk_1");
        });

        modelBuilder.Entity<Pago>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("pagos");

            entity.HasIndex(e => e.UsuarioId, "usuarioId");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Detalle)
                .HasMaxLength(200)
                .HasColumnName("detalle");
            entity.Property(e => e.Fecha)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("timestamp")
                .HasColumnName("fecha");
            entity.Property(e => e.Monto)
                .HasPrecision(12, 2)
                .HasColumnName("monto");
            entity.Property(e => e.Tipo)
                .HasColumnType("enum('Suscripcion','Plan','Destacado')")
                .HasColumnName("tipo");
            entity.Property(e => e.UsuarioId).HasColumnName("usuarioId");

            entity.HasOne(d => d.Usuario).WithMany(p => p.Pagos)
                .HasForeignKey(d => d.UsuarioId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("pagos_ibfk_1");
        });

        modelBuilder.Entity<PlanesConcesionarium>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("planes_concesionaria");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.CantidadPublicaciones).HasColumnName("cantidadPublicaciones");
            entity.Property(e => e.Descripcion)
                .HasMaxLength(200)
                .HasColumnName("descripcion");
            entity.Property(e => e.Nombre)
                .HasMaxLength(50)
                .HasColumnName("nombre");
            entity.Property(e => e.Precio)
                .HasPrecision(10, 2)
                .HasColumnName("precio");
        });

        modelBuilder.Entity<Reporte>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("reportes");

            entity.HasIndex(e => e.UsuarioId, "usuarioId");

            entity.HasIndex(e => e.VehiculoId, "vehiculoId");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Fecha)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("timestamp")
                .HasColumnName("fecha");
            entity.Property(e => e.Motivo)
                .HasMaxLength(200)
                .HasColumnName("motivo");
            entity.Property(e => e.UsuarioId).HasColumnName("usuarioId");
            entity.Property(e => e.VehiculoId).HasColumnName("vehiculoId");

            entity.HasOne(d => d.Usuario).WithMany(p => p.Reportes)
                .HasForeignKey(d => d.UsuarioId)
                .HasConstraintName("reportes_ibfk_1");

            entity.HasOne(d => d.Vehiculo).WithMany(p => p.Reportes)
                .HasForeignKey(d => d.VehiculoId)
                .HasConstraintName("reportes_ibfk_2");
        });

        modelBuilder.Entity<Suscripcione>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("suscripciones");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.CantidadPublicaciones).HasColumnName("cantidad_publicaciones");
            entity.Property(e => e.Descripcion)
                .HasMaxLength(200)
                .HasColumnName("descripcion");
            entity.Property(e => e.Nombre)
                .HasMaxLength(50)
                .HasColumnName("nombre");
            entity.Property(e => e.Precio)
                .HasPrecision(10, 2)
                .HasColumnName("precio");
        });

        modelBuilder.Entity<Usuario>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("usuarios");

            entity.HasIndex(e => e.Email, "email").IsUnique();

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Activo)
                .HasDefaultValueSql("'1'")
                .HasColumnName("activo");
            entity.Property(e => e.Contraseña)
                .HasMaxLength(255)
                .HasColumnName("contraseña");
            entity.Property(e => e.Email)
                .HasMaxLength(100)
                .HasColumnName("email");
            entity.Property(e => e.EsConcesionaria)
                .HasDefaultValueSql("'0'")
                .HasColumnName("esConcesionaria");
            entity.Property(e => e.FotoPerfil)
                .HasMaxLength(200)
                .HasColumnName("fotoPerfil");
            entity.Property(e => e.Nombre)
                .HasMaxLength(100)
                .HasColumnName("nombre");
            entity.Property(e => e.Rol)
                .HasDefaultValueSql("'Usuario'")
                .HasColumnType("enum('Admin','Usuario')")
                .HasColumnName("rol");
            entity.Property(e => e.Telefono)
                .HasMaxLength(20)
                .HasColumnName("telefono");
            entity.Property(e => e.Ubicacion)
                .HasMaxLength(100)
                .HasColumnName("ubicacion");
            entity.Property(e => e.Verificado)
                .HasDefaultValueSql("'0'")
                .HasColumnName("verificado");
        });

        modelBuilder.Entity<Vehiculo>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("vehiculos");

            entity.HasIndex(e => e.IdModelo, "idModelo");

            entity.HasIndex(e => e.UsuarioId, "usuarioId");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Anio).HasColumnName("anio");
            entity.Property(e => e.Combustible)
                .HasMaxLength(20)
                .HasColumnName("combustible");
            entity.Property(e => e.Destacado)
                .HasComment("0 : no\r\n1: si")
                .HasColumnName("destacado");
            entity.Property(e => e.Estado)
                .HasDefaultValueSql("'1'")
                .HasColumnName("estado");
            entity.Property(e => e.IdModelo).HasColumnName("idModelo");
            entity.Property(e => e.Kilometraje).HasColumnName("kilometraje");
            entity.Property(e => e.Precio)
                .HasPrecision(12, 2)
                .HasColumnName("precio");
            entity.Property(e => e.UsuarioId).HasColumnName("usuarioId");

            entity.HasOne(d => d.Modelo).WithMany(p => p.Vehiculos)
                .HasForeignKey(d => d.IdModelo)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("vehiculos_ibfk_2");

            entity.HasOne(d => d.Usuario).WithMany(p => p.Vehiculos)
                .HasForeignKey(d => d.UsuarioId)
                .HasConstraintName("vehiculos_ibfk_1");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
