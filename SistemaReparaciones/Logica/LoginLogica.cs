using System;
using SistemaReparaciones.Datos;
using SistemaReparaciones.Modelo;

namespace SistemaReparaciones.Logica
{
    public class LoginLogica
    {
        private readonly LoginDatos datos = new LoginDatos();

        public UsuarioSistema Validar(string nombreUsuario, string contrasena)
        {
            if (string.IsNullOrWhiteSpace(nombreUsuario) || string.IsNullOrWhiteSpace(contrasena))
            {
                throw new ApplicationException("Escriba el usuario y la contrasena.");
            }

            return datos.Validar(nombreUsuario.Trim(), Seguridad.Cifrar(contrasena));
        }

        public Resumen ObtenerResumen()
        {
            return datos.ObtenerResumen();
        }
    }
}
