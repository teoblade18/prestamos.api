using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace prestamos.api.Models;

public partial class Prestamo
{
    public int IdPrestamo { get; set; }

    public int? IdCliente { get; set; }

    public int? IdPrestamista { get; set; }

    public DateOnly? FechaInicial { get; set; }
    public DateOnly? FechaFinal { get; set; }

    public DateOnly? FechaProximoPago { get; set; }

    public decimal? Porcentaje { get; set; }

    public string? TipoIntereses { get; set; }

    public decimal? DiaCorte { get; set; }

    public decimal? MontoInicial { get; set; }

    public decimal? MontoReal { get; set; }

    public DateOnly? FechaPago { get; set; }

    public string? Estado { get; set; }

    public virtual Cliente? oCliente { get; set; }

    public virtual Prestamista? oPrestamista { get; set; }

    public virtual ICollection<Interes> Intereses { get; set; } = new List<Interes>();

    public virtual ICollection<Abono> Abonos { get; set; } = new List<Abono>();
}
