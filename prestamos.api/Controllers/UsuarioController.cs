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
        public IActionResult Obtener(int idUsuarioObtener)
        {
            Usuario oUsuario = _prestamosContext.Usuarios.Find(idUsuarioObtener);

            if(oUsuario == null)
            {
                return StatusCode(StatusCodes.Status400BadRequest, new { mensaje = "Este usuario no existe" });
            }

            try
            {
                oUsuario = _prestamosContext.Usuarios.Where(u => u.IdUsuario == idUsuarioObtener).FirstOrDefault();
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
        public IActionResult Guardar([FromBody] Usuario usuarioGuardar)
        {
            Usuario oUsuario = _prestamosContext.Usuarios.FirstOrDefault(u => u.NombreUsuario == usuarioGuardar.NombreUsuario);

            if (oUsuario != null)
            {
                return StatusCode(StatusCodes.Status400BadRequest, new { mensaje = "Este nombre de usuario ya existe" });
            }

            oUsuario = _prestamosContext.Usuarios.FirstOrDefault(u => u.Email == usuarioGuardar.Email);

            if (oUsuario != null)
            {
                return StatusCode(StatusCodes.Status400BadRequest, new { mensaje = "Este Email ya fue registrado" });
            }

            try
            {
                _prestamosContext.Usuarios.Add(usuarioGuardar);
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
        public IActionResult Editar([FromBody] Usuario usuarioEditar)
        {
            Usuario oUsuario = _prestamosContext.Usuarios.Find(usuarioEditar.IdUsuario);

            if(oUsuario == null)
            {
                return StatusCode(StatusCodes.Status400BadRequest, new { mensaje = "Este Usuario no existe" });
            }

            oUsuario = _prestamosContext.Usuarios.FirstOrDefault(u => u.NombreUsuario == usuarioEditar.NombreUsuario);

            if (oUsuario != null)
            {
                return StatusCode(StatusCodes.Status400BadRequest, new { mensaje = "Este nombre de usuario ya existe" });
            }

            oUsuario = _prestamosContext.Usuarios.FirstOrDefault(u => u.Email == usuarioEditar.Email);

            if (oUsuario != null)
            {
                return StatusCode(StatusCodes.Status400BadRequest, new { mensaje = "Este Email ya fue registrado" });
            }

            try
            {
                oUsuario.NombreUsuario = usuarioEditar.NombreUsuario is null ? oUsuario.NombreUsuario : usuarioEditar.NombreUsuario;
                oUsuario.Contraseña = usuarioEditar.Contraseña is null ? oUsuario.Contraseña : usuarioEditar.Contraseña;
                oUsuario.Email = usuarioEditar.Email is null ? oUsuario.Email : usuarioEditar.Email;

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
        public IActionResult Eliminar(string idUsuarioEliminar)
        {
            Usuario oUsuario = _prestamosContext.Usuarios.Find(idUsuarioEliminar);

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
