using Microsoft.EntityFrameworkCore;
using prestamos.api.Models;

namespace prestamos.api.Interfaces
{
    public interface IProyectoPrestamosContext
    {
        DbSet<Abono> Abonos { get; set; }
        DbSet<Cliente> Clientes { get; set; }
        DbSet<Interes> Intereses { get; set; }
        DbSet<Prestamista> Prestamistas { get; set; }
        DbSet<Prestamo> Prestamos { get; set; }
        DbSet<Usuario> Usuarios { get; set; }
        int SaveChanges();
    }
}
