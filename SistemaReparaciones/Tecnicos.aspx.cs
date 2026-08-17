using System;
using System.Collections.Generic;
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
            AplicarPermisos();

            if (!IsPostBack)
            {
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
            gvTecnicos.Columns[gvTecnicos.Columns.Count - 1].Visible = false;
        }

        private void CargarLista()
        {
            List<Tecnico> lista = logica.Buscar(txtBuscar.Text);

            // Si se elimino el ultimo registro de la ultima pagina esa pagina
            // ya no existe y la tabla saldria vacia, asi que se retrocede.
            int ultimaPagina = (lista.Count - 1) / gvTecnicos.PageSize;

            if (gvTecnicos.PageIndex > ultimaPagina)
            {
                gvTecnicos.PageIndex = ultimaPagina < 0 ? 0 : ultimaPagina;
            }

            gvTecnicos.DataSource = lista;
            gvTecnicos.DataBind();
        }

        protected void btnBuscar_Click(object sender, EventArgs e)
        {
            // Una busqueda nueva arranca desde la primera pagina
            gvTecnicos.PageIndex = 0;
            CargarLista();
        }

        protected void gvTecnicos_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gvTecnicos.PageIndex = e.NewPageIndex;
            CargarLista();
        }

        protected void btnGuardar_Click(object sender, EventArgs e)
        {
            try
            {
                ExigirAdministrador();

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

            if (!EsAdministrador)
            {
                Mostrar(SinPermiso, false);
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
