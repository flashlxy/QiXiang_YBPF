using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using PF.Models.SQL;
using PF.BLL.SQL;

namespace PF.Web.Score
{
    public partial class ScoreMonth : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            BindData();
        }

        protected void Button_Query_Click(object sender, EventArgs e)
        {
            BindData();
        }
        public void BindData()
        {
            int Year = int.Parse(DropDownList_Year.SelectedItem.Value);
            int Month = int.Parse(DropDownList_Month.SelectedItem.Value);
            Score_Month_BLL bll = new Score_Month_BLL();
            List<Score_Month> list = bll.GetList(a => a.Year == Year && a.Month == Month&&a.YBUserName!="集体").OrderByDescending(a => a.WeightedTotal).ToList();
            Score_Month group = bll.GetList(a => a.Year == Year && a.Month == Month&&a.YBUserName=="集体").FirstOrDefault();
            list.Add(group);
            Repeater_List.DataSource = list;
            Repeater_List.DataBind();
        }

        protected void Button_Calculate_Click(object sender, EventArgs e)
        {
            Calculate2();
        }
        public void Calculate()
        {
            Score_Day_BLL dayBll = new Score_Day_BLL();

            DateTime startTime = DateTime.Parse(DropDownList_Year.SelectedItem.Value + "-" + DropDownList_Month.SelectedItem.Value + "-01");
            DateTime endTime = startTime.AddMonths(1).AddDays(-1);
            List<Score_Day> dayList = dayBll.GetList(a => a.YBDate >= startTime && a.YBDate <= endTime && a.YBUserName != "集体").OrderBy(a => a.YBDate).ToList();
            List<Score_Day> dayList_Group = dayBll.GetList(a => a.YBDate >= startTime && a.YBDate <= endTime && a.YBUserName == "集体").OrderBy(a => a.YBDate).ToList();


            List<Score_Month> list = new List<Score_Month>();

            foreach (var day in dayList)
            {

                var day_group = dayList_Group.Where(a => a.YBDate == day.YBDate && a.YBTime == day.YBTime).FirstOrDefault();


                double Ts晴雨 = Math.Round(((double)day.RainShine24 / (double)day.AllTotal * 10 / 24 + (double)day.RainShine48 / (double)day.AllTotal * 8 / 24 + (double)day.RainShine72 / (double)day.AllTotal * 6 / 24) * 100, 2);
                double Ts晴雨_集体 = Math.Round(((double)day_group.RainShine24 / (double)day_group.AllTotal * 10 / 24 + (double)day_group.RainShine48 / (double)day_group.AllTotal * 8 / 24 + (double)day_group.RainShine72 / (double)day_group.AllTotal * 6 / 24) * 100, 2);

                double Ts高温 = Math.Round(((double)day.MaxTemp24 / (double)day.AllTotal * 10 / 24 + (double)day.MaxTemp48 / (double)day.AllTotal * 8 / 24 + (double)day.MaxTemp72 / (double)day.AllTotal * 6 / 24) * 100, 2);
                double Ts高温_集体 = Math.Round(((double)day_group.MaxTemp24 / (double)day_group.AllTotal * 10 / 24 + (double)day_group.MaxTemp48 / (double)day_group.AllTotal * 8 / 24 + (double)day_group.MaxTemp72 / (double)day_group.AllTotal * 6 / 24) * 100, 2);

                double Ts低温 = Math.Round(((double)day.MinTemp24 / (double)day.AllTotal * 10 / 24 + (double)day.MinTemp48 / (double)day.AllTotal * 8 / 24 + (double)day.MinTemp72 / (double)day.AllTotal * 6 / 24) * 100, 2);
                double Ts低温_集体 = Math.Round(((double)day_group.MinTemp24 / (double)day_group.AllTotal * 10 / 24 + (double)day_group.MinTemp48 / (double)day_group.AllTotal * 8 / 24 + (double)day_group.MinTemp72 / (double)day_group.AllTotal * 6 / 24) * 100, 2);


                //个人降水
                double rainfall24 = 100;
                if (day.Rainfall24 != null && day.Rainfall24Total != null)
                {
                    rainfall24 = (double)day.Rainfall24 / (double)day.Rainfall24Total * 100;
                }
                double rainfall48 = 100;
                if (day.Rainfall48 != null && day.Rainfall48Total != null)
                {
                    rainfall48 = (double)day.Rainfall48 / (double)day.Rainfall48Total * 100;
                }
                double rainfall72 = 100;
                if (day.Rainfall72 != null && day.Rainfall72Total != null)
                {
                    rainfall72 = (double)day.Rainfall72 / (double)day.Rainfall72Total * 100;
                }
                double Ts降水 = Math.Round(rainfall24 * 10 / 24 + rainfall48 * 8 / 24 + rainfall72 * 6 / 24, 2);
                if (day.Rainfall24 == null && day.Rainfall48 == null && day.Rainfall72 == null)
                {
                    Ts降水 = 0.00;
                }

                //集体降水
                double rainfall24_group = 100;
                if (day_group.Rainfall24 != null && day_group.Rainfall24Total != null)
                {
                    rainfall24_group = (double)day_group.Rainfall24 / (double)day_group.Rainfall24Total * 100;
                }
                double rainfall48_group = 100;
                if (day_group.Rainfall48 != null && day_group.Rainfall48Total != null)
                {
                    rainfall48_group = (double)day_group.Rainfall48 / (double)day_group.Rainfall48Total * 100;
                }
                double rainfall72_group = 100;
                if (day_group.Rainfall72 != null && day_group.Rainfall72Total != null)
                {
                    rainfall72_group = (double)day_group.Rainfall72 / (double)day_group.Rainfall72Total * 100;
                }

                double Ts降水_集体 = Math.Round(rainfall24_group * 10 / 24 + rainfall48_group * 8 / 24 + rainfall72_group * 6 / 24, 2);
                if (day_group.Rainfall24 == null && day_group.Rainfall48 == null && day_group.Rainfall72 == null)
                {
                    Ts降水_集体 = 0.00;
                }
                //个人暴雨
                double rainstorm24 = 100;
                if (day.Rainstorm24 != null && day.Rainstorm24Total != null)
                {
                    rainstorm24 = (double)day.Rainstorm24 / (double)day.Rainstorm24Total * 100;
                }
                double rainstorm48 = 100;
                if (day.Rainstorm48 != null && day.Rainstorm48Total != null)
                {
                    rainstorm48 = (double)day.Rainstorm48 / (double)day.Rainstorm48Total * 100;
                }


                double Ts暴雨 = Math.Round(rainstorm24 * 10 / 18 + rainstorm48 * 8 / 18, 2);

                if (day.Rainstorm24 == null && day.Rainfall48 == null && day.Rainfall72 == null)
                {
                    Ts暴雨 = 0.00;
                }

                //集体暴雨
                double rainstorm24_group = 100;
                if (day_group.Rainstorm24 != null && day_group.Rainstorm24Total != null)
                {
                    rainstorm24_group = (double)day_group.Rainstorm24 / (double)day_group.Rainstorm24Total * 100;
                }
                double rainstorm48_group = 100;
                if (day_group.Rainstorm48 != null && day_group.Rainstorm48Total != null)
                {
                    rainstorm48_group = (double)day_group.Rainstorm48 / (double)day_group.Rainstorm48Total * 100;
                }


                double Ts暴雨_集体 = Math.Round(rainstorm24_group * 10 / 18 + rainstorm48_group * 8 / 18, 2);
                if (day_group.Rainstorm24 == null && day_group.Rainfall48 == null && day_group.Rainfall72 == null)
                {
                    Ts暴雨_集体 = 0.00;
                }


                double Ts总分 = Math.Round(Ts晴雨 * 30 / 100 + Ts降水 * 30 / 100 + Ts高温 * 20 / 100 + Ts低温 * 20 / 100, 2);
                double Ts总分_集体 = Math.Round(Ts晴雨_集体 * 30 / 100 + Ts降水_集体 * 30 / 100 + Ts高温_集体 * 20 / 100 + Ts低温_集体 * 20 / 100, 2);


                double Ts加权总分 = Ts总分_集体 * 60 / 100 + Ts总分 * 40 / 100;



                Score_Month month = new Score_Month();
                month.YBUserName = day.YBUserName;
                month.YBUserID = day.YBUserID;
                month.RainShine = decimal.Parse(Ts晴雨.ToString());
                month.RainShine_Group = decimal.Parse(Ts晴雨_集体.ToString());
                month.MinTemp = decimal.Parse(Ts低温.ToString());
                month.MinTemp_Group = decimal.Parse(Ts低温_集体.ToString());
                month.MaxTemp = decimal.Parse(Ts高温.ToString());
                month.MaxTemp_Group = decimal.Parse(Ts高温_集体.ToString());
                month.Rainfall = decimal.Parse(Ts降水.ToString());
                month.Rainfall_Group = decimal.Parse(Ts降水_集体.ToString());
                month.Rainstorm = decimal.Parse(Ts暴雨.ToString());
                month.Rainstorm_Group = decimal.Parse(Ts暴雨_集体.ToString());
                month.Total = decimal.Parse(Ts总分.ToString());
                month.Total_Group = decimal.Parse(Ts总分_集体.ToString());
                month.WeightedTotal = decimal.Parse(Ts加权总分.ToString());

                list.Add(month);

                //if (day.YBUserName == "毕玮")
                //{
                //    Response.Write( day.YBDate.ToString()+"："+month.Rainfall + "<br/>");

                //}
            }


            List<Score_Month> vlist = list.GroupBy(a => new { a.YBUserName }).Select(g => new Score_Month
            {

                YBUserName = g.Key.YBUserName,
                RainShine = g.Average(a => a.RainShine),
                RainShine_Group = g.Average(a => a.RainShine_Group),
                MinTemp = g.Average(a => a.MinTemp),
                MinTemp_Group = g.Average(a => a.MinTemp_Group),
                MaxTemp = g.Average(a => a.MaxTemp),
                MaxTemp_Group = g.Average(a => a.MaxTemp_Group),
                Rainfall = g.Average(a => a.Rainfall),
                Rainfall_Group = g.Average(a => a.Rainfall_Group),
                Rainstorm = g.Average(a => a.Rainstorm),
                Rainstorm_Group = g.Average(a => a.Rainstorm_Group),
                Total = g.Average(a => a.Total),
                Total_Group = g.Average(a => a.Total_Group),
                WeightedTotal = g.Average(a => a.WeightedTotal)

            }).ToList();


            List<YbUsers> ulist = new YbUsers_BLL().GetList(a=>a.Work=="预报").ToList();
            Score_Month_BLL bll = new Score_Month_BLL();
            foreach (var item in vlist)
            {
                item.ScoreID = Guid.NewGuid();
                item.Year = int.Parse(DropDownList_Year.SelectedItem.Value);
                item.Month = int.Parse(DropDownList_Month.SelectedItem.Value);
                item.YBUserID = ulist.Where(a => a.YBUserName == item.YBUserName).FirstOrDefault().YBUserID;
                bll.Add(item, false);

            }
            if (vlist.Count() > 0)
            {
                int Year = int.Parse(DropDownList_Year.SelectedItem.Value);
                int Month = int.Parse(DropDownList_Month.SelectedItem.Value);
                bll.Delete(a => a.Year == Year && a.Month == Month);
                bll.SaveChange();

            }
            BindData();
            Response.Write("<script language=javascript defer>alert('计算成功！');</script>");
        }

        //重新计算
        public void Calculate2()
        {
            Score_Day_BLL dayBll = new Score_Day_BLL();

            DateTime startTime = DateTime.Parse(DropDownList_Year.SelectedItem.Value + "-" + DropDownList_Month.SelectedItem.Value + "-01");
            DateTime endTime = startTime.AddMonths(1).AddDays(-1);
            List<Score_Day> dayList = dayBll.GetList(a => a.YBDate >= startTime && a.YBDate <= endTime ).OrderBy(a => a.YBDate).ToList();
            List<Score_Day> dayList_Group = dayBll.GetList(a => a.YBDate >= startTime && a.YBDate <= endTime && a.YBUserName == "集体").OrderBy(a => a.YBDate).ToList();


            List<Score_Month> list = new List<Score_Month>();



            var allUsers = dayList.GroupBy(a => new { a.YBUserName,a.YBUserID }).ToList();

            Score_Month_BLL bll = new Score_Month_BLL();

            foreach (var user in allUsers)
            {
                if (user.Key.YBUserName == "时晓曚")
                {
                    var ssd = user;
                }

                List<Score_Day> userList = dayList.Where(a => a.YBUserName == user.Key.YBUserName).ToList();

                List<Score_Day> groupList = new List<Score_Day>();
                foreach (var usr in userList)
                {
                  var group=  dayList_Group.Where(a => a.YBDate == usr.YBDate && a.YBTime == usr.YBTime).FirstOrDefault();
                    groupList.Add(group);
                }


                //个人

                double Ts晴雨=  Math.Round(
                    ((double) userList.Sum(a => a.RainShine24)/(double) userList.Sum(a => a.AllTotal)*10/24 +
                     (double) userList.Sum(a => a.RainShine48)/(double) userList.Sum(a => a.AllTotal) *8/24 +
                     (double) userList.Sum(a => a.RainShine72)/(double) userList.Sum(a => a.AllTotal) *6/24)*100, 2);



                double Ts高温 = Math.Round(
                   ((double)userList.Sum(a => a.MaxTemp24) / (double)userList.Sum(a => a.AllTotal) * 10 / 24 +
                    (double)userList.Sum(a => a.MaxTemp48) / (double)userList.Sum(a => a.AllTotal) * 8 / 24 +
                    (double)userList.Sum(a => a.MaxTemp72) / (double)userList.Sum(a => a.AllTotal) * 6 / 24) * 100, 2);

                double Ts低温 = Math.Round(
                   ((double)userList.Sum(a => a.MinTemp24) / (double)userList.Sum(a => a.AllTotal) * 10 / 24 +
                    (double)userList.Sum(a => a.MinTemp48) / (double)userList.Sum(a => a.AllTotal) * 8 / 24 +
                    (double)userList.Sum(a => a.MinTemp72) / (double)userList.Sum(a => a.AllTotal) * 6 / 24) * 100, 2);


                //个人降水

                Nullable<int> rainfall24_Sum = userList.Sum(a => a.Rainfall24);
                Nullable<int> rainfall24_Total_Sum = userList.Sum(a => a.Rainfall24Total);

                Nullable<int> rainfall48_Sum = userList.Sum(a => a.Rainfall48);
                Nullable<int> rainfall48_Total_Sum = userList.Sum(a => a.Rainfall48Total);

                Nullable<int> rainfall72_Sum = userList.Sum(a => a.Rainfall72);
                Nullable<int> rainfall72_Total_Sum = userList.Sum(a => a.Rainfall72Total);


                double rainfall24 = 100;
                if (rainfall24_Total_Sum != 0)
                {
                    rainfall24=(double) rainfall24_Sum/(double) rainfall24_Total_Sum * 100;
                }
                double rainfall48 = 100;
                if (rainfall48_Total_Sum != 0)
                {
                    rainfall48 = (double)rainfall48_Sum / (double)rainfall48_Total_Sum * 100;
                }
                double rainfall72 = 100;
                if (rainfall72_Total_Sum != 0)
                {
                    rainfall72 = (double)rainfall72_Sum / (double)rainfall72_Total_Sum * 100;
                }
                double Ts降水 = 100;

                if (rainfall24_Total_Sum == 0 && rainfall48_Total_Sum == 0 && rainfall72_Total_Sum == 0)
                {
                     Ts降水 = 0.00;

                }
                else
                {
                     Ts降水 = Math.Round(rainfall24 * 10 / 24 + rainfall48 * 8 / 24 + rainfall72 * 6 / 24, 2);

                }



                //个人暴雨

                Nullable<int> rainstorm24_Sum = userList.Sum(a => a.Rainstorm24);
                Nullable<int> rainstorm24_Total_Sum = userList.Sum(a => a.Rainstorm24Total);

                Nullable<int> rainstorm48_Sum = userList.Sum(a => a.Rainstorm48);
                Nullable<int> rainstorm48_Total_Sum = userList.Sum(a => a.Rainstorm48Total);
                double rainstorm24 = 100;
                if (rainstorm24_Total_Sum != 0)
                {
                    rainstorm24 = (double)rainstorm24_Sum / (double)rainstorm24_Total_Sum * 100;
                }
                double rainstorm48 = 100;
                if (rainstorm48_Total_Sum != 0)
                {
                    rainstorm48 = (double)rainstorm48_Sum / (double)rainstorm48_Total_Sum * 100;
                }
                double Ts暴雨 = 100;
                if (rainstorm24_Total_Sum == 0 && rainstorm48_Total_Sum == 0)
                {
                    Ts暴雨 = 0.00;
                }
                else if (rainstorm24_Total_Sum!=0&& rainstorm48_Total_Sum == 0)
                {
                    
                    
                    Ts暴雨 = Math.Round(rainstorm24 * 10 / 18  , 2);
                }
                else if (rainstorm24_Total_Sum == 0 && rainstorm48_Total_Sum != 0)
                {
                    Ts暴雨 = Math.Round(rainstorm48 * 8 / 18, 2);

                }
                else if (rainstorm24_Total_Sum != 0 && rainstorm48_Total_Sum != 0)
                {
                    Ts暴雨 = Math.Round(rainstorm24 * 10 / 18 + rainstorm48 * 8 / 18, 2);

                }




                //集体

                double Ts晴雨_Group = Math.Round(
                    ((double)groupList.Sum(a => a.RainShine24) / (double)groupList.Sum(a => a.AllTotal) * 10 / 24 +
                     (double)groupList.Sum(a => a.RainShine48) / (double)groupList.Sum(a => a.AllTotal) * 8 / 24 +
                     (double)groupList.Sum(a => a.RainShine72) / (double)groupList.Sum(a => a.AllTotal) * 6 / 24) * 100, 2);



                double Ts高温_Group = Math.Round(
                   ((double)groupList.Sum(a => a.MaxTemp24) / (double)groupList.Sum(a => a.AllTotal) * 10 / 24 +
                    (double)groupList.Sum(a => a.MaxTemp48) / (double)groupList.Sum(a => a.AllTotal) * 8 / 24 +
                    (double)groupList.Sum(a => a.MaxTemp72) / (double)groupList.Sum(a => a.AllTotal) * 6 / 24) * 100, 2);

                double Ts低温_Group = Math.Round(
                   ((double)groupList.Sum(a => a.MinTemp24) / (double)groupList.Sum(a => a.AllTotal) * 10 / 24 +
                    (double)groupList.Sum(a => a.MinTemp48) / (double)groupList.Sum(a => a.AllTotal) * 8 / 24 +
                    (double)groupList.Sum(a => a.MinTemp72) / (double)groupList.Sum(a => a.AllTotal) * 6 / 24) * 100, 2);


                //集体降水

                Nullable<int> rainfall24_Sum_Group = groupList.Sum(a => a.Rainfall24);
                Nullable<int> rainfall24_Total_Sum_Group = groupList.Sum(a => a.Rainfall24Total);

                Nullable<int> rainfall48_Sum_Group = groupList.Sum(a => a.Rainfall48);
                Nullable<int> rainfall48_Total_Sum_Group = groupList.Sum(a => a.Rainfall48Total);

                Nullable<int> rainfall72_Sum_Group = groupList.Sum(a => a.Rainfall72);
                Nullable<int> rainfall72_Total_Sum_Group = groupList.Sum(a => a.Rainfall72Total);


                double rainfall24_Group = 100;
                if (rainfall24_Total_Sum_Group != 0)
                {
                    rainfall24_Group = (double)rainfall24_Sum_Group / (double)rainfall24_Total_Sum_Group * 100;
                }
                double rainfall48_Group = 100;
                if (rainfall48_Total_Sum_Group != 0)
                {
                    rainfall48_Group = (double)rainfall48_Sum_Group / (double)rainfall48_Total_Sum_Group * 100;
                }
                double rainfall72_Group = 100;
                if (rainfall72_Total_Sum_Group != 0)
                {
                    rainfall72_Group = (double)rainfall72_Sum_Group / (double)rainfall72_Total_Sum_Group * 100;
                }
                double Ts降水_Group = 100.00;

                if (rainfall24_Total_Sum_Group == 0 && rainfall48_Total_Sum_Group == 0 && rainfall72_Total_Sum_Group == 0)
                {
                    Ts降水_Group = 0.00;

                }
                else
                {
                    Ts降水_Group = Math.Round(rainfall24_Group * 10 / 24 + rainfall48_Group * 8 / 24 + rainfall72_Group * 6 / 24, 2);

                }



                //集体暴雨

                Nullable<int> rainstorm24_Sum_Group = groupList.Sum(a => a.Rainstorm24);
                Nullable<int> rainstorm24_Total_Sum_Group = groupList.Sum(a => a.Rainstorm24Total);

                Nullable<int> rainstorm48_Sum_Group = groupList.Sum(a => a.Rainstorm48);
                Nullable<int> rainstorm48_Total_Sum_Group = groupList.Sum(a => a.Rainstorm48Total);
                double rainstorm24_Group = 100;
                if (rainstorm24_Total_Sum_Group != 0)
                {
                    rainstorm24_Group = (double)rainstorm24_Sum_Group / (double)rainstorm24_Total_Sum_Group * 100;
                }
                double rainstorm48_Group = 100;
                if (rainstorm48_Total_Sum_Group != 0)
                {
                    rainstorm48_Group = (double)rainstorm48_Sum_Group / (double)rainstorm48_Total_Sum_Group * 100;
                }
                double Ts暴雨_Group = 100.00;
                if (rainstorm24_Total_Sum_Group == 0 && rainstorm48_Total_Sum_Group == 0)
                {
                    Ts暴雨_Group = 0.00;
                }
                else if (rainstorm24_Total_Sum_Group != 0 && rainstorm48_Total_Sum_Group == 0)
                {
                    Ts暴雨_Group = Math.Round(rainstorm24_Group * 10 / 18 , 2);
                }
                else if (rainstorm24_Total_Sum_Group == 0 && rainstorm48_Total_Sum_Group != 0)
                {
                    Ts暴雨_Group = Math.Round( rainstorm48_Group * 8 / 18, 2);
                }
                else if (rainstorm24_Total_Sum_Group != 0 && rainstorm48_Total_Sum_Group != 0)
                {
                    Ts暴雨_Group = Math.Round(rainstorm24_Group * 10 / 18 + rainstorm48_Group * 8 / 18, 2);
                }





                double Ts总分 = Math.Round(Ts晴雨 * 30 / 100 + Ts降水 * 30 / 100 + Ts高温 * 20 / 100 + Ts低温 * 20 / 100, 2);
                double Ts总分_集体 = Math.Round(Ts晴雨_Group * 30 / 100 + Ts降水_Group * 30 / 100 + Ts高温_Group * 20 / 100 + Ts低温_Group * 20 / 100, 2);


                double Ts加权总分 = Ts总分_集体 * 60 / 100 + Ts总分 * 40 / 100;

                Score_Month month = new Score_Month();
                month.ScoreID = Guid.NewGuid();
                month.Year = int.Parse(DropDownList_Year.SelectedItem.Value);
                month.Month = int.Parse(DropDownList_Month.SelectedItem.Value);
                          
                month.YBUserName = user.Key.YBUserName;
                month.YBUserID = user.Key.YBUserID;
                month.RainShine = decimal.Parse(Ts晴雨.ToString());
                month.RainShine_Group = decimal.Parse(Ts晴雨_Group.ToString());
                month.MinTemp = decimal.Parse(Ts低温.ToString());
                month.MinTemp_Group = decimal.Parse(Ts低温_Group.ToString());
                month.MaxTemp = decimal.Parse(Ts高温.ToString());
                month.MaxTemp_Group = decimal.Parse(Ts高温_Group.ToString());
                month.Rainfall = decimal.Parse(Ts降水.ToString());
                month.Rainfall_Group = decimal.Parse(Ts降水_Group.ToString());
                month.Rainstorm = decimal.Parse(Ts暴雨.ToString());
                month.Rainstorm_Group = decimal.Parse(Ts暴雨_Group.ToString());
                month.Total = decimal.Parse(Ts总分.ToString());
                month.Total_Group = decimal.Parse(Ts总分_集体.ToString());
                month.WeightedTotal = decimal.Parse(Ts加权总分.ToString());

                
                bll.Add(month, false);

            }


            int Year = int.Parse(DropDownList_Year.SelectedItem.Value);
            int Month = int.Parse(DropDownList_Month.SelectedItem.Value);
            bll.Delete(a => a.Year == Year && a.Month == Month);
            bll.SaveChange();





        
            BindData();
            Response.Write("<script language=javascript defer>alert('计算成功！');</script>");
        }

        
    }
}