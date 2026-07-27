using System;
using System.IO;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using SistemaReparaciones.Modelo;

namespace SistemaReparaciones
{
    public partial class SiteMaster : MasterPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            UsuarioSistema usuario = Session["Usuario"] as UsuarioSistema;

            if (usuario != null)
            {
                litUsuario.Text = "Conectado como <strong>" +
                                  HttpUtility.HtmlEncode(usuario.NombreUsuario) + "</strong> (" +
                                  HttpUtility.HtmlEncode(usuario.Rol) + ")";
            }

            MarcarOpcionActiva();
        }

        // Le pone la clase "activo" al enlace de la pagina en la que estamos
        private void MarcarOpcionActiva()
        {
            string paginaActual = Path.GetFileName(Request.Path).ToLower();

            HtmlAnchor[] enlaces =
            {
                navInicio, navEquipos, navUsuarios, navTecnicos,
                navReparaciones, navDetalles, navAsignaciones
            };

            foreach (HtmlAnchor enlace in enlaces)
            {
                string destino = Path.GetFileName(enlace.HRef).ToLower();

                if (destino == paginaActual)
                {
                    enlace.Attributes["class"] = "activo";
                }
            }
        }

        protected void btnSalir_Click(object sender, EventArgs e)
        {
            Session.Clear();
            Session.Abandon();
            Response.Redirect("~/Login.aspx");
        }
    }
}
