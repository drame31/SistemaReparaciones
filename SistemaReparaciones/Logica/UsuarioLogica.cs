using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using SistemaReparaciones.Datos;
using SistemaReparaciones.Modelo;

namespace SistemaReparaciones.Logica
{
    public class UsuarioLogica
    {
        private readonly UsuarioDatos datos = new UsuarioDatos();

        public List<Usuario> Listar()
        {
            return datos.Listar();
        }

        // Filtro de la caja de busqueda de la pantalla
        public List<Usuario> Buscar(string texto)
        {
            return datos.Listar(Filtro.Preparar(texto));
        }

        public Usuario Obtener(int usuarioID)
        {
            return datos.Obtener(usuarioID);
        }

        private void Validar(Usuario u)
        {
            if (string.IsNullOrWhiteSpace(u.Nombre))
            {
                throw new ApplicationException("El nombre es obligatorio.");
            }

            if (u.Nombre.Trim().Length < 3)
            {
                throw new ApplicationException("El nombre debe tener al menos 3 letras.");
            }

            if (string.IsNullOrWhiteSpace(u.CorreoElectronico))
            {
                throw new ApplicationException("El correo electronico es obligatorio.");
            }

            if (!Regex.IsMatch(u.CorreoElectronico.Trim(), @"^[^@\s]+@[^@\s]+\.[a-zA-Z]{2,}$"))
            {
                throw new ApplicationException("El correo electronico no tiene un formato valido.");
            }

            // El telefono es opcional, pero si lo escriben debe traer numeros
            if (!string.IsNullOrWhiteSpace(u.Telefono) &&
                !Regex.IsMatch(u.Telefono.Trim(), @"^[0-9\s\-\+\(\)]{8,20}$"))
            {
                throw new ApplicationException("El telefono solo puede tener numeros, espacios y guiones.");
            }
        }

        public void Guardar(Usuario u)
        {
            Validar(u);

            u.Nombre = u.Nombre.Trim();
            u.CorreoElectronico = u.CorreoElectronico.Trim();
            u.Telefono = u.Telefono == null ? "" : u.Telefono.Trim();

            if (u.UsuarioID == 0)
            {
                datos.Insertar(u);
            }
            else
            {
                datos.Actualizar(u);
            }
        }

        public void Eliminar(int usuarioID)
        {
            datos.Eliminar(usuarioID);
        }
    }
}
