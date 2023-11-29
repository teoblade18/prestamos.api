using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace prestamos.api.Models;

public partial class Cliente
{
    public string Cedula { get; set; } = null!;

    public string? Nombre { get; set; }

    public decimal? Puntaje { get; set; }

    [JsonIgnore]
    public virtual ICollection<Prestamo> Prestamos { get; set; } = new List<Prestamo>();
}
