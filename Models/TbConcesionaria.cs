using System;
using System.Collections.Generic;

namespace concesionaria_menichetti.Models;

public partial class TbConcesionaria
{
    public int Id { get; set; }

    public int? UsuarioId { get; set; }

    public string? Nombre { get; set; }

    public string? Cuit { get; set; }

    public string? Direccion { get; set; }

    public virtual ICollection<TbContratosPlane> TbContratosPlanes { get; set; } = new List<TbContratosPlane>();

    public virtual ICollection<TbEmpleadosConcesionarium> TbEmpleadosConcesionaria { get; set; } = new List<TbEmpleadosConcesionarium>();

    public virtual TbUsuario? Usuario { get; set; }
}
