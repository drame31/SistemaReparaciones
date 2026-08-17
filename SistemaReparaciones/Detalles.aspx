<%@ Page Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Detalles.aspx.cs" Inherits="SistemaReparaciones.Detalles" Title="Detalles - Taller de Reparaciones" %>

<asp:Content ID="cuerpo" ContentPlaceHolderID="Contenido" runat="server">

    <h1 class="titulo-pagina">Detalles de reparacion</h1>
    <p class="subtitulo-pagina">Trabajos concretos que se hacen dentro de cada reparacion.</p>

    <asp:Label ID="lblMensaje" runat="server" Visible="false" />

    <div class="panel">
        <h2><asp:Literal ID="litTituloFormulario" runat="server" Text="Agregar detalle" /></h2>
        <div class="cuerpo">

            <asp:HiddenField ID="hdnDetalleID" runat="server" Value="0" />

            <div class="campos">
                <div class="campo ancho">
                    <label for="<%= ddlReparacion.ClientID %>">Reparacion</label>
                    <asp:DropDownList ID="ddlReparacion" runat="server" />
                </div>
                <div class="campo ancho">
                    <label for="<%= txtDescripcion.ClientID %>">Trabajo realizado</label>
                    <asp:TextBox ID="txtDescripcion" runat="server" TextMode="MultiLine" MaxLength="250" />
                </div>
                <div class="campo">
                    <label for="<%= txtInicio.ClientID %>">Fecha de inicio</label>
                    <asp:TextBox ID="txtInicio" runat="server" TextMode="Date" />
                </div>
                <div class="campo">
                    <label for="<%= txtFin.ClientID %>">Fecha de fin (dejar vacia si sigue abierto)</label>
                    <asp:TextBox ID="txtFin" runat="server" TextMode="Date" />
                </div>
            </div>

            <div class="acciones">
                <asp:Button ID="btnGuardar" runat="server" Text="Guardar" CssClass="boton principal" OnClick="btnGuardar_Click" />
                <asp:Button ID="btnCancelar" runat="server" Text="Cancelar" CssClass="boton secundario" CausesValidation="false" OnClick="btnCancelar_Click" />
            </div>

        </div>
    </div>

    <div class="panel">
        <h2>Detalles registrados</h2>

        <div class="buscador">
            <asp:TextBox ID="txtBuscar" runat="server" placeholder="Buscar por descripcion o equipo" />
            <asp:Button ID="btnBuscar" runat="server" Text="Buscar" CssClass="boton secundario" OnClick="btnBuscar_Click" />
        </div>

        <asp:GridView ID="gvDetalles" runat="server" AutoGenerateColumns="false"
            CssClass="tabla" GridLines="None" UseAccessibleHeader="true"
            AlternatingRowStyle-CssClass="fila-alterna"
            AllowPaging="true" PageSize="10"
            PagerStyle-CssClass="paginador"
            PagerSettings-Mode="NumericFirstLast"
            PagerSettings-FirstPageText="Primera" PagerSettings-LastPageText="Ultima"
            OnPageIndexChanging="gvDetalles_PageIndexChanging"
            OnRowCommand="gvDetalles_RowCommand">
            <Columns>
                <asp:BoundField DataField="DetalleID" HeaderText="#"
                    ItemStyle-CssClass="columna-id" HeaderStyle-CssClass="columna-id" />
                <asp:BoundField DataField="ReparacionID" HeaderText="Rep." />
                <asp:BoundField DataField="DescripcionEquipo" HeaderText="Equipo" />
                <asp:BoundField DataField="Descripcion" HeaderText="Trabajo" />
                <asp:BoundField DataField="FechaInicio" HeaderText="Inicio"
                    DataFormatString="{0:dd/MM/yyyy}" />
                <asp:TemplateField HeaderText="Fin">
                    <ItemTemplate>
                        <%# Eval("FechaFin") == null ? "<span class=\"estado estado-enproceso\">En curso</span>"
                                                     : Eval("FechaFin", "{0:dd/MM/yyyy}") %>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Acciones"
                    ItemStyle-CssClass="columna-acciones" HeaderStyle-CssClass="columna-acciones">
                    <ItemTemplate>
                        <asp:LinkButton runat="server" CommandName="Editar"
                            CommandArgument='<%# Eval("DetalleID") %>' Text="Editar" />
                        <asp:LinkButton runat="server" CommandName="Borrar" CssClass="enlace-borrar"
                            CommandArgument='<%# Eval("DetalleID") %>' Text="Eliminar"
                            OnClientClick="return confirm('Seguro que desea eliminar este detalle?');" />
                    </ItemTemplate>
                </asp:TemplateField>
            </Columns>
            <EmptyDataTemplate>
                <div class="vacio">No se encontraron detalles.</div>
            </EmptyDataTemplate>
        </asp:GridView>
    </div>

</asp:Content>
