using System;
using System.Web.UI.WebControls;
using SistemaReparaciones.Logica;
using SistemaReparaciones.Modelo;

namespace SistemaReparaciones
{
    public partial class Detalles : PaginaBase
    {
        private readonly DetalleLogica logica = new DetalleLogica();
        private readonly ReparacionLogica reparaciones = new ReparacionLogica();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                CargarReparaciones();
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
                               " (" + r.NombreUsuario + ")";
                ddlReparacion.Items.Add(new ListItem(texto, r.ReparacionID.ToString()));
            }
        }

        private void CargarLista()
        {
            gvDetalles.DataSource = logica.Buscar(txtBuscar.Text);
            gvDetalles.DataBind();
        }

        protected void btnBuscar_Click(object sender, EventArgs e)
        {
            CargarLista();
        }

        // Las cajas de fecha pueden quedar vacias, por eso devuelve un nullable
        private DateTime? LeerFecha(string texto, string nombreCampo)
        {
            if (string.IsNullOrWhiteSpace(texto))
            {
                return null;
            }

            DateTime fecha;
            if (!DateTime.TryParse(texto, out fecha))
            {
                throw new ApplicationException("La " + nombreCampo + " no es una fecha valida.");
            }

            return fecha;
        }

        protected void btnGuardar_Click(object sender, EventArgs e)
        {
            try
            {
                DetalleReparacion d = new DetalleReparacion();
                d.DetalleID = int.Parse(hdnDetalleID.Value);
                d.ReparacionID = int.Parse(ddlReparacion.SelectedValue);
                d.Descripcion = txtDescripcion.Text;
                d.FechaInicio = LeerFecha(txtInicio.Text, "fecha de inicio");
                d.FechaFin = LeerFecha(txtFin.Text, "fecha de fin");

                bool esNuevo = d.DetalleID == 0;
                logica.Guardar(d);

                LimpiarFormulario();
                CargarLista();
                Mostrar(esNuevo ? "Detalle agregado correctamente." : "Detalle actualizado correctamente.", true);
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

        protected void gvDetalles_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName != "Editar" && e.CommandName != "Borrar")
            {
                return;
            }

            int detalleID = Convert.ToInt32(e.CommandArgument);

            if (e.CommandName == "Editar")
            {
                DetalleReparacion d = logica.Obtener(detalleID);

                if (d == null)
                {
                    Mostrar("Ese detalle ya no existe.", false);
                    CargarLista();
                    return;
                }

                hdnDetalleID.Value = d.DetalleID.ToString();
                SeleccionarEnLista(ddlReparacion, d.ReparacionID.ToString());
                txtDescripcion.Text = d.Descripcion;
                txtInicio.Text = d.FechaInicio.HasValue ? d.FechaInicio.Value.ToString("yyyy-MM-dd") : "";
                txtFin.Text = d.FechaFin.HasValue ? d.FechaFin.Value.ToString("yyyy-MM-dd") : "";

                litTituloFormulario.Text = "Editando detalle #" + d.DetalleID;
                OcultarMensaje();
            }
            else
            {
                try
                {
                    logica.Eliminar(detalleID);
                    LimpiarFormulario();
                    CargarLista();
                    Mostrar("Detalle eliminado.", true);
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
            hdnDetalleID.Value = "0";
            ddlReparacion.SelectedIndex = 0;
            txtDescripcion.Text = "";
            txtInicio.Text = DateTime.Today.ToString("yyyy-MM-dd");
            txtFin.Text = "";
            litTituloFormulario.Text = "Agregar detalle";
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
