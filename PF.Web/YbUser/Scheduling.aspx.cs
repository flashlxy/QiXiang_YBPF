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
            if (!IsPostBack)
            {
                InitUser();
            }
        }

        protected void Button_Query_Click(object sender, EventArgs e)
        {
            Query();


        }

        public void Query()
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
                    Date = startTime.AddDays(i),
                    Next_Date = startTime.AddDays(i + 1),
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

       


        public void InitUser()
        {
            List<YbUsers> users = new YbUsers_BLL().GetList(a=>a.YBUserName!="集体").ToList();
            foreach (YbUsers user in users)
            {
                ListItem li = new ListItem()
                {
                    Text=user.YBUserName,
                    Value=user.YBUserID.ToString()

                    
                };

                DropDownList_YbUser.Items.Add(li);
            }
        }

        protected void Button_AddUser_Click(object sender, EventArgs e)
        {
            DateTime currentDay = ASPxDateEdit_YBDate.Date;
            Score_Day_BLL scBll = new Score_Day_BLL();
            Score_Day groupScoreDay =scBll.Get(a => a.YBTime == DropDownList_Time_AddUser.SelectedItem.Value && a.YBDate == currentDay && a.YBUserName == "集体");
            Score_Day exceptScoreDay = new Score_Day()
            {
                ScoreID = Guid.NewGuid(),
                AllTotal = groupScoreDay.AllTotal,
                CreateTime = DateTime.Now,
                MaxTemp24 = groupScoreDay.MaxTemp24,
                MaxTemp48 = groupScoreDay.MaxTemp48,
                MaxTemp72 = groupScoreDay.MaxTemp72,
                MinTemp24 = groupScoreDay.MinTemp24,
                MinTemp48 = groupScoreDay.MinTemp48,
                MinTemp72 = groupScoreDay.MinTemp72,
                Rainfall24 = groupScoreDay.Rainfall24,
                Rainfall24Total = groupScoreDay.Rainfall24Total,
                Rainfall48 = groupScoreDay.Rainfall48,
                Rainfall48Total = groupScoreDay.Rainfall48Total,
                Rainfall72 = groupScoreDay.Rainfall72,
                Rainfall72Total = groupScoreDay.Rainfall72Total,
                RainShine24 = groupScoreDay.RainShine24,
                RainShine48 = groupScoreDay.RainShine48,
                RainShine72 = groupScoreDay.RainShine72,
                Rainstorm24 = groupScoreDay.Rainstorm24,
                Rainstorm24Total = groupScoreDay.Rainstorm24Total,
                Rainstorm48 = groupScoreDay.Rainstorm48,
                Rainstorm48Total = groupScoreDay.Rainstorm48Total,
                YBDate = groupScoreDay.YBDate,
                YBTime = groupScoreDay.YBTime,
                YBUserID = Guid.Parse(DropDownList_YbUser.SelectedItem.Value),
                YBUserName = DropDownList_YbUser.SelectedItem.Text,
                Remark = "来自集体"
            };

            scBll.Add(exceptScoreDay);
            Query();

            Response.Write("<script language=javascript defer>alert('添加成功！');</script>");

        }

        protected void Repeater_YbUser_ItemCreated(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem || e.Item.ItemType == ListItemType.SelectedItem)
            {
              



                Repeater Repeater_YbUser_Morning = (Repeater)e.Item.FindControl("Repeater_YbUser_Morning");
                Repeater_YbUser_Morning.ItemCommand += new RepeaterCommandEventHandler(rpt_moring_ItemCommand);

                Repeater Repeater_YbUser_Night = (Repeater)e.Item.FindControl("Repeater_YbUser_Night");
                Repeater_YbUser_Night.ItemCommand += new RepeaterCommandEventHandler(rpt_night_ItemCommand);

            }
           
        }
        void rpt_moring_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            if (e.CommandName == "DeleteUser")
            {

                string sid = e.CommandArgument.ToString().Split(',')[0].ToString();
                Guid scoid = Guid.Parse(sid);
                Score_Day_BLL bll = new Score_Day_BLL();
                bll.Delete(a => a.ScoreID == scoid);

                Query();

                Response.Write("<script>alert('删除成功！')</script>");
            }
        }
        void rpt_night_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            if (e.CommandName == "DeleteUser")
            {

                string sid = e.CommandArgument.ToString().Split(',')[0].ToString();
                Guid scoid = Guid.Parse(sid);
                Score_Day_BLL bll = new Score_Day_BLL();
                bll.Delete(a => a.ScoreID == scoid);

                Query();

                Response.Write("<script>alert('删除成功！')</script>");
            }
        }

    }
}