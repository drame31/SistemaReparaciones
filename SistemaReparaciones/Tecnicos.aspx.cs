using System;
using System.Web.UI.WebControls;
using SistemaReparaciones.Logica;
using SistemaReparaciones.Modelo;

namespace SistemaReparaciones
{
    public partial class Tecnicos : PaginaBase
    {
        private readonly TecnicoLogica logica = new TecnicoLogica();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                CargarLista();
            }
        }

        private void CargarLista()
        {
            gvTecnicos.DataSource = logica.Buscar(txtBuscar.Text);
            gvTecnicos.DataBind();
        }

        protected void btnBuscar_Click(object sender, EventArgs e)
        {
            CargarLista();
        }

        protected void btnGuardar_Click(object sender, EventArgs e)
        {
            try
            {
                Tecnico t = new Tecnico();
                t.TecnicoID = int.Parse(hdnTecnicoID.Value);
                t.Nombre = txtNombre.Text;
                t.Especialidad = txtEspecialidad.Text;

                bool esNuevo = t.TecnicoID == 0;
                logica.Guardar(t);

                LimpiarFormulario();
                CargarLista();
                Mostrar(esNuevo ? "Tecnico agregado correctamente." : "Tecnico actualizado correctamente.", true);
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

        protected void gvTecnicos_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName != "Editar" && e.CommandName != "Borrar")
            {
                return;
            }

            int tecnicoID = Convert.ToInt32(e.CommandArgument);

            if (e.CommandName == "Editar")
            {
                Tecnico t = logica.Obtener(tecnicoID);

                if (t == null)
                {
                    Mostrar("Ese tecnico ya no existe.", false);
                    CargarLista();
                    return;
                }

                hdnTecnicoID.Value = t.TecnicoID.ToString();
                txtNombre.Text = t.Nombre;
                txtEspecialidad.Text = t.Especialidad;

                litTituloFormulario.Text = "Editando tecnico #" + t.TecnicoID;
                OcultarMensaje();
            }
            else
            {
                try
                {
                    logica.Eliminar(tecnicoID);
                    LimpiarFormulario();
                    CargarLista();
                    Mostrar("Tecnico eliminado.", true);
                }
                catch (Exception ex)
                {
                    Mostrar(ex.Message, false);
                }
            }
        }

        private void LimpiarFormulario()
        {
            hdnTecnicoID.Value = "0";
            txtNombre.Text = "";
            txtEspecialidad.Text = "";
            litTituloFormulario.Text = "Agregar tecnico";
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
