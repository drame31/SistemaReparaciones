using System;
using System.Web.UI;
using SistemaReparaciones.Logica;
using SistemaReparaciones.Modelo;

namespace SistemaReparaciones
{
    public partial class Login : Page
    {
        private readonly LoginLogica logica = new LoginLogica();

        protected void Page_Load(object sender, EventArgs e)
        {
            // Si ya hay sesion abierta no tiene sentido volver a pedir el login
            if (!IsPostBack && Session["Usuario"] != null)
            {
                Response.Redirect("~/Default.aspx");
            }
        }

        protected void btnEntrar_Click(object sender, EventArgs e)
        {
            try
            {
                UsuarioSistema usuario = logica.Validar(txtUsuario.Text, txtContrasena.Text);

                if (usuario == null)
                {
                    MostrarError("Usuario o contrasena incorrectos.");
                    return;
                }

                // Sesion nueva para que no se reutilice el id anterior
                Session.Clear();
                Session["Usuario"] = usuario;

                Response.Redirect("~/Default.aspx");
            }
            catch (ApplicationException ex)
            {
                MostrarError(ex.Message);
            }
            catch (Exception)
            {
                MostrarError("No se pudo conectar con la base de datos. Revise que SQL Server este encendido.");
            }
        }

        private void MostrarError(string texto)
        {
            lblMensaje.Text = texto;
            lblMensaje.Visible = true;
            txtContrasena.Text = "";
        }
    }
}
