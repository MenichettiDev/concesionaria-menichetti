﻿using System;
using System.Collections.Generic;

namespace concesionaria_menichetti.Models;

public partial class ContratosPlane
{
    public int Id { get; set; }

    public int? ConcesionariaId { get; set; }

    public int? PlanId { get; set; }

    public DateOnly? FechaInicio { get; set; }

    public DateOnly? FechaFin { get; set; }

    public bool? Activo { get; set; }

    public virtual Concesionaria? Concesionaria { get; set; }

    public virtual PlanesConcesionarium? Plan { get; set; }
}
