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
    public class ClienteController : ControllerBase
    {
        public readonly ProyectoPrestamosContext _prestamosContext;

        public ClienteController(ProyectoPrestamosContext _context)
        {
            _prestamosContext = _context;
        }

        [HttpGet]
        [Route("Lista")]
        [EnableCors("ReglasCors")]
        public IActionResult Lista()
        {
            List<Cliente> lista = new List<Cliente>();

            try
            {
                lista = _prestamosContext.Clientes.ToList();
                return StatusCode(StatusCodes.Status200OK, new { mensaje = "ok", response = lista });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status200OK, new { mensaje = ex.Message, response = lista });
            }
        }

        [EnableCors("ReglasCors")]
        [HttpPost]
        [Route("Guardar")]
        public IActionResult Guardar([FromBody] Cliente objeto)
        {
            try
            {
                Prestamista oPrestamista = _prestamosContext.Prestamistas.Find(objeto.IdPrestamista);

                objeto.oPrestamista = oPrestamista;

                _prestamosContext.Clientes.Add(objeto);
                _prestamosContext.SaveChanges();

                Cliente oCliente = _prestamosContext.Clientes.Find(objeto.IdCliente);

                return StatusCode(StatusCodes.Status200OK, new { mensaje = "Cliente registrado", response = oCliente });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status400BadRequest, new { mensaje = ex.Message });
            }
        }

        [EnableCors("ReglasCors")]
        [HttpGet]
        [Route("ObtenerClientesXPrestamista/{idPrestamista}")]
        public IActionResult ObtenerClientesXPrestamista(int idPrestamista)
        {
            List<Cliente> clientes = new List<Cliente>();

            try
            {
                clientes = _prestamosContext.Clientes.Where(c => c.IdPrestamista == idPrestamista).ToList();

                if(clientes.Count > 0)
                {
                    return StatusCode(StatusCodes.Status200OK, new { mensaje = "ok", response = clientes });
                }

                return StatusCode(StatusCodes.Status200OK, new { mensaje = "Prestamista sin clientes" });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status400BadRequest, new { mensaje = ex.Message });
            }
        }

        [EnableCors("ReglasCors")]
        [HttpDelete]
        [Route("Eliminar/{idCliente}")]
        public IActionResult Eliminar(int idCliente)
        {
            Cliente oCliente = _prestamosContext.Clientes.Find(idCliente);

            if (oCliente == null)
            {
                return BadRequest("Este cliente no existe");
            }

            try
            {
                _prestamosContext.Clientes.Remove(oCliente);
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
