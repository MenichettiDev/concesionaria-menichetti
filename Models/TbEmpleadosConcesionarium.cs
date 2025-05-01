using System;
using System.Collections.Generic;

namespace concesionaria_menichetti.Models;

public partial class TbEmpleadosConcesionarium
{
    public int Id { get; set; }

    public int? ConcesionariaId { get; set; }

    public int? UsuarioId { get; set; }

    public virtual TbConcesionaria? Concesionaria { get; set; }

    public virtual TbUsuario? Usuario { get; set; }
}
