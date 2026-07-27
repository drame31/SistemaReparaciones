namespace SistemaReparaciones.Modelo
{
    // Cliente del taller (el dueño del equipo)
    public class Usuario
    {
        public int UsuarioID { get; set; }
        public string Nombre { get; set; }
        public string CorreoElectronico { get; set; }
        public string Telefono { get; set; }
    }
}
