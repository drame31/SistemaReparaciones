namespace SistemaReparaciones.Modelo
{
    public class Equipo
    {
        public int EquipoID { get; set; }
        public string TipoEquipo { get; set; }
        public string Modelo { get; set; }
        public int UsuarioID { get; set; }

        // Viene del join, solo para mostrar en la tabla
        public string NombreUsuario { get; set; }
    }
}
