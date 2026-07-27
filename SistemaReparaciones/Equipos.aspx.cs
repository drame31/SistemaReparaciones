using System;
using System.Web.UI.WebControls;
using SistemaReparaciones.Logica;
using SistemaReparaciones.Modelo;

namespace SistemaReparaciones
{
    public partial class Equipos : PaginaBase
    {
        private readonly EquipoLogica logica = new EquipoLogica();
        private readonly UsuarioLogica usuarios = new UsuarioLogica();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                CargarClientes();
                CargarLista();
            }
        }

        private void CargarClientes()
        {
            ddlCliente.DataSource = usuarios.Listar();
            ddlCliente.DataTextField = "Nombre";
            ddlCliente.DataValueField = "UsuarioID";
            ddlCliente.DataBind();

            ddlCliente.Items.Insert(0, new ListItem("-- Seleccione --", "0"));
        }

        private void CargarLista()
        {
            gvEquipos.DataSource = logica.Buscar(txtBuscar.Text);
            gvEquipos.DataBind();
        }

        protected void btnBuscar_Click(object sender, EventArgs e)
        {
            CargarLista();
        }

        protected void btnGuardar_Click(object sender, EventArgs e)
        {
            try
            {
                Equipo eq = new Equipo();
                eq.EquipoID = int.Parse(hdnEquipoID.Value);
                eq.TipoEquipo = ddlTipo.SelectedValue;
                eq.Modelo = txtModelo.Text;
                eq.UsuarioID = int.Parse(ddlCliente.SelectedValue);

                bool esNuevo = eq.EquipoID == 0;
                logica.Guardar(eq);

                LimpiarFormulario();
                CargarLista();
                Mostrar(esNuevo ? "Equipo registrado correctamente." : "Equipo actualizado correctamente.", true);
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

        protected void gvEquipos_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName != "Editar" && e.CommandName != "Borrar")
            {
                return;
            }

            int equipoID = Convert.ToInt32(e.CommandArgument);

            if (e.CommandName == "Editar")
            {
                Equipo eq = logica.Obtener(equipoID);

                if (eq == null)
                {
                    Mostrar("Ese equipo ya no existe.", false);
                    CargarLista();
                    return;
                }

                hdnEquipoID.Value = eq.EquipoID.ToString();
                SeleccionarEnLista(ddlTipo, eq.TipoEquipo);
                txtModelo.Text = eq.Modelo;
                SeleccionarEnLista(ddlCliente, eq.UsuarioID.ToString());

                litTituloFormulario.Text = "Editando equipo #" + eq.EquipoID;
                OcultarMensaje();
            }
            else
            {
                try
                {
                    logica.Eliminar(equipoID);
                    LimpiarFormulario();
                    CargarLista();
                    Mostrar("Equipo eliminado.", true);
                }
                catch (Exception ex)
                {
                    Mostrar(ex.Message, false);
                }
            }
        }

        // Si el valor guardado ya no esta en la lista se deja sin seleccionar
        private void SeleccionarEnLista(DropDownList lista, string valor)
        {
            lista.ClearSelection();
            ListItem item = lista.Items.FindByValue(valor);

            if (item != null)
            {
                item.Selected = true;
            }
        }

        private void LimpiarFormulario()
        {
            hdnEquipoID.Value = "0";
            ddlTipo.SelectedIndex = 0;
            txtModelo.Text = "";
            ddlCliente.SelectedIndex = 0;
            litTituloFormulario.Text = "Registrar equipo";
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
