using System;
using System.Collections.Generic;

namespace concesionaria_menichetti.Models;

public partial class Vehiculo
{
    public int Id { get; set; }

    public int? UsuarioId { get; set; }

    public int? IdModelo { get; set; }

    public int? Anio { get; set; }

    public decimal? Precio { get; set; }

    public int? Kilometraje { get; set; }

    public string? Combustible { get; set; }

    public bool? Destacado { get; set; }

    public DateTime? FechaDestacado { get; set; }

    public int Estado { get; set; }

    public virtual ICollection<AccesosPagado> AccesosPagados { get; set; } = new List<AccesosPagado>();

    public virtual ICollection<Destacado> Destacados { get; set; } = new List<Destacado>();

    public virtual ICollection<Favorito> Favoritos { get; set; } = new List<Favorito>();

    public virtual ICollection<FotosVehiculo> FotosVehiculos { get; set; } = new List<FotosVehiculo>();

    public virtual Modelo Modelo { get; set; } = null!;

    public virtual ICollection<Reporte> Reportes { get; set; } = new List<Reporte>();

    public virtual Usuario? Usuario { get; set; }
}
