using System;
using System.Collections.Generic;
using SistemaReparaciones.Datos;
using SistemaReparaciones.Modelo;

namespace SistemaReparaciones.Logica
{
    public class EquipoLogica
    {
        private readonly EquipoDatos datos = new EquipoDatos();

        public List<Equipo> Listar()
        {
            return datos.Listar();
        }

        public List<Equipo> Buscar(string texto)
        {
            return datos.Listar(Filtro.Preparar(texto));
        }

        public Equipo Obtener(int equipoID)
        {
            return datos.Obtener(equipoID);
        }

        private void Validar(Equipo e)
        {
            if (string.IsNullOrWhiteSpace(e.TipoEquipo))
            {
                throw new ApplicationException("Seleccione el tipo de equipo.");
            }

            if (string.IsNullOrWhiteSpace(e.Modelo))
            {
                throw new ApplicationException("El modelo es obligatorio.");
            }

            if (e.UsuarioID <= 0)
            {
                throw new ApplicationException("Seleccione el cliente dueño del equipo.");
            }
        }

        public void Guardar(Equipo e)
        {
            Validar(e);

            e.TipoEquipo = e.TipoEquipo.Trim();
            e.Modelo = e.Modelo.Trim();

            if (e.EquipoID == 0)
            {
                datos.Insertar(e);
            }
            else
            {
                datos.Actualizar(e);
            }
        }

        public void Eliminar(int equipoID)
        {
            datos.Eliminar(equipoID);
        }
    }
}
