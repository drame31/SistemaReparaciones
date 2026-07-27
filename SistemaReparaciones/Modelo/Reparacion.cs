using System;

namespace SistemaReparaciones.Modelo
{
    public class Reparacion
    {
        public int ReparacionID { get; set; }
        public int EquipoID { get; set; }
        public DateTime FechaSolicitud { get; set; }
        public string Estado { get; set; }

        public string DescripcionEquipo { get; set; }
        public string NombreUsuario { get; set; }
    }
}
