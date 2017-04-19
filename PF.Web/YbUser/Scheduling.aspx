<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Scheduling.aspx.cs" Inherits="PF.Web.YbUser.Scheduling" %>

<%@ Register Assembly="DevExpress.Web.v16.2, Version=16.2.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web" TagPrefix="dx" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title></title>
    <style type="text/css">
        * {
            font-family: 'Microsoft YaHei UI';
        }

        .table-query {
            width: 100%;
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


        .label_red {
            color: red;
        }

        .table_ybuser {
            border-collapse: collapse;
            width: 100%;
        }

            .table_ybuser th, .table_ybuser td {
                padding: 3px;
                font-size: 12px;
                border: 1px solid #808080;
            }

            .table_ybuser tr:nth-child(2n+1) {
                background-color: #d3f3fc;
            }

            .table_ybuser tr:nth-child(2n) {
                background-color: #fcf9d3;
            }

        .auto-style1 {
            height: 25px;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
        <table style="margin: 0 auto;">
            <tr>
                <td style="font-size: 1.5em; text-align: center; padding: 15px; background-color: #f6d699">预报员值班表
                    <br />
                    <span style="font-size: 14px; color: red;">（注：该功能需要先计算出预报每日成绩方可使用）</span>
                </td>
            </tr>
            <tr>
                <td>
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
                </td>
            </tr>
            <tr>
                <td style="padding: 10px 0;">
                    <table style="width: 100%; border-collapse: collapse;">
                        <tr>
                            <td style="padding: 10px 40px; border: 1px solid #808080; text-align: center; font-size: 18px; background-color: #d3f3fc;">
                                <asp:DropDownList ID="DropDownList_Time_AddUser" runat="server" Font-Size="14pt">
                                    <asp:ListItem Text="08时" Value="08时"></asp:ListItem>
                                    <asp:ListItem Text="20时" Value="20时" Selected="True"></asp:ListItem>
                                </asp:DropDownList>

                            </td>
                            <td style="padding: 10px 40px; border: 1px solid #808080; text-align: center; font-size: 18px; background-color: #d3f3fc;">
                                <dx:ASPxDateEdit ID="ASPxDateEdit_YBDate" runat="server" Height="30px" Theme="iOS"></dx:ASPxDateEdit>
                            </td>
                            <td style="padding: 10px 40px; border: 1px solid #808080; text-align: center; font-size: 18px; background-color: #d3f3fc;">
                                <asp:DropDownList ID="DropDownList_YbUser" runat="server" Font-Size="14pt">
                                </asp:DropDownList>


                            </td>
                            <td style="padding: 10px 40px; border: 1px solid #808080; text-align: center; font-size: 18px; background-color: #d3f3fc;">
                                <asp:Button ID="Button_AddUser" CssClass="btn-query" runat="server" Text="添加预报员" OnClick="Button_AddUser_Click" />

                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
            <tr>
                <td>
                    <table class="table_ybuser">

                        <asp:Repeater ID="Repeater_YbUser" runat="server" OnItemDataBound="Repeater_YbUser_ItemDataBound" OnItemCreated="Repeater_YbUser_ItemCreated">
                            <HeaderTemplate>
                                <tr>
                                    <th>日期</th>
                                    <th>预报员（下午）</th>
                                    <th>日期</th>
                                    <th>预报员（上午）</th>
                                </tr>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <tr>
                                    <td>
                                        <%# DateTime.Parse( DataBinder.Eval(Container.DataItem,"Date").ToString()).ToShortDateString() %>
                                    </td>
                                    <td>
                                        <ol>
                                            <asp:Repeater ID="Repeater_YbUser_Night" runat="server">
                                                <ItemTemplate>

                                                    <li class='<%# Eval("YBUserName").ToString()=="集体"?"label_red":""%>'>

                                                        <%#Eval("Remark")==null?Eval("YBUserName"):Eval("YBUserName").ToString()+"  (手工补调)" %>      
                                                        <asp:LinkButton ID="LinkButton_Delete" runat="server" CommandArgument='<%#Eval("ScoreID")+","+(Container as RepeaterItem).ItemIndex%>' CommandName="DeleteUser" OnClientClick="javascript:return confirm( '确定要删除吗？ ');">删除</asp:LinkButton>

                                                    </li>


                                                </ItemTemplate>
                                            </asp:Repeater>
                                        </ol>


                                    </td>
                                    <td>

                                        <%# DateTime.Parse( DataBinder.Eval(Container.DataItem,"Next_Date").ToString()).ToShortDateString() %>
                                           
                                    </td>

                                    <td>
                                        <ol>
                                            <asp:Repeater ID="Repeater_YbUser_Morning" runat="server">
                                                <ItemTemplate>
                                                    <li class='<%# Eval("YBUserName").ToString()=="集体"?"label_red":""%>'>
                                                        <%--<%# DataBinder.Eval(Container.DataItem, "YBUserName") %>--%>
                                                        <%#Eval("Remark")==null?Eval("YBUserName"):Eval("YBUserName").ToString()+"  (手工补调)" %>
                                                        <asp:LinkButton ID="LinkButton_Delete" runat="server" CommandArgument='<%#Eval("ScoreID")+","+(Container as RepeaterItem).ItemIndex%>' CommandName="DeleteUser" OnClientClick="javascript:return confirm( '确定要删除吗？ ');">删除</asp:LinkButton>
                                                    </li>
                                                </ItemTemplate>
                                            </asp:Repeater>
                                        </ol>


                                    </td>
                                </tr>
                            </ItemTemplate>
                        </asp:Repeater>


                    </table>

                </td>
            </tr>
        </table>
    </form>
</body>
</html>
