using System;
using System.Collections.Generic;
using SistemaReparaciones.Datos;
using SistemaReparaciones.Modelo;

namespace SistemaReparaciones.Logica
{
    public class ReparacionLogica
    {
        private readonly ReparacionDatos datos = new ReparacionDatos();

        // Los mismos valores que acepta el CHECK de la tabla
        public static readonly string[] Estados =
        {
            "Pendiente", "En proceso", "Terminada", "Entregada"
        };

        public List<Reparacion> Listar()
        {
            return datos.Listar();
        }

        public List<Reparacion> Buscar(string texto)
        {
            List<Reparacion> todas = datos.Listar();

            if (string.IsNullOrWhiteSpace(texto))
            {
                return todas;
            }

            texto = texto.Trim().ToLower();
            List<Reparacion> encontradas = new List<Reparacion>();

            foreach (Reparacion r in todas)
            {
                if (r.DescripcionEquipo.ToLower().Contains(texto) ||
                    r.NombreUsuario.ToLower().Contains(texto) ||
                    r.Estado.ToLower().Contains(texto))
                {
                    encontradas.Add(r);
                }
            }

            return encontradas;
        }

        public Reparacion Obtener(int reparacionID)
        {
            return datos.Obtener(reparacionID);
        }

        private void Validar(Reparacion r)
        {
            if (r.EquipoID <= 0)
            {
                throw new ApplicationException("Seleccione el equipo que se va a reparar.");
            }

            if (r.FechaSolicitud == DateTime.MinValue)
            {
                throw new ApplicationException("La fecha de solicitud es obligatoria.");
            }

            if (r.FechaSolicitud.Date > DateTime.Today)
            {
                throw new ApplicationException("La fecha de solicitud no puede ser futura.");
            }

            if (Array.IndexOf(Estados, r.Estado) < 0)
            {
                throw new ApplicationException("Seleccione un estado valido.");
            }
        }

        public void Guardar(Reparacion r)
        {
            Validar(r);

            if (r.ReparacionID == 0)
            {
                datos.Insertar(r);
            }
            else
            {
                datos.Actualizar(r);
            }
        }

        public void Eliminar(int reparacionID)
        {
            datos.Eliminar(reparacionID);
        }
    }
}
