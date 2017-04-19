<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="YbUserScheduling.aspx.cs" Inherits="PF.Web.YbUser.YbUserScheduling" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title></title>
    <style type="text/css">
           .table-query {
           
            border-collapse: collapse;
        }

            .table-query td {
                padding: 10px 40px;
                border: 1px solid #808080;
                text-align: center;
                font-size: 18px;
                background-color: #d3f3fc;
            }

        .btn-query {
            border: none;
            padding: 5px 20px;
            background-color: #0094ff;
            color: white;
            border-radius: 10px;
        }

            .btn-query:hover {
                background-color: #ff6a00;
                cursor: pointer;
            }
         .day_item {
             float: left;
             border: 1px solid #ff6a00;
             width: 150px;
             /*height: 100px;*/
             padding: 0;
         }
        .table_item {
            margin:0;
            width:100%;
            border-collapse: collapse;
            border: none;
        }
        .table_item th {
            background-color:rgba(6, 150, 132, 0.31);
        }
        
        .table_item th, .table_item td{
            padding: 10px;
            font-size: 14px;
            vertical-align: middle;
            text-align: center;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
        <div>
               <table class="table-query">
                        <tr>
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
                                <asp:Button ID="Button_Query" CssClass="btn-query" runat="server" Text="查询" OnClick="Button_Query_Click" />
                            </td>

                        </tr>


                    </table>
            <br/>
            <asp:Repeater ID="RepeaterScheduling" runat="server" OnItemDataBound="RepeaterScheduling_ItemDataBound">
                <ItemTemplate>
                    <div class="day_item">
                        <table class="table_item">
                            <tr>
                                <th colspan="2">
                                    <asp:HiddenField ID="HiddenField_DayTime" runat="server" Value='<%#Eval("DayTimeString") %>' />
                                    <%#Eval("DayTimeString") %></th>
                            </tr>
                            <tr>
                                <td>首席</td>
                                <td><asp:DropDownList ID="DropDownList_ShouXi" runat="server" AutoPostBack="True" OnSelectedIndexChanged="DropDownList_ShouXi_SelectedIndexChanged"></asp:DropDownList></td>
                            </tr>
                            <tr>
                                <td>领班</td>
                                <td><asp:DropDownList ID="DropDownList_LingBan" runat="server" AutoPostBack="True" OnSelectedIndexChanged="DropDownList_LingBan_SelectedIndexChanged"></asp:DropDownList></td>
                            </tr>
                            <tr>
                                <td>值班</td>
                                <td><asp:DropDownList ID="DropDownList_ZhiBan" runat="server" AutoPostBack="True" OnSelectedIndexChanged="DropDownList_ZhiBan_SelectedIndexChanged"></asp:DropDownList></td>
                            </tr>
                        </table>


                      
                      
                    </div>
                </ItemTemplate>
            </asp:Repeater>
        </div>
    </form>
</body>
</html>
