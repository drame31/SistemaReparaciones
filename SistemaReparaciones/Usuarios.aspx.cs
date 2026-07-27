using System;
using System.Web.UI.WebControls;
using SistemaReparaciones.Logica;
using SistemaReparaciones.Modelo;

namespace SistemaReparaciones
{
    public partial class Usuarios : PaginaBase
    {
        private readonly UsuarioLogica logica = new UsuarioLogica();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                CargarLista();
            }
        }

        private void CargarLista()
        {
            gvUsuarios.DataSource = logica.Buscar(txtBuscar.Text);
            gvUsuarios.DataBind();
        }

        protected void btnBuscar_Click(object sender, EventArgs e)
        {
            CargarLista();
        }

        protected void btnGuardar_Click(object sender, EventArgs e)
        {
            try
            {
                Usuario u = new Usuario();
                u.UsuarioID = int.Parse(hdnUsuarioID.Value);
                u.Nombre = txtNombre.Text;
                u.CorreoElectronico = txtCorreo.Text;
                u.Telefono = txtTelefono.Text;

                bool esNuevo = u.UsuarioID == 0;
                logica.Guardar(u);

                LimpiarFormulario();
                CargarLista();
                Mostrar(esNuevo ? "Cliente agregado correctamente." : "Cliente actualizado correctamente.", true);
            }
            catch (Exception ex)
            {
                Mostrar(ex.Message, false);
            }
        }

        protected void btnCancelar_Click(object sender, EventArgs e)
        {
            LimpiarFormulario();
            OcultarMensaje();
        }

        protected void gvUsuarios_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName != "Editar" && e.CommandName != "Borrar")
            {
                return;
            }

            int usuarioID = Convert.ToInt32(e.CommandArgument);

            if (e.CommandName == "Editar")
            {
                Usuario u = logica.Obtener(usuarioID);

                if (u == null)
                {
                    Mostrar("Ese cliente ya no existe.", false);
                    CargarLista();
                    return;
                }

                hdnUsuarioID.Value = u.UsuarioID.ToString();
                txtNombre.Text = u.Nombre;
                txtCorreo.Text = u.CorreoElectronico;
                txtTelefono.Text = u.Telefono;

                litTituloFormulario.Text = "Editando cliente #" + u.UsuarioID;
                OcultarMensaje();
            }
            else
            {
                try
                {
                    logica.Eliminar(usuarioID);
                    LimpiarFormulario();
                    CargarLista();
                    Mostrar("Cliente eliminado.", true);
                }
                catch (Exception ex)
                {
                    Mostrar(ex.Message, false);
                }
            }
        }

        private void LimpiarFormulario()
        {
            hdnUsuarioID.Value = "0";
            txtNombre.Text = "";
            txtCorreo.Text = "";
            txtTelefono.Text = "";
            litTituloFormulario.Text = "Agregar cliente";
        }

        private void Mostrar(string texto, bool correcto)
        {
            lblMensaje.Text = texto;
            lblMensaje.CssClass = correcto ? "mensaje mensaje-ok" : "mensaje mensaje-error";
            lblMensaje.Visible = true;
        }

        private void OcultarMensaje()
        {
            lblMensaje.Visible = false;
        }
    }
}
