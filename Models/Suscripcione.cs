using System;
using System.Collections.Generic;

namespace concesionaria_menichetti.Models;

public partial class Suscripcione
{
    public int Id { get; set; }

    public string Nombre { get; set; } = null!;

    public decimal Precio { get; set; }

    public string Descripcion { get; set; } = null!;

    public int CantidadPublicaciones { get; set; }

    public virtual ICollection<ContratosSuscripcion> ContratosSuscripcions { get; set; } = new List<ContratosSuscripcion>();
}
