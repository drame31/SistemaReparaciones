using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using SistemaReparaciones.Modelo;

namespace SistemaReparaciones.Datos
{
    public class ReparacionDatos
    {
        private Reparacion Mapear(DataRow fila)
        {
            Reparacion r = new Reparacion();
            r.ReparacionID = Convert.ToInt32(fila["ReparacionID"]);
            r.EquipoID = Convert.ToInt32(fila["EquipoID"]);
            r.FechaSolicitud = Convert.ToDateTime(fila["FechaSolicitud"]);
            r.Estado = fila["Estado"].ToString();
            r.DescripcionEquipo = fila["DescripcionEquipo"].ToString();
            r.NombreUsuario = fila["NombreUsuario"].ToString();
            return r;
        }

        public List<Reparacion> Listar()
        {
            return Listar(null);
        }

        // El filtro se lo lleva el procedimiento almacenado. Si busqueda
        // viene en null devuelve la tabla completa.
        public List<Reparacion> Listar(string busqueda)
        {
            List<Reparacion> lista = new List<Reparacion>();

            DataTable tabla = Conexion.Consultar("sp_ListarReparaciones",
                new SqlParameter("@Busqueda", (object)busqueda ?? DBNull.Value));

            foreach (DataRow fila in tabla.Rows)
            {
                lista.Add(Mapear(fila));
            }

            return lista;
        }

        public Reparacion Obtener(int reparacionID)
        {
            DataTable tabla = Conexion.Consultar("sp_ObtenerReparacion",
                new SqlParameter("@ReparacionID", reparacionID));

            if (tabla.Rows.Count == 0)
            {
                return null;
            }

            return Mapear(tabla.Rows[0]);
        }

        public void Insertar(Reparacion r)
        {
            Conexion.Ejecutar("sp_InsertarReparacion",
                new SqlParameter("@EquipoID", r.EquipoID),
                new SqlParameter("@FechaSolicitud", r.FechaSolicitud),
                new SqlParameter("@Estado", r.Estado));
        }

        public void Actualizar(Reparacion r)
        {
            Conexion.Ejecutar("sp_ActualizarReparacion",
                new SqlParameter("@ReparacionID", r.ReparacionID),
                new SqlParameter("@EquipoID", r.EquipoID),
                new SqlParameter("@FechaSolicitud", r.FechaSolicitud),
                new SqlParameter("@Estado", r.Estado));
        }

        public void Eliminar(int reparacionID)
        {
            Conexion.Ejecutar("sp_EliminarReparacion",
                new SqlParameter("@ReparacionID", reparacionID));
        }
    }
}
