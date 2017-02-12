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
            List<Score_Month> list = bll.GetList(a => a.Year == Year && a.Month == Month).OrderByDescending(a => a.WeightedTotal).ToList();
            Repeater_List.DataSource = list;
            Repeater_List.DataBind();
        }

        protected void Button_Calculate_Click(object sender, EventArgs e)
        {
            Calculate();
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
                double rainfall24 = 1;
                if (day.Rainfall24 != null && day.Rainfall24Total != null)
                {
                    rainfall24 = (double)day.Rainfall24 / (double)day.Rainfall24Total;
                }
                double rainfall48 = 1;
                if (day.Rainfall48 != null && day.Rainfall48Total != null)
                {
                    rainfall48 = (double)day.Rainfall48 / (double)day.Rainfall48Total;
                }
                double rainfall72 = 1;
                if (day.Rainfall72 != null && day.Rainfall72Total != null)
                {
                    rainfall72 = (double)day.Rainfall72 / (double)day.Rainfall72Total;
                }

                double Ts降水 = Math.Round(rainfall24 * 10 / 24 + rainfall48 * 8 / 24 + rainfall72 * 6 / 24, 2);
                //集体降水
                double rainfall24_group = 1;
                if (day_group.Rainfall24 != null && day_group.Rainfall24Total != null)
                {
                    rainfall24_group = (double)day_group.Rainfall24 / (double)day_group.Rainfall24Total;
                }
                double rainfall48_group = 1;
                if (day_group.Rainfall48 != null && day_group.Rainfall48Total != null)
                {
                    rainfall48_group = (double)day_group.Rainfall48 / (double)day_group.Rainfall48Total;
                }
                double rainfall72_group = 1;
                if (day_group.Rainfall72 != null && day_group.Rainfall72Total != null)
                {
                    rainfall72_group = (double)day_group.Rainfall72 / (double)day_group.Rainfall72Total;
                }

                double Ts降水_集体 = Math.Round(rainfall24_group * 10 / 24 + rainfall48_group * 8 / 24 + rainfall72_group * 6 / 24, 2);

                //个人暴雨
                double rainstorm24 = 1;
                if (day.Rainstorm24 != null && day.Rainstorm24Total != null)
                {
                    rainstorm24 = (double)day.Rainstorm24 / (double)day.Rainstorm24Total;
                }
                double rainstorm48 = 1;
                if (day.Rainstorm48 != null && day.Rainstorm48Total != null)
                {
                    rainstorm48 = (double)day.Rainstorm48 / (double)day.Rainstorm48Total;
                }


                double Ts暴雨 = Math.Round(rainstorm24 * 10 / 18 + rainstorm48 * 8 / 18, 2);
                //集体暴雨
                double rainstorm24_group = 1;
                if (day_group.Rainstorm24 != null && day_group.Rainstorm24Total != null)
                {
                    rainstorm24_group = (double)day_group.Rainstorm24 / (double)day_group.Rainstorm24Total;
                }
                double rainstorm48_group = 1;
                if (day_group.Rainstorm48 != null && day_group.Rainstorm48Total != null)
                {
                    rainstorm48_group = (double)day_group.Rainstorm48 / (double)day_group.Rainstorm48Total;
                }


                double Ts暴雨_集体 = Math.Round(rainstorm24_group * 10 / 18 + rainstorm48_group * 8 / 18, 2);


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

            }


            List<Score_Month> vlist = list.GroupBy(a => new { a.YBUserName }).Select(g => new Score_Month
            {

                YBUserName = g.Key.YBUserName,
                RainShine = g.Average(a => a.RainShine),
                RainShine_Group = g.Average(a => a.RainShine_Group),
                MinTemp = g.Average(a => a.RainShine),
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


            List<YbUsers> ulist = new YbUsers_BLL().GetList().ToList();
            Score_Month_BLL bll = new Score_Month_BLL();
            foreach (var item in vlist)
            {
                item.ScoreID = Guid.NewGuid();
                item.Year =int.Parse( DropDownList_Year.SelectedItem.Value);
                item.Month =int.Parse( DropDownList_Month.SelectedItem.Value);
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

    }
}