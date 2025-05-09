using System;
using System.Collections.Generic;

namespace concesionaria_menichetti.Models;

public partial class Pago
{
    public int Id { get; set; }

    public int? UsuarioId { get; set; }

    public string? Tipo { get; set; }

    public decimal? Monto { get; set; }

    public DateTime? Fecha { get; set; }

    public string? Detalle { get; set; }

    public virtual Usuario? Usuario { get; set; }
}
