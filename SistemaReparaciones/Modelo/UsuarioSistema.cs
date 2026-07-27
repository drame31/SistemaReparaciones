namespace SistemaReparaciones.Modelo
{
    // Persona que entra al sistema. No es lo mismo que la tabla Usuarios,
    // esos son los clientes del taller.
    public class UsuarioSistema
    {
        public int UsuarioSistemaID { get; set; }
        public string NombreUsuario { get; set; }
        public string Rol { get; set; }
    }
}
