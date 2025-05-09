using System;
using System.Collections.Generic;

namespace concesionaria_menichetti.Models;

public partial class EmpleadosConcesionarium
{
    public int Id { get; set; }

    public int? ConcesionariaId { get; set; }

    public int? UsuarioId { get; set; }

    public virtual Concesionaria? Concesionaria { get; set; }

    public virtual Usuario? Usuario { get; set; }
}
