<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="WebERPPrint.aspx.cs" Inherits="WebERPPrintSample.WebForm1" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
        <div>
            <br />
            <asp:TextBox ID="txtDoc" runat="server">FA</asp:TextBox>
            (Documento)
        <br />
            <asp:TextBox ID="txtSerie" runat="server">A</asp:TextBox>
            (Série)
        <br />
            <asp:TextBox ID="txtNum" runat="server">1</asp:TextBox>
            (Número)
        <br />
            <asp:TextBox ID="txtReport" runat="server">GCPVLS01</asp:TextBox>
            (Report)
        <br />
            <asp:TextBox ID="txtCopies" runat="server">1</asp:TextBox>
            (Cópias)
        <br />
            <asp:TextBox ID="txtEmpresa" runat="server">[cod empresa]</asp:TextBox>
            (Empresa)
        <br />
            <asp:TextBox ID="txtUtil" runat="server">[nome utilizador]</asp:TextBox>
            (Utilizador)
        <br />
            <asp:TextBox ID="txtPassword" runat="server"></asp:TextBox>
            (Password)
        <br />
            <asp:DropDownList ID="ddlVersao" runat="server" Height="16px" Width="91px"
                OnSelectedIndexChanged="DropDownList1_SelectedIndexChanged">
                <asp:ListItem Value="09.00">900</asp:ListItem>
                <asp:ListItem Value="08.00">800</asp:ListItem>
            </asp:DropDownList>
            (Versão ERP)
        <br />
            <asp:CheckBox ID="ckbImp" runat="server" Text="Alterar impressora" Visible="False" />
            <br />
            <asp:Button ID="btImprimir" runat="server" Text="Imprimir" OnClick="Button1_Click" Width="130px" />
            <br />
            <asp:Timer ID="Timer1" runat="server" Interval="500" OnTick="Timer1_Tick">
            </asp:Timer>
            <br />
            <asp:ScriptManager ID="ScriptManagerPrint" runat="server" AsyncPostBackTimeout="0"></asp:ScriptManager>
            <asp:UpdatePanel ID="PrintUpdate" UpdateMode="Conditional" runat="server" ChildrenAsTriggers="False">
                <ContentTemplate>
                    <br />
                    <fieldset>
                        <legend>Estado da impressão:</legend>
                        <asp:Label ID="lblOutput" runat="server" Text="" />
                    </fieldset>
                </ContentTemplate>
                <Triggers>
                    <asp:AsyncPostBackTrigger ControlID="Timer1" EventName="Tick" />
                </Triggers>
            </asp:UpdatePanel>
        </div>
        <p>
            &nbsp;</p>
    </form>
</body>
</html>
