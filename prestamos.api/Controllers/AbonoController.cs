using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using prestamos.api.Models;

namespace prestamos.api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AbonoController : ControllerBase
    {
        public readonly ProyectoPrestamosContext _prestamosContext;

        public AbonoController(ProyectoPrestamosContext _context)
        {
            _prestamosContext = _context;
        }

        [EnableCors("ReglasCors")]
        [HttpPost]
        [Route("Guardar")]
        public IActionResult Guardar([FromBody] Abono abonoGuardar)
        {
            try
            {

                _prestamosContext.Abonos.Add(abonoGuardar);
                _prestamosContext.SaveChanges();

                return StatusCode(StatusCodes.Status200OK, new { mensaje = "ok" });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status400BadRequest, new { mensaje = ex.Message });
            }
        }

        [EnableCors("ReglasCors")]
        [HttpDelete]
        [Route("Eliminar/{idAbono}")]
        public IActionResult Eliminar(int idAbonoEliminar)
        {
            Abono oAbono = _prestamosContext.Abonos.Find(idAbonoEliminar);

            if (oAbono == null)
            {
                return StatusCode(StatusCodes.Status400BadRequest, new { mensaje = "Este abono no existe" });
            }

            try
            {
                _prestamosContext.Abonos.Remove(oAbono);
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
