<%@ Page Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Usuarios.aspx.cs" Inherits="SistemaReparaciones.Usuarios" Title="Clientes - Taller de Reparaciones" %>

<asp:Content ID="cuerpo" ContentPlaceHolderID="Contenido" runat="server">

    <h1 class="titulo-pagina">Clientes</h1>
    <p class="subtitulo-pagina">Personas y empresas que dejan equipos en el taller.</p>

    <asp:Label ID="lblMensaje" runat="server" Visible="false" />

    <div class="panel">
        <h2><asp:Literal ID="litTituloFormulario" runat="server" Text="Agregar cliente" /></h2>
        <div class="cuerpo">

            <asp:HiddenField ID="hdnUsuarioID" runat="server" Value="0" />

            <div class="campos">
                <div class="campo">
                    <label for="<%= txtNombre.ClientID %>">Nombre completo</label>
                    <asp:TextBox ID="txtNombre" runat="server" MaxLength="100" />
                </div>
                <div class="campo">
                    <label for="<%= txtCorreo.ClientID %>">Correo electronico</label>
                    <asp:TextBox ID="txtCorreo" runat="server" MaxLength="100" />
                </div>
                <div class="campo">
                    <label for="<%= txtTelefono.ClientID %>">Telefono (opcional)</label>
                    <asp:TextBox ID="txtTelefono" runat="server" MaxLength="20" />
                </div>
            </div>

            <div class="acciones">
                <asp:Button ID="btnGuardar" runat="server" Text="Guardar" CssClass="boton principal" OnClick="btnGuardar_Click" />
                <asp:Button ID="btnCancelar" runat="server" Text="Cancelar" CssClass="boton secundario" CausesValidation="false" OnClick="btnCancelar_Click" />
            </div>

        </div>
    </div>

    <div class="panel">
        <h2>Clientes registrados</h2>

        <div class="buscador">
            <asp:TextBox ID="txtBuscar" runat="server" placeholder="Buscar por nombre, correo o telefono" />
            <asp:Button ID="btnBuscar" runat="server" Text="Buscar" CssClass="boton secundario" OnClick="btnBuscar_Click" />
        </div>

        <asp:GridView ID="gvUsuarios" runat="server" AutoGenerateColumns="false"
            CssClass="tabla" GridLines="None" UseAccessibleHeader="true"
            AlternatingRowStyle-CssClass="fila-alterna"
            OnRowCommand="gvUsuarios_RowCommand">
            <Columns>
                <asp:BoundField DataField="UsuarioID" HeaderText="#"
                    ItemStyle-CssClass="columna-id" HeaderStyle-CssClass="columna-id" />
                <asp:BoundField DataField="Nombre" HeaderText="Nombre" />
                <asp:BoundField DataField="CorreoElectronico" HeaderText="Correo" />
                <asp:BoundField DataField="Telefono" HeaderText="Telefono" />
                <asp:TemplateField HeaderText="Acciones"
                    ItemStyle-CssClass="columna-acciones" HeaderStyle-CssClass="columna-acciones">
                    <ItemTemplate>
                        <asp:LinkButton runat="server" CommandName="Editar"
                            CommandArgument='<%# Eval("UsuarioID") %>' Text="Editar" />
                        <asp:LinkButton runat="server" CommandName="Borrar" CssClass="enlace-borrar"
                            CommandArgument='<%# Eval("UsuarioID") %>' Text="Eliminar"
                            OnClientClick="return confirm('Seguro que desea eliminar este cliente?');" />
                    </ItemTemplate>
                </asp:TemplateField>
            </Columns>
            <EmptyDataTemplate>
                <div class="vacio">No se encontraron clientes.</div>
            </EmptyDataTemplate>
        </asp:GridView>
    </div>

</asp:Content>
