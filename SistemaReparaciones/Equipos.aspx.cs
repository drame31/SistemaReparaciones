using System;
using System.Collections.Generic;
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
            AplicarPermisos();

            if (!IsPostBack)
            {
                CargarClientes();
                CargarLista();
            }
        }

        // El tecnico puede consultar el listado pero no modificarlo. Se corre
        // en cada carga, tambien en las postbacks, para que los botones no
        // vuelvan a aparecer despues de buscar o cambiar de pagina.
        private void AplicarPermisos()
        {
            if (EsAdministrador)
            {
                return;
            }

            pnlFormulario.Visible = false;
            pnlSoloLectura.Visible = true;
            gvEquipos.Columns[gvEquipos.Columns.Count - 1].Visible = false;
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
            List<Equipo> lista = logica.Buscar(txtBuscar.Text);

            // Si se elimino el ultimo registro de la ultima pagina esa pagina
            // ya no existe y la tabla saldria vacia, asi que se retrocede.
            int ultimaPagina = (lista.Count - 1) / gvEquipos.PageSize;

            if (gvEquipos.PageIndex > ultimaPagina)
            {
                gvEquipos.PageIndex = ultimaPagina < 0 ? 0 : ultimaPagina;
            }

            gvEquipos.DataSource = lista;
            gvEquipos.DataBind();
        }

        protected void btnBuscar_Click(object sender, EventArgs e)
        {
            // Una busqueda nueva arranca desde la primera pagina
            gvEquipos.PageIndex = 0;
            CargarLista();
        }

        protected void gvEquipos_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gvEquipos.PageIndex = e.NewPageIndex;
            CargarLista();
        }

        protected void btnGuardar_Click(object sender, EventArgs e)
        {
            try
            {
                ExigirAdministrador();

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

            if (!EsAdministrador)
            {
                Mostrar(SinPermiso, false);
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
