using System;
using System.Collections.Generic;

namespace concesionaria_menichetti.Models;

public partial class PlanesConcesionarium
{
    public int Id { get; set; }

    public string? Nombre { get; set; }

    public decimal? Precio { get; set; }

    public string? Descripcion { get; set; }

    public int? CantidadPublicaciones { get; set; }

    public virtual ICollection<ContratosPlane> ContratosPlanes { get; set; } = new List<ContratosPlane>();
}
