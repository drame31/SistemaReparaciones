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

            // Se le quitan los espacios de los extremos a la contrasena porque al
            // copiarla y pegarla es facil arrastrar uno sin darse cuenta, y como
            // va cifrada un solo espacio de mas cambia el hash completo.
            return datos.Validar(nombreUsuario.Trim(), Seguridad.Cifrar(contrasena.Trim()));
        }

        public Resumen ObtenerResumen()
        {
            return datos.ObtenerResumen();
        }
    }
}
