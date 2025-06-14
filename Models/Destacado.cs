﻿using System;
using System.Collections.Generic;

namespace concesionaria_menichetti.Models;

public partial class Destacado
{
    public int Id { get; set; }

    public int? VehiculoId { get; set; }

    public DateTime? FechaInicio { get; set; }

    public DateTime? FechaFin { get; set; }

    public virtual Vehiculo? Vehiculo { get; set; }
}
