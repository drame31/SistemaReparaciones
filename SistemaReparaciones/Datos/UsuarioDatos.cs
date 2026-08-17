using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using SistemaReparaciones.Modelo;

namespace SistemaReparaciones.Datos
{
    public class UsuarioDatos
    {
        private Usuario Mapear(DataRow fila)
        {
            Usuario u = new Usuario();
            u.UsuarioID = Convert.ToInt32(fila["UsuarioID"]);
            u.Nombre = fila["Nombre"].ToString();
            u.CorreoElectronico = fila["CorreoElectronico"].ToString();
            u.Telefono = fila["Telefono"] == DBNull.Value ? "" : fila["Telefono"].ToString();
            return u;
        }

        public List<Usuario> Listar()
        {
            return Listar(null);
        }

        // El filtro se lo lleva el procedimiento almacenado. Si busqueda
        // viene en null devuelve la tabla completa.
        public List<Usuario> Listar(string busqueda)
        {
            List<Usuario> lista = new List<Usuario>();

            DataTable tabla = Conexion.Consultar("sp_ListarUsuarios",
                new SqlParameter("@Busqueda", (object)busqueda ?? DBNull.Value));

            foreach (DataRow fila in tabla.Rows)
            {
                lista.Add(Mapear(fila));
            }

            return lista;
        }

        public Usuario Obtener(int usuarioID)
        {
            DataTable tabla = Conexion.Consultar("sp_ObtenerUsuario",
                new SqlParameter("@UsuarioID", usuarioID));

            if (tabla.Rows.Count == 0)
            {
                return null;
            }

            return Mapear(tabla.Rows[0]);
        }

        public void Insertar(Usuario u)
        {
            Conexion.Ejecutar("sp_InsertarUsuario",
                new SqlParameter("@Nombre", u.Nombre),
                new SqlParameter("@CorreoElectronico", u.CorreoElectronico),
                new SqlParameter("@Telefono", string.IsNullOrEmpty(u.Telefono) ? (object)DBNull.Value : u.Telefono));
        }

        public void Actualizar(Usuario u)
        {
            Conexion.Ejecutar("sp_ActualizarUsuario",
                new SqlParameter("@UsuarioID", u.UsuarioID),
                new SqlParameter("@Nombre", u.Nombre),
                new SqlParameter("@CorreoElectronico", u.CorreoElectronico),
                new SqlParameter("@Telefono", string.IsNullOrEmpty(u.Telefono) ? (object)DBNull.Value : u.Telefono));
        }

        public void Eliminar(int usuarioID)
        {
            Conexion.Ejecutar("sp_EliminarUsuario",
                new SqlParameter("@UsuarioID", usuarioID));
        }
    }
}
