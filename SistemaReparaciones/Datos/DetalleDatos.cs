using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using SistemaReparaciones.Modelo;

namespace SistemaReparaciones.Datos
{
    public class DetalleDatos
    {
        private DetalleReparacion Mapear(DataRow fila)
        {
            DetalleReparacion d = new DetalleReparacion();
            d.DetalleID = Convert.ToInt32(fila["DetalleID"]);
            d.ReparacionID = Convert.ToInt32(fila["ReparacionID"]);
            d.Descripcion = fila["Descripcion"].ToString();
            d.DescripcionEquipo = fila["DescripcionEquipo"].ToString();

            if (fila["FechaInicio"] != DBNull.Value)
            {
                d.FechaInicio = Convert.ToDateTime(fila["FechaInicio"]);
            }

            if (fila["FechaFin"] != DBNull.Value)
            {
                d.FechaFin = Convert.ToDateTime(fila["FechaFin"]);
            }

            return d;
        }

        // Las fechas pueden venir vacias, por eso se convierten a DBNull
        private object Fecha(DateTime? valor)
        {
            if (valor.HasValue)
            {
                return valor.Value;
            }

            return DBNull.Value;
        }

        public List<DetalleReparacion> Listar()
        {
            return Listar(null);
        }

        // El filtro se lo lleva el procedimiento almacenado. Si busqueda
        // viene en null devuelve la tabla completa.
        public List<DetalleReparacion> Listar(string busqueda)
        {
            List<DetalleReparacion> lista = new List<DetalleReparacion>();

            DataTable tabla = Conexion.Consultar("sp_ListarDetalles",
                new SqlParameter("@Busqueda", (object)busqueda ?? DBNull.Value));

            foreach (DataRow fila in tabla.Rows)
            {
                lista.Add(Mapear(fila));
            }

            return lista;
        }

        public DetalleReparacion Obtener(int detalleID)
        {
            DataTable tabla = Conexion.Consultar("sp_ObtenerDetalle",
                new SqlParameter("@DetalleID", detalleID));

            if (tabla.Rows.Count == 0)
            {
                return null;
            }

            return Mapear(tabla.Rows[0]);
        }

        public void Insertar(DetalleReparacion d)
        {
            Conexion.Ejecutar("sp_InsertarDetalle",
                new SqlParameter("@ReparacionID", d.ReparacionID),
                new SqlParameter("@Descripcion", d.Descripcion),
                new SqlParameter("@FechaInicio", Fecha(d.FechaInicio)),
                new SqlParameter("@FechaFin", Fecha(d.FechaFin)));
        }

        public void Actualizar(DetalleReparacion d)
        {
            Conexion.Ejecutar("sp_ActualizarDetalle",
                new SqlParameter("@DetalleID", d.DetalleID),
                new SqlParameter("@ReparacionID", d.ReparacionID),
                new SqlParameter("@Descripcion", d.Descripcion),
                new SqlParameter("@FechaInicio", Fecha(d.FechaInicio)),
                new SqlParameter("@FechaFin", Fecha(d.FechaFin)));
        }

        public void Eliminar(int detalleID)
        {
            Conexion.Ejecutar("sp_EliminarDetalle",
                new SqlParameter("@DetalleID", detalleID));
        }
    }
}
