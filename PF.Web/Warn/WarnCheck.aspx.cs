using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using PF.Models.SQL;
using PF.BLL.SQL;
using Aspose.Words;
using System.Drawing;
using Aspose.Words.Tables;
using PF.Models.Oracle;
using PF.ViewModels;

namespace PF.Web.Warn
{
    public partial class WarnCheck : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void Button_Query_Click(object sender, EventArgs e)
        {


            BindData();
        }

        public void BindData()
        {
            WarnCheck_BLL bll = new WarnCheck_BLL();
            DateTime startTime = DateTime.Parse(DropDownList_Year.SelectedItem.Value + "-" + DropDownList_Month.SelectedItem.Value + "-01");
            DateTime endTime = startTime.AddMonths(1);


            List<PF.Models.SQL.WarnCheck> list = bll.GetList(a => a.ReleaseTime >= startTime && a.ReleaseTime < endTime).ToList();
            if (DropDownList_WarnCategory.SelectedItem.Value != "全部")
            {
                list = list.Where(a => a.WarningCategory == DropDownList_WarnCategory.SelectedItem.Value).ToList();
            }
            if (DropDownList_WarnLevel.SelectedItem.Value != "全部")
            {
                list = list.Where(a => a.WarningLevel == DropDownList_WarnLevel.SelectedItem.Value).ToList();
            }
            GridView_List.DataSource = list.OrderBy(a => a.ReleaseTime);
            GridView_List.DataBind();
        }

        protected void Button_Caculate_Click(object sender, EventArgs e)
        {
            WarnCheck_BLL bll = new WarnCheck_BLL();
            DateTime startTime = DateTime.Parse(DropDownList_Year.SelectedItem.Value + "-" + DropDownList_Month.SelectedItem.Value + "-01");
            DateTime endTime = startTime.AddMonths(1);


            List<PF.Models.SQL.WarnCheck> list = bll.GetList(a => a.ReleaseTime >= startTime && a.ReleaseTime < endTime).ToList();
            if (DropDownList_WarnCategory.SelectedItem.Value != "全部")
            {
                list = list.Where(a => a.WarningCategory == DropDownList_WarnCategory.SelectedItem.Value).ToList();
            }
            if (DropDownList_WarnLevel.SelectedItem.Value != "全部")
            {
                list = list.Where(a => a.WarningLevel == DropDownList_WarnLevel.SelectedItem.Value).ToList();
            }
            foreach (Models.SQL.WarnCheck warn in list)
            {
                if (warn.WarningCategory == "大风")
                {
                    WarnCheck_ReachStation_ViewModel vm= bll.Caculate_Wind(warn);
                    if (vm != null)
                    {
                        warn.ReachTime = vm.DateTime;
                        TimeSpan ts = vm.DateTime - (DateTime) warn.ReleaseTime;
                        warn.ReachSpendMinute = (int) Math.Round(ts.TotalMinutes, 0);
                        warn.Accuracy = "正确";
                        warn.ReachStation = vm.StationName;
                        warn.ReachValue = vm.Value;

                    }
                    else
                    {
                        warn.ReachSpendMinute = null;
                        
                        warn.ReachStation =null;
                        warn.ReachValue = null;
                        warn.Accuracy = "错误";
                    }
                    bll.Update(warn);
                }
                 
            }
            BindData();


        }
    }
}