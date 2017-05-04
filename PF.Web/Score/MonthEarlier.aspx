<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="MonthEarlier.aspx.cs" Inherits="PF.Web.Score.MonthEarlier" %>

<%@ Register Assembly="DevExpress.Web.v16.2, Version=16.2.6.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web" TagPrefix="dx" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
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
            font-size: 14px;
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
                <td style="font-size: 1.5em; text-align: center; padding: 15px; background-color: #f6d699">预报每日成绩
                </td>
            </tr>
            <tr>
                <td>
                    <table class="table-query">
                        <tr>
                            <td>预报时间
                            </td>
                            <td>
                                <dx:ASPxDateEdit ID="ASPxDateEdit_Date" runat="server"></dx:ASPxDateEdit>
                            </td>

                          
                            <td>
                                <asp:Button ID="Button_Query" CssClass="btn-query" runat="server" Text="查询"  />
                            </td>
                            
                        </tr>
                        
                        <tr>
                            <td>
                                青岛
                            </td>
                            <td>
                                
                            </td>
                        </tr>

                    </table>
                </td>
            </tr>
            <tr>
                <td>

                    <asp:GridView ID="GridView_List" CssClass="gridview-table" runat="server" AutoGenerateColumns="False" AllowPaging="True" PageSize="20">
                        
                    </asp:GridView>

                </td>
            </tr>
        </table>
    </form>
</body>
</html>
