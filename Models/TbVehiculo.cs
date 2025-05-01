using System;
using System.Collections.Generic;

namespace concesionaria_menichetti.Models;

public partial class TbVehiculo
{
    public int Id { get; set; }

    public int? UsuarioId { get; set; }

    public string? Marca { get; set; }

    public string? Modelo { get; set; }

    public int? Año { get; set; }

    public decimal? Precio { get; set; }

    public int? Kilometraje { get; set; }

    public string? Combustible { get; set; }

    public string? Estado { get; set; }

    public bool? Destacado { get; set; }

    public DateTime? FechaDestacado { get; set; }

    public virtual ICollection<TbAccesosPagado> TbAccesosPagados { get; set; } = new List<TbAccesosPagado>();

    public virtual ICollection<TbDestacado> TbDestacados { get; set; } = new List<TbDestacado>();

    public virtual ICollection<TbFavorito> TbFavoritos { get; set; } = new List<TbFavorito>();

    public virtual ICollection<TbFotosVehiculo> TbFotosVehiculos { get; set; } = new List<TbFotosVehiculo>();

    public virtual ICollection<TbReporte> TbReportes { get; set; } = new List<TbReporte>();

    public virtual TbUsuario? Usuario { get; set; }
}
