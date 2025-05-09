using System;
using System.Collections.Generic;

namespace concesionaria_menichetti.Models;

public partial class AccesosPagado
{
    public int Id { get; set; }

    public int? UsuarioId { get; set; }

    public int? VehiculoId { get; set; }

    public DateTime? Fecha { get; set; }

    public virtual Usuario? Usuario { get; set; }

    public virtual Vehiculo? Vehiculo { get; set; }
}
