using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PF.Models.SQL;

namespace PF.BLL.SQL
{
   public partial  class WarnCheck_BLL
    {
        public void Caculate_Wind(WarnCheck wc)
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

            foreach (WarnCheck_Station station in stations)
            {
                
            }
        }
    }
}
