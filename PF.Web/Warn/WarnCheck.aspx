<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="WarnCheck.aspx.cs" Inherits="PF.Web.Warn.WarnCheck" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title></title>
    <style type="text/css">
        * {
            font-family:'Microsoft YaHei UI';
        }
        .table-query {
            width:100%;
            border-collapse: collapse;
        }

            .table-query td {
                padding: 10px 40px;
                border: 1px solid #808080;
                text-align: center;
                font-size: 18px;
                background-color:#d3f3fc;
            }

        .btn-query {
            border: none;
            padding: 10px 20px;
            background-color: #0094ff;
            color: white;
            border-radius: 10px;
        }

            .btn-query:hover {
                background-color: #ff6a00;
                cursor: pointer;
            }

        .gridview-table {
            width:100%;
        }

        .gridview-table th, .gridview-table td {
            padding: 10px;
            text-align: center;
            border: 1px solid #808080;
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
        <table style="margin:0 auto;">
            <tr>
                <td style="font-size:1.5em;text-align:center;padding:15px;background-color:#f6d699">
                    预警信号检验
                </td>
            </tr>
            <tr>
                <td>
                    <table class="table-query">
                        <tr>
                            <td>月份</td>
                            <td>预警类型</td>
                            <td>级别</td>
                            <td rowspan="2">
                                <asp:Button ID="Button_Query" CssClass="btn-query" runat="server" Text="查询" Font-Size="14pt" OnClick="Button_Query_Click" />
                            </td>
                            <td rowspan="2">
                                <asp:Button ID="Button_Caculate" CssClass="btn-query" runat="server" Text="计算" Font-Size="14pt" OnClick="Button_Caculate_Click"  />
                            </td>
                         
                            
                        </tr>
                        <tr>
                            <td>
                                <asp:DropDownList ID="DropDownList_Year" runat="server" Font-Size="14pt">
                                    <asp:ListItem Text="2016年" Value="2016"></asp:ListItem>
                                    <asp:ListItem Text="2017年" Value="2017" Selected="True"></asp:ListItem>
                                </asp:DropDownList>
                                <asp:DropDownList ID="DropDownList_Month" runat="server" Font-Size="14pt">
                                    <asp:ListItem Text="01月" Value="01"></asp:ListItem>
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
                                <asp:DropDownList ID="DropDownList_WarnCategory" runat="server" Font-Size="14pt">
                                    <asp:ListItem Text="全部" Value="全部" Selected="True"></asp:ListItem>
                                    <asp:ListItem Text="暴雨" Value="暴雨"></asp:ListItem>
                                    <asp:ListItem Text="暴雪" Value="暴雪"></asp:ListItem>
                                    <asp:ListItem Text="大风" Value="大风"></asp:ListItem>
                                    <asp:ListItem Text="大雾" Value="大雾"></asp:ListItem>
                                    <asp:ListItem Text="霾" Value="霾"></asp:ListItem>
                                    <asp:ListItem Text="雷电" Value="雷电"></asp:ListItem>
                                    <asp:ListItem Text="冰雹" Value="冰雹"></asp:ListItem>
                                </asp:DropDownList>
                            </td>
                            <td>
                                <asp:DropDownList ID="DropDownList_WarnLevel" runat="server" Font-Size="14pt">
                                    <asp:ListItem Text="全部" Value="全部" Selected="True"></asp:ListItem>
                                    <asp:ListItem Text="蓝色" Value="蓝色"></asp:ListItem>
                                    <asp:ListItem Text="黄色" Value="黄色"></asp:ListItem>
                                    <asp:ListItem Text="橙色" Value="橙色"></asp:ListItem>
                                    <asp:ListItem Text="红色" Value="红色"></asp:ListItem>
                                </asp:DropDownList>

                            </td>

                        </tr>
                    </table>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:GridView ID="GridView_List" CssClass="gridview-table" runat="server" AutoGenerateColumns="False">
                        <Columns>
                            <asp:TemplateField HeaderText="序号" InsertVisible="False">
                                <ItemTemplate>
                                    <%#Container.DataItemIndex+1%>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:BoundField DataField="ReleaseTime" HeaderText="发布时间" />
                            <asp:BoundField DataField="WarningCategory" HeaderText="预警类型" />
                            <asp:BoundField DataField="WarningLevel" HeaderText="预警级别" />
                            <asp:BoundField DataField="ReachTime" HeaderText="最早达到预警级别时间" />
                            <asp:BoundField DataField="ReachSpendMinute" HeaderText="提前量（分钟）" />
                            <asp:BoundField DataField="Accuracy" HeaderText="准确性" />
                            <asp:BoundField DataField="ReachStation" HeaderText="最早达到预警站点" />
                            <asp:BoundField DataField="ReachValue" HeaderText="最早到达数值" />
                        </Columns>
                    </asp:GridView>

                </td>
            </tr>
        </table>



    </form>
</body>
</html>
