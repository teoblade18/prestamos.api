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
        public IActionResult Guardar([FromBody] Prestamo objeto)
        {
            try
            {
                _prestamosContext.Prestamos.Add(objeto);
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
                numeroPrestamos = _prestamosContext.Prestamos.Where(p => p.IdCliente == idCliente).Count();
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
                prestamos = _prestamosContext.Prestamos.Where(p => p.IdPrestamista == p.IdPrestamista && (p.Estado == "Abonado" || p.Estado == "Impago"))
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

    }
}
