using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace prestamos.api.Models;

public partial class HistorialPrestamo : Prestamo
{
    public double TotalAbonos { get; set; }
    public double TotalIntereses { get; set; }
    public double DeudaActual { get; set; }
}
