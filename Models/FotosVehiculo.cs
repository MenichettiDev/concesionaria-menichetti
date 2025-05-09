using System;
using System.Collections.Generic;

namespace concesionaria_menichetti.Models;

public partial class FotosVehiculo
{
    public int Id { get; set; }

    public int? VehiculoId { get; set; }

    public string FotoArchivo { get; set; } = null!;

    public string? Descripcion { get; set; }

    public DateTime? Fecha { get; set; }

    public virtual Vehiculo? Vehiculo { get; set; }
}
