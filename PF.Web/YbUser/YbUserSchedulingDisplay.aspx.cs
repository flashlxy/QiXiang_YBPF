using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using PF.BLL.SQL;
using PF.Models.SQL;
using PF.ViewModels;

namespace PF.Web.YbUser
{
    public partial class YbUserSchedulingDisplay : System.Web.UI.Page
    {
        private List<PF.Models.SQL.Scheduling> sclist = new List<Models.SQL.Scheduling>();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                InitDateTime();
                InitDay();
                GetDescription();
                InitTodayScheduling();
            }
        }


        public void InitDateTime()
        {
            int startYear = 2016;

            int countYear = DateTime.Now.Year - startYear;

            for (int i = 0; i <= countYear; i++)
            {
                int cYear = startYear + i;
                ListItem li = new ListItem() { Text = cYear.ToString() + "年", Value = cYear.ToString() };
                DropDownList_Year.Items.Add(li);
            }

            foreach (ListItem item in DropDownList_Year.Items)
            {
                if (item.Value == DateTime.Now.ToString("yyyy"))
                {
                    item.Selected = true;
                }
                else
                {
                    item.Selected = false;
                }
            }
            foreach (ListItem item in DropDownList_Month.Items)
            {
                if (item.Value == DateTime.Now.ToString("MM"))
                {
                    item.Selected = true;
                }
                else
                {
                    item.Selected = false;
                }
            }
        }


        public void InitDay()
        {



            DateTime firstDay = DateTime.Parse(DropDownList_Year.SelectedItem.Value + "-" + DropDownList_Month.SelectedItem.Value + "-01");
            DateTime endDay = firstDay.AddMonths(1);
            TimeSpan ts = endDay - firstDay;

            PF.BLL.SQL.Scheduling_BLL scbll = new Scheduling_BLL();
            sclist = scbll.GetList(a => a.Date >= firstDay && a.Date < endDay).ToList();


            List<Day_Scheduling_ViewModel> list = new List<Day_Scheduling_ViewModel>();
            for (int i = 0; i < ts.Days; i++)
            {
                // DateTime dt = firstDay.AddDays(i);




                Day_Scheduling_ViewModel ds = new Day_Scheduling_ViewModel()
                {
                    DayTime = firstDay.AddDays(i),
                    DayTimeString = firstDay.AddDays(i).ToString("yyyy-MM-dd"),
                    //Week = firstDay.AddDays(i).DayOfWeek.ToString("d"),
                    Week = GetWeekNumber(firstDay.AddDays(i).DayOfWeek.ToString()),
                    IsCurrentMonth = true

                };
                list.Add(ds);
            }

            Day_Scheduling_ViewModel firstDayScheduling = list.FirstOrDefault();

            int needDay = firstDayScheduling.Week - 1;
            for (int i = 1; i <= needDay; i++)
            {
                Day_Scheduling_ViewModel ds = new Day_Scheduling_ViewModel()
                {
                    DayTime = firstDayScheduling.DayTime.AddDays(-i),
                    DayTimeString = firstDayScheduling.DayTime.AddDays(-i).ToString("yyyy-MM-dd"),
                    //Week = firstDay.AddDays(i).DayOfWeek.ToString("d"),
                    Week = GetWeekNumber(firstDayScheduling.DayTime.AddDays(-i).DayOfWeek.ToString()),
                    IsCurrentMonth = false

                };
                list.Add(ds);
            }


            list = list.OrderBy(a => a.DayTime).ToList();


            RepeaterScheduling.DataSource = list;
            RepeaterScheduling.DataBind();
        }

        protected void RepeaterScheduling_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            YbUsers_BLL ubll = new YbUsers_BLL();
            List<PF.Models.SQL.YbUsers> users = ubll.GetList().ToList();
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                //找到分类Repeater关联的数据项 
                Day_Scheduling_ViewModel dsv = (Day_Scheduling_ViewModel)e.Item.DataItem;


                Label Label_ShouXi = (Label)e.Item.FindControl("Label_ShouXi");
                Label Label_LingBan = (Label)e.Item.FindControl("Label_LingBan");
                Label Label_ZhiBan = (Label)e.Item.FindControl("Label_ZhiBan");

              







                HiddenField hid = (HiddenField)e.Item.FindControl("HiddenField_DayTime");
                DateTime dt = DateTime.Parse(hid.Value);



                PF.Models.SQL.Scheduling shouxi = sclist.Where(a => a.Date == dt && a.Work == "首席").FirstOrDefault();

                if (shouxi != null)
                {
                    Label_ShouXi.Text = shouxi.YBUserName;
                }
                else
                {
                    Label_ShouXi.Text = string.Empty;
                }



                
                PF.Models.SQL.Scheduling lingban = sclist.Where(a => a.Date == dt && a.Work == "领班").FirstOrDefault();
                if (lingban != null)
                {
                    Label_LingBan.Text = lingban.YBUserName;
                }
                else
                {
                    Label_LingBan.Text = string.Empty;
                }



               
                PF.Models.SQL.Scheduling zhiban = sclist.Where(a => a.Date == dt && a.Work == "值班").FirstOrDefault();

                if (zhiban != null)
                {
                    Label_ZhiBan.Text = zhiban.YBUserName;


                }
                else
                {
                    Label_LingBan.Text = string.Empty;

                }



            }
        }

       
        protected void Button_Query_Click(object sender, EventArgs e)
        {
            InitDay();
            GetDescription();

        }
        public int GetWeekNumber(string weekName)
        {
            int week = 1;
            switch (weekName)
            {
                case "Sunday":
                    week = 7;
                    break;
                case "Monday":
                    week = 1;
                    break;
                case "Tuesday":
                    week = 2;
                    break;
                case "Wednesday":
                    week = 3;
                    break;
                case "Thursday":
                    week = 4;
                    break;
                case "Friday":
                    week = 5;
                    break;
                case "Saturday":
                    week = 6;
                    break;
            }
            return week;

        }


        public void GetDescription()
        {
            int year = int.Parse(DropDownList_Year.SelectedItem.Value);
            int month = int.Parse(DropDownList_Month.SelectedItem.Value);
            Scheduling_Description_BLL bll = new Scheduling_Description_BLL();
            Scheduling_Description model = bll.Get(a => a.Year == year && a.Month == month);
            if (model != null)
            {
                Label_Description.Text = model.Description;
            }
            else
            {
                Label_Description.Text = "";
            }
        }

        public void InitTodayScheduling()
        {
            Scheduling_BLL bll = new Scheduling_BLL();
            DateTime today = DateTime.Parse(DateTime.Now.ToShortDateString());



         string xingqi=   GetWeekNumberZh(today.DayOfWeek.ToString());
            Label_Today_Date.Text = today.ToString("yyyy年MM月dd日") ;
            Label_Today_Week.Text = xingqi;
            Label_Today_day.Text = today.Day.ToString();

            List<PF.Models.SQL.Scheduling> list = bll.GetList(a => a.Date == today).ToList();


            PF.Models.SQL.Scheduling shouxi= list.Where(a => a.Work == "首席").FirstOrDefault();
            if (shouxi != null)
            {
                Label_ShouXi_Today.Text = shouxi.YBUserName;
            }
            else
            {
                Label_ShouXi_Today.Text = string.Empty;
            }
            PF.Models.SQL.Scheduling lingban= list.Where(a => a.Work == "领班").FirstOrDefault();
            if (lingban != null)
            {
                Label_LingBan_Today.Text = lingban.YBUserName;
            }
            else
            {
                Label_LingBan_Today.Text = string.Empty;
            }
            PF.Models.SQL.Scheduling zhiban= list.Where(a => a.Work == "值班").FirstOrDefault();
            if (zhiban != null)
            {
                Label_ZhiBan_Today.Text = zhiban.YBUserName;
            }
            else
            {
                Label_ZhiBan_Today.Text = string.Empty;
            }




        }

        public string GetWeekNumberZh(string weekName)
        {
            string week=string.Empty;
            switch (weekName)
            {
                case "Sunday":
                    week = "星期日";
                    break;
                case "Monday":
                    week = "星期一";
                    break;
                case "Tuesday":
                    week = "星期二";
                    break;
                case "Wednesday":
                    week = "星期三";
                    break;
                case "Thursday":
                    week = "星期四";
                    break;
                case "Friday":
                    week = "星期五";
                    break;
                case "Saturday":
                    week = "星期五";
                    break;
                    
            }
            return week;
        }
    }
  


}