using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using SistemaReparaciones.Modelo;

namespace SistemaReparaciones.Datos
{
    public class TecnicoDatos
    {
        private Tecnico Mapear(DataRow fila)
        {
            Tecnico t = new Tecnico();
            t.TecnicoID = Convert.ToInt32(fila["TecnicoID"]);
            t.Nombre = fila["Nombre"].ToString();
            t.Especialidad = fila["Especialidad"].ToString();
            return t;
        }

        public List<Tecnico> Listar()
        {
            return Listar(null);
        }

        // El filtro se lo lleva el procedimiento almacenado. Si busqueda
        // viene en null devuelve la tabla completa.
        public List<Tecnico> Listar(string busqueda)
        {
            List<Tecnico> lista = new List<Tecnico>();

            DataTable tabla = Conexion.Consultar("sp_ListarTecnicos",
                new SqlParameter("@Busqueda", (object)busqueda ?? DBNull.Value));

            foreach (DataRow fila in tabla.Rows)
            {
                lista.Add(Mapear(fila));
            }

            return lista;
        }

        public Tecnico Obtener(int tecnicoID)
        {
            DataTable tabla = Conexion.Consultar("sp_ObtenerTecnico",
                new SqlParameter("@TecnicoID", tecnicoID));

            if (tabla.Rows.Count == 0)
            {
                return null;
            }

            return Mapear(tabla.Rows[0]);
        }

        public void Insertar(Tecnico t)
        {
            Conexion.Ejecutar("sp_InsertarTecnico",
                new SqlParameter("@Nombre", t.Nombre),
                new SqlParameter("@Especialidad", t.Especialidad));
        }

        public void Actualizar(Tecnico t)
        {
            Conexion.Ejecutar("sp_ActualizarTecnico",
                new SqlParameter("@TecnicoID", t.TecnicoID),
                new SqlParameter("@Nombre", t.Nombre),
                new SqlParameter("@Especialidad", t.Especialidad));
        }

        public void Eliminar(int tecnicoID)
        {
            Conexion.Ejecutar("sp_EliminarTecnico",
                new SqlParameter("@TecnicoID", tecnicoID));
        }
    }
}
