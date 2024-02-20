using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using prestamos.api.Models;

namespace prestamos.api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class InteresController : ControllerBase
    {
        public readonly ProyectoPrestamosContext _prestamosContext;

        public InteresController(ProyectoPrestamosContext _context)
        {
            _prestamosContext = _context;
        }

        [EnableCors("ReglasCors")]
        [HttpPost]
        [Route("Guardar")]
        public IActionResult Guardar([FromBody] Interes objeto)
        {
            try
            {

                _prestamosContext.Intereses.Add(objeto);
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
        [Route("Eliminar/{idInteres}")]
        public IActionResult Eliminar(int idInteres)
        {
            Interes oInteres = _prestamosContext.Intereses.Find(idInteres);

            if (oInteres == null)
            {
                return BadRequest("Este interés no existe");
            }

            try
            {
                _prestamosContext.Intereses.Remove(oInteres);
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
