﻿using Microsoft.AspNetCore.Http;
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

    }
}
