using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using PF.BLL.SQL;

namespace PF.Web.YbUser
{
    public partial class YbUserScheduling : System.Web.UI.Page
    {
        private List<PF.Models.SQL.Scheduling> sclist = new List<Models.SQL.Scheduling>();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                InitDay();
            }
        }


        public void InitDay()
        {



            DateTime firstDay = DateTime.Parse(DropDownList_Year.SelectedItem.Value+"-"+ DropDownList_Month.SelectedItem.Value + "-01");
            DateTime endDay = firstDay.AddMonths(1);
            TimeSpan ts = endDay - firstDay;

            PF.BLL.SQL.Scheduling_BLL scbll = new Scheduling_BLL();
            sclist = scbll.GetList(a => a.Date >= firstDay && a.Date < endDay).ToList();


            List<DayScheduling> list = new List<DayScheduling>();
            for (int i = 0; i < ts.Days; i++)
            {
                // DateTime dt = firstDay.AddDays(i);




                DayScheduling ds = new DayScheduling()
                {
                    DayTime = firstDay.AddDays(i),
                    DayTimeString = firstDay.AddDays(i).ToShortDateString()
                };
                list.Add(ds);
            }

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
                DayScheduling dsv = (DayScheduling)e.Item.DataItem;

                DropDownList DropDownList_ShouXi = (DropDownList)e.Item.FindControl("DropDownList_ShouXi");
                DropDownList DropDownList_LingBan = (DropDownList)e.Item.FindControl("DropDownList_LingBan");
                DropDownList DropDownList_ZhiBan = (DropDownList)e.Item.FindControl("DropDownList_ZhiBan");


                ListItem lino1 = new ListItem() { Text = "   ", Value = "未选择" };
                ListItem lino2 = new ListItem() { Text = "   ", Value = "未选择" };
                ListItem lino3 = new ListItem() { Text = "   ", Value = "未选择" };
                DropDownList_ShouXi.Items.Add(lino1);
                DropDownList_LingBan.Items.Add(lino2);
                DropDownList_ZhiBan.Items.Add(lino3);


                foreach (var ybUser in users)
                {
                    ListItem li1 = new ListItem() { Text = ybUser.YBUserName, Value = ybUser.YBUserID.ToString() };
                    ListItem li2 = new ListItem() { Text = ybUser.YBUserName, Value = ybUser.YBUserID.ToString() };
                    ListItem li3 = new ListItem() { Text = ybUser.YBUserName, Value = ybUser.YBUserID.ToString() };
                    DropDownList_ShouXi.Items.Add(li1);
                    DropDownList_LingBan.Items.Add(li2);
                    DropDownList_ZhiBan.Items.Add(li3);
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

        protected void Button_Query_Click(object sender, EventArgs e)
        {
            InitDay();
        }
    }
    public class DayScheduling
    {
        public DateTime DayTime { get; set; }
        public string DayTimeString { get; set; }
        public string User1 { get; set; }
        public string User2 { get; set; }
        public string User3 { get; set; }
    }
}