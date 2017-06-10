using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PF.BLL.Oracle;
using PF.Models.Oracle;
using PF.Models.SQL;
using PF.ViewModels;

namespace PF.BLL.SQL
{
   public partial  class WarnCheck_BLL
   {
       private OracleSAEntities db = new OracleSAEntities();
        public WarnCheck_ReachStation_ViewModel Caculate_Wind(WarnCheck wc)
        {
            double minValue = 10.8;
            if (wc.WarningLevel == "蓝色")
            {
                minValue = 10.8;
            }
            else if (wc.WarningLevel == "黄色")
            {
                minValue = 17.2;
            }
            else if (wc.WarningLevel == "橙色")
            {
                minValue = 24.5;
            }
            else if (wc.WarningLevel == "红色")
            {
                minValue = 32.7;
            }

            List<WarnCheck_Station> stations = new WarnCheck_Station_BLL().GetList().ToList();

           


            DATAMINUTE_BLL dbll = new DATAMINUTE_BLL();

            StringBuilder sb = new StringBuilder();

            for (int i = 0; i < stations.Count; i++)
            {
                if (i != stations.Count-1)
                {
                    sb.Append("'" + stations.ElementAt(i).StationName + "',");
                }
                else
                {
                    sb.Append("'" + stations.ElementAt(i).StationName + "'");
                }

            }

            DateTime startTime =((DateTime) wc.ReleaseTime).AddHours(-2);
            DateTime endTime =((DateTime) wc.ReleaseTime).AddHours(24);

            string sql =
                "select staname as StationName,fdate as DateTime,dwspeed as Value from (SELECT * FROM DATAMINUTE a WHERE a.staname in (" +
                sb.ToString() + ")  and a.fdate>= to_date('" + startTime.ToString("yyyy-MM-dd HH:mm") +
                "','yyyy-mm-dd hh24:mi') and  a.fdate<= to_date('" + endTime.ToString("yyyy-MM-dd HH:mm") +
                "','yyyy-mm-dd hh24:mi') and dwspeed > "+ minValue + "  order by a.fdate) where rownum <= 1";


            List<WarnCheck_ReachStation_ViewModel> list = db.Database.SqlQuery<WarnCheck_ReachStation_ViewModel>(sql)
                .ToList();


            return list.FirstOrDefault();


        }
    }
}
