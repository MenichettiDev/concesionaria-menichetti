using System;
using System.Collections.Generic;

namespace concesionaria_menichetti.Models;

public partial class TbFavorito
{
    public int Id { get; set; }

    public int? UsuarioId { get; set; }

    public int? VehiculoId { get; set; }

    public virtual TbUsuario? Usuario { get; set; }

    public virtual TbVehiculo? Vehiculo { get; set; }
}
