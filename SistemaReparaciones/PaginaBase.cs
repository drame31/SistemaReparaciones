using System;
using System.Web.UI;
using SistemaReparaciones.Modelo;

namespace SistemaReparaciones
{
    /// <summary>
    /// Todas las pantallas del sistema heredan de aqui. Antes de cargar
    /// cualquier cosa revisa que haya una sesion abierta; si no la hay
    /// devuelve al login. Asi no hay que repetir el chequeo en cada pagina.
    /// </summary>
    public class PaginaBase : Page
    {
        protected UsuarioSistema UsuarioActual
        {
            get { return Session["Usuario"] as UsuarioSistema; }
        }

        protected const string SinPermiso =
            "Su usuario no tiene permiso para modificar este mantenimiento.";

        protected bool EsAdministrador
        {
            get { return UsuarioActual != null && UsuarioActual.Rol == "Administrador"; }
        }

        /// <summary>
        /// Corta la operacion cuando el que entro no es administrador. Se llama
        /// antes de guardar o borrar en los mantenimientos que solo el
        /// administrador puede tocar. Esconder los botones no alcanza: el
        /// navegador puede reenviar el formulario de todas formas, asi que el
        /// permiso se revisa aqui en el servidor.
        /// </summary>
        protected void ExigirAdministrador()
        {
            if (!EsAdministrador)
            {
                throw new ApplicationException(SinPermiso);
            }
        }

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);

            if (UsuarioActual == null)
            {
                Session.Clear();
                Response.Redirect("~/Login.aspx");
            }
        }
    }
}
