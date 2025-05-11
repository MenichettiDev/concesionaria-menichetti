using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace concesionaria_menichetti.Models;

public partial class Modelo
{
    public int Id { get; set; }

    public string Descripcion { get; set; } = null!;

    public int IdMarca { get; set; }

    [ForeignKey("IdMarca")]
    public virtual Marca Marca { get; set; } = null!;

    public virtual ICollection<Vehiculo> Vehiculos { get; set; } = new List<Vehiculo>();
}
