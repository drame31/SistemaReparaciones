using System;
using System.Collections.Generic;
using System.Web.UI.WebControls;
using SistemaReparaciones.Logica;
using SistemaReparaciones.Modelo;

namespace SistemaReparaciones
{
    public partial class Reparaciones : PaginaBase
    {
        private readonly ReparacionLogica logica = new ReparacionLogica();
        private readonly EquipoLogica equipos = new EquipoLogica();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                CargarEquipos();
                CargarEstados();
                LimpiarFormulario();
                CargarLista();
            }
        }

        private void CargarEquipos()
        {
            ddlEquipo.Items.Clear();
            ddlEquipo.Items.Add(new ListItem("-- Seleccione --", "0"));

            // Se arma el texto a mano para que se vea el equipo y de quien es
            foreach (Equipo eq in equipos.Listar())
            {
                string texto = eq.TipoEquipo + " " + eq.Modelo + " - " + eq.NombreUsuario;
                ddlEquipo.Items.Add(new ListItem(texto, eq.EquipoID.ToString()));
            }
        }

        private void CargarEstados()
        {
            ddlEstado.Items.Clear();

            foreach (string estado in ReparacionLogica.Estados)
            {
                ddlEstado.Items.Add(new ListItem(estado, estado));
            }
        }

        private void CargarLista()
        {
            gvReparaciones.DataSource = logica.Buscar(txtBuscar.Text);
            gvReparaciones.DataBind();
        }

        protected void btnBuscar_Click(object sender, EventArgs e)
        {
            CargarLista();
        }

        protected void btnGuardar_Click(object sender, EventArgs e)
        {
            try
            {
                Reparacion r = new Reparacion();
                r.ReparacionID = int.Parse(hdnReparacionID.Value);
                r.EquipoID = int.Parse(ddlEquipo.SelectedValue);
                r.Estado = ddlEstado.SelectedValue;

                DateTime fecha;
                if (!DateTime.TryParse(txtFecha.Text, out fecha))
                {
                    throw new ApplicationException("Escriba una fecha de solicitud valida.");
                }
                r.FechaSolicitud = fecha;

                bool esNueva = r.ReparacionID == 0;
                logica.Guardar(r);

                LimpiarFormulario();
                CargarLista();
                Mostrar(esNueva ? "Reparacion registrada correctamente." : "Reparacion actualizada correctamente.", true);
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

        protected void gvReparaciones_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName != "Editar" && e.CommandName != "Borrar")
            {
                return;
            }

            int reparacionID = Convert.ToInt32(e.CommandArgument);

            if (e.CommandName == "Editar")
            {
                Reparacion r = logica.Obtener(reparacionID);

                if (r == null)
                {
                    Mostrar("Esa reparacion ya no existe.", false);
                    CargarLista();
                    return;
                }

                hdnReparacionID.Value = r.ReparacionID.ToString();
                SeleccionarEnLista(ddlEquipo, r.EquipoID.ToString());
                SeleccionarEnLista(ddlEstado, r.Estado);
                txtFecha.Text = r.FechaSolicitud.ToString("yyyy-MM-dd");

                litTituloFormulario.Text = "Editando reparacion #" + r.ReparacionID;
                OcultarMensaje();
            }
            else
            {
                try
                {
                    logica.Eliminar(reparacionID);
                    LimpiarFormulario();
                    CargarLista();
                    Mostrar("Reparacion eliminada.", true);
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

        protected string ClaseEstado(string estado)
        {
            return "estado-" + estado.Replace(" ", "").ToLower();
        }

        private void LimpiarFormulario()
        {
            hdnReparacionID.Value = "0";
            ddlEquipo.SelectedIndex = 0;
            ddlEstado.SelectedIndex = 0;
            // Por defecto se propone la fecha de hoy, que es lo normal al recibir un equipo
            txtFecha.Text = DateTime.Today.ToString("yyyy-MM-dd");
            litTituloFormulario.Text = "Abrir reparacion";
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
