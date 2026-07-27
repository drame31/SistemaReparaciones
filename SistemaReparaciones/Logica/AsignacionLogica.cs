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
            List<Asignacion> todas = datos.Listar();

            if (string.IsNullOrWhiteSpace(texto))
            {
                return todas;
            }

            texto = texto.Trim().ToLower();
            List<Asignacion> encontradas = new List<Asignacion>();

            foreach (Asignacion a in todas)
            {
                if (a.NombreTecnico.ToLower().Contains(texto) ||
                    a.DescripcionEquipo.ToLower().Contains(texto))
                {
                    encontradas.Add(a);
                }
            }

            return encontradas;
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
