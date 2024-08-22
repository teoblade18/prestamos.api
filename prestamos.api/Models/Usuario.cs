using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;
using prestamos.api.Models;
using prestamos.api.Interfaces;

public partial class Usuario : IUsuario
{
    public int IdUsuario { get; set; }

    public string NombreUsuario { get; set; } = null!;

    public string Contraseña { get; set; } = null!;

    public string Email { get; set; } = null!;

    [JsonIgnore]
    public virtual ICollection<Prestamista> Prestamista { get; set; } = new List<Prestamista>();
}
