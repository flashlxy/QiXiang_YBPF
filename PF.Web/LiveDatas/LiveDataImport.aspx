<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="LiveDataImport.aspx.cs" Inherits="PF.Web.LiveDatas.LiveDataImport" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title></title>
    <style type="text/css">
        #table-info {
            border-collapse: collapse;
        }

            #table-info th, #table-info td {
                border: 1px solid #0094ff;
                font-size: 10px;
                padding: 5px;
            }

        .ul-datamiss li {
            padding: 5px;
            /*background-color:#ff6a00;
                border:1px solid #ffffff*/
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
        <table style="margin: 0 auto;">

            <tr>
                <td>实况类型
                </td>
                <td>
                    <asp:DropDownList ID="DropDownList_YBTime" runat="server" Font-Size="14pt">
                        <asp:ListItem Text="08时" Value="08时"></asp:ListItem>
                        <asp:ListItem Text="20时" Value="20时" Selected="True"></asp:ListItem>
                    </asp:DropDownList>
                </td>
                <td>选择日期</td>
                <td>
                    <asp:DropDownList ID="DropDownList_Year" runat="server" Font-Size="14pt">
                        <asp:ListItem Text="2016年" Value="2016"></asp:ListItem>
                        <asp:ListItem Text="2017年" Value="2017" Selected="True"></asp:ListItem>
                    </asp:DropDownList>
                    <asp:DropDownList ID="DropDownList_Month" runat="server" Font-Size="14pt">
                        <asp:ListItem Text="01月" Value="01" Selected="True"></asp:ListItem>
                        <asp:ListItem Text="02月" Value="02"></asp:ListItem>
                        <asp:ListItem Text="03月" Value="03"></asp:ListItem>
                        <asp:ListItem Text="04月" Value="04"></asp:ListItem>
                        <asp:ListItem Text="05月" Value="05"></asp:ListItem>
                        <asp:ListItem Text="06月" Value="06"></asp:ListItem>
                        <asp:ListItem Text="07月" Value="07"></asp:ListItem>
                        <asp:ListItem Text="08月" Value="08"></asp:ListItem>
                        <asp:ListItem Text="09月" Value="09"></asp:ListItem>
                        <asp:ListItem Text="10月" Value="10"></asp:ListItem>
                        <asp:ListItem Text="11月" Value="11"></asp:ListItem>
                        <asp:ListItem Text="12月" Value="12"></asp:ListItem>
                    </asp:DropDownList>

                </td>
                <td>
                    <asp:Button ID="Btn_DataCheck" runat="server" Text="数据检测" OnClick="Btn_DataCheck_Click" />
                </td>
            </tr>
            <tr>
                <td colspan="5" style="text-align: center; padding: 10px;">

                    <asp:Label ID="Label_DataMiss" runat="server" Font-Size="18" ForeColor="Red"></asp:Label>



                </td>
            </tr>
            <tr>
                <td colspan="5">
                    <asp:Panel ID="Panel_DataMiss" runat="server">

                        <%--<asp:GridView ID="GridView1" runat="server"></asp:GridView>--%>
                        <table id="table-info" style="width: 100%;">
                            <asp:Repeater ID="Repeater_DataCheck" runat="server">
                                <HeaderTemplate>
                                    <tr>
                                        <th colspan="2">数据缺少明细</th>
                                    </tr>
                                    <tr>
                                        <th>日期</th>
                                        <th>缺少数据</th>
                                    </tr>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <tr>
                                        <td>
                                            <%#DateTime.Parse( Eval("Date").ToString()).ToString("yyyy年MM月dd日") %>
                                        </td>
                                        <td>
                                            <ul class="ul-datamiss">

                                                <%#Eval("DataMiss") %>
                                            </ul>

                                        </td>
                                    </tr>
                                </ItemTemplate>
                            </asp:Repeater>
                        </table>
                    </asp:Panel>
                </td>
            </tr>
        </table>
    </form>
</body>
</html>
