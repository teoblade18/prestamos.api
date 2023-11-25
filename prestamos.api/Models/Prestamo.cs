using System;
using System.Collections.Generic;

namespace prestamos.api.Models;

public partial class Prestamo
{
    public int IdPrestamo { get; set; }

    public string? CedulaCliente { get; set; }

    public string? IdPrestamista { get; set; }

    public DateOnly? FechaInicial { get; set; }

    public decimal? Porcentaje { get; set; }

    public string? TipoIntereses { get; set; }

    public decimal? DiaCorte { get; set; }

    public decimal? MontoInicial { get; set; }

    public decimal? MontoReal { get; set; }

    public DateOnly? FechaPago { get; set; }

    public virtual ICollection<Abono> Abonos { get; set; } = new List<Abono>();

    public virtual Cliente? CedulaClienteNavigation { get; set; }

    public virtual Prestamista? IdPrestamistaNavigation { get; set; }

    public virtual ICollection<Interese> Intereses { get; set; } = new List<Interese>();
}
