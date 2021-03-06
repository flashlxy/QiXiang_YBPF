﻿<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="DataCheck.aspx.cs" Inherits="PF.Web.Score.DataCheck" %>

<%@ Register Assembly="DevExpress.Web.v17.1, Version=17.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web" TagPrefix="dx" %>

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
           .data_table {
               border-collapse: collapse;
           }
           .data_table th, .data_table td {
               padding:3px;
               text-align: center;
               border: 1px #0094ff solid;
               
           }
    </style>
</head>
<body>
    <form id="form1" runat="server">
        <dx:ASPxPageControl ID="ASPxPageControl1" runat="server" ActiveTabIndex="3" Theme="Material">
            <TabPages>
                <dx:TabPage Text="报文校验">
                    <ContentCollection>
                        <dx:ContentControl runat="server">
                            <table style="margin: 0 auto; width: 600px;">

                                <tr>
                                    <td style="vertical-align: top;">

                                        <table class="table-query" style="width: 100%;">
                                            <tr>
                                                <td>预报日期</td>
                                                <td>
                                                    <asp:DropDownList ID="DDL_Year_BaoWen" runat="server" Font-Size="14pt">
                                                        <asp:ListItem Text="2016年" Value="2016"></asp:ListItem>
                                                        <asp:ListItem Text="2017年" Value="2017" Selected="True"></asp:ListItem>
                                                    </asp:DropDownList>
                                                    <asp:DropDownList ID="DDL_Month_BaoWen" runat="server" Font-Size="14pt">
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
                                                    <asp:Button ID="Btn_DataCheck_BaoWen" CssClass="btn btn-query" runat="server" Text="数据校验" OnClick="Btn_DataCheck_BaoWen_Click" />
                                                </td>
                                            </tr>


                                        </table>
                                    </td>

                                </tr>
                                <tr>
                                    <td>
                                        <table class="table_ybuser">

                                            <asp:Repeater ID="Repeater_YbUser" runat="server" OnItemDataBound="Repeater_YbUser_ItemDataBound">
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
                                                        <td style="text-align: center;">
                                                            <%# DateTime.Parse( DataBinder.Eval(Container.DataItem,"Date").ToString()).ToString("yyyy-MM-dd") %>
                                                        </td>
                                                        <td>
                                                            <ol>
                                                                <asp:Repeater ID="Repeater_YbUser_Night" runat="server">
                                                                    <ItemTemplate>

                                                                        <li class='<%# Eval("YbUserName").ToString()=="集体"?"label_red":""%>'>
                                                                            <label style=""><%#Eval("Work")%></label>
                                                                            <label style="color: #0094ff;"><%#Eval("YbUserName")%></label>


                                                                            <label style="color: red;"><%#Eval("Message")%></label>


                                                                        </li>


                                                                    </ItemTemplate>
                                                                </asp:Repeater>
                                                            </ol>


                                                        </td>
                                                        <td style="text-align: center;">

                                                            <%# DateTime.Parse( DataBinder.Eval(Container.DataItem,"Next_Date").ToString()).ToString("yyyy-MM-dd") %>
                                           
                                                        </td>

                                                        <td>
                                                            <ol>
                                                                <asp:Repeater ID="Repeater_YbUser_Morning" runat="server">
                                                                    <ItemTemplate>
                                                                        <li class='<%# Eval("YbUserName").ToString()=="集体"?"label_red":""%>'>
                                                                            <label style=""><%#Eval("Work")%></label>
                                                                            <label style="color: #0094ff;"><%#Eval("YbUserName")%></label>

                                                                            <label style="color: red;"><%#Eval("Message")%></label>
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
                        </dx:ContentControl>
                    </ContentCollection>
                </dx:TabPage>
                <dx:TabPage Text="实况校验">
                    <ContentCollection>
                        <dx:ContentControl runat="server">
                            <table style="margin: 0 auto; width: 600px;">

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


                                        </table>
                                    </td>

                                </tr>
                                <tr>
                                    <td style="vertical-align: top;">
                                        <table id="table-main" style="margin: 0 auto; width: 100%;">
                                            <tr>
                                                <td style="padding: 10px;">

                                                    <asp:Label ID="Label_DataMiss" runat="server" Font-Size="18" ForeColor="Red"></asp:Label>



                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
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
                        </dx:ContentControl>
                    </ContentCollection>
                </dx:TabPage>
                <dx:TabPage Text="实况数据">
                    <ContentCollection>
                        <dx:ContentControl runat="server">
                            <table style="margin: 0 auto; width: 600px;">

                                <tr>
                                    <td style="vertical-align: top;">

                                        <table class="table-query" style="width: 100%;">
                                            <tr>
                                                <td>预报日期</td>
                                                <td>
                                                    <asp:DropDownList ID="DDL_LiveData_Query_Year" runat="server" Font-Size="14pt">
                                                        <asp:ListItem Text="2016年" Value="2016"></asp:ListItem>
                                                        <asp:ListItem Text="2017年" Value="2017" Selected="True"></asp:ListItem>
                                                    </asp:DropDownList>
                                                    <asp:DropDownList ID="DDL_LiveData_Query_Month" runat="server" Font-Size="14pt">
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
                                                <td>时段</td>
                                                <td>
                                                    <asp:DropDownList ID="DDL_LiveData_Query_Time" runat="server" Font-Size="14pt">
                                                        <asp:ListItem Value="08时" Text="08-08时"></asp:ListItem>
                                                        <asp:ListItem Value="20时" Text="20-20时" Selected="True"></asp:ListItem>
                                                    </asp:DropDownList>
                                                </td>

                                                <td>
                                                    <asp:Button ID="Btn_LiveData_Query" CssClass="btn btn-query" runat="server" Text="查询" OnClick="Btn_LiveData_Query_Click"  />
                                                </td>
                                            </tr>
                                            

                                        </table>
                                    </td>

                                </tr>
                                <tr>
                                 
                                        <td>
                                            <asp:GridView ID="GridView_LiveData" CssClass="data_table" runat="server" AutoGenerateColumns="False" Width="100%">
                                                <Columns>
                                                    <asp:BoundField DataField="FDate" HeaderText="日期" DataFormatString="{0:yyyy年MM月dd日}" />
                                                    <asp:BoundField DataField="CountryName" HeaderText="区县" />
                                                    <asp:BoundField DataField="Rain" HeaderText="降水量" />
                                                    <asp:BoundField DataField="MinTemp" HeaderText="最低温度" />
                                                    <asp:BoundField DataField="MaxTemp" HeaderText="最高温度" />
                                                    <asp:BoundField DataField="Category" HeaderText="时段" />
                                                </Columns>
                                            </asp:GridView>
                                        </td>
                                      
                                    
                                </tr>
                            </table>
                        </dx:ContentControl>
                    </ContentCollection>
                </dx:TabPage>
                <dx:TabPage Text="预警信号">
                    <ContentCollection>
                        <dx:ContentControl runat="server">
                            <table>
                                <tr>
                                    <td>
                                        <table class="table-query" style="width: 100%;">
                                            <tr>
                                                <td>预报日期</td>
                                                <td>
                                                    <asp:DropDownList ID="DDL_Year_Warn" runat="server" Font-Size="14pt">
                                                        <asp:ListItem Text="2016年" Value="2016"></asp:ListItem>
                                                        <asp:ListItem Text="2017年" Value="2017" Selected="True"></asp:ListItem>
                                                    </asp:DropDownList>
                                                    <asp:DropDownList ID="DDL_Month_Warn" runat="server" Font-Size="14pt">
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
                                                    <asp:Button ID="Btn_DataImport_Warn" CssClass="btn btn-query" runat="server" Text="数据导入" OnClick="Btn_DataImport_Warn_Click" />
                                                </td>
                                                <td>
                                                    <asp:Button ID="Btn_DataCheck_Warn" CssClass="btn btn-query" runat="server" Text="数据校验" OnClick="Btn_DataCheck_Warn_Click" />
                                                </td>
                                            </tr>


                                        </table>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:GridView ID="GridView_Warn"  CssClass="data_table" runat="server" AutoGenerateColumns="False">
                                            <Columns>
                                                <asp:BoundField DataField="ReleaseTime" DataFormatString="{0:yyyy-MM-dd HH:mm}" HeaderText="发布时间" />
                                                <asp:BoundField DataField="WarningCategory" HeaderText="预警类型" />
                                                <asp:BoundField DataField="WarningLevel" HeaderText="预警级别" />
                                            </Columns>
                                        </asp:GridView> 
                                    </td>
                                </tr>
                            </table>
                        </dx:ContentControl>
                    </ContentCollection>
                </dx:TabPage>
            </TabPages>
        </dx:ASPxPageControl>
    </form>
</body>
</html>
