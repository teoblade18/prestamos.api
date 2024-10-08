﻿using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Cors;
using prestamos.api.Util;
using prestamos.api.Interfaces;

namespace prestamos.api.Controllers
{
    [EnableCors("ReglasCors")]
    [Route("api/[controller]")]
    [ApiController]
    public class PrestamistaController : ControllerBase
    {
        private readonly IProyectoPrestamosContext _prestamosContext;
        private IEncrypter _encrypter;

        public PrestamistaController(IProyectoPrestamosContext _context, IEncrypter encrypter)
        {
            _prestamosContext = _context;
            _encrypter = encrypter;
        }

        [EnableCors("ReglasCors")]
        [HttpGet]
        [Route("Obtener/{idPrestamistaObtener}")]
        public IActionResult Obtener(int idPrestamistaObtener)
        {
            IPrestamista oPrestamista = _prestamosContext.Prestamistas.Find(idPrestamistaObtener);

            if(oPrestamista == null)
            {
                return StatusCode(StatusCodes.Status400BadRequest, new { mensaje = "Prestamista no encontrado"});
            }

            try
            {
                oPrestamista = _prestamosContext.Prestamistas.Where(p => p.IdPrestamista == idPrestamistaObtener).FirstOrDefault();
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
        public IActionResult ConsultarXUsuario([FromBody] IUsuario usuario)
        {
            try
            {
                //Se valida que el nombre de usuario exista
                IUsuario oUsuario = _prestamosContext.Usuarios.FirstOrDefault(u => u.NombreUsuario == usuario.NombreUsuario);

                if (oUsuario == null)
                {
                    //Se valida que el email exista
                    oUsuario = _prestamosContext.Usuarios.FirstOrDefault(u => u.Email == usuario.Email);
                }
                
                if (oUsuario == null)
                {
                    return StatusCode(StatusCodes.Status400BadRequest, new { mensaje = "Usuario/Email no existe"});
                }

                string passComparacion = _encrypter.Encrypt(usuario.Contraseña);

                //Se valida que la contraseña sea correcta
                if (oUsuario.Contraseña == _encrypter.Encrypt(usuario.Contraseña))
                {
                    IPrestamista oPrestamista = _prestamosContext.Prestamistas.FirstOrDefault(u => u.IdUsuario == oUsuario.IdUsuario);

                    return StatusCode(StatusCodes.Status200OK, new { mensaje = "ok", response = oPrestamista.IdPrestamista });
                }
                else
                {
                    return StatusCode(StatusCodes.Status400BadRequest, new { mensaje = "Contraseña incorrecta"});
                }
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status400BadRequest, new { mensaje = ex.Message });
            }
        }

        [EnableCors("ReglasCors")]
        [HttpGet]
        [Route("ConsultarCapitalXPrestamista/{idPrestamista}")]
        public IActionResult ConsultarCapitalXPrestamista(int idPrestamista)
        {
            try
            {
                IPrestamista oPrestamista = _prestamosContext.Prestamistas.Find(idPrestamista);

                if (oPrestamista == null)
                {
                    return StatusCode(StatusCodes.Status400BadRequest, new { mensaje = "Prestamista no encontrado" });
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
        public IActionResult Guardar([FromBody] IPrestamista prestamistaGuardar)
        {
            IPrestamista oPrestamista = _prestamosContext.Prestamistas.FirstOrDefault(u => u.oUsuario.NombreUsuario == prestamistaGuardar.oUsuario.NombreUsuario);

            //Se valida que el prestamista no exista ya.
            if (oPrestamista != null)
            {
                return StatusCode(StatusCodes.Status400BadRequest, new { mensaje = "Nombre usuario ya existe" });
            }

            oPrestamista = _prestamosContext.Prestamistas.FirstOrDefault(u => u.oUsuario.Email == prestamistaGuardar.oUsuario.Email);

            //Se valida que el email no exista ya.
            if (oPrestamista != null)
            {
                return StatusCode(StatusCodes.Status400BadRequest, new { mensaje = "Email ya existe" });
            }

            try
            {
                prestamistaGuardar.oUsuario.Contraseña = _encrypter.Encrypt(prestamistaGuardar.oUsuario.Contraseña);

                _prestamosContext.Prestamistas.Add((Prestamista)prestamistaGuardar);
                _prestamosContext.SaveChanges();

                oPrestamista = _prestamosContext.Prestamistas.FirstOrDefault(u => u.oUsuario.Email == prestamistaGuardar.oUsuario.Email);

                return StatusCode(StatusCodes.Status200OK, new { mensaje = "ok", response = oPrestamista.IdPrestamista });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status400BadRequest, new { mensaje = ex.Message });
            }
        }

        [EnableCors("ReglasCors")]
        [HttpPut]
        [Route("Editar")]
        public IActionResult Editar([FromBody] IPrestamista prestamistaEditar)
        {
            IPrestamista oPrestamista = _prestamosContext.Prestamistas.Find(prestamistaEditar.IdPrestamista);

            if(oPrestamista == null)
            {
                return StatusCode(StatusCodes.Status400BadRequest, new { mensaje = "Este prestamista no existe" });
            }

            try
            {
                oPrestamista.Nombre = prestamistaEditar.Nombre is null ? oPrestamista.Nombre : prestamistaEditar.Nombre;
                oPrestamista.Capital = prestamistaEditar.Capital is null ? oPrestamista.Capital : prestamistaEditar.Capital;
                oPrestamista.NumeroCuenta = prestamistaEditar.NumeroCuenta is null ? oPrestamista.NumeroCuenta : prestamistaEditar.NumeroCuenta;

                _prestamosContext.Prestamistas.Update((Prestamista)oPrestamista);
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
        public IActionResult Eliminar(String idPrestamistaEliminar)
        {
            IPrestamista oPrestamista = _prestamosContext.Prestamistas.Find(idPrestamistaEliminar);

            if (oPrestamista == null)
            {
                return StatusCode(StatusCodes.Status400BadRequest, new { mensaje = "Este prestamista no existe" });
            }

            try
            {
                _prestamosContext.Prestamistas.Remove((Prestamista)oPrestamista);
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
