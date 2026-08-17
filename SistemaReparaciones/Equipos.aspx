<%@ Page Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Equipos.aspx.cs" Inherits="SistemaReparaciones.Equipos" Title="Equipos - Taller de Reparaciones" %>

<asp:Content ID="cuerpo" ContentPlaceHolderID="Contenido" runat="server">

    <h1 class="titulo-pagina">Equipos</h1>
    <p class="subtitulo-pagina">Equipos que los clientes han dejado en el taller.</p>

    <asp:Label ID="lblMensaje" runat="server" Visible="false" />

    <asp:Panel ID="pnlSoloLectura" runat="server" CssClass="mensaje mensaje-aviso" Visible="false">
        Entro como tecnico. Este mantenimiento lo puede consultar, pero agregar,
        editar o eliminar equipos le corresponde al administrador.
    </asp:Panel>

    <asp:Panel ID="pnlFormulario" runat="server" CssClass="panel">
        <h2><asp:Literal ID="litTituloFormulario" runat="server" Text="Registrar equipo" /></h2>
        <div class="cuerpo">

            <asp:HiddenField ID="hdnEquipoID" runat="server" Value="0" />

            <div class="campos">
                <div class="campo">
                    <label for="<%= ddlTipo.ClientID %>">Tipo de equipo</label>
                    <asp:DropDownList ID="ddlTipo" runat="server">
                        <asp:ListItem Text="Laptop" Value="Laptop" />
                        <asp:ListItem Text="Desktop" Value="Desktop" />
                        <asp:ListItem Text="Impresora" Value="Impresora" />
                        <asp:ListItem Text="Monitor" Value="Monitor" />
                        <asp:ListItem Text="Tablet" Value="Tablet" />
                        <asp:ListItem Text="Telefono" Value="Telefono" />
                        <asp:ListItem Text="Otro" Value="Otro" />
                    </asp:DropDownList>
                </div>
                <div class="campo">
                    <label for="<%= txtModelo.ClientID %>">Marca y modelo</label>
                    <asp:TextBox ID="txtModelo" runat="server" MaxLength="80" />
                </div>
                <div class="campo">
                    <label for="<%= ddlCliente.ClientID %>">Cliente</label>
                    <asp:DropDownList ID="ddlCliente" runat="server" />
                </div>
            </div>

            <div class="acciones">
                <asp:Button ID="btnGuardar" runat="server" Text="Guardar" CssClass="boton principal" OnClick="btnGuardar_Click" />
                <asp:Button ID="btnCancelar" runat="server" Text="Cancelar" CssClass="boton secundario" CausesValidation="false" OnClick="btnCancelar_Click" />
            </div>

        </div>
    </asp:Panel>

    <div class="panel">
        <h2>Equipos registrados</h2>

        <div class="buscador">
            <asp:TextBox ID="txtBuscar" runat="server" placeholder="Buscar por tipo, modelo o cliente" />
            <asp:Button ID="btnBuscar" runat="server" Text="Buscar" CssClass="boton secundario" OnClick="btnBuscar_Click" />
        </div>

        <asp:GridView ID="gvEquipos" runat="server" AutoGenerateColumns="false"
            CssClass="tabla" GridLines="None" UseAccessibleHeader="true"
            AlternatingRowStyle-CssClass="fila-alterna"
            AllowPaging="true" PageSize="10"
            PagerStyle-CssClass="paginador"
            PagerSettings-Mode="NumericFirstLast"
            PagerSettings-FirstPageText="Primera" PagerSettings-LastPageText="Ultima"
            OnPageIndexChanging="gvEquipos_PageIndexChanging"
            OnRowCommand="gvEquipos_RowCommand">
            <Columns>
                <asp:BoundField DataField="EquipoID" HeaderText="#"
                    ItemStyle-CssClass="columna-id" HeaderStyle-CssClass="columna-id" />
                <asp:BoundField DataField="TipoEquipo" HeaderText="Tipo" />
                <asp:BoundField DataField="Modelo" HeaderText="Marca y modelo" />
                <asp:BoundField DataField="NombreUsuario" HeaderText="Cliente" />
                <asp:TemplateField HeaderText="Acciones"
                    ItemStyle-CssClass="columna-acciones" HeaderStyle-CssClass="columna-acciones">
                    <ItemTemplate>
                        <asp:LinkButton runat="server" CommandName="Editar"
                            CommandArgument='<%# Eval("EquipoID") %>' Text="Editar" />
                        <asp:LinkButton runat="server" CommandName="Borrar" CssClass="enlace-borrar"
                            CommandArgument='<%# Eval("EquipoID") %>' Text="Eliminar"
                            OnClientClick="return confirm('Seguro que desea eliminar este equipo?');" />
                    </ItemTemplate>
                </asp:TemplateField>
            </Columns>
            <EmptyDataTemplate>
                <div class="vacio">No se encontraron equipos.</div>
            </EmptyDataTemplate>
        </asp:GridView>
    </div>

</asp:Content>
