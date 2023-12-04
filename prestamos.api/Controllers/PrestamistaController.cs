using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using prestamos.api.Models;
using Microsoft.AspNetCore.Cors;

namespace prestamos.api.Controllers
{
    [EnableCors("ReglasCors")]
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
        [EnableCors("ReglasCors")]
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

        [EnableCors("ReglasCors")]
        [HttpGet]
        [Route("Obtener/{idPrestamista}")]
        public IActionResult Obtener(String idPrestamista)
        {
            Prestamista oPrestamista = _prestamosContext.Prestamistas.Find(idPrestamista);

            if(oPrestamista == null)
            {
                return BadRequest("Prestamista no encontrado");
            }

            try
            {
                oPrestamista = _prestamosContext.Prestamistas.Where(p => p.IdPrestamista == idPrestamista).FirstOrDefault();
                return StatusCode(StatusCodes.Status200OK, new { mensaje = "ok", response = oPrestamista });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status200OK, new { mensaje = ex.Message, response = oPrestamista });
            }
        }

        [EnableCors("ReglasCors")]
        [HttpPost]
        [Route("Guardar")]
        public IActionResult Guardar([FromBody] Prestamista objeto)
        {
            try
            {
                _prestamosContext.Prestamistas.Add(objeto);
                _prestamosContext.SaveChanges();

                return StatusCode(StatusCodes.Status200OK, new { mensaje = "Prestamista registrado"});
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status200OK, new { mensaje = ex.Message });
            }
        }

        [EnableCors("ReglasCors")]
        [HttpPut]
        [Route("Editar")]
        public IActionResult Editar([FromBody] Prestamista objeto)
        {
            Prestamista oPrestamista = _prestamosContext.Prestamistas.Find(objeto.IdPrestamista);

            if(oPrestamista == null)
            {
                return BadRequest("Este prestamista no existe");
            }

            try
            {
                oPrestamista.Nombre = objeto.Nombre is null ? oPrestamista.Nombre : objeto.Nombre;
                oPrestamista.Capital = objeto.Capital is null ? oPrestamista.Capital : objeto.Capital;
                oPrestamista.NumeroCuenta = objeto.NumeroCuenta is null ? oPrestamista.NumeroCuenta : objeto.NumeroCuenta;

                _prestamosContext.Prestamistas.Update(oPrestamista);
                _prestamosContext.SaveChanges();

                return StatusCode(StatusCodes.Status200OK, new { mensaje = "Prestamista actualizado" });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status200OK, new { mensaje = ex.Message });
            }
        }

        [EnableCors("ReglasCors")]
        [HttpDelete]
        [Route("Eliminar/{idPrestamista}")]
        public IActionResult Eliminar(String idPrestamista)
        {
            Prestamista oPrestamista = _prestamosContext.Prestamistas.Find(idPrestamista);

            if (oPrestamista == null)
            {
                return BadRequest("Este prestamista no existe");
            }

            try
            {
                _prestamosContext.Prestamistas.Remove(oPrestamista);
                _prestamosContext.SaveChanges();

                return StatusCode(StatusCodes.Status200OK, new { mensaje = "Prestamista eliminado" });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status200OK, new { mensaje = ex.Message });
            }
        }
    }
}
