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
    public partial class YbUserScheduling : System.Web.UI.Page
    {
        private List<PF.Models.SQL.Scheduling> sclist = new List<Models.SQL.Scheduling>();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                InitDateTime();
                InitDay();
                GetDescription();
            }
        }


        public void InitDateTime()
        {
            int startYear = 2016;

            int countYear = DateTime.Now.Year - startYear;

            for (int i = 0; i <= countYear; i++)
            {
                int cYear = startYear + i;
                ListItem li = new ListItem() { Text= cYear.ToString()+"年",Value=cYear.ToString()};
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



            DateTime firstDay = DateTime.Parse(DropDownList_Year.SelectedItem.Value+"-"+ DropDownList_Month.SelectedItem.Value + "-01");
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
                    Week= GetWeekNumber(firstDay.AddDays(i).DayOfWeek.ToString()),
                    IsCurrentMonth=true

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
            List<PF.Models.SQL.YbUsers> users = ubll.GetList(a=>a.Work=="预报").OrderBy(a=>a.Order).ToList();
            List<PF.Models.SQL.YbUsers> users_LianXian = ubll.GetList(a=>a.Work=="连线").OrderBy(a=>a.Order).ToList();
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                //找到分类Repeater关联的数据项 
                Day_Scheduling_ViewModel dsv = (Day_Scheduling_ViewModel)e.Item.DataItem;

                DropDownList DropDownList_ShouXi = (DropDownList)e.Item.FindControl("DropDownList_ShouXi");
                DropDownList DropDownList_LingBan = (DropDownList)e.Item.FindControl("DropDownList_LingBan");
                DropDownList DropDownList_ZhiBan = (DropDownList)e.Item.FindControl("DropDownList_ZhiBan");
                DropDownList DropDownList_LianXian = (DropDownList)e.Item.FindControl("DropDownList_LianXian");


                ListItem lino1 = new ListItem() { Text = "   ", Value = "未选择" };
                ListItem lino2 = new ListItem() { Text = "   ", Value = "未选择" };
                ListItem lino3 = new ListItem() { Text = "   ", Value = "未选择" };
                ListItem lino4 = new ListItem() { Text = "   ", Value = "未选择" };
                DropDownList_ShouXi.Items.Add(lino1);
                DropDownList_LingBan.Items.Add(lino2);
                DropDownList_ZhiBan.Items.Add(lino3);
                DropDownList_LianXian.Items.Add(lino4);


                foreach (var ybUser in users)
                {
                    ListItem li1 = new ListItem() { Text = ybUser.YBUserName, Value = ybUser.YBUserID.ToString() };
                    ListItem li2 = new ListItem() { Text = ybUser.YBUserName, Value = ybUser.YBUserID.ToString() };
                    ListItem li3 = new ListItem() { Text = ybUser.YBUserName, Value = ybUser.YBUserID.ToString() };
                    DropDownList_ShouXi.Items.Add(li1);
                    DropDownList_LingBan.Items.Add(li2);
                    DropDownList_ZhiBan.Items.Add(li3);
                }
                foreach (var ybUser in users_LianXian)
                {
                  
                    ListItem li4 = new ListItem() { Text = ybUser.YBUserName, Value = ybUser.YBUserID.ToString() };
                   
                    DropDownList_LianXian.Items.Add(li4);
                }






                HiddenField hid = (HiddenField)e.Item.FindControl("HiddenField_DayTime");
                DateTime dt = DateTime.Parse(hid.Value);



                PF.Models.SQL.Scheduling shouxi = sclist.Where(a => a.Date == dt&&a.Work=="首席").FirstOrDefault();

                foreach (ListItem lis in DropDownList_ShouXi.Items)
                {
                    if (shouxi != null)
                    {
                        if(lis.Text == shouxi.YBUserName)
                        {
                            lis.Selected = true;
                        }
                        else
                        {
                            lis.Selected = false;
                        }
                    }
                    else
                    {
                        if (lis.Value == "未选择")
                        {
                            lis.Selected = true;
                        }
                        else
                        {
                            lis.Selected = false;
                        }
                    }
                }
                PF.Models.SQL.Scheduling lingban = sclist.Where(a => a.Date == dt && a.Work == "领班").FirstOrDefault();

                foreach (ListItem li in DropDownList_LingBan.Items)
                {
                    if (lingban != null)
                    {
                        if (li.Text == lingban.YBUserName)
                        {
                            li.Selected = true;
                        }
                        else
                        {
                            li.Selected = false;
                        }
                    }
                    else
                    {
                        if (li.Value == "未选择")
                        {
                            li.Selected = true;
                        }
                        else
                        {
                            li.Selected = false;
                        }
                    }
                }
                PF.Models.SQL.Scheduling zhiban = sclist.Where(a => a.Date == dt && a.Work == "值班").FirstOrDefault();

                foreach (ListItem li in DropDownList_ZhiBan.Items)
                {
                    if (zhiban != null)
                    {
                        
                        if (li.Text == zhiban.YBUserName)
                        {
                            li.Selected = true;
                        }
                        else
                        {
                            li.Selected = false;
                        }
                    }
                    else
                    {
                        if (li.Value == "未选择")
                        {
                            li.Selected = true;
                        }
                        else
                        {
                            li.Selected = false;
                        }
                    }
                }


                PF.Models.SQL.Scheduling lianxian = sclist.Where(a => a.Date == dt && a.Work == "连线").FirstOrDefault();

                foreach (ListItem li in DropDownList_LianXian.Items)
                {
                    if (lianxian != null)
                    {

                        if (li.Text == lianxian.YBUserName)
                        {
                            li.Selected = true;
                        }
                        else
                        {
                            li.Selected = false;
                        }
                    }
                    else
                    {
                        if (li.Value == "未选择")
                        {
                            li.Selected = true;
                        }
                        else
                        {
                            li.Selected = false;
                        }
                    }
                }



            }
        }

        protected void DropDownList_ShouXi_SelectedIndexChanged(object sender, EventArgs e)
        {
            DropDownList drd = sender as DropDownList;
            Repeater rps = drd.Parent.Parent as Repeater;
            int n = ((RepeaterItem)drd.Parent).ItemIndex;
            HiddenField hid = (HiddenField)(rps.Items[n].FindControl("HiddenField_DayTime"));
            string daystring = hid.Value;
            DateTime dt = DateTime.Parse(daystring);

            //已经拿到了id，可以进行操作   
            DropDownList ddl = (DropDownList)(rps.Items[n].FindControl("DropDownList_ShouXi"));





            PF.BLL.SQL.Scheduling_BLL scbll = new Scheduling_BLL();


            if (ddl.SelectedItem.Value == "未选择")
            {
                scbll.Delete(a => a.Date == dt && a.Work == "首席");
            }
            else
            {



                PF.Models.SQL.Scheduling sc = scbll.Get(a => a.Date == dt && a.Work == "首席");
                if (sc == null)
                {
                    sc = new Models.SQL.Scheduling()
                    {
                        SchedulingID = Guid.NewGuid(),
                        CreateTime = DateTime.Now,
                        Date = dt,
                        Work = "首席",
                        YBUserID = Guid.Parse(ddl.SelectedItem.Value),
                        YBUserName = ddl.SelectedItem.Text,

                    };
                    scbll.Add(sc);
                }
                else
                {
                    sc.YBUserID = Guid.Parse(ddl.SelectedItem.Value);
                    sc.YBUserName = ddl.SelectedItem.Text;
                    sc.CreateTime = DateTime.Now;
                    scbll.Update(sc);
                }
            }

        }

        protected void DropDownList_LingBan_SelectedIndexChanged(object sender, EventArgs e)
        {
            DropDownList drd = sender as DropDownList;
            Repeater rps = drd.Parent.Parent as Repeater;
            int n = ((RepeaterItem)drd.Parent).ItemIndex;
            HiddenField hid = (HiddenField)(rps.Items[n].FindControl("HiddenField_DayTime"));
            string daystring = hid.Value;
            DateTime dt = DateTime.Parse(daystring);
            DropDownList ddl = (DropDownList)(rps.Items[n].FindControl("DropDownList_LingBan"));


            PF.BLL.SQL.Scheduling_BLL scbll = new Scheduling_BLL();
            if (ddl.SelectedItem.Value == "未选择")
            {
                scbll.Delete(a => a.Date == dt && a.Work == "领班");
            }
            else
            {

                PF.Models.SQL.Scheduling sc = scbll.Get(a => a.Date == dt && a.Work == "领班");
                if (sc == null)
                {
                    sc = new Models.SQL.Scheduling()
                    {
                        SchedulingID = Guid.NewGuid(),
                        CreateTime = DateTime.Now,
                        Date = dt,
                        Work = "领班",
                        YBUserID = Guid.Parse(ddl.SelectedItem.Value),
                        YBUserName = ddl.SelectedItem.Text,

                    };
                    scbll.Add(sc);
                }
                else
                {
                    sc.YBUserID = Guid.Parse(ddl.SelectedItem.Value);
                    sc.YBUserName = ddl.SelectedItem.Text;
                    sc.CreateTime = DateTime.Now;
                    scbll.Update(sc);
                }
            }
        }

        protected void DropDownList_ZhiBan_SelectedIndexChanged(object sender, EventArgs e)
        {
            DropDownList drd = sender as DropDownList;
            Repeater rps = drd.Parent.Parent as Repeater;
            int n = ((RepeaterItem)drd.Parent).ItemIndex;
            HiddenField hid = (HiddenField)(rps.Items[n].FindControl("HiddenField_DayTime"));
            string daystring = hid.Value;
            DateTime dt = DateTime.Parse(daystring);
            DropDownList ddl = (DropDownList)(rps.Items[n].FindControl("DropDownList_ZhiBan"));


            PF.BLL.SQL.Scheduling_BLL scbll = new Scheduling_BLL();
            if (ddl.SelectedItem.Value == "未选择")
            {
                scbll.Delete(a => a.Date == dt && a.Work == "值班");
            }
            else
            {
                PF.Models.SQL.Scheduling sc = scbll.Get(a => a.Date == dt && a.Work == "值班");
                if (sc == null)
                {
                    sc = new Models.SQL.Scheduling()
                    {
                        SchedulingID = Guid.NewGuid(),
                        CreateTime = DateTime.Now,
                        Date = dt,
                        Work = "值班",
                        YBUserID = Guid.Parse(ddl.SelectedItem.Value),
                        YBUserName = ddl.SelectedItem.Text,

                    };
                    scbll.Add(sc);
                }
                else
                {
                    sc.YBUserID = Guid.Parse(ddl.SelectedItem.Value);
                    sc.YBUserName = ddl.SelectedItem.Text;
                    sc.CreateTime = DateTime.Now;
                    scbll.Update(sc);
                }
            }
        }
        protected void DropDownList_LianXian_SelectedIndexChanged(object sender, EventArgs e)
        {
            DropDownList drd = sender as DropDownList;
            Repeater rps = drd.Parent.Parent as Repeater;
            int n = ((RepeaterItem)drd.Parent).ItemIndex;
            HiddenField hid = (HiddenField)(rps.Items[n].FindControl("HiddenField_DayTime"));
            string daystring = hid.Value;
            DateTime dt = DateTime.Parse(daystring);
            DropDownList ddl = (DropDownList)(rps.Items[n].FindControl("DropDownList_LianXian"));


            PF.BLL.SQL.Scheduling_BLL scbll = new Scheduling_BLL();
            if (ddl.SelectedItem.Value == "未选择")
            {
                scbll.Delete(a => a.Date == dt && a.Work == "连线");
            }
            else
            {
                PF.Models.SQL.Scheduling sc = scbll.Get(a => a.Date == dt && a.Work == "连线");
                if (sc == null)
                {
                    sc = new Models.SQL.Scheduling()
                    {
                        SchedulingID = Guid.NewGuid(),
                        CreateTime = DateTime.Now,
                        Date = dt,
                        Work = "连线",
                        YBUserID = Guid.Parse(ddl.SelectedItem.Value),
                        YBUserName = ddl.SelectedItem.Text,

                    };
                    scbll.Add(sc);
                }
                else
                {
                    sc.YBUserID = Guid.Parse(ddl.SelectedItem.Value);
                    sc.YBUserName = ddl.SelectedItem.Text;
                    sc.CreateTime = DateTime.Now;
                    scbll.Update(sc);
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
            int week=1;
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
                    week =3;
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
            int year =int.Parse( DropDownList_Year.SelectedItem.Value);
            int month = int.Parse(DropDownList_Month.SelectedItem.Value);
            Scheduling_Description_BLL bll = new Scheduling_Description_BLL();
            Scheduling_Description model= bll.Get(a => a.Year == year && a.Month == month);
            if (model != null)
            {
                TextBox_Description.Text = model.Description;
            }
            else
            {
                TextBox_Description.Text = "";
            }
        }

        protected void Button_SaveDescription_Click(object sender, EventArgs e)
        {
            int year = int.Parse(DropDownList_Year.SelectedItem.Value);
            int month = int.Parse(DropDownList_Month.SelectedItem.Value);
            Scheduling_Description_BLL bll = new Scheduling_Description_BLL();
            Scheduling_Description model = bll.Get(a => a.Year == year && a.Month == month);
            if (model == null)
            {
                model = new Scheduling_Description()
                {
                    SchDesID = Guid.NewGuid(),
                    Year = year,
                    Month = month,
                    Description = TextBox_Description.Text.Trim(),
                    CreateTime=DateTime.Now,
                    UpdateTime=DateTime.Now

                };
                bll.Add(model);
            }
            else
            {
                model.Description = TextBox_Description.Text.Trim();
                model.UpdateTime = DateTime.Now;

                bll.Update(model);
            }
            Response.Write("<script language=javascript defer>alert('保存成功！');</script>");
        }
        //public string GetWeekNumber(string weekName)
        //{
        //    string week;
        //    switch (weekName)
        //    {
        //        case "Sunday":
        //            week = "星期日";
        //            break;
        //        case "Monday":
        //            week = "星期一";
        //            break;
        //        case "Tuesday":
        //            week = "星期二";
        //            break;
        //        case "Wednesday":
        //            week = "星期三";
        //            break;
        //        case "Thursday":
        //            week = "星期四";
        //            break;
        //        case "Friday":
        //            week = "星期五";
        //            break;
        //        case "Saturday":
        //            week = "星期五";
        //            break;
        //            return week;
        //    }
        //}
    }
   


}