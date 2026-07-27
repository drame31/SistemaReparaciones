using System;
using System.Data;
using System.Data.SqlClient;
using SistemaReparaciones.Modelo;

namespace SistemaReparaciones.Datos
{
    public class LoginDatos
    {
        /// <summary>
        /// Devuelve el usuario si las credenciales son correctas, o null si no.
        /// La contrasena ya viene convertida a SHA-256 desde la capa de logica.
        /// </summary>
        public UsuarioSistema Validar(string nombreUsuario, string contrasenaCifrada)
        {
            DataTable tabla = Conexion.Consultar("sp_ValidarLogin",
                new SqlParameter("@NombreUsuario", nombreUsuario),
                new SqlParameter("@Contrasena", contrasenaCifrada));

            if (tabla.Rows.Count == 0)
            {
                return null;
            }

            DataRow fila = tabla.Rows[0];

            UsuarioSistema u = new UsuarioSistema();
            u.UsuarioSistemaID = Convert.ToInt32(fila["UsuarioSistemaID"]);
            u.NombreUsuario = fila["NombreUsuario"].ToString();
            u.Rol = fila["Rol"].ToString();

            return u;
        }

        public Resumen ObtenerResumen()
        {
            DataTable tabla = Conexion.Consultar("sp_ResumenGeneral");
            DataRow fila = tabla.Rows[0];

            Resumen r = new Resumen();
            r.TotalEquipos = Convert.ToInt32(fila["TotalEquipos"]);
            r.TotalUsuarios = Convert.ToInt32(fila["TotalUsuarios"]);
            r.TotalTecnicos = Convert.ToInt32(fila["TotalTecnicos"]);
            r.ReparacionesAbiertas = Convert.ToInt32(fila["ReparacionesAbiertas"]);

            return r;
        }
    }
}
