using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace prestamos.api.Models;

public partial class HistorialPrestamo : Prestamo
{
    public decimal TotalAbonos { get; set; }
    public decimal TotalIntereses { get; set; }
    public decimal DeudaActual { get; set; }

    public HistorialPrestamo()
    {
    }

    public HistorialPrestamo(Prestamo prestamo)
    {
        // Copiar propiedades de la clase base
        IdPrestamo = prestamo.IdPrestamo;
        IdCliente = prestamo.IdCliente;
        IdPrestamista = prestamo.IdPrestamista;
        FechaInicial = prestamo.FechaInicial;
        FechaFinal = prestamo.FechaFinal;
        FechaProximoPago = prestamo.FechaProximoPago;
        Porcentaje = prestamo.Porcentaje;
        TipoIntereses = prestamo.TipoIntereses;
        DiaCorte = prestamo.DiaCorte;
        MontoInicial = prestamo.MontoInicial;
        MontoReal = prestamo.MontoReal;
        FechaPago = prestamo.FechaPago;
        Estado = prestamo.Estado;
        oCliente = prestamo.oCliente;
        oPrestamista = prestamo.oPrestamista;
        Intereses = prestamo.Intereses;
        Abonos = prestamo.Abonos;

        // Calcular los atributos adicionales
        TotalAbonos = (decimal)(Abonos.Sum(a => a.Valor ?? 0));
        TotalIntereses = (decimal)(Intereses.Sum(i => i.Valor ?? 0));
        DeudaActual = (decimal)(MontoInicial + TotalIntereses - TotalAbonos ?? 0);
    }
}
