using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using SistemaReparaciones.Modelo;

namespace SistemaReparaciones.Datos
{
    public class EquipoDatos
    {
        private Equipo Mapear(DataRow fila)
        {
            Equipo e = new Equipo();
            e.EquipoID = Convert.ToInt32(fila["EquipoID"]);
            e.TipoEquipo = fila["TipoEquipo"].ToString();
            e.Modelo = fila["Modelo"].ToString();
            e.UsuarioID = Convert.ToInt32(fila["UsuarioID"]);
            e.NombreUsuario = fila["NombreUsuario"].ToString();
            return e;
        }

        public List<Equipo> Listar()
        {
            return Listar(null);
        }

        // El filtro se lo lleva el procedimiento almacenado. Si busqueda
        // viene en null devuelve la tabla completa.
        public List<Equipo> Listar(string busqueda)
        {
            List<Equipo> lista = new List<Equipo>();

            DataTable tabla = Conexion.Consultar("sp_ListarEquipos",
                new SqlParameter("@Busqueda", (object)busqueda ?? DBNull.Value));

            foreach (DataRow fila in tabla.Rows)
            {
                lista.Add(Mapear(fila));
            }

            return lista;
        }

        public Equipo Obtener(int equipoID)
        {
            DataTable tabla = Conexion.Consultar("sp_ObtenerEquipo",
                new SqlParameter("@EquipoID", equipoID));

            if (tabla.Rows.Count == 0)
            {
                return null;
            }

            return Mapear(tabla.Rows[0]);
        }

        public void Insertar(Equipo e)
        {
            Conexion.Ejecutar("sp_InsertarEquipo",
                new SqlParameter("@TipoEquipo", e.TipoEquipo),
                new SqlParameter("@Modelo", e.Modelo),
                new SqlParameter("@UsuarioID", e.UsuarioID));
        }

        public void Actualizar(Equipo e)
        {
            Conexion.Ejecutar("sp_ActualizarEquipo",
                new SqlParameter("@EquipoID", e.EquipoID),
                new SqlParameter("@TipoEquipo", e.TipoEquipo),
                new SqlParameter("@Modelo", e.Modelo),
                new SqlParameter("@UsuarioID", e.UsuarioID));
        }

        public void Eliminar(int equipoID)
        {
            Conexion.Ejecutar("sp_EliminarEquipo",
                new SqlParameter("@EquipoID", equipoID));
        }
    }
}
