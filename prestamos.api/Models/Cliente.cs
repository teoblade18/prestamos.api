using System;
using System.Collections.Generic;

namespace prestamos.api.Models;

public partial class Cliente
{
    public int IdCliente { get; set; }

    public string Cedula { get; set; } = null!;

    public string? Nombre { get; set; }

    public decimal? Puntaje { get; set; }

    public string? IdPrestamista { get; set; }

    public virtual Prestamista? oPrestamista { get; set; }

    public virtual ICollection<Prestamo> Prestamos { get; set; } = new List<Prestamo>();
}
