using System;
using System.Collections.Generic;

namespace concesionaria_menichetti.Models;

public partial class TbComentario
{
    public int Id { get; set; }

    public int? CompradorId { get; set; }

    public int? VendedorId { get; set; }

    public string? Texto { get; set; }

    public int? Calificacion { get; set; }

    public DateTime? Fecha { get; set; }

    public virtual TbUsuario? Comprador { get; set; }

    public virtual TbUsuario? Vendedor { get; set; }
}
