using System;
using System.Collections.Generic;

namespace prestamos.api.Models;

public partial class Prestamista
{
    public string IdPrestamista { get; set; } = null!;

    public string? Nombre { get; set; }

    public decimal? Capital { get; set; }

    public decimal? NumeroCuenta { get; set; }

    public virtual ICollection<Prestamo> Prestamos { get; set; } = new List<Prestamo>();
}
