using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using PF.BLL.Oracle;
using PF.Models.Oracle;
using PF.Models.SQL;
using PF.Utility;
using PF.ViewModels;

namespace PF.BLL.SQL
{
    public partial class LiveData_BLL
    {

        public string DataImportAFile(DateTime yearMonth)
        {
            StringBuilder message = new StringBuilder();

            string yearMonthString = yearMonth.ToString("yyyyMM");

            LiveData_BLL bll = new LiveData_BLL();
            List<FileInfo> allFileList = FileHelper.GetShareFileInfos(@"\\172.18.226.109\市县一体化平台文档\检验\A", "A*-" + yearMonthString + "*", "administrator", "yubk0501!");
            if (allFileList.Count==0)
            {
                return "未发现该月份A文件！<br/>";
            }
            foreach (FileInfo fileInfo in allFileList)
            {
                List<LiveData> sklist = new List<LiveData>();

                //string contents = File.ReadAllText(@"\\172.18.226.10\nt40\zdzh\" + item.Value);
                string countrycode = fileInfo.Name.Substring(1, 5);
                //string apath = @"P:\zdzh\" + item.Value;
                string contents = FileHelper.GetShareTextContent(@"\\172.18.226.109\市县一体化平台文档\检验\A\" + fileInfo.Name, "Administrator", "yubk0501!", Encoding.Default);
                string tbcontents =
                    contents.Substring(contents.IndexOf("TB") + 4,
                        (contents.IndexOf("IB") - contents.IndexOf("TB") - 7)).Replace("\r\n", "");
                string[] tb = tbcontents.Split('.');


                for (int i = 0; i < tb.Length; i++)
                {
                    LiveData shikuang = new LiveData();
                    shikuang.LDID = Guid.NewGuid();
                    shikuang.FDate = DateTime.ParseExact(fileInfo.Name.Substring(7, 6) + (i + 1).ToString("00"), "yyyyMMdd", CultureInfo.InvariantCulture);
                    shikuang.CountryCode = countrycode;
                    shikuang.CreateTime = DateTime.Now;
                    shikuang.CountryName = CityUtility.GetName(countrycode);
                    shikuang.Category = "20时";

                    shikuang.MaxTemp = decimal.Parse(tb[i].Substring(119, 4)) / 10;
                    shikuang.MinTemp = decimal.Parse(tb[i].Substring(129, 4)) / 10;
                    sklist.Add(shikuang);
                }
                string raincontents =
                    contents.Substring(contents.IndexOf("R6") + 4, tb.Length * 16 - 2).Replace("\r\n", ".");
                string raincontents2 =
                    contents.Substring(contents.IndexOf("R6") + 4, tb.Length * 18 - 3).Replace("\r\n", ".");
                string[] rains = raincontents.Split('.');


                for (int i = 0; i < rains.Length; i++)
                {

                    string rainstr = rains[i].Substring(10, 4);
                    if (rainstr != ",,,,")
                    {
                        sklist.ElementAt(i).Rain = decimal.Parse(rainstr) / 10;
                    }
                    else
                    {
                        sklist.ElementAt(i).Rain = (decimal)0.01;
                    }


                }

                foreach (var item in sklist)
                {

                    bll.Delete(
                        a => a.CountryCode == item.CountryCode && a.FDate == item.FDate && a.Category == "20时");

                    bll.Add(item);


                }

                //message.Append(countrycode + " " + fileInfo.Name.Substring(7, 6) + sklist.Count().ToString()+"<br/>");

            }

            message.Append("导入A文件成功,共计："+allFileList.Count().ToString() + "个文件。<br/>");
            return message.ToString();

        }

        public string DataImportTemp08(DateTime yearMonth)
        {
            StringBuilder message = new StringBuilder();

            string yearMonthString = yearMonth.ToString("yyyyMM");
            DATAHOUR_BLL hbll = new DATAHOUR_BLL();
            LiveData_BLL lbll = new LiveData_BLL();
            DateTime startTime = DateTime.ParseExact(yearMonthString + "01" + "08", "yyyyMMddHH", CultureInfo.InvariantCulture);
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
                        //message.Append(cityname + "<br/>");
                    }

                }

                //message.Append("导入08时温度成功，"stime.ToString("yyyy-MM-dd HH:mm") + "~" + etime.ToString("yyyy-MM-dd HH:mm") + "<br/>");
            }
            message.Append("导入08时温度成功,共计"+ts.Days + "天数据。<br/>");

            return message.ToString();
        }

        public string DataImportRain08(DateTime yearMonth)
        {
            StringBuilder message = new StringBuilder();

            string yearMonthString = yearMonth.ToString("yyyyMM");

            LiveData_BLL bll = new LiveData_BLL();
            List<FileInfo> allFileList = FileHelper.GetShareFileInfos(@"\\172.18.226.109\市县一体化平台文档\检验\r24-8-p", yearMonthString.Substring(2,4)+ "*.000", "administrator", "yubk0501!");
            if (allFileList.Count == 0)
            {
                return "未发现该月份降水文件!<br/>";
            }
            foreach (FileInfo fileInfo in allFileList)
            {
                //DateTime datetime = DateTime.ParseExact("20" + fileInfo.Name.Substring(0, 6), "yyyyMMdd", CultureInfo.InvariantCulture);
                DateTime fileDateTime = DateTime.ParseExact("20" + fileInfo.Name.Substring(0, 6), "yyyyMMdd", CultureInfo.InvariantCulture);


                if (fileDateTime.Day == 9)
                {
                    var aaa = fileDateTime;
                }
                DateTime dataDateTime = fileDateTime.AddDays(-1);

                string[] contents = FileHelper.GetShareTextLines(@"\\172.18.226.109\市县一体化平台文档\检验\r24-8-p\" + fileInfo.Name, "Administrator", "yubk0501!");
                List<string> citycodes = CityUtility.AllCodeList();
                foreach (string citycode in citycodes)
                {
                    string line = contents.Where(a => a.Contains(citycode)).FirstOrDefault();
                    decimal rain = 0;
                    if (!string.IsNullOrEmpty(line))
                    {
                        rain = decimal.Parse(line.Substring(27, line.Length-27).Trim());

                    }
                    LiveData liveData = bll.Get(a => a.CountryCode == citycode && a.FDate == dataDateTime && a.Category == "08时");
                    if (liveData != null)
                    {
                        liveData.Rain = rain;
                        bll.Update(liveData);
                    }
                    else
                    {
                        LiveData newModel = new LiveData();
                        newModel.LDID = Guid.NewGuid();
                        newModel.Category = "08时";
                        newModel.CountryCode = citycode;
                        newModel.CountryName = CityUtility.GetName(citycode);
                        newModel.CreateTime = DateTime.Now;
                        newModel.Rain = rain;
                        newModel.FDate = dataDateTime;
                        bll.Add(newModel);
                    }
                }


            }

            message.Append("导入降水量文件成功,共计导入" + allFileList.Count.ToString()+"个文件。<br/>");
            return message.ToString();
        }

        public bool DataCheck(DateTime yearMonth, string category)
        {
            DateTime startTime = yearMonth;
            DateTime endTime = startTime.AddMonths(1);
            LiveData_BLL bll = new LiveData_BLL();
            List<LiveData> list =
                bll.GetList(
                    a =>
                        a.FDate >= startTime & a.FDate < endTime && a.Category == category)
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
           
            if (ldcList.Count() > 0)
            {
                return false;
            }
            else
            {
                return true;

            }
        }


        /////////////////////////////
        public string DataImportTemp20_FromDataBase(DateTime yearMonth)
        {
            StringBuilder message = new StringBuilder();

            string yearMonthString = yearMonth.ToString("yyyyMM");
            DATAHOUR_BLL hbll = new DATAHOUR_BLL();
            LiveData_BLL lbll = new LiveData_BLL();
            DateTime startTime = DateTime.ParseExact(yearMonthString + "01" + "20", "yyyyMMddHH", CultureInfo.InvariantCulture);
            DateTime endTime = startTime.AddMonths(1);

            DateTime startTime2 = startTime.AddDays(-1);

            DateTime endTime2 = endTime.AddMonths(-1);

            TimeSpan ts = endTime2 - startTime2;
            for (int i = 0; i < ts.Days; i++)
            {
                DateTime stime = startTime2.AddDays(i);
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
                            newModel.FDate = stime;
                            newModel.Category = "20时";
                            newModel.CountryCode = CityUtility.GetCode(cityname);
                            newModel.CountryName = cityname;
                            newModel.CreateTime = DateTime.Now;
                            newModel.MaxTemp = dlist.Max(a => a.MAXTEMP);
                            newModel.MinTemp = dlist.Min(a => a.MINTEMP);
                            lbll.Add(newModel);
                        }
                        //message.Append(cityname + "<br/>");
                    }

                }

                //message.Append("导入08时温度成功，"stime.ToString("yyyy-MM-dd HH:mm") + "~" + etime.ToString("yyyy-MM-dd HH:mm") + "<br/>");
            }
            message.Append("导入08时温度成功,共计" + ts.Days + "天数据。<br/>");

            return message.ToString();
        }
    }
}
