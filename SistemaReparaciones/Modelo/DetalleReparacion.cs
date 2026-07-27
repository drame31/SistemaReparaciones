using System;

namespace SistemaReparaciones.Modelo
{
    public class DetalleReparacion
    {
        public int DetalleID { get; set; }
        public int ReparacionID { get; set; }
        public string Descripcion { get; set; }

        // La fecha de fin queda nula mientras el trabajo siga abierto
        public DateTime? FechaInicio { get; set; }
        public DateTime? FechaFin { get; set; }

        public string DescripcionEquipo { get; set; }
    }
}
