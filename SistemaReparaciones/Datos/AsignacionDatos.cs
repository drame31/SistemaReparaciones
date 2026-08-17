using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using SistemaReparaciones.Modelo;

namespace SistemaReparaciones.Datos
{
    public class AsignacionDatos
    {
        private Asignacion Mapear(DataRow fila)
        {
            Asignacion a = new Asignacion();
            a.AsignacionID = Convert.ToInt32(fila["AsignacionID"]);
            a.ReparacionID = Convert.ToInt32(fila["ReparacionID"]);
            a.TecnicoID = Convert.ToInt32(fila["TecnicoID"]);
            a.FechaAsignacion = Convert.ToDateTime(fila["FechaAsignacion"]);
            a.NombreTecnico = fila["NombreTecnico"].ToString();
            a.DescripcionEquipo = fila["DescripcionEquipo"].ToString();
            return a;
        }

        public List<Asignacion> Listar()
        {
            return Listar(null);
        }

        // El filtro se lo lleva el procedimiento almacenado. Si busqueda
        // viene en null devuelve la tabla completa.
        public List<Asignacion> Listar(string busqueda)
        {
            List<Asignacion> lista = new List<Asignacion>();

            DataTable tabla = Conexion.Consultar("sp_ListarAsignaciones",
                new SqlParameter("@Busqueda", (object)busqueda ?? DBNull.Value));

            foreach (DataRow fila in tabla.Rows)
            {
                lista.Add(Mapear(fila));
            }

            return lista;
        }

        public Asignacion Obtener(int asignacionID)
        {
            DataTable tabla = Conexion.Consultar("sp_ObtenerAsignacion",
                new SqlParameter("@AsignacionID", asignacionID));

            if (tabla.Rows.Count == 0)
            {
                return null;
            }

            return Mapear(tabla.Rows[0]);
        }

        public void Insertar(Asignacion a)
        {
            Conexion.Ejecutar("sp_InsertarAsignacion",
                new SqlParameter("@ReparacionID", a.ReparacionID),
                new SqlParameter("@TecnicoID", a.TecnicoID),
                new SqlParameter("@FechaAsignacion", a.FechaAsignacion));
        }

        public void Actualizar(Asignacion a)
        {
            Conexion.Ejecutar("sp_ActualizarAsignacion",
                new SqlParameter("@AsignacionID", a.AsignacionID),
                new SqlParameter("@ReparacionID", a.ReparacionID),
                new SqlParameter("@TecnicoID", a.TecnicoID),
                new SqlParameter("@FechaAsignacion", a.FechaAsignacion));
        }

        public void Eliminar(int asignacionID)
        {
            Conexion.Ejecutar("sp_EliminarAsignacion",
                new SqlParameter("@AsignacionID", asignacionID));
        }
    }
}
