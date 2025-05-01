using System;
using System.Collections.Generic;

namespace concesionaria_menichetti.Models;

public partial class TbUsuario
{
    public int Id { get; set; }

    public string? Nombre { get; set; }

    public string Email { get; set; } = null!;

    public string Contraseña { get; set; } = null!;

    public bool? EsConcesionaria { get; set; }

    public bool? Verificado { get; set; }

    public string? Telefono { get; set; }

    public string? Ubicacion { get; set; }

    public string? FotoPerfil { get; set; }

    public bool? Activo { get; set; }

    public virtual ICollection<TbAccesosPagado> TbAccesosPagados { get; set; } = new List<TbAccesosPagado>();

    public virtual ICollection<TbComentario> TbComentarioCompradors { get; set; } = new List<TbComentario>();

    public virtual ICollection<TbComentario> TbComentarioVendedors { get; set; } = new List<TbComentario>();

    public virtual ICollection<TbConcesionaria> TbConcesionaria { get; set; } = new List<TbConcesionaria>();

    public virtual ICollection<TbEmpleadosConcesionarium> TbEmpleadosConcesionaria { get; set; } = new List<TbEmpleadosConcesionarium>();

    public virtual ICollection<TbFavorito> TbFavoritos { get; set; } = new List<TbFavorito>();

    public virtual ICollection<TbPago> TbPagos { get; set; } = new List<TbPago>();

    public virtual ICollection<TbReporte> TbReportes { get; set; } = new List<TbReporte>();

    public virtual ICollection<TbSuscripcione> TbSuscripciones { get; set; } = new List<TbSuscripcione>();

    public virtual ICollection<TbVehiculo> TbVehiculos { get; set; } = new List<TbVehiculo>();
}
