<%@ Page Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Reparaciones.aspx.cs" Inherits="SistemaReparaciones.Reparaciones" Title="Reparaciones - Taller de Reparaciones" %>

<asp:Content ID="cuerpo" ContentPlaceHolderID="Contenido" runat="server">

    <h1 class="titulo-pagina">Reparaciones</h1>
    <p class="subtitulo-pagina">Ordenes de trabajo abiertas para cada equipo.</p>

    <asp:Label ID="lblMensaje" runat="server" Visible="false" />

    <div class="panel">
        <h2><asp:Literal ID="litTituloFormulario" runat="server" Text="Abrir reparacion" /></h2>
        <div class="cuerpo">

            <asp:HiddenField ID="hdnReparacionID" runat="server" Value="0" />

            <div class="campos">
                <div class="campo">
                    <label for="<%= ddlEquipo.ClientID %>">Equipo</label>
                    <asp:DropDownList ID="ddlEquipo" runat="server" />
                </div>
                <div class="campo">
                    <label for="<%= txtFecha.ClientID %>">Fecha de solicitud</label>
                    <asp:TextBox ID="txtFecha" runat="server" TextMode="Date" />
                </div>
                <div class="campo">
                    <label for="<%= ddlEstado.ClientID %>">Estado</label>
                    <asp:DropDownList ID="ddlEstado" runat="server" />
                </div>
            </div>

            <div class="acciones">
                <asp:Button ID="btnGuardar" runat="server" Text="Guardar" CssClass="boton principal" OnClick="btnGuardar_Click" />
                <asp:Button ID="btnCancelar" runat="server" Text="Cancelar" CssClass="boton secundario" CausesValidation="false" OnClick="btnCancelar_Click" />
            </div>

        </div>
    </div>

    <div class="panel">
        <h2>Reparaciones registradas</h2>

        <div class="buscador">
            <asp:TextBox ID="txtBuscar" runat="server" placeholder="Buscar por equipo, cliente o estado" />
            <asp:Button ID="btnBuscar" runat="server" Text="Buscar" CssClass="boton secundario" OnClick="btnBuscar_Click" />
        </div>

        <asp:GridView ID="gvReparaciones" runat="server" AutoGenerateColumns="false"
            CssClass="tabla" GridLines="None" UseAccessibleHeader="true"
            AlternatingRowStyle-CssClass="fila-alterna"
            OnRowCommand="gvReparaciones_RowCommand">
            <Columns>
                <asp:BoundField DataField="ReparacionID" HeaderText="#"
                    ItemStyle-CssClass="columna-id" HeaderStyle-CssClass="columna-id" />
                <asp:BoundField DataField="DescripcionEquipo" HeaderText="Equipo" />
                <asp:BoundField DataField="NombreUsuario" HeaderText="Cliente" />
                <asp:BoundField DataField="FechaSolicitud" HeaderText="Solicitud"
                    DataFormatString="{0:dd/MM/yyyy}" />
                <asp:TemplateField HeaderText="Estado">
                    <ItemTemplate>
                        <span class='estado <%# ClaseEstado(Eval("Estado").ToString()) %>'>
                            <%# Eval("Estado") %></span>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Acciones"
                    ItemStyle-CssClass="columna-acciones" HeaderStyle-CssClass="columna-acciones">
                    <ItemTemplate>
                        <asp:LinkButton runat="server" CommandName="Editar"
                            CommandArgument='<%# Eval("ReparacionID") %>' Text="Editar" />
                        <asp:LinkButton runat="server" CommandName="Borrar" CssClass="enlace-borrar"
                            CommandArgument='<%# Eval("ReparacionID") %>' Text="Eliminar"
                            OnClientClick="return confirm('Seguro que desea eliminar esta reparacion?');" />
                    </ItemTemplate>
                </asp:TemplateField>
            </Columns>
            <EmptyDataTemplate>
                <div class="vacio">No se encontraron reparaciones.</div>
            </EmptyDataTemplate>
        </asp:GridView>
    </div>

</asp:Content>
