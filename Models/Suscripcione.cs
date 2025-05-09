using System;
using System.Collections.Generic;

namespace concesionaria_menichetti.Models;

public partial class Suscripcione
{
    public int Id { get; set; }

    public int? UsuarioId { get; set; }

    public DateOnly? FechaInicio { get; set; }

    public DateOnly? FechaFin { get; set; }

    public bool? Activa { get; set; }

    public virtual Usuario? Usuario { get; set; }
}
