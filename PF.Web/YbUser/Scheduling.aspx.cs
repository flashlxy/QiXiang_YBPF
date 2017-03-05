using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using PF.BLL.SQL;
using PF.Models.SQL;
using PF.ViewModels;

namespace PF.Web.YbUser
{
    public partial class Scheduling : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void Button_Query_Click(object sender, EventArgs e)
        {
            DateTime startTime = DateTime.Parse(DropDownList_Year.SelectedItem.Value + "-" + DropDownList_Month.SelectedItem.Value + "-01");
            DateTime endTime = startTime.AddMonths(1).AddDays(-1);
            //YbUsers_BLL bll = new YbUsers_BLL();
            Score_Day_BLL bll = new Score_Day_BLL();
            //List<Score_Day> list = bll.GetList(a => a.YBTime == DropDownList_YBTime.SelectedItem.Value && a.YBDate >= startTime && a.YBDate < endTime).OrderBy(a=>a.YBDate).ToList();


            //var 

          
            List<YBUsers_Date_ViewModel> list = new List<YBUsers_Date_ViewModel>();
            TimeSpan ts = endTime - startTime;
            for (int i = 0; i <= ts.Days; i++)
            {
                YBUsers_Date_ViewModel vm = new YBUsers_Date_ViewModel()
                {
                    Date= startTime.AddDays(i),
                    Next_Date= startTime.AddDays(i+1),
                };
                list.Add(vm);
            }

            Repeater_YbUser.DataSource = list;
            Repeater_YbUser.DataBind();


        }

        protected void Repeater_YbUser_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            Score_Day_BLL bll = new Score_Day_BLL();
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                Repeater Repeater_YbUser_Morning = (Repeater)e.Item.FindControl("Repeater_YbUser_Morning");
                //找到分类Repeater关联的数据项 
                YBUsers_Date_ViewModel drv = (YBUsers_Date_ViewModel)e.Item.DataItem;
                //提取分类ID 
                DateTime date = (DateTime)drv.Date;
                DateTime next_date = (DateTime)drv.Next_Date;
                //根据分类ID查询该分类下的产品，并绑定产品Repeater 
                List<Score_Day> mlist = bll.GetList(a => a.YBTime == "08时" && a.YBDate == next_date).OrderBy(a => a.YBUserName).OrderBy(a=>a.YbUsers.Order).ToList();


                Repeater_YbUser_Morning.DataSource = mlist;
                Repeater_YbUser_Morning.DataBind();

                Repeater Repeater_YbUser_Night = (Repeater)e.Item.FindControl("Repeater_YbUser_Night");
               
             
                //根据分类ID查询该分类下的产品，并绑定产品Repeater 
                List<Score_Day> nlist = bll.GetList(a => a.YBTime == "20时" && a.YBDate == date).OrderBy(a => a.YBUserName).OrderBy(a => a.YbUsers.Order).ToList();


                Repeater_YbUser_Night.DataSource = nlist;
                Repeater_YbUser_Night.DataBind();
            }
        }
    }
}