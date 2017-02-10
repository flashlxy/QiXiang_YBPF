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
using PF.Utility;

namespace PF.Web.Temp
{
    public partial class Temp08 : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void BtnImport_Click(object sender, EventArgs e)
        {
            DATAHOUR_BLL hbll = new DATAHOUR_BLL();
            LiveData_BLL lbll = new LiveData_BLL();
            DateTime startTime = DateTime.ParseExact("2016" + MonthDDL.SelectedItem.Value + "01" + "08", "yyyyMMddHH", CultureInfo.InvariantCulture);
            DateTime endTime = startTime.AddMonths(1);

            TimeSpan ts = endTime - startTime;
            for (int i = 0; i < ts.Days; i++)
            {
                DateTime stime = startTime.AddDays(i);
                DateTime etime = stime.AddDays(1);

                List<string> citynames = CityUtility.AllNameList();
                foreach (string cityname in citynames)
                {
                    string selectname = cityname;
                    if (cityname == "黄岛")
                    {
                        selectname = "胶南";
                    }

                    List<DATAHOUR> dlist = hbll.GetList(a => a.STANAME == selectname && a.MINTEMP != 9999 && a.MAXTEMP != 9999 && a.FDATE >= stime && a.FDATE < etime).ToList();
                    if (dlist.Count > 0)
                    {
                        DateTime seletime = DateTime.Parse(stime.ToShortDateString());

                        LiveData liveData = lbll.Get(a => a.CountryName == cityname && a.FDate == seletime && a.Category == "08时");
                        if (liveData != null)
                        {
                            liveData.MaxTemp = dlist.Max(a => a.MAXTEMP);
                            liveData.MinTemp = dlist.Min(a => a.MINTEMP);
                            lbll.Update(liveData);
                        }
                        else
                        {
                            LiveData newModel = new LiveData();
                            newModel.LDID = Guid.NewGuid();
                            newModel.FDate = stime;
                            newModel.Category = "08时";
                            newModel.CountryCode = CityUtility.GetCode(cityname);
                            newModel.CountryName = cityname;
                            newModel.CreateTime = DateTime.Now;
                            newModel.MaxTemp = dlist.Max(a => a.MAXTEMP);
                            newModel.MinTemp = dlist.Min(a => a.MINTEMP);
                            lbll.Add(newModel);
                        }
                        Response.Write(cityname + "<br/>");
                    }
                   
                }

                Response.Write(stime.ToString("yyyy-MM-dd HH:mm") + "~" + etime.ToString("yyyy-MM-dd HH:mm") + "<br/>");
            }
        }
    }
}