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
                DateTime datetime = DateTime.ParseExact("20" + fileInfo.Name.Substring(0, 6), "yyyyMMdd", CultureInfo.InvariantCulture);
                string[] contents = FileHelper.GetShareTextLines(@"\\172.18.226.109\市县一体化平台文档\检验\r24-8-p\" + fileInfo.Name, "Administrator", "yubk0501!");
                List<string> citycodes = CityUtility.AllCodeList();
                foreach (string citycode in citycodes)
                {
                    string line = contents.Where(a => a.Contains(citycode)).FirstOrDefault();
                    decimal rain = 0;
                    if (!string.IsNullOrEmpty(line))
                    {
                        rain = decimal.Parse(line.Substring(27, 5).Trim());

                    }
                    LiveData liveData = bll.Get(a => a.CountryCode == citycode && a.FDate == datetime && a.Category == "08时");
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
                        newModel.FDate = datetime;
                        bll.Add(newModel);
                    }
                }


            }

            message.Append("导入降水量文件成功,共计导入" + allFileList.Count.ToString()+"个文件。<br/>");
            return message.ToString();
        }
    }
}
