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
