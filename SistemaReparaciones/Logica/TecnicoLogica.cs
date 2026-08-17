using System;
using System.Collections.Generic;
using SistemaReparaciones.Datos;
using SistemaReparaciones.Modelo;

namespace SistemaReparaciones.Logica
{
    public class TecnicoLogica
    {
        private readonly TecnicoDatos datos = new TecnicoDatos();

        public List<Tecnico> Listar()
        {
            return datos.Listar();
        }

        public List<Tecnico> Buscar(string texto)
        {
            return datos.Listar(Filtro.Preparar(texto));
        }

        public Tecnico Obtener(int tecnicoID)
        {
            return datos.Obtener(tecnicoID);
        }

        private void Validar(Tecnico t)
        {
            if (string.IsNullOrWhiteSpace(t.Nombre))
            {
                throw new ApplicationException("El nombre del tecnico es obligatorio.");
            }

            if (t.Nombre.Trim().Length < 3)
            {
                throw new ApplicationException("El nombre debe tener al menos 3 letras.");
            }

            if (string.IsNullOrWhiteSpace(t.Especialidad))
            {
                throw new ApplicationException("La especialidad es obligatoria.");
            }
        }

        public void Guardar(Tecnico t)
        {
            Validar(t);

            t.Nombre = t.Nombre.Trim();
            t.Especialidad = t.Especialidad.Trim();

            if (t.TecnicoID == 0)
            {
                datos.Insertar(t);
            }
            else
            {
                datos.Actualizar(t);
            }
        }

        public void Eliminar(int tecnicoID)
        {
            datos.Eliminar(tecnicoID);
        }
    }
}
