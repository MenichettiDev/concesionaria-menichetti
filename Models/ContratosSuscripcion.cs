using System;
using System.Collections.Generic;

namespace concesionaria_menichetti.Models;

public partial class ContratosSuscripcion
{
    public int Id { get; set; }

    public int? UsuarioId { get; set; }

    public int SuscripcionId { get; set; }

    public DateOnly? FechaInicio { get; set; }

    public DateOnly? FechaFin { get; set; }

    public bool? Activo { get; set; }

    public virtual Suscripcione Suscripcion { get; set; } = null!;

    public virtual Usuario? Usuario { get; set; }
}
