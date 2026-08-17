<%@ Page Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Asignaciones.aspx.cs" Inherits="SistemaReparaciones.Asignaciones" Title="Asignaciones - Taller de Reparaciones" %>

<asp:Content ID="cuerpo" ContentPlaceHolderID="Contenido" runat="server">

    <h1 class="titulo-pagina">Asignaciones</h1>
    <p class="subtitulo-pagina">Que tecnico esta atendiendo cada reparacion.</p>

    <asp:Label ID="lblMensaje" runat="server" Visible="false" />

    <div class="panel">
        <h2><asp:Literal ID="litTituloFormulario" runat="server" Text="Asignar tecnico" /></h2>
        <div class="cuerpo">

            <asp:HiddenField ID="hdnAsignacionID" runat="server" Value="0" />

            <div class="campos">
                <div class="campo ancho">
                    <label for="<%= ddlReparacion.ClientID %>">Reparacion</label>
                    <asp:DropDownList ID="ddlReparacion" runat="server" />
                </div>
                <div class="campo">
                    <label for="<%= ddlTecnico.ClientID %>">Tecnico</label>
                    <asp:DropDownList ID="ddlTecnico" runat="server" />
                </div>
                <div class="campo">
                    <label for="<%= txtFecha.ClientID %>">Fecha de asignacion</label>
                    <asp:TextBox ID="txtFecha" runat="server" TextMode="Date" />
                </div>
            </div>

            <div class="acciones">
                <asp:Button ID="btnGuardar" runat="server" Text="Guardar" CssClass="boton principal" OnClick="btnGuardar_Click" />
                <asp:Button ID="btnCancelar" runat="server" Text="Cancelar" CssClass="boton secundario" CausesValidation="false" OnClick="btnCancelar_Click" />
            </div>

        </div>
    </div>

    <div class="panel">
        <h2>Asignaciones registradas</h2>

        <div class="buscador">
            <asp:TextBox ID="txtBuscar" runat="server" placeholder="Buscar por tecnico o equipo" />
            <asp:Button ID="btnBuscar" runat="server" Text="Buscar" CssClass="boton secundario" OnClick="btnBuscar_Click" />
        </div>

        <asp:GridView ID="gvAsignaciones" runat="server" AutoGenerateColumns="false"
            CssClass="tabla" GridLines="None" UseAccessibleHeader="true"
            AlternatingRowStyle-CssClass="fila-alterna"
            AllowPaging="true" PageSize="10"
            PagerStyle-CssClass="paginador"
            PagerSettings-Mode="NumericFirstLast"
            PagerSettings-FirstPageText="Primera" PagerSettings-LastPageText="Ultima"
            OnPageIndexChanging="gvAsignaciones_PageIndexChanging"
            OnRowCommand="gvAsignaciones_RowCommand">
            <Columns>
                <asp:BoundField DataField="AsignacionID" HeaderText="#"
                    ItemStyle-CssClass="columna-id" HeaderStyle-CssClass="columna-id" />
                <asp:BoundField DataField="ReparacionID" HeaderText="Rep." />
                <asp:BoundField DataField="DescripcionEquipo" HeaderText="Equipo" />
                <asp:BoundField DataField="NombreTecnico" HeaderText="Tecnico" />
                <asp:BoundField DataField="FechaAsignacion" HeaderText="Asignado el"
                    DataFormatString="{0:dd/MM/yyyy}" />
                <asp:TemplateField HeaderText="Acciones"
                    ItemStyle-CssClass="columna-acciones" HeaderStyle-CssClass="columna-acciones">
                    <ItemTemplate>
                        <asp:LinkButton runat="server" CommandName="Editar"
                            CommandArgument='<%# Eval("AsignacionID") %>' Text="Editar" />
                        <asp:LinkButton runat="server" CommandName="Borrar" CssClass="enlace-borrar"
                            CommandArgument='<%# Eval("AsignacionID") %>' Text="Eliminar"
                            OnClientClick="return confirm('Seguro que desea eliminar esta asignacion?');" />
                    </ItemTemplate>
                </asp:TemplateField>
            </Columns>
            <EmptyDataTemplate>
                <div class="vacio">No se encontraron asignaciones.</div>
            </EmptyDataTemplate>
        </asp:GridView>
    </div>

</asp:Content>
