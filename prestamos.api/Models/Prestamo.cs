﻿using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace prestamos.api.Models;

public partial class Prestamo
{
    public int IdPrestamo { get; set; }

    public int? IdCliente { get; set; }

    public int? IdPrestamista { get; set; }

    public DateOnly? FechaInicial { get; set; }

    public decimal? Porcentaje { get; set; }

    public string? TipoIntereses { get; set; }

    public decimal? DiaCorte { get; set; }

    public decimal? MontoInicial { get; set; }

    public decimal? MontoReal { get; set; }

    public DateOnly? FechaPago { get; set; }

    public virtual Cliente? oCliente { get; set; }

    public virtual Prestamista? oPrestamista { get; set; }

    [JsonIgnore]
    public virtual ICollection<Interes> Intereses { get; set; } = new List<Interes>();
    [JsonIgnore]
    public virtual ICollection<Abono> Abonos { get; set; } = new List<Abono>();
}
