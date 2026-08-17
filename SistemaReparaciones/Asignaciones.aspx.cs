using System;
using System.Collections.Generic;
using System.Web.UI.WebControls;
using SistemaReparaciones.Logica;
using SistemaReparaciones.Modelo;

namespace SistemaReparaciones
{
    public partial class Asignaciones : PaginaBase
    {
        private readonly AsignacionLogica logica = new AsignacionLogica();
        private readonly ReparacionLogica reparaciones = new ReparacionLogica();
        private readonly TecnicoLogica tecnicos = new TecnicoLogica();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                CargarReparaciones();
                CargarTecnicos();
                LimpiarFormulario();
                CargarLista();
            }
        }

        private void CargarReparaciones()
        {
            ddlReparacion.Items.Clear();
            ddlReparacion.Items.Add(new ListItem("-- Seleccione --", "0"));

            foreach (Reparacion r in reparaciones.Listar())
            {
                string texto = "#" + r.ReparacionID + " - " + r.DescripcionEquipo +
                               " (" + r.Estado + ")";
                ddlReparacion.Items.Add(new ListItem(texto, r.ReparacionID.ToString()));
            }
        }

        private void CargarTecnicos()
        {
            ddlTecnico.Items.Clear();
            ddlTecnico.Items.Add(new ListItem("-- Seleccione --", "0"));

            foreach (Tecnico t in tecnicos.Listar())
            {
                ddlTecnico.Items.Add(new ListItem(t.Nombre + " - " + t.Especialidad,
                                                  t.TecnicoID.ToString()));
            }
        }

        private void CargarLista()
        {
            List<Asignacion> lista = logica.Buscar(txtBuscar.Text);

            // Si se elimino el ultimo registro de la ultima pagina esa pagina
            // ya no existe y la tabla saldria vacia, asi que se retrocede.
            int ultimaPagina = (lista.Count - 1) / gvAsignaciones.PageSize;

            if (gvAsignaciones.PageIndex > ultimaPagina)
            {
                gvAsignaciones.PageIndex = ultimaPagina < 0 ? 0 : ultimaPagina;
            }

            gvAsignaciones.DataSource = lista;
            gvAsignaciones.DataBind();
        }

        protected void btnBuscar_Click(object sender, EventArgs e)
        {
            // Una busqueda nueva arranca desde la primera pagina
            gvAsignaciones.PageIndex = 0;
            CargarLista();
        }

        protected void gvAsignaciones_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gvAsignaciones.PageIndex = e.NewPageIndex;
            CargarLista();
        }

        protected void btnGuardar_Click(object sender, EventArgs e)
        {
            try
            {
                Asignacion a = new Asignacion();
                a.AsignacionID = int.Parse(hdnAsignacionID.Value);
                a.ReparacionID = int.Parse(ddlReparacion.SelectedValue);
                a.TecnicoID = int.Parse(ddlTecnico.SelectedValue);

                DateTime fecha;
                if (!DateTime.TryParse(txtFecha.Text, out fecha))
                {
                    throw new ApplicationException("Escriba una fecha de asignacion valida.");
                }
                a.FechaAsignacion = fecha;

                bool esNueva = a.AsignacionID == 0;
                logica.Guardar(a);

                LimpiarFormulario();
                CargarLista();
                Mostrar(esNueva ? "Tecnico asignado correctamente." : "Asignacion actualizada correctamente.", true);
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

        protected void gvAsignaciones_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName != "Editar" && e.CommandName != "Borrar")
            {
                return;
            }

            int asignacionID = Convert.ToInt32(e.CommandArgument);

            if (e.CommandName == "Editar")
            {
                Asignacion a = logica.Obtener(asignacionID);

                if (a == null)
                {
                    Mostrar("Esa asignacion ya no existe.", false);
                    CargarLista();
                    return;
                }

                hdnAsignacionID.Value = a.AsignacionID.ToString();
                SeleccionarEnLista(ddlReparacion, a.ReparacionID.ToString());
                SeleccionarEnLista(ddlTecnico, a.TecnicoID.ToString());
                txtFecha.Text = a.FechaAsignacion.ToString("yyyy-MM-dd");

                litTituloFormulario.Text = "Editando asignacion #" + a.AsignacionID;
                OcultarMensaje();
            }
            else
            {
                try
                {
                    logica.Eliminar(asignacionID);
                    LimpiarFormulario();
                    CargarLista();
                    Mostrar("Asignacion eliminada.", true);
                }
                catch (Exception ex)
                {
                    Mostrar(ex.Message, false);
                }
            }
        }

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
            hdnAsignacionID.Value = "0";
            ddlReparacion.SelectedIndex = 0;
            ddlTecnico.SelectedIndex = 0;
            txtFecha.Text = DateTime.Today.ToString("yyyy-MM-dd");
            litTituloFormulario.Text = "Asignar tecnico";
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
