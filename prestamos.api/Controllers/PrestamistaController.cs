using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using prestamos.api.Models;

namespace prestamos.api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PrestamistaController : ControllerBase
    {
        public readonly ProyectoPrestamosContext _prestamosContext;

        public PrestamistaController(ProyectoPrestamosContext _context)
        {
            _prestamosContext = _context;
        }

        [HttpGet]
        [Route("Lista")]

        public IActionResult Lista() {
            List<Prestamista> lista = new List<Prestamista>();

            try
            {
                lista = _prestamosContext.Prestamistas.ToList();

                return StatusCode(StatusCodes.Status200OK, new { mensaje = "ok", response = lista });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status200OK, new { mensaje = ex.Message, response = lista });
            }
        }
    }
}
