using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;
using prestamos.api.Models;

public partial class Usuario
{
    public int IdUsuario { get; set; }

    public string NombreUsuario { get; set; } = null!;

    public string Contraseña { get; set; } = null!;

    public string Email { get; set; } = null!;

    [JsonIgnore]
    public virtual ICollection<Prestamista> Prestamista { get; set; } = new List<Prestamista>();
}
