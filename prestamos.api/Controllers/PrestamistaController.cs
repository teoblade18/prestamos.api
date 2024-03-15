using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Cors;
using prestamos.api.Util;

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

        [EnableCors("ReglasCors")]
        [HttpGet]
        [Route("Obtener/{idPrestamista}")]
        public IActionResult Obtener(int idPrestamista)
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
                return StatusCode(StatusCodes.Status400BadRequest, new { mensaje = ex.Message, response = oPrestamista });
            }
        }

        [EnableCors("ReglasCors")]
        [HttpPost]
        [Route("ConsultarXUsuario")]
        public IActionResult ConsultarXUsuario([FromBody] Usuario objeto)
        {
            try
            {
                //Se valida que el nombre de usuario exista
                Usuario oUsuario = _prestamosContext.Usuarios.FirstOrDefault(u => u.NombreUsuario == objeto.NombreUsuario);
                Encrypter encrypter = new Encrypter();

                if (oUsuario == null)
                {
                    //Se valida que el email exista
                    oUsuario = _prestamosContext.Usuarios.FirstOrDefault(u => u.Email == objeto.Email);

                    if (oUsuario == null)
                    {
                        return StatusCode(StatusCodes.Status400BadRequest, new { mensaje = "Usuario/Email no existe"});
                    }

                    string passComparacion = encrypter.Encrypt(objeto.Contraseña);

                    //Se valida que la contraseña sea correcta
                    if (oUsuario.Contraseña == encrypter.Encrypt(objeto.Contraseña))
                    {
                        Prestamista oPrestamista = _prestamosContext.Prestamistas.FirstOrDefault(u => u.IdUsuario == oUsuario.IdUsuario);

                        return StatusCode(StatusCodes.Status200OK, new { mensaje = "ok", response = oPrestamista.IdPrestamista });
                    }
                    else
                    {
                        return StatusCode(StatusCodes.Status400BadRequest, new { mensaje = "Contraseña incorrecta"});
                    }
                }
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status400BadRequest, new { mensaje = ex.Message });
            }

            return StatusCode(StatusCodes.Status400BadRequest, new { mensaje = "No se concluyó la consulta" });
        }

        [EnableCors("ReglasCors")]
        [HttpGet]
        [Route("ConsultarCapital/{idPrestamista}")]
        public IActionResult ConsultarCapital(int idPrestamista)
        {
            try
            {
                Prestamista oPrestamista = _prestamosContext.Prestamistas.Find(idPrestamista);

                if (oPrestamista == null)
                {
                    return BadRequest("Prestamista no encontrado");
                }

                try
                {
                    oPrestamista = _prestamosContext.Prestamistas.Where(p => p.IdPrestamista == idPrestamista).FirstOrDefault();
                    return StatusCode(StatusCodes.Status200OK, new { mensaje = "ok", response = oPrestamista.Capital });
                }
                catch (Exception ex)
                {
                    return StatusCode(StatusCodes.Status400BadRequest, new { mensaje = ex.Message, response = oPrestamista });
                }
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status400BadRequest, new { mensaje = ex.Message });
            }
        }

        [EnableCors("ReglasCors")]
        [HttpPost]
        [Route("Guardar")]
        public IActionResult Guardar([FromBody] Prestamista objeto)
        {
            Prestamista oPrestamista = _prestamosContext.Prestamistas.FirstOrDefault(u => u.oUsuario.NombreUsuario == objeto.oUsuario.NombreUsuario);
            Encrypter encrypter = new Encrypter();

            if (oPrestamista != null)
            {
                return BadRequest("Nombre usuario ya existe");
            }

            oPrestamista = _prestamosContext.Prestamistas.FirstOrDefault(u => u.oUsuario.Email == objeto.oUsuario.Email);

            if (oPrestamista != null)
            {
                return BadRequest("Email ya existe");
            }

            try
            {
                objeto.oUsuario.Contraseña = encrypter.Encrypt(objeto.oUsuario.Contraseña);

                _prestamosContext.Prestamistas.Add(objeto);
                _prestamosContext.SaveChanges();

                oPrestamista = _prestamosContext.Prestamistas.FirstOrDefault(u => u.oUsuario.Email == objeto.oUsuario.Email);

                return StatusCode(StatusCodes.Status200OK, new { mensaje = "Prestamista registrado", response = oPrestamista.IdPrestamista });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status400BadRequest, new { mensaje = ex.Message });
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

                return StatusCode(StatusCodes.Status200OK, new { mensaje = "ok" });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status400BadRequest, new { mensaje = ex.Message });
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
                return StatusCode(StatusCodes.Status400BadRequest, new { mensaje = ex.Message });
            }
        }
    }
}
