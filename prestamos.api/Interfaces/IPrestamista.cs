using prestamos.api.Models;

namespace prestamos.api.Interfaces
{
    public interface IPrestamista
    {
        int IdPrestamista { get; set; }
        string? Nombre { get; set; }
        decimal? Capital { get; set; }
        string? NumeroCuenta { get; set; }
        int IdUsuario { get; set; }
        ICollection<Cliente> Clientes { get; set; }
        Usuario oUsuario { get; set; }
        ICollection<Prestamo> Prestamos { get; set; }
    }
}
