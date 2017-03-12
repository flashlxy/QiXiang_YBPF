<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="LiveDataImport.aspx.cs" Inherits="PF.Web.LiveDatas.LiveDataImport" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title></title>
    <style type="text/css">
        #table-main td {
            padding: 5px;
        }

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

        .btn {
            border: 0;
            padding: 10px;
            border-radius: 5px;
            background-color: #0094ff;
            color: white;
        }

        .table-query {
            width: 100%;
            border-collapse: collapse;
        }

            .table-query td {
                padding: 10px;
                border: 1px solid #808080;
                text-align: center;
                font-size: 18px;
                background-color: #d3f3fc;
            }
    </style>
</head>
<body>
    <form id="form1" runat="server">
        <table style="margin: 0 auto;">
            <tr>
                <td style="text-align: center; font-size: 2em;">实况数据校验和导入
                </td>

            </tr>
            <tr>
                <td style="vertical-align: top;">

                    <table class="table-query" style="width: 100%;">
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
                                <asp:Button ID="Btn_DataCheck" CssClass="btn btn-query" runat="server" Text="数据校验" OnClick="Btn_DataCheck_Click" />
                            </td>
                        </tr>

                        <tr>
                            <td>
                                <asp:Button ID="Btn_Import_Rain08" CssClass="btn btn-query" runat="server" Text="导入08时降水" OnClick="Btn_Import_Rain08_Click" />
                               <br />
                                <span>r24-8-p</span>
                            </td>
                            <td>
                                <asp:Button ID="Btn_Import_Temp08" CssClass="btn btn-query" runat="server" Text="导入08时温度" OnClick="Btn_Import_Temp08_Click" />
                                   <br />
                                <span>数据库</span>
                            </td>
                            <td>
                                <asp:Button ID="Btn_Import_TempAndRain20" CssClass="btn btn-query" runat="server" Text="导入20时温度和降水" OnClick="Btn_Import_TempAndRain20_Click" />
                                <br />
                                <span>A文件数据</span>
                            </td>
                            <td colspan="2">
                                <asp:Button ID="Btn_Import_All" CssClass="btn btn-query" runat="server" Text="导入全部数据" BackColor="#FF3300" OnClick="Btn_Import_All_Click" />
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Button ID="Button1" CssClass="btn btn-query" runat="server" Text="导入20时温度(数据库)" BackColor="#FF3300" OnClick="Btn_Import_All_Click" />
                                
                            </td>
                        </tr>
                    </table>

                </td>
                <td rowspan="2" style="vertical-align: top; padding: 25px; width: 300px;">
                    <fieldset>
                        <legend>注意事项</legend>
                        1、导入08时降水：需将"r24-8-p"文件放置到"\\172.18.226.109\市县一体化平台文档\检验\r24-8-p"目录下。<br />
                        2、导入08时温度：该程序自动从自动站实况数据库中计算获得，无需另外添加数据。<br />
                        3、导入20时降水和温度，需将"A"文件放置到"\\172.18.226.109\市县一体化平台文档\检验\A"目录下。<br />
                        <span style="color: red;">注：以上目录地址为固定格式，请勿擅自更改！</span>
                    </fieldset>
                </td>
            </tr>
            <tr>
                <td style="vertical-align: top;">
                    <table id="table-main" style="margin: 0 auto; width: 100%;">




                        <tr>
                            <td colspan="5" style="padding: 10px;">

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
                                                    <td style="text-align: center; font-size: 12px;">
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
                </td>
            </tr>
        </table>

    </form>
</body>
</html>
