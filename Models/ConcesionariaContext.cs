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

    public virtual DbSet<TbAccesosPagado> TbAccesosPagados { get; set; }

    public virtual DbSet<TbComentario> TbComentarios { get; set; }

    public virtual DbSet<TbConcesionaria> TbConcesionarias { get; set; }

    public virtual DbSet<TbContratosPlane> TbContratosPlanes { get; set; }

    public virtual DbSet<TbDestacado> TbDestacados { get; set; }

    public virtual DbSet<TbEmpleadosConcesionarium> TbEmpleadosConcesionaria { get; set; }

    public virtual DbSet<TbFavorito> TbFavoritos { get; set; }

    public virtual DbSet<TbFotosVehiculo> TbFotosVehiculos { get; set; }

    public virtual DbSet<TbPago> TbPagos { get; set; }

    public virtual DbSet<TbPlanesConcesionarium> TbPlanesConcesionaria { get; set; }

    public virtual DbSet<TbReporte> TbReportes { get; set; }

    public virtual DbSet<TbSuscripcione> TbSuscripciones { get; set; }

    public virtual DbSet<TbUsuario> TbUsuarios { get; set; }

    public virtual DbSet<TbVehiculo> TbVehiculos { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        => optionsBuilder.UseMySql("server=localhost;port=3306;database=concesionaria;user=root", Microsoft.EntityFrameworkCore.ServerVersion.Parse("8.0.30-mysql"));

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder
            .UseCollation("utf8mb4_0900_ai_ci")
            .HasCharSet("utf8mb4");

        modelBuilder.Entity<TbAccesosPagado>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("tb_accesos_pagados");

            entity.HasIndex(e => e.UsuarioId, "usuarioId");

            entity.HasIndex(e => e.VehiculoId, "vehiculoId");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Fecha)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("timestamp")
                .HasColumnName("fecha");
            entity.Property(e => e.UsuarioId).HasColumnName("usuarioId");
            entity.Property(e => e.VehiculoId).HasColumnName("vehiculoId");

            entity.HasOne(d => d.Usuario).WithMany(p => p.TbAccesosPagados)
                .HasForeignKey(d => d.UsuarioId)
                .HasConstraintName("tb_accesos_pagados_ibfk_1");

            entity.HasOne(d => d.Vehiculo).WithMany(p => p.TbAccesosPagados)
                .HasForeignKey(d => d.VehiculoId)
                .HasConstraintName("tb_accesos_pagados_ibfk_2");
        });

        modelBuilder.Entity<TbComentario>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("tb_comentarios");

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

            entity.HasOne(d => d.Comprador).WithMany(p => p.TbComentarioCompradors)
                .HasForeignKey(d => d.CompradorId)
                .HasConstraintName("tb_comentarios_ibfk_1");

            entity.HasOne(d => d.Vendedor).WithMany(p => p.TbComentarioVendedors)
                .HasForeignKey(d => d.VendedorId)
                .HasConstraintName("tb_comentarios_ibfk_2");
        });

        modelBuilder.Entity<TbConcesionaria>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("tb_concesionarias");

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

            entity.HasOne(d => d.Usuario).WithMany(p => p.TbConcesionaria)
                .HasForeignKey(d => d.UsuarioId)
                .HasConstraintName("tb_concesionarias_ibfk_1");
        });

        modelBuilder.Entity<TbContratosPlane>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("tb_contratos_planes");

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

            entity.HasOne(d => d.Concesionaria).WithMany(p => p.TbContratosPlanes)
                .HasForeignKey(d => d.ConcesionariaId)
                .HasConstraintName("tb_contratos_planes_ibfk_1");

            entity.HasOne(d => d.Plan).WithMany(p => p.TbContratosPlanes)
                .HasForeignKey(d => d.PlanId)
                .HasConstraintName("tb_contratos_planes_ibfk_2");
        });

        modelBuilder.Entity<TbDestacado>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("tb_destacados");

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

            entity.HasOne(d => d.Vehiculo).WithMany(p => p.TbDestacados)
                .HasForeignKey(d => d.VehiculoId)
                .HasConstraintName("tb_destacados_ibfk_1");
        });

        modelBuilder.Entity<TbEmpleadosConcesionarium>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("tb_empleados_concesionaria");

            entity.HasIndex(e => e.ConcesionariaId, "concesionariaId");

            entity.HasIndex(e => e.UsuarioId, "usuarioId");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.ConcesionariaId).HasColumnName("concesionariaId");
            entity.Property(e => e.UsuarioId).HasColumnName("usuarioId");

            entity.HasOne(d => d.Concesionaria).WithMany(p => p.TbEmpleadosConcesionaria)
                .HasForeignKey(d => d.ConcesionariaId)
                .HasConstraintName("tb_empleados_concesionaria_ibfk_1");

            entity.HasOne(d => d.Usuario).WithMany(p => p.TbEmpleadosConcesionaria)
                .HasForeignKey(d => d.UsuarioId)
                .HasConstraintName("tb_empleados_concesionaria_ibfk_2");
        });

        modelBuilder.Entity<TbFavorito>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("tb_favoritos");

            entity.HasIndex(e => e.UsuarioId, "usuarioId");

            entity.HasIndex(e => e.VehiculoId, "vehiculoId");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.UsuarioId).HasColumnName("usuarioId");
            entity.Property(e => e.VehiculoId).HasColumnName("vehiculoId");

            entity.HasOne(d => d.Usuario).WithMany(p => p.TbFavoritos)
                .HasForeignKey(d => d.UsuarioId)
                .HasConstraintName("tb_favoritos_ibfk_1");

            entity.HasOne(d => d.Vehiculo).WithMany(p => p.TbFavoritos)
                .HasForeignKey(d => d.VehiculoId)
                .HasConstraintName("tb_favoritos_ibfk_2");
        });

        modelBuilder.Entity<TbFotosVehiculo>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("tb_fotos_vehiculo");

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

            entity.HasOne(d => d.Vehiculo).WithMany(p => p.TbFotosVehiculos)
                .HasForeignKey(d => d.VehiculoId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("tb_fotos_vehiculo_ibfk_1");
        });

        modelBuilder.Entity<TbPago>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("tb_pagos");

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
                .HasMaxLength(30)
                .HasColumnName("tipo");
            entity.Property(e => e.UsuarioId).HasColumnName("usuarioId");

            entity.HasOne(d => d.Usuario).WithMany(p => p.TbPagos)
                .HasForeignKey(d => d.UsuarioId)
                .HasConstraintName("tb_pagos_ibfk_1");
        });

        modelBuilder.Entity<TbPlanesConcesionarium>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("tb_planes_concesionaria");

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

        modelBuilder.Entity<TbReporte>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("tb_reportes");

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

            entity.HasOne(d => d.Usuario).WithMany(p => p.TbReportes)
                .HasForeignKey(d => d.UsuarioId)
                .HasConstraintName("tb_reportes_ibfk_1");

            entity.HasOne(d => d.Vehiculo).WithMany(p => p.TbReportes)
                .HasForeignKey(d => d.VehiculoId)
                .HasConstraintName("tb_reportes_ibfk_2");
        });

        modelBuilder.Entity<TbSuscripcione>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("tb_suscripciones");

            entity.HasIndex(e => e.UsuarioId, "usuarioId");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Activa).HasColumnName("activa");
            entity.Property(e => e.FechaFin).HasColumnName("fechaFin");
            entity.Property(e => e.FechaInicio).HasColumnName("fechaInicio");
            entity.Property(e => e.UsuarioId).HasColumnName("usuarioId");

            entity.HasOne(d => d.Usuario).WithMany(p => p.TbSuscripciones)
                .HasForeignKey(d => d.UsuarioId)
                .HasConstraintName("tb_suscripciones_ibfk_1");
        });

        modelBuilder.Entity<TbUsuario>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("tb_usuarios");

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

        modelBuilder.Entity<TbVehiculo>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("tb_vehiculos");

            entity.HasIndex(e => e.UsuarioId, "usuarioId");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Año).HasColumnName("año");
            entity.Property(e => e.Combustible)
                .HasMaxLength(20)
                .HasColumnName("combustible");
            entity.Property(e => e.Destacado)
                .HasDefaultValueSql("'0'")
                .HasColumnName("destacado");
            entity.Property(e => e.Estado)
                .HasMaxLength(20)
                .HasColumnName("estado");
            entity.Property(e => e.FechaDestacado)
                .HasColumnType("timestamp")
                .HasColumnName("fechaDestacado");
            entity.Property(e => e.Kilometraje).HasColumnName("kilometraje");
            entity.Property(e => e.Marca)
                .HasMaxLength(50)
                .HasColumnName("marca");
            entity.Property(e => e.Modelo)
                .HasMaxLength(50)
                .HasColumnName("modelo");
            entity.Property(e => e.Precio)
                .HasPrecision(12, 2)
                .HasColumnName("precio");
            entity.Property(e => e.UsuarioId).HasColumnName("usuarioId");

            entity.HasOne(d => d.Usuario).WithMany(p => p.TbVehiculos)
                .HasForeignKey(d => d.UsuarioId)
                .HasConstraintName("tb_vehiculos_ibfk_1");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
