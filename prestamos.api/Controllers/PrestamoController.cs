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
            try
            {
                var prestamos = ObtenerPrestamos(idPrestamista, true);
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
            try
            {
                var prestamos = ObtenerPrestamos(idPrestamista, false);
                var historialprestamos = CrearHistorialPrestamos(prestamos);
                return StatusCode(StatusCodes.Status200OK, new { mensaje = "ok", response = historialprestamos });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status400BadRequest, new { mensaje = ex.Message });
            }
        }

        /// <summary>
        /// Devuelve los préstamos presentes en la base de datos asociados a un prestamista específico.
        /// En caso de que la consulta sea para el apartado "Prestamos" se buscan solo los activos
        /// Si la consulta es para el apartado "Historial de prestamos" se entregan todos los préstamos.
        /// </summary>
        /// <param name="idPrestamista"> Id del prestamista que se desea filtrar.</param>
        /// <param name="soloActivos"> Indica si se deben buscar solo los préstamos activos (true) o todos los préstamos (false).</param>
        /// <returns>Lista de préstamos asociados a un prestamista.</returns>
        private List<Prestamo> ObtenerPrestamos(int idPrestamista, bool soloActivos)
        {
            var prestamos = _prestamosContext.Prestamos
                .Where(p => p.IdPrestamista == idPrestamista)
                .Include(c => c.oCliente)
                .Include(a => a.Abonos)
                .Include(i => i.Intereses);

            if (soloActivos)
            {
                return prestamos.Where(p => p.Estado == "Abonado" || p.Estado == "Impago").OrderByDescending(p => p.Estado).ToList();
            }

            return prestamos.OrderByDescending(p => p.Estado).ToList();
        }

        /// <summary>
        /// Construye una lista de objetos HistorialPrestamo a partir de una lista de préstamos.
        /// </summary>
        /// <param name="prestamos">Lista de préstamos del prestamista.</param>
        /// <returns>Lista de historial de préstamos con totales calculados y deuda actual.</returns>
        private List<HistorialPrestamo> CrearHistorialPrestamos(List<Prestamo> prestamos)
        {
            var historialprestamos = new List<HistorialPrestamo>();

            foreach (var prestamo in prestamos)
            {
                int totalAbonos = (int)(prestamo.Abonos.Sum(a => a.Valor ?? 0));
                int totalIntereses = (int)(prestamo.Intereses.Sum(i => i.Valor ?? 0));
                int deudaActual = (int)(prestamo.MontoInicial + totalIntereses - totalAbonos);

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

                historialprestamos.Add(historialPrestamo);
            }

            return historialprestamos;
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
