using System;
using System.Collections.Generic;

namespace concesionaria_menichetti.Models;

public partial class Usuario
{
    public int Id { get; set; }

    public string Rol { get; set; } = null!;

    public string? Nombre { get; set; }

    public string Email { get; set; } = null!;

    public string Contraseña { get; set; } = null!;

    public bool? EsConcesionaria { get; set; }

    public bool? Verificado { get; set; }

    public string? Telefono { get; set; }

    public string? Ubicacion { get; set; }

    public string? FotoPerfil { get; set; }

    public bool? Activo { get; set; }

    public virtual ICollection<AccesosPagado> AccesosPagados { get; set; } = new List<AccesosPagado>();

    public virtual ICollection<Comentario> ComentarioCompradors { get; set; } = new List<Comentario>();

    public virtual ICollection<Comentario> ComentarioVendedors { get; set; } = new List<Comentario>();

    public virtual ICollection<Concesionaria> Concesionaria { get; set; } = new List<Concesionaria>();

    public virtual ICollection<ContratosSuscripcion> ContratosSuscripcions { get; set; } = new List<ContratosSuscripcion>();

    public virtual ICollection<EmpleadosConcesionarium> EmpleadosConcesionaria { get; set; } = new List<EmpleadosConcesionarium>();

    public virtual ICollection<Favorito> Favoritos { get; set; } = new List<Favorito>();

    public virtual ICollection<Pago> Pagos { get; set; } = new List<Pago>();

    public virtual ICollection<Reporte> Reportes { get; set; } = new List<Reporte>();

    public virtual ICollection<Vehiculo> Vehiculos { get; set; } = new List<Vehiculo>();
}
