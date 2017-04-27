using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using PF.BLL.Oracle;
using PF.BLL.SQL;
using PF.Models.Oracle;
using PF.Models.SQL;
using PF.Utility;

namespace PF.CA_LiveData
{
    public  class LiveTemp
    {
        public void Temp08()
        {
            DATAHOUR_BLL hbll = new DATAHOUR_BLL();
            LiveData_BLL lbll = new LiveData_BLL();




            DateTime.Now.AddMonths(-1).ToString("yyyyMMdd");


            DateTime startTime = DateTime.ParseExact(DateTime.Now.AddDays(-5).ToString("yyyyMMdd")+ "08", "yyyyMMddHH", CultureInfo.InvariantCulture);
            DateTime endTime = startTime.AddDays(4);

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

                        Console.WriteLine(cityname);
                        
                    }

                }

                Console.WriteLine(stime.ToString("yyyy-MM-dd HH:mm") + "~" + etime.ToString("yyyy-MM-dd HH:mm") );
            }
        }
        public void Temp20()
        {
            DATAHOUR_BLL hbll = new DATAHOUR_BLL();
            LiveData_BLL lbll = new LiveData_BLL();




            DateTime.Now.AddDays(-1).ToString("yyyyMMdd");


            DateTime startTime = DateTime.ParseExact(DateTime.Now.AddDays(-5).ToString("yyyyMMdd") + "20", "yyyyMMddHH", CultureInfo.InvariantCulture);
            DateTime endTime = startTime.AddDays(4);

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
                        DateTime seletime = DateTime.Parse(etime.ToShortDateString());

                        LiveData liveData = lbll.Get(a => a.CountryName == cityname && a.FDate == seletime && a.Category == "20时");
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
                            newModel.FDate = etime;
                            newModel.Category = "20时";
                            newModel.CountryCode = CityUtility.GetCode(cityname);
                            newModel.CountryName = cityname;
                            newModel.CreateTime = DateTime.Now;
                            newModel.MaxTemp = dlist.Max(a => a.MAXTEMP);
                            newModel.MinTemp = dlist.Min(a => a.MINTEMP);
                            lbll.Add(newModel);
                        }

                        Console.WriteLine(cityname);

                    }

                }

                Console.WriteLine(stime.ToString("yyyy-MM-dd HH:mm") + "~" + etime.ToString("yyyy-MM-dd HH:mm"));
            }
        }
    }
}
