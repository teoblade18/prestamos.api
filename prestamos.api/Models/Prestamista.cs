using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace prestamos.api.Models;

public partial class Prestamista
{
    public string IdPrestamista { get; set; } = null!;

    public string? Nombre { get; set; }

    public decimal? Capital { get; set; }

    public string? NumeroCuenta { get; set; }

    [JsonIgnore]
    public virtual ICollection<Prestamo> Prestamos { get; set; } = new List<Prestamo>();
}
