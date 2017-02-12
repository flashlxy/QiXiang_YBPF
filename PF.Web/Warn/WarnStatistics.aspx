<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="WarnStatistics.aspx.cs" Inherits="PF.Web.Warn.WarnStatistics" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
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
            padding: 5px 20px;
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
            border-collapse:collapse;
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
                    每月统计表
                </td>
            </tr>
            <tr>
                <td>
                    <table class="table-query">
                    
                        <tr>
                            <td>月份</td>
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

                                <asp:Button ID="Button_Query" CssClass="btn-query" runat="server" Text="查询" Font-Size="12pt" OnClick="Button_Query_Click" />
                          </td>

                        </tr>
                    </table>
                </td>
            </tr>
            <tr>
                <td>
        
                    <table class="gridview-table">
                        <tr >
                            <th rowspan="2">日期</th>
                            <th rowspan="2">预警信号类别</th>
                            <th rowspan="2">预警信号级别</th>
                            <th rowspan="2">TS评分（正确率）</th>
                            <th rowspan="2">命中率</th>
                            <th rowspan="2">漏报率</th>
                            <th rowspan="2">空报率</th>
                            <th colspan="3">时间提前量</th>                            
                        </tr>
                         <tr >
                            <th>T1</th>
                            <th>T2</th>
                            <th>T3</th>                          
                        </tr>
                        <asp:Repeater ID="Repeater_List" runat="server">
                            <ItemTemplate>
                               <tr>
                                   <td><%#Eval("Year") %>年<%#Eval("Month") %>月</td>
                                   <td><%#Eval("WarnCategory") %></td>
                                   <td><%#Eval("WarnLevel") %></td>
                                   <td><%#Eval("TSScore") %></td>
                                   <td><%#Eval("HitRate") %></td>
                                   <td><%#Eval("MissRate") %></td>
                                   <td><%#Eval("EmptyRate") %></td>
                                   <td><%#Eval("ReachSpendMinute1") %></td>
                                   <td><%#Eval("ReachSpendMinute2") %></td>
                                   <td><%#Eval("ReachSpendMinute3") %></td>
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
