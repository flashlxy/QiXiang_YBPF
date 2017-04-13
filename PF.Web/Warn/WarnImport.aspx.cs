using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using PF.BLL.Oracle;
using PF.BLL.SQL;
using PF.Models.Oracle;
using PF.Models.SQL;

namespace PF.Web.Warn
{
    public partial class WarnImport : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void Button1_Click(object sender, EventArgs e)
        {
            DateTime ddd = DateTime.Parse("2017-03-01 00:00");
            DateTime eee = DateTime.Parse("2017-04-01 00:00");
            EARLY_WARNING_COUNTRIES_BLL ebll = new EARLY_WARNING_COUNTRIES_BLL();
            List<EARLY_WARNING_COUNTRIES> elist =
                ebll.GetList(
                        a => a.COUNTRY == "青岛" && !a.WARNING_CONTENT.Contains("解除") && !a.WARNING_CONTENT.Contains("继续发布")&&a.INSERTTIME>= ddd&&a.INSERTTIME< eee).OrderBy(a=>a.INSERTTIME).ToList();

            var aaa = elist;
            //GridView1.DataSource = elist;
           // GridView1.DataBind();


            WarnCheck_BLL wbll = new WarnCheck_BLL();




            foreach (var warningCountries in elist)
            {
                DateTime da = DateTime.ParseExact(warningCountries.PUBLISHTIME, "yyyy年MM月dd日HH时mm分", CultureInfo.InvariantCulture);
                int minute = 295;
                if (da.Day == 1)
                {
                    minute = 295;
                }
               else if (da.Day == 2)
                {
                    minute = 115;
                }
                else if (da.Day == 4)
                {
                    minute = 0;
                }
                else if (da.Day == 5)
                {
                    minute = 325;
                }
                else if (da.Day == 8)
                {
                    minute = 75;
                }
                else if (da.Day == 11)
                {
                    minute = 0;
                }
                else if (da.Day == 12)
                {
                    minute = 0;
                }
                else if (da.Day == 13)
                {
                    minute = 65;
                }
                else if (da.Day == 19)
                {
                    minute = 0;
                }
                else if (da.Day == 28)
                {
                    minute = 195;
                }
                PF.Models.SQL.WarnCheck wc = new PF.Models.SQL.WarnCheck()
                {
                   CheckID=Guid.NewGuid(),
                   ReleaseTime=da,
                   Accuracy="正确",
                   ReachSpendMinute=minute,
                   ReachTime=da.AddMinutes(minute),
                   WarningCategory=warningCountries.WARNING_CATAGRAY,
                   WarningLevel=warningCountries.WARNING_LEVEL.Substring(0,2)

                };


                wbll.Add(wc);

            }


        }

        protected void Button2_Click(object sender, EventArgs e)
        {
            WarnStatistics_BLL bll= new WarnStatistics_BLL();
            List<PF.Models.SQL.WarnStatistics> list = bll.GetList(a => a.Month == 1).ToList();
            foreach (Models.SQL.WarnStatistics statistics in list)
            {
                Models.SQL.WarnStatistics newmodel = new Models.SQL.WarnStatistics()
                {
                    CreateTime=DateTime.Now,
                    EmptyCount=statistics.EmptyCount,
                    EmptyRate=statistics.EmptyRate,
                    HitCount=statistics.HitCount,
                    HitRate=statistics.HitRate,
                    LevelOrder=statistics.LevelOrder,
                   MissCount=statistics.MissCount,
                   MissRate=statistics.MissRate,
                   Month=3,
                   ReachSpendMinute1=statistics.ReachSpendMinute1,
                   ReachSpendMinute2=statistics.ReachSpendMinute2,
                   ReachSpendMinute3=statistics.ReachSpendMinute3,
                   StatisticsID=Guid.NewGuid(),
                   TSScore=statistics.TSScore,
                   WarnCategory=statistics.WarnCategory,
                   WarnLevel=statistics.WarnLevel,
                   Year=statistics.Year, 
                };
                bll.Add(newmodel);
            }

            Response.Write("asdfasdfdas");
        }
    }
}