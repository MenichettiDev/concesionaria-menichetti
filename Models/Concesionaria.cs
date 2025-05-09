using System;
using System.Collections.Generic;

namespace concesionaria_menichetti.Models;

public partial class Concesionaria
{
    public int Id { get; set; }

    public int? UsuarioId { get; set; }

    public string? Nombre { get; set; }

    public string? Cuit { get; set; }

    public string? Direccion { get; set; }

    public virtual ICollection<ContratosPlane> ContratosPlanes { get; set; } = new List<ContratosPlane>();

    public virtual ICollection<EmpleadosConcesionarium> EmpleadosConcesionaria { get; set; } = new List<EmpleadosConcesionarium>();

    public virtual Usuario? Usuario { get; set; }
}
