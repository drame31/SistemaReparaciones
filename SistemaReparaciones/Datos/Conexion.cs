using System.Configuration;
using System.Data;
using System.Data.SqlClient;

namespace SistemaReparaciones.Datos
{
    /// <summary>
    /// Punto unico de contacto con SQL Server. Todas las clases de la capa
    /// de datos pasan por aqui, asi la cadena de conexion se escribe una sola
    /// vez y siempre se cierra la conexion.
    /// </summary>
    public static class Conexion
    {
        private static string Cadena
        {
            get { return ConfigurationManager.ConnectionStrings["TallerReparaciones"].ConnectionString; }
        }

        private static SqlCommand PrepararComando(SqlConnection cn, string procedimiento, SqlParameter[] parametros)
        {
            SqlCommand cmd = new SqlCommand(procedimiento, cn);
            cmd.CommandType = CommandType.StoredProcedure;

            if (parametros != null)
            {
                foreach (SqlParameter p in parametros)
                {
                    cmd.Parameters.Add(p);
                }
            }

            return cmd;
        }

        // Para los procedimientos que devuelven filas
        public static DataTable Consultar(string procedimiento, params SqlParameter[] parametros)
        {
            DataTable tabla = new DataTable();

            using (SqlConnection cn = new SqlConnection(Cadena))
            using (SqlCommand cmd = PrepararComando(cn, procedimiento, parametros))
            {
                cn.Open();
                using (SqlDataAdapter adaptador = new SqlDataAdapter(cmd))
                {
                    adaptador.Fill(tabla);
                }
            }

            return tabla;
        }

        // Para insertar, actualizar y borrar
        public static int Ejecutar(string procedimiento, params SqlParameter[] parametros)
        {
            using (SqlConnection cn = new SqlConnection(Cadena))
            using (SqlCommand cmd = PrepararComando(cn, procedimiento, parametros))
            {
                cn.Open();
                return cmd.ExecuteNonQuery();
            }
        }
    }
}
