using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using PF.BLL.SQL;
using PF.Models.SQL;
using PF.Utility;
using PF.ViewModels;

namespace PF.Web.Score
{
    public partial class DataCheck : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                Panel_DataMiss.Visible = false;

            }
        }

        protected void Btn_DataCheck_Click(object sender, EventArgs e)
        {
            DateTime startTime = DateTime.Parse(DropDownList_Year.SelectedItem.Value + "-" + DropDownList_Month.SelectedItem.Value + "-01");
            DateTime endTime = startTime.AddMonths(1);
            LiveData_BLL bll = new LiveData_BLL();
            List<LiveData> list =
                bll.GetList(
                        a =>
                            a.FDate >= startTime & a.FDate < endTime && a.Category == DropDownList_YBTime.SelectedItem.Value)
                    .OrderBy(a => a.FDate).ThenBy(a => a.CountryCode)
                    .ToList();
            //GridView1.DataSource = list;
            //GridView1.DataBind();

            TimeSpan timeSpan = endTime - startTime;

            List<LiveData_Check> ldcList = new List<LiveData_Check>();
            List<string> citys = CityUtility.AllNameList();
            for (int i = 0; i < timeSpan.Days; i++)
            {
                DateTime currentDate = startTime.AddDays(i);

                List<LiveData> daylist = list.Where(a => a.FDate == currentDate).ToList();

                LiveData_Check ldc = new LiveData_Check();
                ldc.Date = currentDate;
                StringBuilder dataMissStr = new StringBuilder();
                foreach (string city in citys)
                {
                    LiveData dayCity = daylist.Where(a => a.CountryName == city).FirstOrDefault();

                    if (dayCity != null)
                    {
                        if (dayCity.MaxTemp == null || dayCity.MinTemp == null || dayCity.Rain == null)
                        {
                            dataMissStr.Append("<li style='color:#ff6a00;'>" + city + " 缺少 ");
                            string missElement = string.Empty;
                            if (dayCity.MaxTemp == null)
                            {
                                missElement += "最高温度 ";
                            }
                            if (dayCity.MinTemp == null)
                            {
                                missElement += "最低温度 ";
                            }
                            if (dayCity.Rain == null)
                            {
                                missElement += "降水 ";
                            }

                            dataMissStr.Append(missElement);
                            dataMissStr.Append("数据<br/></li>");

                        }
                    }
                    else
                    {
                        dataMissStr.Append("<li  style='color:#ff0000;'>" + city + " 缺少 最高温度 最低温度 降水 数据<br/></li>");

                    }


                }
                ldc.DataMiss = dataMissStr.ToString();
                if (!String.IsNullOrWhiteSpace(ldc.DataMiss))
                {
                    ldcList.Add(ldc);

                }
            }
            Repeater_DataCheck.DataSource = ldcList;
            Repeater_DataCheck.DataBind();
            if (ldcList.Count() > 0)
            {
                Label_DataMiss.Text = startTime.ToString("yyyy年MM月") + DropDownList_YBTime.SelectedItem.Value + "实况数据有缺失，请及时补充！";
                Panel_DataMiss.Visible = true;
            }
            else
            {
                Label_DataMiss.Text = startTime.ToString("yyyy年MM月") + DropDownList_YBTime.SelectedItem.Value + "实况数据齐全！";
                Panel_DataMiss.Visible = false;

            }
        }

        protected void Btn_DataCheck_BaoWen_Click(object sender, EventArgs e)
        {


            DateTime startTime = DateTime.Parse(DDL_Year_BaoWen.SelectedItem.Value + "-" + DDL_Month_BaoWen.SelectedItem.Value + "-01").AddDays(-1);
            DateTime endTime = startTime.AddMonths(1);
            //BaoWens_BLL bwBll = new BaoWens_BLL();
            //List<BaoWens> bwList = bwBll.GetList(a => a.YBDateTime >= startTime && a.YBDateTime < endTime &&
            //                                          a.BWType == DDL_Type_BaoWen.SelectedItem.Value).ToList();


            List<YBUsers_Date_ViewModel> list = new List<YBUsers_Date_ViewModel>();
            TimeSpan ts = endTime - startTime;
            for (int i = 0; i < ts.Days; i++)
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
            BwYbs_BLL bll = new BwYbs_BLL();
            Scheduling_BLL sbll = new Scheduling_BLL();
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                Repeater Repeater_YbUser_Morning = (Repeater)e.Item.FindControl("Repeater_YbUser_Morning");
                //找到分类Repeater关联的数据项 
                YBUsers_Date_ViewModel drv = (YBUsers_Date_ViewModel)e.Item.DataItem;
                //提取分类ID 
                DateTime date = (DateTime)drv.Date;
                DateTime next_date = (DateTime)drv.Next_Date;
                //根据分类ID查询该分类下的产品，并绑定产品Repeater 

                DateTime dateNight = date.AddMinutes(990);
                DateTime next_dateMorning = next_date.AddMinutes(405);

                List<BwYbs> mlist = bll.GetList(a => a.YBType == "早间报文" && a.YBDateTime == next_dateMorning).OrderBy(a => a.YBUserName).OrderBy(a => a.YbUsers.Order).ToList();

                //List<YBUsers_ViewModel> userListM = mlist.GroupBy(a => new { a.YBUserName }).Select(a => new YBUsers_ViewModel { YBUserName = a.Key.YBUserName }).ToList();
                List<Scheduling> schListMorning = sbll.GetList(a => a.Date == date && a.Work == "领班").ToList();

                List<BaoWen_Check_ViewModel> bcvListMorning = new List<BaoWen_Check_ViewModel>();


                BaoWen_Check_ViewModel bcv_group_morning = new BaoWen_Check_ViewModel();
                bcv_group_morning.YbUserName = "集体报";
                bcv_group_morning.Work = "集体"; ;
                bcv_group_morning.Date = date;
                BwYbs bwyb_group_morning = mlist.Where(a => a.YBUserName == "集体").FirstOrDefault();
                if (bwyb_group_morning != null)
                {
                    bcv_group_morning.IsMiss = false;
                    bcv_group_morning.Message = "";
                }
                else
                {
                    bcv_group_morning.IsMiss = true;
                    bcv_group_morning.Message = "（缺报）";
                }
                bcvListMorning.Add(bcv_group_morning);


                foreach (Scheduling scheduling in schListMorning)
                {
                    BaoWen_Check_ViewModel bcv = new BaoWen_Check_ViewModel();
                    bcv.Work = scheduling.Work;
                    bcv.Date = (DateTime)scheduling.Date;
                    bcv.YbUserName = scheduling.YBUserName;
                    BwYbs bwyb = mlist.Where(a => a.YBUserName == scheduling.YBUserName).FirstOrDefault();
                    if (bwyb != null)
                    {
                        bcv.IsMiss = false;
                        bcv.Message = "";
                    }
                    else
                    {
                        bcv.IsMiss = true;
                        bcv.Message = "（缺报）";
                    }
                    bcvListMorning.Add(bcv);
                }




                Repeater_YbUser_Morning.DataSource = bcvListMorning;
                Repeater_YbUser_Morning.DataBind();

                Repeater Repeater_YbUser_Night = (Repeater)e.Item.FindControl("Repeater_YbUser_Night");
                //根据分类ID查询该分类下的产品，并绑定产品Repeater 
                List<BwYbs> nlist = bll.GetList(a => a.YBType == "晚间报文" && a.YBDateTime == dateNight).OrderBy(a => a.YBUserName).OrderBy(a => a.YbUsers.Order).ToList();
                //List<YBUsers_ViewModel> userListN = nlist.GroupBy(a => new { a.YBUserName }).Select(a => new YBUsers_ViewModel { YBUserName = a.Key.YBUserName }).ToList();



                List<Scheduling> schListNight = sbll.GetList(a => a.Date == date).OrderBy(a => a.Order).ToList();

                List<BaoWen_Check_ViewModel> bcvListNight = new List<BaoWen_Check_ViewModel>();

                BaoWen_Check_ViewModel bcv_group_night = new BaoWen_Check_ViewModel();
                bcv_group_night.YbUserName = "集体报";
                bcv_group_night.Work = "集体";
                bcv_group_night.Date = date;
                BwYbs bwyb_group_night = nlist.Where(a => a.YBUserName == "集体").FirstOrDefault();
                if (bwyb_group_night != null)
                {
                    bcv_group_night.IsMiss = false;
                    bcv_group_night.Message = "";
                }
                else
                {
                    bcv_group_night.IsMiss = true;
                    bcv_group_night.Message = "（缺报）";
                }
                bcvListNight.Add(bcv_group_night);


                foreach (Scheduling scheduling in schListNight)
                {
                    BaoWen_Check_ViewModel bcv = new BaoWen_Check_ViewModel();
                    bcv.Work = scheduling.Work;
                    bcv.Date = (DateTime)scheduling.Date;
                    bcv.YbUserName = scheduling.YBUserName;
                    BwYbs bwyb = nlist.Where(a => a.YBUserName == scheduling.YBUserName).FirstOrDefault();
                    if (bwyb != null)
                    {
                        bcv.IsMiss = false;

                        bcv.Message = "";
                    }
                    else
                    {
                        bcv.IsMiss = true;
                        bcv.Message = "（缺报）";
                    }
                    bcvListNight.Add(bcv);
                }







                Repeater_YbUser_Night.DataSource = bcvListNight;
                Repeater_YbUser_Night.DataBind();



            }
        }

        protected void Btn_LiveData_Query_Click(object sender, EventArgs e)
        {



            DateTime startTime = DateTime.Parse(DDL_LiveData_Query_Year.SelectedItem.Value + "-" + DDL_LiveData_Query_Month.SelectedItem.Value + "-01");
            DateTime endTime = startTime.AddMonths(1);
            LiveData_BLL bll = new LiveData_BLL();
            List<LiveData> list = bll.GetList(a => a.Category == DDL_LiveData_Query_Time.SelectedItem.Value && a.FDate >= startTime && a.FDate < endTime).OrderBy(a => a.FDate).ThenBy(a => a.CountryName).ToList();
            GridView_LiveData.DataSource = list;
            GridView_LiveData.DataBind();

        }
    }
}