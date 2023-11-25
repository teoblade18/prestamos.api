using System;
using System.Collections.Generic;

namespace prestamos.api.Models;

public partial class Interes
{
    public int IdInteres { get; set; }

    public int? IdPrestamo { get; set; }

    public DateOnly? Fecha { get; set; }

    public decimal? Valor { get; set; }

    public string? Tipo { get; set; }

    public virtual Prestamo? oPrestamo { get; set; }
}
