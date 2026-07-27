using System;
using System.Collections.Generic;
using System.Web;
using SistemaReparaciones.Logica;
using SistemaReparaciones.Modelo;

namespace SistemaReparaciones
{
    public partial class Default : PaginaBase
    {
        private readonly LoginLogica logica = new LoginLogica();
        private readonly ReparacionLogica reparaciones = new ReparacionLogica();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (IsPostBack)
            {
                return;
            }

            litSaludo.Text = "Hola, " + HttpUtility.HtmlEncode(UsuarioActual.NombreUsuario);

            Resumen resumen = logica.ObtenerResumen();
            litEquipos.Text = resumen.TotalEquipos.ToString();
            litUsuarios.Text = resumen.TotalUsuarios.ToString();
            litTecnicos.Text = resumen.TotalTecnicos.ToString();
            litAbiertas.Text = resumen.ReparacionesAbiertas.ToString();

            CargarUltimas();
        }

        private void CargarUltimas()
        {
            List<Reparacion> lista = reparaciones.Listar();

            // Solo las 5 mas recientes, la lista ya viene ordenada por fecha
            if (lista.Count > 5)
            {
                lista = lista.GetRange(0, 5);
            }

            gvUltimas.DataSource = lista;
            gvUltimas.DataBind();
        }

        // Convierte "En proceso" en "estado-enproceso" para pintar la etiqueta
        protected string ClaseEstado(string estado)
        {
            return "estado-" + estado.Replace(" ", "").ToLower();
        }
    }
}
