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
    public class UsuarioController : ControllerBase
    {
        public readonly ProyectoPrestamosContext _prestamosContext;

        public UsuarioController(ProyectoPrestamosContext _context)
        {
            _prestamosContext = _context;
        }

        [EnableCors("ReglasCors")]
        [HttpGet]
        [Route("Obtener/{idUsuario}")]
        public IActionResult Obtener(int idUsuario)
        {
            Usuario oUsuario = _prestamosContext.Usuarios.Find(idUsuario);

            if(oUsuario == null)
            {
                return StatusCode(StatusCodes.Status400BadRequest, new { mensaje = "Este usuario no existe" });
            }

            try
            {
                oUsuario = _prestamosContext.Usuarios.Where(u => u.IdUsuario == idUsuario).FirstOrDefault();
                return StatusCode(StatusCodes.Status200OK, new { mensaje = "ok", response = oUsuario });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status400BadRequest, new { mensaje = ex.Message, response = oUsuario });
            }
        }

        [EnableCors("ReglasCors")]
        [HttpPost]
        [Route("Guardar")]
        public IActionResult Guardar([FromBody] Usuario objeto)
        {
            Usuario oUsuario = _prestamosContext.Usuarios.FirstOrDefault(u => u.NombreUsuario == objeto.NombreUsuario);

            if (oUsuario != null)
            {
                return StatusCode(StatusCodes.Status400BadRequest, new { mensaje = "Este nombre de usuario ya existe" });
            }

            oUsuario = _prestamosContext.Usuarios.FirstOrDefault(u => u.Email == objeto.Email);

            if (oUsuario != null)
            {
                return StatusCode(StatusCodes.Status400BadRequest, new { mensaje = "Este Email ya fue registrado" });
            }

            try
            {
                _prestamosContext.Usuarios.Add(objeto);
                _prestamosContext.SaveChanges();

                return StatusCode(StatusCodes.Status200OK, new { mensaje = "Usuario registrado"});
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status400BadRequest, new { mensaje = ex.Message });
            }
        }

        [EnableCors("ReglasCors")]
        [HttpPut]
        [Route("Editar")]
        public IActionResult Editar([FromBody] Usuario objeto)
        {
            Usuario oUsuario = _prestamosContext.Usuarios.Find(objeto.IdUsuario);

            if(oUsuario == null)
            {
                return StatusCode(StatusCodes.Status400BadRequest, new { mensaje = "Este Usuario no existe" });
            }

            oUsuario = _prestamosContext.Usuarios.FirstOrDefault(u => u.NombreUsuario == objeto.NombreUsuario);

            if (oUsuario != null)
            {
                return StatusCode(StatusCodes.Status400BadRequest, new { mensaje = "Este nombre de usuario ya existe" });
            }

            oUsuario = _prestamosContext.Usuarios.FirstOrDefault(u => u.Email == objeto.Email);

            if (oUsuario != null)
            {
                return StatusCode(StatusCodes.Status400BadRequest, new { mensaje = "Este Email ya fue registrado" });
            }

            try
            {
                oUsuario.NombreUsuario = objeto.NombreUsuario is null ? oUsuario.NombreUsuario : objeto.NombreUsuario;
                oUsuario.Contraseña = objeto.Contraseña is null ? oUsuario.Contraseña : objeto.Contraseña;
                oUsuario.Email = objeto.Email is null ? oUsuario.Email : objeto.Email;

                _prestamosContext.Usuarios.Update(oUsuario);
                _prestamosContext.SaveChanges();

                return StatusCode(StatusCodes.Status200OK, new { mensaje = "Usuario actualizado" });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status400BadRequest, new { mensaje = ex.Message });
            }
        }

        [EnableCors("ReglasCors")]
        [HttpDelete]
        [Route("Eliminar/{iUsuario}")]
        public IActionResult Eliminar(string idUsuario)
        {
            Usuario oUsuario = _prestamosContext.Usuarios.Find(idUsuario);

            if (oUsuario == null)
            {
                return StatusCode(StatusCodes.Status400BadRequest, new { mensaje = "Este usuario no existe" });
            }

            try
            {
                _prestamosContext.Usuarios.Remove(oUsuario);
                _prestamosContext.SaveChanges();

                return StatusCode(StatusCodes.Status200OK, new { mensaje = "Usuario eliminado" });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status400BadRequest, new { mensaje = ex.Message });
            }
        }
    }
}
