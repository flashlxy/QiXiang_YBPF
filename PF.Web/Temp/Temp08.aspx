<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Temp08.aspx.cs" Inherits="PF.Web.Temp.Temp08" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <asp:DropDownList ID="MonthDDL" runat="server"  >
             <asp:ListItem Value="0">---选择月份---</asp:ListItem>
                            <asp:ListItem Value="01">1月</asp:ListItem>
                            <asp:ListItem Value="02">2月</asp:ListItem>
                            <asp:ListItem Value="03">3月</asp:ListItem>
                            <asp:ListItem Value="04">4月</asp:ListItem>
                            <asp:ListItem Value="05">5月</asp:ListItem>
                            <asp:ListItem Value="06">6月</asp:ListItem>
                            <asp:ListItem Value="07">7月</asp:ListItem>
                            <asp:ListItem Value="08">8月</asp:ListItem>
                            <asp:ListItem Value="09">9月</asp:ListItem>
                            <asp:ListItem Value="10">10月</asp:ListItem>
                            <asp:ListItem Value="11">11月</asp:ListItem>
                            <asp:ListItem Value="12">12月</asp:ListItem>
        </asp:DropDownList>
        <asp:Button ID="BtnImport" runat="server" Text="Button" OnClick="BtnImport_Click" />
    </div>
    </form>
</body>
</html>
