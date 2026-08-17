<%@ Page Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Tecnicos.aspx.cs" Inherits="SistemaReparaciones.Tecnicos" Title="Tecnicos - Taller de Reparaciones" %>

<asp:Content ID="cuerpo" ContentPlaceHolderID="Contenido" runat="server">

    <h1 class="titulo-pagina">Tecnicos</h1>
    <p class="subtitulo-pagina">Personal del taller y el area que atiende cada uno.</p>

    <asp:Label ID="lblMensaje" runat="server" Visible="false" />

    <asp:Panel ID="pnlSoloLectura" runat="server" CssClass="mensaje mensaje-aviso" Visible="false">
        Entro como tecnico. Este mantenimiento lo puede consultar, pero agregar,
        editar o eliminar tecnicos le corresponde al administrador.
    </asp:Panel>

    <asp:Panel ID="pnlFormulario" runat="server" CssClass="panel">
        <h2><asp:Literal ID="litTituloFormulario" runat="server" Text="Agregar tecnico" /></h2>
        <div class="cuerpo">

            <asp:HiddenField ID="hdnTecnicoID" runat="server" Value="0" />

            <div class="campos">
                <div class="campo">
                    <label for="<%= txtNombre.ClientID %>">Nombre completo</label>
                    <asp:TextBox ID="txtNombre" runat="server" MaxLength="100" />
                </div>
                <div class="campo">
                    <label for="<%= txtEspecialidad.ClientID %>">Especialidad</label>
                    <asp:TextBox ID="txtEspecialidad" runat="server" MaxLength="80" />
                </div>
            </div>

            <div class="acciones">
                <asp:Button ID="btnGuardar" runat="server" Text="Guardar" CssClass="boton principal" OnClick="btnGuardar_Click" />
                <asp:Button ID="btnCancelar" runat="server" Text="Cancelar" CssClass="boton secundario" CausesValidation="false" OnClick="btnCancelar_Click" />
            </div>

        </div>
    </asp:Panel>

    <div class="panel">
        <h2>Tecnicos registrados</h2>

        <div class="buscador">
            <asp:TextBox ID="txtBuscar" runat="server" placeholder="Buscar por nombre o especialidad" />
            <asp:Button ID="btnBuscar" runat="server" Text="Buscar" CssClass="boton secundario" OnClick="btnBuscar_Click" />
        </div>

        <asp:GridView ID="gvTecnicos" runat="server" AutoGenerateColumns="false"
            CssClass="tabla" GridLines="None" UseAccessibleHeader="true"
            AlternatingRowStyle-CssClass="fila-alterna"
            AllowPaging="true" PageSize="10"
            PagerStyle-CssClass="paginador"
            PagerSettings-Mode="NumericFirstLast"
            PagerSettings-FirstPageText="Primera" PagerSettings-LastPageText="Ultima"
            OnPageIndexChanging="gvTecnicos_PageIndexChanging"
            OnRowCommand="gvTecnicos_RowCommand">
            <Columns>
                <asp:BoundField DataField="TecnicoID" HeaderText="#"
                    ItemStyle-CssClass="columna-id" HeaderStyle-CssClass="columna-id" />
                <asp:BoundField DataField="Nombre" HeaderText="Nombre" />
                <asp:BoundField DataField="Especialidad" HeaderText="Especialidad" />
                <asp:TemplateField HeaderText="Acciones"
                    ItemStyle-CssClass="columna-acciones" HeaderStyle-CssClass="columna-acciones">
                    <ItemTemplate>
                        <asp:LinkButton runat="server" CommandName="Editar"
                            CommandArgument='<%# Eval("TecnicoID") %>' Text="Editar" />
                        <asp:LinkButton runat="server" CommandName="Borrar" CssClass="enlace-borrar"
                            CommandArgument='<%# Eval("TecnicoID") %>' Text="Eliminar"
                            OnClientClick="return confirm('Seguro que desea eliminar este tecnico?');" />
                    </ItemTemplate>
                </asp:TemplateField>
            </Columns>
            <EmptyDataTemplate>
                <div class="vacio">No se encontraron tecnicos.</div>
            </EmptyDataTemplate>
        </asp:GridView>
    </div>

</asp:Content>
