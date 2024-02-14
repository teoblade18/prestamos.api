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
        public IActionResult Guardar([FromBody] Abono objeto)
        {
            try
            {

                _prestamosContext.Abonos.Add(objeto);
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
        public IActionResult Eliminar(int idAbono)
        {
            Abono oAbono = _prestamosContext.Abonos.Find(idAbono);

            if (oAbono == null)
            {
                return BadRequest("Este abono no existe");
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
