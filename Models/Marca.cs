using System;
using System.Collections.Generic;

namespace concesionaria_menichetti.Models;

public partial class Marca
{
    public int Id { get; set; }

    public string Descripcion { get; set; } = null!;

    public virtual ICollection<Modelo> Modelos { get; set; } = new List<Modelo>();
}
