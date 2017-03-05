using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PF.Models.SQL;

namespace PF.BLL.SQL
{
  public  partial class Score_Day_BLL
    {
        public void Caculate20(DateTime startDate, DateTime endDate)
        {
            
            dal.Delete(a => a.YBTime == "20时" && a.YBDate >= startDate && a.YBDate <= endDate);

            DateTime preDate = endDate.AddDays(2);

            List<LiveData> liveList =
                new LiveData_BLL().GetList(a => a.FDate >= startDate && a.FDate <= preDate && a.Category == "20时")
                    .ToList();
            BwYbs_BLL bwYbsBll = new BwYbs_BLL();
            List<BwYbs> list = bwYbsBll.GetList(a => a.YBDateTime >= startDate && a.YBDateTime <= endDate && ((DateTime)a.YBDateTime).Hour == 16).ToList();
            List<WeatherDictionary> wdList = new WeatherDictionary_BLL().GetList(a => a.Type == "天气").ToList();
            TimeSpan timeSpan = endDate - startDate;//时间跨度

            for (int i = 0; i < timeSpan.Days; i++)
            {
                //当前天时间
                DateTime dayTime = DateTime.Parse(startDate.AddDays(i).ToShortDateString() + " 16:30");
                //当前天 所有人的数据
                List<BwYbs> dayList = list.Where(a => a.YBDateTime == dayTime).ToList();
                //当前天 所有人的名字集合
                var userNameList = dayList.GroupBy(a => new { a.YBUserName }).ToList();
                //遍历当前天得到每个人数据
                foreach (var userName in userNameList)
                {
                    //当前天当前预报员
                    string name = userName.Key.YBUserName;
                    List<BwYbs> dayPersonList = dayList.Where(a => a.YBUserName == name).OrderBy(a => a.CountryCode).ToList();



                    List<Score_Day> scorePersonList = new List<Score_Day>();

                    //遍历每个人的数据得到每个人每天某个城市的数据
                    foreach (BwYbs dayPerson in dayPersonList)
                    {
                        DateTime currentDay = DateTime.Parse(((DateTime)dayPerson.YBDateTime).ToShortDateString());
                        DateTime secondDay = currentDay.AddDays(1);
                        DateTime thirdDay = currentDay.AddDays(2);
                        DateTime thirsdDay = currentDay.AddDays(3);


                        Score_Day scoreDay = new Score_Day();
                        scoreDay.YBUserName = dayPerson.YBUserName;
                        scoreDay.YBUserID = dayPerson.YBUserID;
                        scoreDay.YBDate = currentDay;
                        scoreDay.YBTime = "20时";
                        scoreDay.CreateTime = DateTime.Now;
                        scoreDay.AllTotal = 1;//只要有站 该值就为1



                        LiveData liveData24 = liveList.Where(a => a.FDate == secondDay && a.CountryName == dayPerson.CountryName).FirstOrDefault();
                        LiveData liveData48 = liveList.Where(a => a.FDate == thirdDay && a.CountryName == dayPerson.CountryName).FirstOrDefault();
                        LiveData liveData72 = liveList.Where(a => a.FDate == thirsdDay && a.CountryName == dayPerson.CountryName).FirstOrDefault();

                        /////////////////温度
                        //24小时温度
                        decimal maxTempSpan24 = Math.Abs((decimal)dayPerson.MaxTemp24 - (decimal)liveData24.MaxTemp);
                        if (maxTempSpan24 <= 2)
                        {
                            scoreDay.MaxTemp24 = 1;
                        }
                        else
                        {
                            scoreDay.MaxTemp24 = 0;

                        }
                        decimal minTempSpan24 = Math.Abs((decimal)dayPerson.MinTemp24 - (decimal)liveData24.MinTemp);
                        if (minTempSpan24 <= 2)
                        {
                            scoreDay.MinTemp24 = 1;
                        }
                        else
                        {
                            scoreDay.MinTemp24 = 0;

                        }
                        //48小时温度
                        decimal maxTempSpan48 = Math.Abs((decimal)dayPerson.MaxTemp48 - (decimal)liveData48.MaxTemp);
                        if (maxTempSpan48 <= 2)
                        {
                            scoreDay.MaxTemp48 = 1;
                        }
                        else
                        {
                            scoreDay.MaxTemp48 = 0;

                        }
                        decimal minTempSpan48 = Math.Abs((decimal)dayPerson.MinTemp48 - (decimal)liveData48.MinTemp);
                        if (minTempSpan48 <= 2)
                        {
                            scoreDay.MinTemp48 = 1;
                        }
                        else
                        {
                            scoreDay.MinTemp48 = 0;

                        }
                        //72小时温度
                        decimal maxTempSpan72 = Math.Abs((decimal)dayPerson.MaxTemp72 - (decimal)liveData72.MaxTemp);
                        if (maxTempSpan72 <= 2)
                        {
                            scoreDay.MaxTemp72 = 1;
                        }
                        else
                        {
                            scoreDay.MaxTemp72 = 0;

                        }
                        decimal minTempSpan72 = Math.Abs((decimal)dayPerson.MinTemp72 - (decimal)liveData72.MinTemp);
                        if (minTempSpan72 <= 2)
                        {
                            scoreDay.MinTemp72 = 1;
                        }
                        else
                        {
                            scoreDay.MinTemp72 = 0;

                        }
                        //当天该城市的24小时实况雨量
                        double liveRain24 = (double)liveData24.Rain;

                        //当天该城市的48小时实况
                        double liveRain48 = (double)liveData48.Rain;
                        //当天该城市的72小时实况
                        double liveRain72 = (double)liveData72.Rain;

                        #region 晴雨预报判断
                        ///晴雨预报判断
                        /// 24小时
                        WeatherDictionary wd12 = wdList.Where(a => a.Code == dayPerson.TianQiCode12).FirstOrDefault();
                        WeatherDictionary wd24 = wdList.Where(a => a.Code == dayPerson.TianQiCode12).FirstOrDefault();
                        WeatherDictionary trueWd24 = (decimal)wd12.Code <= (decimal)wd24.Code ? wd12 : wd24;



                        if (trueWd24.Priority < 11)
                        {
                            //如果有雨
                            if ((double)liveRain24 <= 0.01)
                            {
                                //1
                                scoreDay.RainShine24 = 1;
                            }
                            else
                            {
                                //0
                                scoreDay.RainShine24 = 1;
                            }
                        }
                        else
                        {
                            //如果为晴
                            if (liveRain24 <= 0.01)
                            {
                                //1
                                scoreDay.RainShine24 = 1;
                            }
                            else
                            {
                                //0
                                scoreDay.RainShine24 = 1;
                            }
                        }
                        ///晴雨预报判断
                        /// 48小时
                        WeatherDictionary wd36 = wdList.Where(a => a.Code == dayPerson.TianQiCode12).FirstOrDefault();
                        WeatherDictionary wd48 = wdList.Where(a => a.Code == dayPerson.TianQiCode12).FirstOrDefault();
                        WeatherDictionary trueWd48 = (decimal)wd12.Code <= (decimal)wd24.Code ? wd12 : wd24;




                        if (trueWd48.Priority < 11)
                        {
                            //如果有雨
                            if (liveRain48 <= 0.01)
                            {
                                //1
                                scoreDay.RainShine48 = 1;
                            }
                            else
                            {
                                //0
                                scoreDay.RainShine48 = 1;
                            }
                        }
                        else
                        {
                            //如果为晴
                            if ((double)liveRain48 <= 0.01)
                            {
                                //1
                                scoreDay.RainShine48 = 1;
                            }
                            else
                            {
                                //0
                                scoreDay.RainShine48 = 1;
                            }
                        }

                        ///晴雨预报判断
                        /// 72小时
                        WeatherDictionary wd60 = wdList.Where(a => a.Code == dayPerson.TianQiCode12).FirstOrDefault();
                        WeatherDictionary wd72 = wdList.Where(a => a.Code == dayPerson.TianQiCode12).FirstOrDefault();
                        WeatherDictionary trueWd72 = (decimal)wd12.Code <= (decimal)wd24.Code ? wd12 : wd24;




                        if (trueWd72.Priority < 11)
                        {
                            //如果有雨
                            if ((double)liveRain72 <= 0.01)
                            {
                                //1
                                scoreDay.RainShine72 = 1;
                            }
                            else
                            {
                                //0
                                scoreDay.RainShine72 = 1;
                            }
                        }
                        else
                        {
                            //如果为晴
                            if ((double)liveRain72 <= 0.01)
                            {
                                //1
                                scoreDay.RainShine72 = 1;
                            }
                            else
                            {
                                //0
                                scoreDay.RainShine72 = 1;
                            }
                        }
                        #endregion

                        #region 一般性降水

                        //一般性降水
                        #region 24小时一般性降水
                        //24小时
                        if (trueWd24.Code == 10 || trueWd24.Code == 11 || trueWd24.Code == 12 ||
                            trueWd24.Code == 17 || trueWd24.Code == 24 || trueWd24.Code == 25)
                        {
                            //NaN
                            scoreDay.Rainfall24 = null;
                            scoreDay.Rainfall24Total = null;
                        }
                        else if (trueWd24.Code == 23)
                        {
                            if (liveRain24 <= 37.9)
                            {
                                //NaN
                                scoreDay.Rainfall24 = null;
                                scoreDay.Rainfall24Total = null;
                            }
                            else if (liveRain24 > 37.9 && liveRain24 < 50.0)
                            {
                                //1
                                scoreDay.Rainfall24 = 1;
                                scoreDay.Rainfall24Total = 1;
                            }
                            else // >=50.0
                            {
                                //NaN
                                scoreDay.Rainfall24 = null;
                                scoreDay.Rainfall24Total = null;
                            }
                        }
                        else if (trueWd24.Code == 28)
                        {
                            if (liveRain24 <= 7.4)
                            {
                                //NaN
                                scoreDay.Rainfall24 = null;
                                scoreDay.Rainfall24Total = null;
                            }
                            else if (liveRain24 > 7.4 && liveRain24 < 10.0)
                            {
                                //1
                                scoreDay.Rainfall24 = 1;
                                scoreDay.Rainfall24Total = 1;
                            }
                            else // >=10.0
                            {
                                //NaN
                                scoreDay.Rainfall24 = null;
                                scoreDay.Rainfall24Total = null;
                            }
                        }
                        else if (trueWd24.CodeNew == "rain")
                        {
                            if (liveRain24 <= 0.01)
                            {
                                //0
                                scoreDay.Rainfall24 = 0;
                                scoreDay.Rainfall24Total = 1;
                            }
                            else if (liveRain24 > 0.01 && liveRain24 < 50.0)
                            {
                                //1
                                scoreDay.Rainfall24 = 1;
                                scoreDay.Rainfall24Total = 1;
                            }
                            else // >=50.0
                            {
                                //NaN
                                scoreDay.Rainfall24 = null;
                                scoreDay.Rainfall24Total = null;
                            }
                        }
                        else if (trueWd24.CodeNew == "snow")
                        {
                            if (liveRain24 <= 0.01)
                            {
                                //0
                                scoreDay.Rainfall24 = 0;
                                scoreDay.Rainfall24Total = 1;
                            }
                            else if (liveRain24 > 0.01 && liveRain24 < 10.0)
                            {
                                //1
                                scoreDay.Rainfall24 = 1;
                                scoreDay.Rainfall24Total = 1;
                            }
                            else // >=10.0
                            {
                                //NaN
                                scoreDay.Rainfall24 = null;
                                scoreDay.Rainfall24Total = null;
                            }
                        }
                        else if (trueWd24.CodeNew == "sunny")
                        {
                            if (liveRain24 <= 0.01)
                            {
                                //NaN
                                scoreDay.Rainfall24 = null;
                                scoreDay.Rainfall24Total = null;
                            }
                            else if (liveRain24 > 0.01 && liveRain24 < 50.0)
                            {
                                //1
                                scoreDay.Rainfall24 = 1;
                                scoreDay.Rainfall24Total = 1;
                            }
                            else // >=50.0
                            {
                                //NaN
                                scoreDay.Rainfall24 = null;
                                scoreDay.Rainfall24Total = null;
                            }
                        }
                        #endregion 24小时一般性降水


                        #region 48小时一般性降水
                        //48小时
                        if (trueWd48.Code == 10 || trueWd48.Code == 11 || trueWd48.Code == 12 ||
                            trueWd48.Code == 17 || trueWd48.Code == 24 || trueWd48.Code == 25)
                        {
                            //NaN
                            scoreDay.Rainfall48 = null;
                            scoreDay.Rainfall48Total = null;
                        }
                        else if (trueWd48.Code == 23)
                        {
                            if (liveRain48 <= 37.9)
                            {
                                //NaN
                                scoreDay.Rainfall48 = null;
                                scoreDay.Rainfall48Total = null;
                            }
                            else if (liveRain48 > 37.9 && liveRain48 < 50.0)
                            {
                                //1
                                scoreDay.Rainfall48 = 1;
                                scoreDay.Rainfall48Total = 1;
                            }
                            else // >=50.0
                            {
                                //NaN
                                scoreDay.Rainfall48 = null;
                                scoreDay.Rainfall48Total = null;
                            }
                        }
                        else if (trueWd48.Code == 28)
                        {
                            if (liveRain48 <= 7.4)
                            {
                                //NaN
                                scoreDay.Rainfall48 = null;
                                scoreDay.Rainfall48Total = null;
                            }
                            else if (liveRain48 > 7.4 && liveRain48 < 10.0)
                            {
                                //1
                                scoreDay.Rainfall48 = 1;
                                scoreDay.Rainfall48Total = 1;
                            }
                            else // >=10.0
                            {
                                //NaN
                                scoreDay.Rainfall48 = null;
                                scoreDay.Rainfall48Total = null;
                            }
                        }
                        else if (trueWd48.CodeNew == "rain")
                        {
                            if (liveRain48 <= 0.01)
                            {
                                //0
                                scoreDay.Rainfall48 = 0;
                                scoreDay.Rainfall48Total = 1;
                            }
                            else if (liveRain48 > 0.01 && liveRain48 < 50.0)
                            {
                                //1
                                scoreDay.Rainfall48 = 1;
                                scoreDay.Rainfall48Total = 1;
                            }
                            else // >=50.0
                            {
                                //NaN
                                scoreDay.Rainfall48 = null;
                                scoreDay.Rainfall48Total = null;
                            }
                        }
                        else if (trueWd48.CodeNew == "snow")
                        {
                            if (liveRain48 <= 0.01)
                            {
                                //0
                                scoreDay.Rainfall48 = 0;
                                scoreDay.Rainfall48Total = 1;
                            }
                            else if (liveRain48 > 0.01 && liveRain48 < 10.0)
                            {
                                //1
                                scoreDay.Rainfall48 = 1;
                                scoreDay.Rainfall48Total = 1;
                            }
                            else // >=10.0
                            {
                                //NaN
                                scoreDay.Rainfall48 = null;
                                scoreDay.Rainfall48Total = null;
                            }
                        }
                        else if (trueWd48.CodeNew == "sunny")
                        {
                            if (liveRain48 <= 0.01)
                            {
                                //NaN
                                scoreDay.Rainfall48 = null;
                                scoreDay.Rainfall48Total = null;
                            }
                            else if (liveRain48 > 0.01 && liveRain48 < 50.0)
                            {
                                //1
                                scoreDay.Rainfall48 = 1;
                                scoreDay.Rainfall48Total = 1;
                            }
                            else // >=50.0
                            {
                                //NaN
                                scoreDay.Rainfall48 = null;
                                scoreDay.Rainfall48Total = null;
                            }
                        }
                        #endregion  48小时一般性降水


                        #region 72小时一般性降水
                        //72小时
                        if (trueWd72.Code == 10 || trueWd72.Code == 11 || trueWd72.Code == 12 ||
                            trueWd72.Code == 17 || trueWd72.Code == 24 || trueWd72.Code == 25)
                        {
                            //NaN
                            scoreDay.Rainfall72 = null;
                            scoreDay.Rainfall72Total = null;
                        }
                        else if (trueWd72.Code == 23)
                        {
                            if (liveRain72 <= 37.9)
                            {
                                //NaN
                                scoreDay.Rainfall72 = null;
                                scoreDay.Rainfall72Total = null;
                            }
                            else if (liveRain72 > 37.9 && liveRain72 < 50.0)
                            {
                                //1
                                scoreDay.Rainfall72 = 1;
                                scoreDay.Rainfall72Total = 1;
                            }
                            else // >=50.0
                            {
                                //NaN
                                scoreDay.Rainfall72 = null;
                                scoreDay.Rainfall72Total = null;
                            }
                        }
                        else if (trueWd72.Code == 28)
                        {
                            if (liveRain72 <= 7.4)
                            {
                                //NaN
                                scoreDay.Rainfall72 = null;
                                scoreDay.Rainfall72Total = null;
                            }
                            else if (liveRain72 > 7.4 && liveRain72 < 10.0)
                            {
                                //1
                                scoreDay.Rainfall72 = 1;
                                scoreDay.Rainfall72Total = 1;
                            }
                            else // >=10.0
                            {
                                //NaN
                                scoreDay.Rainfall72 = null;
                                scoreDay.Rainfall72Total = null;
                            }
                        }
                        else if (trueWd72.CodeNew == "rain")
                        {
                            if (liveRain72 <= 0.01)
                            {
                                //0
                                scoreDay.Rainfall72 = 0;
                                scoreDay.Rainfall72Total = 1;
                            }
                            else if (liveRain72 > 0.01 && liveRain72 < 50.0)
                            {
                                //1
                                scoreDay.Rainfall72 = 1;
                                scoreDay.Rainfall72Total = 1;
                            }
                            else // >=50.0
                            {
                                //NaN
                                scoreDay.Rainfall72 = null;
                                scoreDay.Rainfall72Total = null;
                            }
                        }
                        else if (trueWd72.CodeNew == "snow")
                        {
                            if (liveRain72 <= 0.01)
                            {
                                //0
                                scoreDay.Rainfall72 = 0;
                                scoreDay.Rainfall72Total = 1;
                            }
                            else if (liveRain72 > 0.01 && liveRain72 < 10.0)
                            {
                                //1
                                scoreDay.Rainfall72 = 1;
                                scoreDay.Rainfall72Total = 1;
                            }
                            else // >=10.0
                            {
                                //NaN
                                scoreDay.Rainfall72 = null;
                                scoreDay.Rainfall72Total = null;
                            }
                        }
                        else if (trueWd72.CodeNew == "sunny")
                        {
                            if (liveRain72 <= 0.01)
                            {
                                //NaN
                                scoreDay.Rainfall72 = null;
                                scoreDay.Rainfall72Total = null;
                            }
                            else if (liveRain72 > 0.01 && liveRain72 < 50.0)
                            {
                                //1
                                scoreDay.Rainfall72 = 1;
                                scoreDay.Rainfall72Total = 1;
                            }
                            else // >=50.0
                            {
                                //NaN
                                scoreDay.Rainfall72 = null;
                                scoreDay.Rainfall72Total = null;
                            }
                        }
                        #endregion  72小时一般性降水

                        #endregion

                        #region 暴雨（雪）预报判断
                        #region 24小时暴雨（雪）预报判断
                        //24小时
                        if (trueWd24.Code == 17)
                        {
                            if (liveRain24 < 10.0)
                            {
                                //0
                                scoreDay.Rainstorm24 = 0;
                                scoreDay.Rainstorm24Total = 1;
                            }
                            else //>=10.0
                            {
                                //1
                                scoreDay.Rainstorm24 = 1;
                                scoreDay.Rainstorm24Total = 1;
                            }
                        }
                        else if (trueWd24.Code == 28)
                        {
                            if (liveRain24 <= 7.4)
                            {
                                //0
                                scoreDay.Rainstorm24 = 0;
                                scoreDay.Rainstorm24Total = 1;
                            }
                            else if (liveRain24 > 7.4 && liveRain24 < 10.0)
                            {
                                //NaN
                                scoreDay.Rainstorm24 = null;
                                scoreDay.Rainstorm24Total = null;
                            }
                            else //>=10
                            {
                                //1
                                scoreDay.Rainstorm24 = 1;
                                scoreDay.Rainstorm24Total = 1;
                            }
                        }
                        else if (trueWd24.Code == 12)
                        {
                            if (liveRain24 < 250.0)
                            {
                                //0
                                scoreDay.Rainstorm24 = 0;
                                scoreDay.Rainstorm24Total = 1;
                            }
                            else //>=250.0
                            {
                                //1
                                scoreDay.Rainstorm24 = 1;
                                scoreDay.Rainstorm24Total = 1;
                            }
                        }
                        else if (trueWd24.Code == 25)
                        {
                            if (liveRain24 >= 175.0 && liveRain24 <= 299.9)
                            {
                                //1
                                scoreDay.Rainstorm24 = 1;
                                scoreDay.Rainstorm24Total = 1;
                            }
                            else
                            {
                                //0
                                scoreDay.Rainstorm24 = 0;
                                scoreDay.Rainstorm24Total = 1;
                            }
                        }
                        else if (trueWd24.Code == 11)
                        {
                            if (liveRain24 >= 100.0 && liveRain24 <= 249.9)
                            {
                                //1
                                scoreDay.Rainstorm24 = 1;
                                scoreDay.Rainstorm24Total = 1;
                            }
                            else
                            {
                                //0
                                scoreDay.Rainstorm24 = 0;
                                scoreDay.Rainstorm24Total = 1;
                            }
                        }
                        else if (trueWd24.Code == 24)
                        {
                            if (liveRain24 >= 75.0 && liveRain24 <= 174.9)
                            {
                                //1
                                scoreDay.Rainstorm24 = 1;
                                scoreDay.Rainstorm24Total = 1;
                            }
                            else
                            {
                                //0
                                scoreDay.Rainstorm24 = 0;
                                scoreDay.Rainstorm24Total = 1;
                            }
                        }
                        else if (trueWd24.Code == 10)
                        {
                            if (liveRain24 >= 50.0 && liveRain24 <= 99.9)
                            {
                                //1
                                scoreDay.Rainstorm24 = 1;
                                scoreDay.Rainstorm24Total = 1;
                            }
                            else
                            {
                                //0
                                scoreDay.Rainstorm24 = 0;
                                scoreDay.Rainstorm24Total = 1;
                            }
                        }
                        else if (trueWd24.Code == 23)
                        {
                            if (liveRain24 <= 37.9)
                            {
                                //0
                                scoreDay.Rainstorm24 = 0;
                                scoreDay.Rainstorm24Total = 1;
                            }
                            else if (liveRain24 > 37.9 && liveRain24 < 50.0)
                            {
                                //NaN
                                scoreDay.Rainstorm24 = null;
                                scoreDay.Rainstorm24Total = null;
                            }
                            else if (liveRain24 >= 50.0 && liveRain24 < 75.0)
                            {
                                //1
                                scoreDay.Rainstorm24 = 1;
                                scoreDay.Rainstorm24Total = 1;
                            }
                            else //>=75.0
                            {
                                //0
                                scoreDay.Rainstorm24 = 0;
                                scoreDay.Rainstorm24Total = 1;
                            }
                        }
                        else if (trueWd24.CodeNew == "rain")
                        {
                            if (liveRain24 < 50.0)
                            {
                                //NaN
                                scoreDay.Rainstorm24 = null;
                                scoreDay.Rainstorm24Total = null;
                            }
                            else //>=50.0
                            {
                                //0
                                scoreDay.Rainstorm24 = 0;
                                scoreDay.Rainstorm24Total = 1;
                            }
                        }
                        else if (trueWd24.CodeNew == "snow")
                        {
                            if (liveRain24 < 10.0)
                            {
                                //NaN
                                scoreDay.Rainstorm24 = null;
                                scoreDay.Rainstorm24Total = null;
                            }
                            else //>=10.0
                            {
                                //0
                                scoreDay.Rainstorm24 = 0;
                                scoreDay.Rainstorm24Total = 1;
                            }
                        }
                        else if (trueWd24.CodeNew == "sunny")
                        {
                            if (liveRain24 < 50.0)
                            {
                                //NaN
                                scoreDay.Rainstorm24 = null;
                                scoreDay.Rainstorm24Total = null;
                            }
                            else //>=50.0
                            {
                                //0
                                scoreDay.Rainstorm24 = 0;
                                scoreDay.Rainstorm24Total = 1;
                            }
                        }
                        else
                        {
                            scoreDay.Rainstorm24 = null;
                            scoreDay.Rainstorm24Total = null;
                        }
                        #endregion 24小时暴雨（雪）预报判断


                        #region 48小时暴雨（雪）预报判断
                        //48小时
                        if (trueWd48.Code == 17)
                        {
                            if (liveRain48 < 10.0)
                            {
                                //0
                                scoreDay.Rainstorm48 = 0;
                                scoreDay.Rainstorm48Total = 1;
                            }
                            else //>=10.0
                            {
                                //1
                                scoreDay.Rainstorm48 = 1;
                                scoreDay.Rainstorm48Total = 1;
                            }
                        }
                        else if (trueWd48.Code == 28)
                        {
                            if (liveRain48 <= 7.4)
                            {
                                //0
                                scoreDay.Rainstorm48 = null;
                                scoreDay.Rainstorm48Total = 1;
                            }
                            else if (liveRain48 > 7.4 && liveRain48 < 10.0)
                            {
                                //NaN
                                scoreDay.Rainstorm48 = null;
                                scoreDay.Rainstorm48Total = null;
                            }
                            else //>=10
                            {
                                //1
                                scoreDay.Rainstorm48 = 1;
                                scoreDay.Rainstorm48Total = 1;
                            }
                        }
                        else if (trueWd48.Code == 12)
                        {
                            if (liveRain48 < 250.0)
                            {
                                //0
                                scoreDay.Rainstorm48 = 0;
                                scoreDay.Rainstorm48Total = 1;
                            }
                            else //>=250.0
                            {
                                //1
                                scoreDay.Rainstorm48 = 1;
                                scoreDay.Rainstorm48Total = 1;
                            }
                        }
                        else if (trueWd48.Code == 25)
                        {
                            if (liveRain48 >= 175.0 && liveRain48 <= 299.9)
                            {
                                //1
                                scoreDay.Rainstorm48 = 1;
                                scoreDay.Rainstorm48Total = 1;
                            }
                            else
                            {
                                //0
                                scoreDay.Rainstorm48 = 0;
                                scoreDay.Rainstorm48Total = 1;
                            }
                        }
                        else if (trueWd48.Code == 11)
                        {
                            if (liveRain48 >= 100.0 && liveRain48 <= 249.9)
                            {
                                //1
                                scoreDay.Rainstorm48 = 1;
                                scoreDay.Rainstorm48Total = 1;
                            }
                            else
                            {
                                //0
                                scoreDay.Rainstorm48 = 0;
                                scoreDay.Rainstorm48Total = 1;
                            }
                        }
                        else if (trueWd48.Code == 24)
                        {
                            if (liveRain48 >= 75.0 && liveRain48 <= 174.9)
                            {
                                //1
                                scoreDay.Rainstorm48 = 1;
                                scoreDay.Rainstorm48Total = 1;
                            }
                            else
                            {
                                //0
                                scoreDay.Rainstorm48 = 0;
                                scoreDay.Rainstorm48Total = 1;
                            }
                        }
                        else if (trueWd48.Code == 10)
                        {
                            if (liveRain48 >= 50.0 && liveRain48 <= 99.9)
                            {
                                //1
                                scoreDay.Rainstorm48 = 1;
                                scoreDay.Rainstorm48Total = 1;
                            }
                            else
                            {
                                //0
                                scoreDay.Rainstorm48 = 0;
                                scoreDay.Rainstorm48Total = 1;
                            }
                        }
                        else if (trueWd48.Code == 23)
                        {
                            if (liveRain48 <= 37.9)
                            {
                                //0
                                scoreDay.Rainstorm48 = 0;
                                scoreDay.Rainstorm48Total = 1;
                            }
                            else if (liveRain48 > 37.9 && liveRain48 < 50.0)
                            {
                                //NaN
                                scoreDay.Rainstorm48 = null;
                                scoreDay.Rainstorm48Total = null;
                            }
                            else if (liveRain48 >= 50.0 && liveRain48 < 75.0)
                            {
                                //1
                                scoreDay.Rainstorm48 = 1;
                                scoreDay.Rainstorm48Total = 1;
                            }
                            else //>=75.0
                            {
                                //0
                                scoreDay.Rainstorm48 = 0;
                                scoreDay.Rainstorm48Total = 1;
                            }
                        }
                        else if (trueWd48.CodeNew == "rain")
                        {
                            if (liveRain48 < 50.0)
                            {
                                //NaN
                                scoreDay.Rainstorm48 = null;
                                scoreDay.Rainstorm48Total = null;
                            }
                            else //>=50.0
                            {
                                //0
                                scoreDay.Rainstorm48 = 0;
                                scoreDay.Rainstorm48Total = 1;
                            }
                        }
                        else if (trueWd48.CodeNew == "snow")
                        {
                            if (liveRain48 < 10.0)
                            {
                                //NaN
                                scoreDay.Rainstorm48 = null;
                                scoreDay.Rainstorm48Total = null;
                            }
                            else //>=10.0
                            {
                                //0
                                scoreDay.Rainstorm48 = 0;
                                scoreDay.Rainstorm48Total = 1;
                            }
                        }
                        else if (trueWd48.CodeNew == "sunny")
                        {
                            if (liveRain48 < 50.0)
                            {
                                //NaN
                                scoreDay.Rainstorm48 = null;
                                scoreDay.Rainstorm48Total = null;
                            }
                            else //>=50.0
                            {
                                //0
                                scoreDay.Rainstorm48 = 0;
                                scoreDay.Rainstorm48Total = 1;
                            }
                        }
                        else
                        {
                            scoreDay.Rainstorm48 = null;
                            scoreDay.Rainstorm48Total = null;
                        }

                        #endregion 48小时暴雨（雪）预报判断


                        #endregion

                        scorePersonList.Add(scoreDay);
                    }

                    Score_Day scorePerson = new Score_Day();
                    scorePerson.ScoreID = Guid.NewGuid();
                    scorePerson.YBDate = scorePersonList.FirstOrDefault().YBDate;
                    scorePerson.YBTime = scorePersonList.FirstOrDefault().YBTime;
                    scorePerson.YBUserName = scorePersonList.FirstOrDefault().YBUserName;
                    scorePerson.YBUserID = scorePersonList.FirstOrDefault().YBUserID;
                    scorePerson.CreateTime = DateTime.Now;



                    scorePerson.MaxTemp24 = scorePersonList.Where(a => a.MaxTemp24 != null).Count() == 0 ? null : scorePersonList.Sum(a => a.MaxTemp24);
                    scorePerson.MinTemp24 = scorePersonList.Where(a => a.MinTemp24 != null).Count() == 0 ? null : scorePersonList.Sum(a => a.MinTemp24);
                    scorePerson.MaxTemp48 = scorePersonList.Where(a => a.MaxTemp48 != null).Count() == 0 ? null : scorePersonList.Sum(a => a.MaxTemp48);
                    scorePerson.MinTemp48 = scorePersonList.Where(a => a.MinTemp48 != null).Count() == 0 ? null : scorePersonList.Sum(a => a.MinTemp48);
                    scorePerson.MaxTemp72 = scorePersonList.Where(a => a.MaxTemp72 != null).Count() == 0 ? null : scorePersonList.Sum(a => a.MaxTemp72);
                    scorePerson.MinTemp72 = scorePersonList.Where(a => a.MinTemp72 != null).Count() == 0 ? null : scorePersonList.Sum(a => a.MinTemp72);
                    scorePerson.RainShine24 = scorePersonList.Where(a => a.RainShine24 != null).Count() == 0 ? null : scorePersonList.Sum(a => a.RainShine24);
                    scorePerson.RainShine48 = scorePersonList.Where(a => a.RainShine48 != null).Count() == 0 ? null : scorePersonList.Sum(a => a.RainShine48);
                    scorePerson.RainShine72 = scorePersonList.Where(a => a.RainShine72 != null).Count() == 0 ? null : scorePersonList.Sum(a => a.RainShine72);
                    scorePerson.AllTotal = scorePersonList.Where(a => a.AllTotal != null).Count() == 0 ? null : scorePersonList.Sum(a => a.AllTotal);
                    scorePerson.Rainfall24 = scorePersonList.Where(a => a.Rainfall24 != null).Count() == 0 ? null : scorePersonList.Sum(a => a.Rainfall24);
                    scorePerson.Rainfall24Total = scorePersonList.Where(a => a.Rainfall24Total != null).Count() == 0 ? null : scorePersonList.Sum(a => a.Rainfall24Total);
                    scorePerson.Rainfall48 = scorePersonList.Where(a => a.Rainfall48 != null).Count() == 0 ? null : scorePersonList.Sum(a => a.Rainfall48);
                    scorePerson.Rainfall48Total = scorePersonList.Where(a => a.Rainfall48Total != null).Count() == 0 ? null : scorePersonList.Sum(a => a.Rainfall48Total);
                    scorePerson.Rainfall72 = scorePersonList.Where(a => a.Rainfall72 != null).Count() == 0 ? null : scorePersonList.Sum(a => a.Rainfall72);
                    scorePerson.Rainfall72Total = scorePersonList.Where(a => a.Rainfall72Total != null).Count() == 0 ? null : scorePersonList.Sum(a => a.Rainfall72Total);
                    scorePerson.Rainstorm24 = scorePersonList.Where(a => a.Rainstorm24 != null).Count() == 0 ? null : scorePersonList.Sum(a => a.Rainstorm24);
                    scorePerson.Rainstorm24Total = scorePersonList.Where(a => a.Rainstorm24Total != null).Count() == 0 ? null : scorePersonList.Sum(a => a.Rainstorm24Total);
                    scorePerson.Rainstorm48 = scorePersonList.Where(a => a.Rainstorm48 != null).Count() == 0 ? null : scorePersonList.Sum(a => a.Rainstorm48);
                    scorePerson.Rainstorm48Total = scorePersonList.Where(a => a.Rainstorm48Total != null).Count() == 0 ? null : scorePersonList.Sum(a => a.Rainstorm48Total);

                    dal.Add(scorePerson, false);
                }

                dal.SaveChange();
            }

        }

        public void Caculate08(DateTime startDate, DateTime endDate)
        {

            dal.Delete(a =>a.YBTime=="08时"&& a.YBDate >= startDate && a.YBDate <= endDate);

            DateTime preDate = endDate.AddDays(2);

            List<LiveData> liveList =
                new LiveData_BLL().GetList(a => a.FDate >= startDate && a.FDate <= preDate && a.Category == "08时")
                    .ToList();
            BwYbs_BLL bwYbsBll = new BwYbs_BLL();
            List<BwYbs> list = bwYbsBll.GetList(a => a.YBDateTime >= startDate && a.YBDateTime <= endDate && ((DateTime)a.YBDateTime).Hour == 6).ToList();
            List<WeatherDictionary> wdList = new WeatherDictionary_BLL().GetList(a => a.Type == "天气").ToList();
            TimeSpan timeSpan = endDate - startDate;//时间跨度

            for (int i = 0; i < timeSpan.Days; i++)
            {
                //当前天时间
                DateTime dayTime = DateTime.Parse(startDate.AddDays(i).ToShortDateString() + " 06:45");
                //当前天 所有人的数据
                List<BwYbs> dayList = list.Where(a => a.YBDateTime == dayTime).ToList();
                //当前天 所有人的名字集合
                var userNameList = dayList.GroupBy(a => new { a.YBUserName }).ToList();
                //遍历当前天得到每个人数据
                foreach (var userName in userNameList)
                {
                    //当前天当前预报员
                    string name = userName.Key.YBUserName;
                    List<BwYbs> dayPersonList = dayList.Where(a => a.YBUserName == name).OrderBy(a => a.CountryCode).ToList();



                    List<Score_Day> scorePersonList = new List<Score_Day>();

                    //遍历每个人的数据得到每个人每天某个城市的数据
                    foreach (BwYbs dayPerson in dayPersonList)
                    {
                        DateTime currentDay = DateTime.Parse(((DateTime)dayPerson.YBDateTime).ToShortDateString());
                        DateTime secondDay = currentDay.AddDays(1);
                        DateTime thirdDay = currentDay.AddDays(2);
                        DateTime thirsdDay = currentDay.AddDays(3);


                        Score_Day scoreDay = new Score_Day();
                        scoreDay.YBUserName = dayPerson.YBUserName;
                        scoreDay.YBUserID = dayPerson.YBUserID;
                        scoreDay.YBDate = currentDay;
                        scoreDay.YBTime = "08时";
                        scoreDay.CreateTime = DateTime.Now;
                        scoreDay.AllTotal = 1;//只要有站 该值就为1



                        LiveData liveData24 = liveList.Where(a => a.FDate == currentDay && a.CountryName == dayPerson.CountryName).FirstOrDefault();//08时预报应该与当前比较
                        LiveData liveData48 = liveList.Where(a => a.FDate == secondDay && a.CountryName == dayPerson.CountryName).FirstOrDefault();
                        LiveData liveData72 = liveList.Where(a => a.FDate == thirdDay && a.CountryName == dayPerson.CountryName).FirstOrDefault();

                        /////////////////温度
                        //24小时温度
                        decimal maxTempSpan24 = Math.Abs((decimal)dayPerson.MaxTemp24 - (decimal)liveData24.MaxTemp);
                        if (maxTempSpan24 <= 2)
                        {
                            scoreDay.MaxTemp24 = 1;
                        }
                        else
                        {
                            scoreDay.MaxTemp24 = 0;

                        }
                        decimal minTempSpan24 = Math.Abs((decimal)dayPerson.MinTemp24 - (decimal)liveData24.MinTemp);
                        if (minTempSpan24 <= 2)
                        {
                            scoreDay.MinTemp24 = 1;
                        }
                        else
                        {
                            scoreDay.MinTemp24 = 0;

                        }
                        //48小时温度
                        decimal maxTempSpan48 = Math.Abs((decimal)dayPerson.MaxTemp48 - (decimal)liveData48.MaxTemp);
                        if (maxTempSpan48 <= 2)
                        {
                            scoreDay.MaxTemp48 = 1;
                        }
                        else
                        {
                            scoreDay.MaxTemp48 = 0;

                        }
                        decimal minTempSpan48 = Math.Abs((decimal)dayPerson.MinTemp48 - (decimal)liveData48.MinTemp);
                        if (minTempSpan48 <= 2)
                        {
                            scoreDay.MinTemp48 = 1;
                        }
                        else
                        {
                            scoreDay.MinTemp48 = 0;

                        }
                        //72小时温度
                        decimal maxTempSpan72 = Math.Abs((decimal)dayPerson.MaxTemp72 - (decimal)liveData72.MaxTemp);
                        if (maxTempSpan72 <= 2)
                        {
                            scoreDay.MaxTemp72 = 1;
                        }
                        else
                        {
                            scoreDay.MaxTemp72 = 0;

                        }
                        decimal minTempSpan72 = Math.Abs((decimal)dayPerson.MinTemp72 - (decimal)liveData72.MinTemp);
                        if (minTempSpan72 <= 2)
                        {
                            scoreDay.MinTemp72 = 1;
                        }
                        else
                        {
                            scoreDay.MinTemp72 = 0;

                        }
                        //当天该城市的24小时实况雨量
                        double liveRain24 = (double)liveData24.Rain;

                        //当天该城市的48小时实况
                        double liveRain48 = (double)liveData48.Rain;
                        //当天该城市的72小时实况
                        double liveRain72 = (double)liveData72.Rain;

                        #region 晴雨预报判断
                        ///晴雨预报判断
                        /// 24小时
                        WeatherDictionary wd12 = wdList.Where(a => a.Code == dayPerson.TianQiCode12).FirstOrDefault();
                        WeatherDictionary wd24 = wdList.Where(a => a.Code == dayPerson.TianQiCode12).FirstOrDefault();
                        WeatherDictionary trueWd24 = (decimal)wd12.Code <= (decimal)wd24.Code ? wd12 : wd24;



                        if (trueWd24.Priority < 11)
                        {
                            //如果有雨
                            if ((double)liveRain24 <= 0.01)
                            {
                                //1
                                scoreDay.RainShine24 = 1;
                            }
                            else
                            {
                                //0
                                scoreDay.RainShine24 = 1;
                            }
                        }
                        else
                        {
                            //如果为晴
                            if (liveRain24 <= 0.01)
                            {
                                //1
                                scoreDay.RainShine24 = 1;
                            }
                            else
                            {
                                //0
                                scoreDay.RainShine24 = 1;
                            }
                        }
                        ///晴雨预报判断
                        /// 48小时
                        WeatherDictionary wd36 = wdList.Where(a => a.Code == dayPerson.TianQiCode12).FirstOrDefault();
                        WeatherDictionary wd48 = wdList.Where(a => a.Code == dayPerson.TianQiCode12).FirstOrDefault();
                        WeatherDictionary trueWd48 = (decimal)wd12.Code <= (decimal)wd24.Code ? wd12 : wd24;




                        if (trueWd48.Priority < 11)
                        {
                            //如果有雨
                            if (liveRain48 <= 0.01)
                            {
                                //1
                                scoreDay.RainShine48 = 1;
                            }
                            else
                            {
                                //0
                                scoreDay.RainShine48 = 1;
                            }
                        }
                        else
                        {
                            //如果为晴
                            if ((double)liveRain48 <= 0.01)
                            {
                                //1
                                scoreDay.RainShine48 = 1;
                            }
                            else
                            {
                                //0
                                scoreDay.RainShine48 = 1;
                            }
                        }

                        ///晴雨预报判断
                        /// 72小时
                        WeatherDictionary wd60 = wdList.Where(a => a.Code == dayPerson.TianQiCode12).FirstOrDefault();
                        WeatherDictionary wd72 = wdList.Where(a => a.Code == dayPerson.TianQiCode12).FirstOrDefault();
                        WeatherDictionary trueWd72 = (decimal)wd12.Code <= (decimal)wd24.Code ? wd12 : wd24;




                        if (trueWd72.Priority < 11)
                        {
                            //如果有雨
                            if ((double)liveRain72 <= 0.01)
                            {
                                //1
                                scoreDay.RainShine72 = 1;
                            }
                            else
                            {
                                //0
                                scoreDay.RainShine72 = 1;
                            }
                        }
                        else
                        {
                            //如果为晴
                            if ((double)liveRain72 <= 0.01)
                            {
                                //1
                                scoreDay.RainShine72 = 1;
                            }
                            else
                            {
                                //0
                                scoreDay.RainShine72 = 1;
                            }
                        }
                        #endregion

                        #region 一般性降水

                        //一般性降水
                        #region 24小时一般性降水
                        //24小时
                        if (trueWd24.Code == 10 || trueWd24.Code == 11 || trueWd24.Code == 12 ||
                            trueWd24.Code == 17 || trueWd24.Code == 24 || trueWd24.Code == 25)
                        {
                            //NaN
                            scoreDay.Rainfall24 = null;
                            scoreDay.Rainfall24Total = null;
                        }
                        else if (trueWd24.Code == 23)
                        {
                            if (liveRain24 <= 37.9)
                            {
                                //NaN
                                scoreDay.Rainfall24 = null;
                                scoreDay.Rainfall24Total = null;
                            }
                            else if (liveRain24 > 37.9 && liveRain24 < 50.0)
                            {
                                //1
                                scoreDay.Rainfall24 = 1;
                                scoreDay.Rainfall24Total = 1;
                            }
                            else // >=50.0
                            {
                                //NaN
                                scoreDay.Rainfall24 = null;
                                scoreDay.Rainfall24Total = null;
                            }
                        }
                        else if (trueWd24.Code == 28)
                        {
                            if (liveRain24 <= 7.4)
                            {
                                //NaN
                                scoreDay.Rainfall24 = null;
                                scoreDay.Rainfall24Total = null;
                            }
                            else if (liveRain24 > 7.4 && liveRain24 < 10.0)
                            {
                                //1
                                scoreDay.Rainfall24 = 1;
                                scoreDay.Rainfall24Total = 1;
                            }
                            else // >=10.0
                            {
                                //NaN
                                scoreDay.Rainfall24 = null;
                                scoreDay.Rainfall24Total = null;
                            }
                        }
                        else if (trueWd24.CodeNew == "rain")
                        {
                            if (liveRain24 <= 0.01)
                            {
                                //0
                                scoreDay.Rainfall24 = 0;
                                scoreDay.Rainfall24Total = 1;
                            }
                            else if (liveRain24 > 0.01 && liveRain24 < 50.0)
                            {
                                //1
                                scoreDay.Rainfall24 = 1;
                                scoreDay.Rainfall24Total = 1;
                            }
                            else // >=50.0
                            {
                                //NaN
                                scoreDay.Rainfall24 = null;
                                scoreDay.Rainfall24Total = null;
                            }
                        }
                        else if (trueWd24.CodeNew == "snow")
                        {
                            if (liveRain24 <= 0.01)
                            {
                                //0
                                scoreDay.Rainfall24 = 0;
                                scoreDay.Rainfall24Total = 1;
                            }
                            else if (liveRain24 > 0.01 && liveRain24 < 10.0)
                            {
                                //1
                                scoreDay.Rainfall24 = 1;
                                scoreDay.Rainfall24Total = 1;
                            }
                            else // >=10.0
                            {
                                //NaN
                                scoreDay.Rainfall24 = null;
                                scoreDay.Rainfall24Total = null;
                            }
                        }
                        else if (trueWd24.CodeNew == "sunny")
                        {
                            if (liveRain24 <= 0.01)
                            {
                                //NaN
                                scoreDay.Rainfall24 = null;
                                scoreDay.Rainfall24Total = null;
                            }
                            else if (liveRain24 > 0.01 && liveRain24 < 50.0)
                            {
                                //1
                                scoreDay.Rainfall24 = 1;
                                scoreDay.Rainfall24Total = 1;
                            }
                            else // >=50.0
                            {
                                //NaN
                                scoreDay.Rainfall24 = null;
                                scoreDay.Rainfall24Total = null;
                            }
                        }
                        #endregion 24小时一般性降水


                        #region 48小时一般性降水
                        //48小时
                        if (trueWd48.Code == 10 || trueWd48.Code == 11 || trueWd48.Code == 12 ||
                            trueWd48.Code == 17 || trueWd48.Code == 24 || trueWd48.Code == 25)
                        {
                            //NaN
                            scoreDay.Rainfall48 = null;
                            scoreDay.Rainfall48Total = null;
                        }
                        else if (trueWd48.Code == 23)
                        {
                            if (liveRain48 <= 37.9)
                            {
                                //NaN
                                scoreDay.Rainfall48 = null;
                                scoreDay.Rainfall48Total = null;
                            }
                            else if (liveRain48 > 37.9 && liveRain48 < 50.0)
                            {
                                //1
                                scoreDay.Rainfall48 = 1;
                                scoreDay.Rainfall48Total = 1;
                            }
                            else // >=50.0
                            {
                                //NaN
                                scoreDay.Rainfall48 = null;
                                scoreDay.Rainfall48Total = null;
                            }
                        }
                        else if (trueWd48.Code == 28)
                        {
                            if (liveRain48 <= 7.4)
                            {
                                //NaN
                                scoreDay.Rainfall48 = null;
                                scoreDay.Rainfall48Total = null;
                            }
                            else if (liveRain48 > 7.4 && liveRain48 < 10.0)
                            {
                                //1
                                scoreDay.Rainfall48 = 1;
                                scoreDay.Rainfall48Total = 1;
                            }
                            else // >=10.0
                            {
                                //NaN
                                scoreDay.Rainfall48 = null;
                                scoreDay.Rainfall48Total = null;
                            }
                        }
                        else if (trueWd48.CodeNew == "rain")
                        {
                            if (liveRain48 <= 0.01)
                            {
                                //0
                                scoreDay.Rainfall48 = 0;
                                scoreDay.Rainfall48Total = 1;
                            }
                            else if (liveRain48 > 0.01 && liveRain48 < 50.0)
                            {
                                //1
                                scoreDay.Rainfall48 = 1;
                                scoreDay.Rainfall48Total = 1;
                            }
                            else // >=50.0
                            {
                                //NaN
                                scoreDay.Rainfall48 = null;
                                scoreDay.Rainfall48Total = null;
                            }
                        }
                        else if (trueWd48.CodeNew == "snow")
                        {
                            if (liveRain48 <= 0.01)
                            {
                                //0
                                scoreDay.Rainfall48 = 0;
                                scoreDay.Rainfall48Total = 1;
                            }
                            else if (liveRain48 > 0.01 && liveRain48 < 10.0)
                            {
                                //1
                                scoreDay.Rainfall48 = 1;
                                scoreDay.Rainfall48Total = 1;
                            }
                            else // >=10.0
                            {
                                //NaN
                                scoreDay.Rainfall48 = null;
                                scoreDay.Rainfall48Total = null;
                            }
                        }
                        else if (trueWd48.CodeNew == "sunny")
                        {
                            if (liveRain48 <= 0.01)
                            {
                                //NaN
                                scoreDay.Rainfall48 = null;
                                scoreDay.Rainfall48Total = null;
                            }
                            else if (liveRain48 > 0.01 && liveRain48 < 50.0)
                            {
                                //1
                                scoreDay.Rainfall48 = 1;
                                scoreDay.Rainfall48Total = 1;
                            }
                            else // >=50.0
                            {
                                //NaN
                                scoreDay.Rainfall48 = null;
                                scoreDay.Rainfall48Total = null;
                            }
                        }
                        #endregion  48小时一般性降水


                        #region 72小时一般性降水
                        //72小时
                        if (trueWd72.Code == 10 || trueWd72.Code == 11 || trueWd72.Code == 12 ||
                            trueWd72.Code == 17 || trueWd72.Code == 24 || trueWd72.Code == 25)
                        {
                            //NaN
                            scoreDay.Rainfall72 = null;
                            scoreDay.Rainfall72Total = null;
                        }
                        else if (trueWd72.Code == 23)
                        {
                            if (liveRain72 <= 37.9)
                            {
                                //NaN
                                scoreDay.Rainfall72 = null;
                                scoreDay.Rainfall72Total = null;
                            }
                            else if (liveRain72 > 37.9 && liveRain72 < 50.0)
                            {
                                //1
                                scoreDay.Rainfall72 = 1;
                                scoreDay.Rainfall72Total = 1;
                            }
                            else // >=50.0
                            {
                                //NaN
                                scoreDay.Rainfall72 = null;
                                scoreDay.Rainfall72Total = null;
                            }
                        }
                        else if (trueWd72.Code == 28)
                        {
                            if (liveRain72 <= 7.4)
                            {
                                //NaN
                                scoreDay.Rainfall72 = null;
                                scoreDay.Rainfall72Total = null;
                            }
                            else if (liveRain72 > 7.4 && liveRain72 < 10.0)
                            {
                                //1
                                scoreDay.Rainfall72 = 1;
                                scoreDay.Rainfall72Total = 1;
                            }
                            else // >=10.0
                            {
                                //NaN
                                scoreDay.Rainfall72 = null;
                                scoreDay.Rainfall72Total = null;
                            }
                        }
                        else if (trueWd72.CodeNew == "rain")
                        {
                            if (liveRain72 <= 0.01)
                            {
                                //0
                                scoreDay.Rainfall72 = 0;
                                scoreDay.Rainfall72Total = 1;
                            }
                            else if (liveRain72 > 0.01 && liveRain72 < 50.0)
                            {
                                //1
                                scoreDay.Rainfall72 = 1;
                                scoreDay.Rainfall72Total = 1;
                            }
                            else // >=50.0
                            {
                                //NaN
                                scoreDay.Rainfall72 = null;
                                scoreDay.Rainfall72Total = null;
                            }
                        }
                        else if (trueWd72.CodeNew == "snow")
                        {
                            if (liveRain72 <= 0.01)
                            {
                                //0
                                scoreDay.Rainfall72 = 0;
                                scoreDay.Rainfall72Total = 1;
                            }
                            else if (liveRain72 > 0.01 && liveRain72 < 10.0)
                            {
                                //1
                                scoreDay.Rainfall72 = 1;
                                scoreDay.Rainfall72Total = 1;
                            }
                            else // >=10.0
                            {
                                //NaN
                                scoreDay.Rainfall72 = null;
                                scoreDay.Rainfall72Total = null;
                            }
                        }
                        else if (trueWd72.CodeNew == "sunny")
                        {
                            if (liveRain72 <= 0.01)
                            {
                                //NaN
                                scoreDay.Rainfall72 = null;
                                scoreDay.Rainfall72Total = null;
                            }
                            else if (liveRain72 > 0.01 && liveRain72 < 50.0)
                            {
                                //1
                                scoreDay.Rainfall72 = 1;
                                scoreDay.Rainfall72Total = 1;
                            }
                            else // >=50.0
                            {
                                //NaN
                                scoreDay.Rainfall72 = null;
                                scoreDay.Rainfall72Total = null;
                            }
                        }
                        #endregion  72小时一般性降水

                        #endregion

                        #region 暴雨（雪）预报判断
                        #region 24小时暴雨（雪）预报判断
                        //24小时
                        if (trueWd24.Code == 17)
                        {
                            if (liveRain24 < 10.0)
                            {
                                //0
                                scoreDay.Rainstorm24 = 0;
                                scoreDay.Rainstorm24Total = 1;
                            }
                            else //>=10.0
                            {
                                //1
                                scoreDay.Rainstorm24 = 1;
                                scoreDay.Rainstorm24Total = 1;
                            }
                        }
                        else if (trueWd24.Code == 28)
                        {
                            if (liveRain24 <= 7.4)
                            {
                                //0
                                scoreDay.Rainstorm24 = 0;
                                scoreDay.Rainstorm24Total = 1;
                            }
                            else if (liveRain24 > 7.4 && liveRain24 < 10.0)
                            {
                                //NaN
                                scoreDay.Rainstorm24 = null;
                                scoreDay.Rainstorm24Total = null;
                            }
                            else //>=10
                            {
                                //1
                                scoreDay.Rainstorm24 = 1;
                                scoreDay.Rainstorm24Total = 1;
                            }
                        }
                        else if (trueWd24.Code == 12)
                        {
                            if (liveRain24 < 250.0)
                            {
                                //0
                                scoreDay.Rainstorm24 = 0;
                                scoreDay.Rainstorm24Total = 1;
                            }
                            else //>=250.0
                            {
                                //1
                                scoreDay.Rainstorm24 = 1;
                                scoreDay.Rainstorm24Total = 1;
                            }
                        }
                        else if (trueWd24.Code == 25)
                        {
                            if (liveRain24 >= 175.0 && liveRain24 <= 299.9)
                            {
                                //1
                                scoreDay.Rainstorm24 = 1;
                                scoreDay.Rainstorm24Total = 1;
                            }
                            else
                            {
                                //0
                                scoreDay.Rainstorm24 = 0;
                                scoreDay.Rainstorm24Total = 1;
                            }
                        }
                        else if (trueWd24.Code == 11)
                        {
                            if (liveRain24 >= 100.0 && liveRain24 <= 249.9)
                            {
                                //1
                                scoreDay.Rainstorm24 = 1;
                                scoreDay.Rainstorm24Total = 1;
                            }
                            else
                            {
                                //0
                                scoreDay.Rainstorm24 = 0;
                                scoreDay.Rainstorm24Total = 1;
                            }
                        }
                        else if (trueWd24.Code == 24)
                        {
                            if (liveRain24 >= 75.0 && liveRain24 <= 174.9)
                            {
                                //1
                                scoreDay.Rainstorm24 = 1;
                                scoreDay.Rainstorm24Total = 1;
                            }
                            else
                            {
                                //0
                                scoreDay.Rainstorm24 = 0;
                                scoreDay.Rainstorm24Total = 1;
                            }
                        }
                        else if (trueWd24.Code == 10)
                        {
                            if (liveRain24 >= 50.0 && liveRain24 <= 99.9)
                            {
                                //1
                                scoreDay.Rainstorm24 = 1;
                                scoreDay.Rainstorm24Total = 1;
                            }
                            else
                            {
                                //0
                                scoreDay.Rainstorm24 = 0;
                                scoreDay.Rainstorm24Total = 1;
                            }
                        }
                        else if (trueWd24.Code == 23)
                        {
                            if (liveRain24 <= 37.9)
                            {
                                //0
                                scoreDay.Rainstorm24 = 0;
                                scoreDay.Rainstorm24Total = 1;
                            }
                            else if (liveRain24 > 37.9 && liveRain24 < 50.0)
                            {
                                //NaN
                                scoreDay.Rainstorm24 = null;
                                scoreDay.Rainstorm24Total = null;
                            }
                            else if (liveRain24 >= 50.0 && liveRain24 < 75.0)
                            {
                                //1
                                scoreDay.Rainstorm24 = 1;
                                scoreDay.Rainstorm24Total = 1;
                            }
                            else //>=75.0
                            {
                                //0
                                scoreDay.Rainstorm24 = 0;
                                scoreDay.Rainstorm24Total = 1;
                            }
                        }
                        else if (trueWd24.CodeNew == "rain")
                        {
                            if (liveRain24 < 50.0)
                            {
                                //NaN
                                scoreDay.Rainstorm24 = null;
                                scoreDay.Rainstorm24Total = null;
                            }
                            else //>=50.0
                            {
                                //0
                                scoreDay.Rainstorm24 = 0;
                                scoreDay.Rainstorm24Total = 1;
                            }
                        }
                        else if (trueWd24.CodeNew == "snow")
                        {
                            if (liveRain24 < 10.0)
                            {
                                //NaN
                                scoreDay.Rainstorm24 = null;
                                scoreDay.Rainstorm24Total = null;
                            }
                            else //>=10.0
                            {
                                //0
                                scoreDay.Rainstorm24 = 0;
                                scoreDay.Rainstorm24Total = 1;
                            }
                        }
                        else if (trueWd24.CodeNew == "sunny")
                        {
                            if (liveRain24 < 50.0)
                            {
                                //NaN
                                scoreDay.Rainstorm24 = null;
                                scoreDay.Rainstorm24Total = null;
                            }
                            else //>=50.0
                            {
                                //0
                                scoreDay.Rainstorm24 = 0;
                                scoreDay.Rainstorm24Total = 1;
                            }
                        }
                        else
                        {
                            scoreDay.Rainstorm24 = null;
                            scoreDay.Rainstorm24Total = null;
                        }
                        #endregion 24小时暴雨（雪）预报判断


                        #region 48小时暴雨（雪）预报判断
                        //48小时
                        if (trueWd48.Code == 17)
                        {
                            if (liveRain48 < 10.0)
                            {
                                //0
                                scoreDay.Rainstorm48 = 0;
                                scoreDay.Rainstorm48Total = 1;
                            }
                            else //>=10.0
                            {
                                //1
                                scoreDay.Rainstorm48 = 1;
                                scoreDay.Rainstorm48Total = 1;
                            }
                        }
                        else if (trueWd48.Code == 28)
                        {
                            if (liveRain48 <= 7.4)
                            {
                                //0
                                scoreDay.Rainstorm48 = null;
                                scoreDay.Rainstorm48Total = 1;
                            }
                            else if (liveRain48 > 7.4 && liveRain48 < 10.0)
                            {
                                //NaN
                                scoreDay.Rainstorm48 = null;
                                scoreDay.Rainstorm48Total = null;
                            }
                            else //>=10
                            {
                                //1
                                scoreDay.Rainstorm48 = 1;
                                scoreDay.Rainstorm48Total = 1;
                            }
                        }
                        else if (trueWd48.Code == 12)
                        {
                            if (liveRain48 < 250.0)
                            {
                                //0
                                scoreDay.Rainstorm48 = 0;
                                scoreDay.Rainstorm48Total = 1;
                            }
                            else //>=250.0
                            {
                                //1
                                scoreDay.Rainstorm48 = 1;
                                scoreDay.Rainstorm48Total = 1;
                            }
                        }
                        else if (trueWd48.Code == 25)
                        {
                            if (liveRain48 >= 175.0 && liveRain48 <= 299.9)
                            {
                                //1
                                scoreDay.Rainstorm48 = 1;
                                scoreDay.Rainstorm48Total = 1;
                            }
                            else
                            {
                                //0
                                scoreDay.Rainstorm48 = 0;
                                scoreDay.Rainstorm48Total = 1;
                            }
                        }
                        else if (trueWd48.Code == 11)
                        {
                            if (liveRain48 >= 100.0 && liveRain48 <= 249.9)
                            {
                                //1
                                scoreDay.Rainstorm48 = 1;
                                scoreDay.Rainstorm48Total = 1;
                            }
                            else
                            {
                                //0
                                scoreDay.Rainstorm48 = 0;
                                scoreDay.Rainstorm48Total = 1;
                            }
                        }
                        else if (trueWd48.Code == 24)
                        {
                            if (liveRain48 >= 75.0 && liveRain48 <= 174.9)
                            {
                                //1
                                scoreDay.Rainstorm48 = 1;
                                scoreDay.Rainstorm48Total = 1;
                            }
                            else
                            {
                                //0
                                scoreDay.Rainstorm48 = 0;
                                scoreDay.Rainstorm48Total = 1;
                            }
                        }
                        else if (trueWd48.Code == 10)
                        {
                            if (liveRain48 >= 50.0 && liveRain48 <= 99.9)
                            {
                                //1
                                scoreDay.Rainstorm48 = 1;
                                scoreDay.Rainstorm48Total = 1;
                            }
                            else
                            {
                                //0
                                scoreDay.Rainstorm48 = 0;
                                scoreDay.Rainstorm48Total = 1;
                            }
                        }
                        else if (trueWd48.Code == 23)
                        {
                            if (liveRain48 <= 37.9)
                            {
                                //0
                                scoreDay.Rainstorm48 = 0;
                                scoreDay.Rainstorm48Total = 1;
                            }
                            else if (liveRain48 > 37.9 && liveRain48 < 50.0)
                            {
                                //NaN
                                scoreDay.Rainstorm48 = null;
                                scoreDay.Rainstorm48Total = null;
                            }
                            else if (liveRain48 >= 50.0 && liveRain48 < 75.0)
                            {
                                //1
                                scoreDay.Rainstorm48 = 1;
                                scoreDay.Rainstorm48Total = 1;
                            }
                            else //>=75.0
                            {
                                //0
                                scoreDay.Rainstorm48 = 0;
                                scoreDay.Rainstorm48Total = 1;
                            }
                        }
                        else if (trueWd48.CodeNew == "rain")
                        {
                            if (liveRain48 < 50.0)
                            {
                                //NaN
                                scoreDay.Rainstorm48 = null;
                                scoreDay.Rainstorm48Total = null;
                            }
                            else //>=50.0
                            {
                                //0
                                scoreDay.Rainstorm48 = 0;
                                scoreDay.Rainstorm48Total = 1;
                            }
                        }
                        else if (trueWd48.CodeNew == "snow")
                        {
                            if (liveRain48 < 10.0)
                            {
                                //NaN
                                scoreDay.Rainstorm48 = null;
                                scoreDay.Rainstorm48Total = null;
                            }
                            else //>=10.0
                            {
                                //0
                                scoreDay.Rainstorm48 = 0;
                                scoreDay.Rainstorm48Total = 1;
                            }
                        }
                        else if (trueWd48.CodeNew == "sunny")
                        {
                            if (liveRain48 < 50.0)
                            {
                                //NaN
                                scoreDay.Rainstorm48 = null;
                                scoreDay.Rainstorm48Total = null;
                            }
                            else //>=50.0
                            {
                                //0
                                scoreDay.Rainstorm48 = 0;
                                scoreDay.Rainstorm48Total = 1;
                            }
                        }
                        else
                        {
                            scoreDay.Rainstorm48 = null;
                            scoreDay.Rainstorm48Total = null;
                        }

                        #endregion 48小时暴雨（雪）预报判断


                        #endregion

                        scorePersonList.Add(scoreDay);
                    }

                    Score_Day scorePerson = new Score_Day();
                    scorePerson.ScoreID = Guid.NewGuid();
                    scorePerson.YBDate = scorePersonList.FirstOrDefault().YBDate;
                    scorePerson.YBTime = scorePersonList.FirstOrDefault().YBTime;
                    scorePerson.YBUserName = scorePersonList.FirstOrDefault().YBUserName;
                    scorePerson.YBUserID = scorePersonList.FirstOrDefault().YBUserID;
                    scorePerson.CreateTime = DateTime.Now;



                    scorePerson.MaxTemp24 = scorePersonList.Where(a => a.MaxTemp24 != null).Count() == 0 ? null : scorePersonList.Sum(a => a.MaxTemp24);
                    scorePerson.MinTemp24 = scorePersonList.Where(a => a.MinTemp24 != null).Count() == 0 ? null : scorePersonList.Sum(a => a.MinTemp24);
                    scorePerson.MaxTemp48 = scorePersonList.Where(a => a.MaxTemp48 != null).Count() == 0 ? null : scorePersonList.Sum(a => a.MaxTemp48);
                    scorePerson.MinTemp48 = scorePersonList.Where(a => a.MinTemp48 != null).Count() == 0 ? null : scorePersonList.Sum(a => a.MinTemp48);
                    scorePerson.MaxTemp72 = scorePersonList.Where(a => a.MaxTemp72 != null).Count() == 0 ? null : scorePersonList.Sum(a => a.MaxTemp72);
                    scorePerson.MinTemp72 = scorePersonList.Where(a => a.MinTemp72 != null).Count() == 0 ? null : scorePersonList.Sum(a => a.MinTemp72);
                    scorePerson.RainShine24 = scorePersonList.Where(a => a.RainShine24 != null).Count() == 0 ? null : scorePersonList.Sum(a => a.RainShine24);
                    scorePerson.RainShine48 = scorePersonList.Where(a => a.RainShine48 != null).Count() == 0 ? null : scorePersonList.Sum(a => a.RainShine48);
                    scorePerson.RainShine72 = scorePersonList.Where(a => a.RainShine72 != null).Count() == 0 ? null : scorePersonList.Sum(a => a.RainShine72);
                    scorePerson.AllTotal = scorePersonList.Where(a => a.AllTotal != null).Count() == 0 ? null : scorePersonList.Sum(a => a.AllTotal);
                    scorePerson.Rainfall24 = scorePersonList.Where(a => a.Rainfall24 != null).Count() == 0 ? null : scorePersonList.Sum(a => a.Rainfall24);
                    scorePerson.Rainfall24Total = scorePersonList.Where(a => a.Rainfall24Total != null).Count() == 0 ? null : scorePersonList.Sum(a => a.Rainfall24Total);
                    scorePerson.Rainfall48 = scorePersonList.Where(a => a.Rainfall48 != null).Count() == 0 ? null : scorePersonList.Sum(a => a.Rainfall48);
                    scorePerson.Rainfall48Total = scorePersonList.Where(a => a.Rainfall48Total != null).Count() == 0 ? null : scorePersonList.Sum(a => a.Rainfall48Total);
                    scorePerson.Rainfall72 = scorePersonList.Where(a => a.Rainfall72 != null).Count() == 0 ? null : scorePersonList.Sum(a => a.Rainfall72);
                    scorePerson.Rainfall72Total = scorePersonList.Where(a => a.Rainfall72Total != null).Count() == 0 ? null : scorePersonList.Sum(a => a.Rainfall72Total);
                    scorePerson.Rainstorm24 = scorePersonList.Where(a => a.Rainstorm24 != null).Count() == 0 ? null : scorePersonList.Sum(a => a.Rainstorm24);
                    scorePerson.Rainstorm24Total = scorePersonList.Where(a => a.Rainstorm24Total != null).Count() == 0 ? null : scorePersonList.Sum(a => a.Rainstorm24Total);
                    scorePerson.Rainstorm48 = scorePersonList.Where(a => a.Rainstorm48 != null).Count() == 0 ? null : scorePersonList.Sum(a => a.Rainstorm48);
                    scorePerson.Rainstorm48Total = scorePersonList.Where(a => a.Rainstorm48Total != null).Count() == 0 ? null : scorePersonList.Sum(a => a.Rainstorm48Total);

                    dal.Add(scorePerson, false);
                }

                dal.SaveChange();
            }

        }
    }
}
