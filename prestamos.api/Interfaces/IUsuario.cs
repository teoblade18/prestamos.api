namespace prestamos.api.Interfaces
{
    public interface IUsuario
    {
        int IdUsuario { get; set; }
        string NombreUsuario { get; set; }
        string Contraseña { get; set; }
        string Email { get; set; }
        ICollection<Prestamista> Prestamista { get; set; }
    }
}
