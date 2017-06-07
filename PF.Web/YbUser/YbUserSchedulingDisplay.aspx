<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="YbUserSchedulingDisplay.aspx.cs" Inherits="PF.Web.YbUser.YbUserSchedulingDisplay" %>

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

        .day_head {
            float: left;
            /*border: 1px solid #808080;*/
            margin: 1px;
            width: 118px;
            text-align: center;
            padding: 5px 0;
        }

        .day_item {
            float: left;
            /*border: 1px solid #808080;*/
            /*margin: 1px;*/
            width: 120px;
            /*height: 100px;*/
            padding: 0;
        }

        .table_item {
            margin: 0;
            width: 100%;
            border-collapse: collapse;
            border: none;
        }

        .table_item_workday {
            /*background-color: rgba(0, 148, 255, 0.51);*/
            background-color: rgba(178, 189, 197, 0.51);
            
        }

        .table_item_weekday {
            /*background-color: rgba(255, 106, 0,0.5);*/
            background-color:rgba(226, 150, 96, 0.5);
            
        }

        .table_item th {
            /*background-color:rgba(6, 150, 132, 0.31);*/
        }

        .table_item th, .table_item td {
            padding: 2px 3px;
            font-size: 12px;
            vertical-align: middle;
            text-align: center;
            border:1px solid #eee;
        }

        .workday {
            background-color: rgba(0, 148, 255, 0.51);
            /*color: white;*/
        }

        .weekday {
            background-color: rgba(255, 106, 0,0.5);
            /*color: white;*/
        }

        .notcurrentmonth {
            visibility: hidden;
        }

        .Label_Description {
            margin:15px;
        }

        .table_today  td{
            text-align: center;
            padding:10px;
        }

        .lable_user {
            font-size:30px;
            text-shadow:2px 2px 2px #333;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">

        <table style="margin: 0 auto; border-collapse: collapse;">
            <tr>
                <td  style="padding:10px 0;">
                    <table class="table-query">
                        <tr>
                            <td>预报值班表</td>
                            <td>
                                <asp:DropDownList ID="DropDownList_Year" runat="server" Font-Size="14pt">
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
                                <asp:Button ID="Button_Query" CssClass="btn-query" runat="server" Text="查询" OnClick="Button_Query_Click" />
                            </td>

                        </tr>


                    </table>
                </td>
                <td></td>
               
            </tr>
            <tr>
                <td style="width:840px; border: 2px solid #5BADFF;" >
                    <div style="max-width: 840px; margin: 0 auto; min-width: 840px;">

                        <div class="day_head workday">星期一</div>
                        <div class="day_head workday">星期二</div>
                        <div class="day_head workday">星期三</div>
                        <div class="day_head workday">星期四</div>
                        <div class="day_head workday">星期五</div>
                        <div class="day_head weekday">星期六</div>
                        <div class="day_head weekday">星期日</div>

                        <asp:Repeater ID="RepeaterScheduling" runat="server" OnItemDataBound="RepeaterScheduling_ItemDataBound">
                            <ItemTemplate>
                                <div class="day_item">
                                    <table class="table_item <%#int.Parse(Eval("Week").ToString())>5?"table_item_weekday":"table_item_workday" %> <%#bool.Parse(Eval("IsCurrentMonth").ToString())==true?"currentmonth":"notcurrentmonth" %>">
                                        <tr>
                                            <th colspan="2" class="<%#int.Parse(Eval("Week").ToString())>5?"weekday":"workday" %>">
                                                <asp:HiddenField ID="HiddenField_DayTime" runat="server" Value='<%#Eval("DayTimeString") %>' />
                                                <%#Eval("DayTimeString") %></th>
                                        </tr>
                                        <tr>
                                            <td>首席</td>
                                            <td>
                                                <asp:Label ID="Label_ShouXi" runat="server" Text=""></asp:Label>

                                            </td>
                                        </tr>
                                        <tr>
                                            <td>领班</td>
                                            <td>
                                                <asp:Label ID="Label_LingBan" runat="server" Text=""></asp:Label>

                                            </td>
                                        </tr>
                                        <tr>
                                            <td>值班</td>
                                            <td>
                                                <asp:Label ID="Label_ZhiBan" runat="server" Text=""></asp:Label>

                                            </td>
                                        </tr>
                                        <tr style="color: red;">
                                            <td>连线</td>
                                            <td>
                                                <asp:Label ID="Label_LianXian" runat="server" Text=""></asp:Label>

                                            </td>
                                        </tr>
                                    </table>




                                </div>
                            </ItemTemplate>
                        </asp:Repeater>
                    </div>
                </td>
                <td  style="vertical-align: top;background-color: #5BADFF; border: 2px solid #5BADFF;max-width:206px;  min-width:206px;" >
                   
                    <table class="table_today" style=" color: white;" >
                        <tr>
                            <td colspan="2" style="font-size:30px;">
                                今日值班
                                
                            </td>
                        </tr>
                        <tr>
                            <td colspan="2">
                                <div style="margin:0 auto;width:75px;height:75px; text-align: center;font-size:50px; background-color: #FFBB00;line-height:75px; border-radius:10px;box-shadow:3px 3px 5px #888888;">
                                    <asp:Label ID="Label_Today_day" runat="server" Text=""></asp:Label>
                                </div>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="2">
                                <asp:Label ID="Label_Today_Date" runat="server" Text=""></asp:Label>
                                
                            </td>
                        </tr>
                        <tr>
                            <td colspan="2">
                                <asp:Label ID="Label_Today_Week" runat="server" Text=""></asp:Label>
                                
                            </td>
                        </tr>
             
                        <tr>
                        
                            <td style="text-align:center">
                                <asp:Label ID="Label_ShouXi_Today" CssClass="lable_user" runat="server" Text=""></asp:Label>
                            </td>
                            <td  style="text-align:left; color: #eee;">
                                （首席）
                            </td>
                        </tr>
                        <tr>
                          
                            <td style="text-align:center">
                                <asp:Label ID="Label_LingBan_Today" CssClass="lable_user" runat="server" Text=""></asp:Label>
                            </td>
                            <td  style="text-align: left; color: #eee;">
                                （领班）
                            </td>
                        </tr>
                        <tr>
                          
                            <td style="text-align:center">
                                <asp:Label ID="Label_ZhiBan_Today" CssClass="lable_user" runat="server" Text=""></asp:Label>
                            </td>
                            <td  style="text-align:left; color: #eee;">
                                （值班）
                            </td>
                        </tr>
                        <tr>
                          
                            <td style="text-align: center; ">
                                <asp:Label ID="Label_LianXian_Today" CssClass="lable_user" runat="server" Text=""></asp:Label>
                            </td>
                            <td  style="text-align:left; ">
                                （连线）
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
            <tr>

                <td  colspan="2">
                    <div style="width: 100%; border: 1px dashed #ff6a00;padding:15px 0;margin-top:15px; ">
                        <label style="color: #ff6a00; font-size: 18px; margin: 15px;">排班说明</label><br />
                        <asp:Label ID="Label_Description" runat="server" Font-Size="10pt" ForeColor="#666666" CssClass="Label_Description"></asp:Label>

                    </div>


                </td>

            </tr>
        </table>





    </form>
</body>
</html>
