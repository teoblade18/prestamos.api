using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace prestamos.api.Models;

public partial class Cliente
{
    public int IdCliente { get; set; }

    public string Cedula { get; set; } = null!;

    public string? Nombre { get; set; }

    public decimal? Puntaje { get; set; }

    public decimal? MaxPrestar { get; set; }

    public string? NumeroCuenta { get; set; }

    public int? IdPrestamista { get; set; }

    public virtual Prestamista? oPrestamista { get; set; }

    [JsonIgnore]
    public virtual ICollection<Prestamo> Prestamos { get; set; } = new List<Prestamo>();
}
