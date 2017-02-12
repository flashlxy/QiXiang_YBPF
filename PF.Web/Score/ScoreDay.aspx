<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ScoreDay.aspx.cs" Inherits="PF.Web.Score.ScoreDay" %>

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

        .gridview-table th, .gridview-table td {
            padding: 3px;
            text-align: center;
            border: 1px solid #808080;
            font-size:12px;
        }

        .gridview-table tr:nth-child(2n+1) {
            background-color: #d3f3fc;
        }

        .gridview-table tr:nth-child(2n) {
            background-color: #fcf9d3;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
        <table style="margin: 0 auto;">
            <tr>
                <td style="font-size: 1.5em; text-align: center; padding: 15px; background-color: #f6d699">每日成绩
                </td>
            </tr>
            <tr>
                <td>
                    <table class="table-query">
                        <tr>
                            <td>预报时间
                            </td>
                            <td>
                                <asp:DropDownList ID="DropDownList_YBTime" runat="server" Font-Size="14pt">
                                    <asp:ListItem Text="08时" Value="08时"></asp:ListItem>
                                    <asp:ListItem Text="20时" Value="20时"  Selected="True"></asp:ListItem>
                                </asp:DropDownList>
                            </td>

                            <td>选择日期</td>
                            <td>
                                <asp:DropDownList ID="DropDownList_Year" runat="server" Font-Size="14pt">
                                    <asp:ListItem Text="2016年" Value="2016"></asp:ListItem>
                                    <asp:ListItem Text="2017年" Value="2017" Selected="True"></asp:ListItem>
                                </asp:DropDownList>
                                <asp:DropDownList ID="DropDownList_Month" runat="server" Font-Size="14pt">
                                    <asp:ListItem Text="01月" Value="01"  Selected="True"></asp:ListItem>
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
                                <asp:Button ID="Button_Query" CssClass="btn-query" runat="server" Text="确定" OnClick="Button_Query_Click" />
                            </td>
                        </tr>

                    </table>
                </td>
            </tr>
            <tr>
                <td>
                    <table>
                        <tr>
                            <td>
                                <asp:GridView ID="GridView_List" CssClass="gridview-table" runat="server" AutoGenerateColumns="False" AllowPaging="True" OnPageIndexChanging="GridView_List_PageIndexChanging" PageSize="20">
                                    <Columns>
                                        <asp:BoundField DataField="YBDate" DataFormatString="{0:yyyy年M月dd日}" HeaderText="预报日期" />
                                        <asp:BoundField DataField="YBTime" HeaderText="预报时间" />
                                        <asp:BoundField DataField="YBUserName" HeaderText="姓名" />
                                        <asp:BoundField DataField="RainShine24" HeaderText="晴雨24" />
                                        <asp:BoundField DataField="MaxTemp24" HeaderText="高温24" />
                                        <asp:BoundField DataField="MinTemp24" HeaderText="低温24" />
                                        <asp:BoundField DataField="RainShine48" HeaderText="晴雨48" />
                                        <asp:BoundField DataField="MaxTemp48" HeaderText="高温48" />
                                        <asp:BoundField DataField="MinTemp48" HeaderText="低温48" />
                                        <asp:BoundField DataField="RainShine72" HeaderText="晴雨72" />
                                        <asp:BoundField DataField="MaxTemp72" HeaderText="高温72" />
                                        <asp:BoundField DataField="MinTemp72" HeaderText="低温72" />
                                        <asp:BoundField DataField="AllTotal" HeaderText="总数" />
                                        <asp:BoundField DataField="Rainfall24" HeaderText="降水24" />
                                        <asp:BoundField DataField="Rainfall24Total" HeaderText="降水24总数" />
                                        <asp:BoundField DataField="Rainfall48" HeaderText="降水48" />
                                        <asp:BoundField DataField="Rainfall48Total" HeaderText="降水48总数" />
                                        <asp:BoundField DataField="Rainfall72" HeaderText="降水72" />
                                        <asp:BoundField DataField="Rainfall72Total" HeaderText="降水72总数" />
                                        <asp:BoundField DataField="Rainstorm24" HeaderText="暴雨24" />
                                        <asp:BoundField DataField="Rainstorm24Total" HeaderText="暴雨24总数" />
                                        <asp:BoundField DataField="Rainstorm48" HeaderText="暴雨48" />
                                        <asp:BoundField DataField="Rainstorm48Total" HeaderText="暴雨48总数" />
                                    </Columns>

                                </asp:GridView>
                            </td>

                        </tr>
                    </table>
                </td>
            </tr>
        </table>
    </form>
</body>
</html>
