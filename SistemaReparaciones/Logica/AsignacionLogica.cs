using System;
using System.Collections.Generic;
using SistemaReparaciones.Datos;
using SistemaReparaciones.Modelo;

namespace SistemaReparaciones.Logica
{
    public class AsignacionLogica
    {
        private readonly AsignacionDatos datos = new AsignacionDatos();

        public List<Asignacion> Listar()
        {
            return datos.Listar();
        }

        public List<Asignacion> Buscar(string texto)
        {
            return datos.Listar(Filtro.Preparar(texto));
        }

        public Asignacion Obtener(int asignacionID)
        {
            return datos.Obtener(asignacionID);
        }

        private void Validar(Asignacion a)
        {
            if (a.ReparacionID <= 0)
            {
                throw new ApplicationException("Seleccione la reparacion.");
            }

            if (a.TecnicoID <= 0)
            {
                throw new ApplicationException("Seleccione el tecnico.");
            }

            if (a.FechaAsignacion == DateTime.MinValue)
            {
                throw new ApplicationException("La fecha de asignacion es obligatoria.");
            }
        }

        public void Guardar(Asignacion a)
        {
            Validar(a);

            if (a.AsignacionID == 0)
            {
                datos.Insertar(a);
            }
            else
            {
                datos.Actualizar(a);
            }
        }

        public void Eliminar(int asignacionID)
        {
            datos.Eliminar(asignacionID);
        }
    }
}
