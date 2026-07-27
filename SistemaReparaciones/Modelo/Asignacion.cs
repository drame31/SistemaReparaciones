using System;

namespace SistemaReparaciones.Modelo
{
    public class Asignacion
    {
        public int AsignacionID { get; set; }
        public int ReparacionID { get; set; }
        public int TecnicoID { get; set; }
        public DateTime FechaAsignacion { get; set; }

        public string NombreTecnico { get; set; }
        public string DescripcionEquipo { get; set; }
    }
}
