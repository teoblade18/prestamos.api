using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using prestamos.api.Models;

namespace prestamos.api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PrestamoController : ControllerBase
    {
        public readonly ProyectoPrestamosContext _prestamosContext;

        public PrestamoController(ProyectoPrestamosContext _context)
        {
            _prestamosContext = _context;
        }


        [EnableCors("ReglasCors")]
        [HttpPost]
        [Route("Guardar")]
        public IActionResult Guardar([FromBody] Prestamo prestamoGuardar)
        {
            try
            {
                _prestamosContext.Prestamos.Add(prestamoGuardar);
                _prestamosContext.SaveChanges();

                return StatusCode(StatusCodes.Status200OK, new { mensaje = "ok" });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status400BadRequest, new { mensaje = ex.Message });
            }
        }

        [EnableCors("ReglasCors")]
        [HttpGet]
        [Route("ConsultarNumeroPrestamosXCliente/{idCliente}")]
        public IActionResult ConsultarNumeroPrestamosXCliente(int idCliente)
        {
            int numeroPrestamos;

            try
            {
                numeroPrestamos = _prestamosContext.Prestamos.Where(p => p.IdCliente == idCliente && (p.Estado == "Abonado" || p.Estado == "Impago")).Count();
                return StatusCode(StatusCodes.Status200OK, new { mensaje = "ok", response = numeroPrestamos });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status400BadRequest, new { mensaje = ex.Message });
            }
        }

        [EnableCors("ReglasCors")]
        [HttpGet]
        [Route("ConsultarPrestamosXPrestamista/{idPrestamista}")]
        public IActionResult ConsultarPrestamosXPrestamista(int idPrestamista)
        {
            List<Prestamo> prestamos = new List<Prestamo>();

            try
            {
                prestamos = _prestamosContext.Prestamos.Where(p => p.IdPrestamista == idPrestamista && (p.Estado == "Abonado" || p.Estado == "Impago"))
                                                       .Include(c => c.oCliente)
                                                       .Include(a => a.Abonos)
                                                       .Include(i => i.Intereses)
                                                       .OrderByDescending(p => p.Estado).ToList();

                return StatusCode(StatusCodes.Status200OK, new { mensaje = "ok", response = prestamos });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status400BadRequest, new { mensaje = ex.Message });
            }
        }

        [EnableCors("ReglasCors")]
        [HttpGet]
        [Route("ConsultarHistorialPrestamos/{idPrestamista}")]
        public IActionResult ConsultarHistorialPrestamos(int idPrestamista)
        {
            List<HistorialPrestamo> historialprestamos = new List<HistorialPrestamo>();

            try
            {
                // Consultar los préstamos del prestamista incluyendo los abonos e intereses relacionados
                var prestamos = _prestamosContext.Prestamos
                    .Where(p => p.IdPrestamista == idPrestamista)
                    .Include(c => c.oCliente)
                    .Include(a => a.Abonos)
                    .Include(i => i.Intereses)
                    .OrderByDescending(p => p.Estado)
                    .ToList();

                // Calcular los totales y la deuda actual para cada préstamo
                foreach (var prestamo in prestamos)
                {
                    int totalAbonos = (int)(prestamo.Abonos.Sum(a => a.Valor ?? 0));
                    int totalIntereses = (int)(prestamo.Intereses.Sum(i => i.Valor ?? 0));
                    int deudaActual = (int)(prestamo.MontoInicial + totalIntereses - totalAbonos);

                    // Crear un objeto HistorialPrestamo con los datos del préstamo y los cálculos realizados
                    var historialPrestamo = new HistorialPrestamo
                    {
                        IdPrestamo = prestamo.IdPrestamo,
                        IdCliente = prestamo.IdCliente,
                        IdPrestamista = prestamo.IdPrestamista,
                        FechaInicial = prestamo.FechaInicial,
                        FechaFinal = prestamo.FechaFinal,
                        FechaProximoPago = prestamo.FechaProximoPago,
                        Porcentaje = prestamo.Porcentaje,
                        TipoIntereses = prestamo.TipoIntereses,
                        DiaCorte = prestamo.DiaCorte,
                        MontoInicial = prestamo.MontoInicial,
                        MontoReal = prestamo.MontoReal,
                        FechaPago = prestamo.FechaPago,
                        Estado = prestamo.Estado,
                        oCliente = prestamo.oCliente,
                        oPrestamista = prestamo.oPrestamista,
                        Intereses = prestamo.Intereses,
                        Abonos = prestamo.Abonos,
                        TotalAbonos = totalAbonos,
                        TotalIntereses = totalIntereses,
                        DeudaActual = deudaActual
                    };

                    // Agregar el objeto HistorialPrestamo a la lista
                    historialprestamos.Add(historialPrestamo);
                }

                return StatusCode(StatusCodes.Status200OK, new { mensaje = "ok", response = historialprestamos });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status400BadRequest, new { mensaje = ex.Message });
            }
        }

        [EnableCors("ReglasCors")]
        [HttpPut]
        [Route("Cancelar")]
        public IActionResult Cancelar([FromBody] Prestamo prestamoCancelar)
        {
            Prestamo oPrestamo = _prestamosContext.Prestamos.Find(prestamoCancelar.IdPrestamo);

            if (oPrestamo == null)
            {
                return StatusCode(StatusCodes.Status400BadRequest, new { mensaje = "Este préstamo no existe" });
            }

            try
            {
                oPrestamo.Estado = "Cancelado";
                oPrestamo.FechaFinal = DateOnly.FromDateTime(DateTime.Today);

                _prestamosContext.Prestamos.Update(oPrestamo);
                _prestamosContext.SaveChanges();

                return StatusCode(StatusCodes.Status200OK, new { mensaje = "ok" });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status400BadRequest, new { mensaje = ex.Message });
            }
        }

    }
}
