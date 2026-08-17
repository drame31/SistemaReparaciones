using System;
using System.Collections.Generic;
using SistemaReparaciones.Datos;
using SistemaReparaciones.Modelo;

namespace SistemaReparaciones.Logica
{
    public class DetalleLogica
    {
        private readonly DetalleDatos datos = new DetalleDatos();

        public List<DetalleReparacion> Listar()
        {
            return datos.Listar();
        }

        public List<DetalleReparacion> Buscar(string texto)
        {
            return datos.Listar(Filtro.Preparar(texto));
        }

        public DetalleReparacion Obtener(int detalleID)
        {
            return datos.Obtener(detalleID);
        }

        private void Validar(DetalleReparacion d)
        {
            if (d.ReparacionID <= 0)
            {
                throw new ApplicationException("Seleccione la reparacion.");
            }

            if (string.IsNullOrWhiteSpace(d.Descripcion))
            {
                throw new ApplicationException("La descripcion del trabajo es obligatoria.");
            }

            if (d.Descripcion.Trim().Length < 10)
            {
                throw new ApplicationException("Describa el trabajo con un poco mas de detalle.");
            }

            // Misma regla que el CHECK de la tabla, pero avisando antes de ir a la BD
            if (d.FechaInicio.HasValue && d.FechaFin.HasValue &&
                d.FechaFin.Value < d.FechaInicio.Value)
            {
                throw new ApplicationException("La fecha de fin no puede ser anterior a la de inicio.");
            }

            if (d.FechaFin.HasValue && !d.FechaInicio.HasValue)
            {
                throw new ApplicationException("Si pone fecha de fin tambien tiene que poner la de inicio.");
            }
        }

        public void Guardar(DetalleReparacion d)
        {
            Validar(d);

            d.Descripcion = d.Descripcion.Trim();

            if (d.DetalleID == 0)
            {
                datos.Insertar(d);
            }
            else
            {
                datos.Actualizar(d);
            }
        }

        public void Eliminar(int detalleID)
        {
            datos.Eliminar(detalleID);
        }
    }
}
