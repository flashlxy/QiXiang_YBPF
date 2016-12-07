using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using PF.BLL.SQL;
using PF.Models.SQL;
using PF.Utility;

namespace PF.CA_BW_Night
{
    public class BaoWenFile
    {
        public void BaoWen_Import()
        {
            BaoWens_BLL bll = new BaoWens_BLL();
            YbUsers_BLL ubll = new YbUsers_BLL();


            List<FileInfo> allFileList = FileHelper.GetShareFileInfos(@"\\172.18.226.109\市县一体化平台文档\基本预报\城镇报文\下午16点15前",
                "Z_SEVP_C_BEQD_*16812*", "administrator", "yubk0501!");

            List<FileInfo> nearFileList = allFileList.Where(a => a.CreationTime >= DateTime.Now.AddDays(-7)).ToList();


            int total = 0;
            foreach (FileInfo fileInfo in nearFileList)
            {
                string fileName = @"\\172.18.226.109\市县一体化平台文档\基本预报\城镇报文\下午16点15前\" + fileInfo.Name;
                if (bll.GetCount(a => a.FileName == fileName) <= 0)
                {
                    if (!fileInfo.Name.Contains("-16812_."))
                    {
                        Models.SQL.BaoWens baowen = new Models.SQL.BaoWens();
                        baowen.BWID = Guid.NewGuid();
                        baowen.BWType = "晚间报文";
                        baowen.CreateTime = DateTime.Now;
                        baowen.YBDateTime =
                            DateTime.ParseExact(fileInfo.Name.Substring(14, 12), "yyyyMMddHHmm",
                                CultureInfo.InvariantCulture).AddHours(8);

                        //int leftcount = fileInfo.Name.Length - fileInfo.Name.IndexOf("-16812");

                        string fileNameUpper = fileInfo.Name.ToUpper();
                        if (fileNameUpper.Contains("-16812_") && !fileNameUpper.Contains("-16812_."))
                        {


                            baowen.YBUserName = fileNameUpper.Substring(fileNameUpper.IndexOf("-16812_") + 7,
                                fileNameUpper.Length - fileNameUpper.IndexOf("-16812_") - 7 - 4);

                        }
                        else
                        {

                            baowen.YBUserName = "集体";

                        }

                        YbUsers user = ubll.Get(a => a.YBUserName == baowen.YBUserName);
                        if (user == null)
                        {
                            YbUsers newuser = new YbUsers
                            {
                                YBUserID = Guid.NewGuid(),
                                YBUserName = baowen.YBUserName
                            };
                            baowen.YBUserID = newuser.YBUserID;
                            ubll.Add(newuser);
                        }
                        else
                        {
                            baowen.YBUserID = user.YBUserID;
                        }

                        baowen.Content =
                            FileHelper.GetShareTextContent(
                                @"\\172.18.226.109\市县一体化平台文档\基本预报\城镇报文\下午16点15前\" + fileInfo.Name, "administrator",
                                "yubk0501!", Encoding.Default);
                        baowen.FileName = @"\\172.18.226.109\市县一体化平台文档\基本预报\城镇报文\下午16点15前\" + fileInfo.Name;
                        baowen.IsTranslate = false;
                        bll.Add(baowen);
                        total++;
                    }
                }
            }
            Console.WriteLine("晚间报文导入成功，总数：" + total);







        }





        public int JieXiBaoWen(BaoWens baowen)
        {
            try
            {
                string[] contents = baowen.Content.Replace("\r\n", ";").Split(';');

                List<string> citycodes = CityUtility.AllCodeList();


                BwYbs_BLL bll = new BwYbs_BLL();

                foreach (string citycode in citycodes)
                {
                    BwYbs model = new BwYbs();
                    model.BWID = baowen.BWID;
                    model.BWYBID = Guid.NewGuid();
                    model.CountryCode = citycode;
                    model.CountryName = CityUtility.GetName(citycode);
                    model.CreateTime = DateTime.Now;
                    model.BWFileName = baowen.FileName;
                    model.YBDateTime = baowen.YBDateTime;
                    model.YBType = baowen.BWType;
                    model.YBUserID = baowen.YBUserID;
                    model.YBUserName = baowen.YBUserName;



                    string stringStart = contents.Where(a => a.Contains(citycode)).FirstOrDefault();
                    int indexStart = Array.IndexOf(contents, stringStart);

                    model.TianQiCode12 = decimal.Parse(contents[indexStart + 1].Substring(113, 5).Trim());
                    model.TianQiCode24 = decimal.Parse(contents[indexStart + 2].Substring(113, 5).Trim());
                    model.TianQiCode36 = decimal.Parse(contents[indexStart + 3].Substring(113, 5).Trim());
                    model.TianQiCode48 = decimal.Parse(contents[indexStart + 4].Substring(113, 5).Trim());
                    model.TianQiCode60 = decimal.Parse(contents[indexStart + 5].Substring(113, 5).Trim());
                    model.TianQiCode72 = decimal.Parse(contents[indexStart + 6].Substring(113, 5).Trim());

                    model.WindDirCode12 = decimal.Parse(contents[indexStart + 1].Substring(119, 5).Trim());
                    model.WindDirCode24 = decimal.Parse(contents[indexStart + 2].Substring(119, 5).Trim());
                    model.WindDirCode36 = decimal.Parse(contents[indexStart + 3].Substring(119, 5).Trim());
                    model.WindDirCode48 = decimal.Parse(contents[indexStart + 4].Substring(119, 5).Trim());
                    model.WindDirCode60 = decimal.Parse(contents[indexStart + 5].Substring(119, 5).Trim());
                    model.WindDirCode72 = decimal.Parse(contents[indexStart + 6].Substring(119, 5).Trim());

                    model.WindSpeCode12 = decimal.Parse(contents[indexStart + 1].Substring(125, contents[indexStart + 1].Length - 125).Trim());
                    model.WindSpeCode24 = decimal.Parse(contents[indexStart + 2].Substring(125, contents[indexStart + 1].Length - 125).Trim());
                    model.WindSpeCode36 = decimal.Parse(contents[indexStart + 3].Substring(125, contents[indexStart + 1].Length - 125).Trim());
                    model.WindSpeCode48 = decimal.Parse(contents[indexStart + 4].Substring(125, contents[indexStart + 1].Length - 125).Trim());
                    model.WindSpeCode60 = decimal.Parse(contents[indexStart + 5].Substring(125, contents[indexStart + 1].Length - 125).Trim());
                    model.WindSpeCode72 = decimal.Parse(contents[indexStart + 6].Substring(125, contents[indexStart + 1].Length - 125).Trim());

                    model.MaxTemp24 = decimal.Parse(contents[indexStart + 2].Substring(65, 5).Trim());
                    model.MaxTemp48 = decimal.Parse(contents[indexStart + 4].Substring(65, 5).Trim());
                    model.MaxTemp72 = decimal.Parse(contents[indexStart + 6].Substring(65, 5).Trim());

                    model.MinTemp24 = decimal.Parse(contents[indexStart + 2].Substring(71, 5).Trim());
                    model.MinTemp48 = decimal.Parse(contents[indexStart + 4].Substring(71, 5).Trim());
                    model.MinTemp72 = decimal.Parse(contents[indexStart + 6].Substring(71, 5).Trim());



                    model.TianQiName12 = WeatherUtility.TQCodeToText(string.Format("{0:D2}", Convert.ToInt32(model.TianQiCode12)));
                    model.TianQiName24 = WeatherUtility.TQCodeToText(string.Format("{0:D2}", Convert.ToInt32(model.TianQiCode24)));
                    model.TianQiName36 = WeatherUtility.TQCodeToText(string.Format("{0:D2}", Convert.ToInt32(model.TianQiCode36)));
                    model.TianQiName48 = WeatherUtility.TQCodeToText(string.Format("{0:D2}", Convert.ToInt32(model.TianQiCode48)));
                    model.TianQiName60 = WeatherUtility.TQCodeToText(string.Format("{0:D2}", Convert.ToInt32(model.TianQiCode60)));
                    model.TianQiName72 = WeatherUtility.TQCodeToText(string.Format("{0:D2}", Convert.ToInt32(model.TianQiCode72)));


                    model.WindDirName12 = WeatherUtility.FXCodeToText(Convert.ToInt32(model.WindDirCode12).ToString());
                    model.WindDirName24 = WeatherUtility.FXCodeToText(Convert.ToInt32(model.WindDirCode24).ToString());
                    model.WindDirName36 = WeatherUtility.FXCodeToText(Convert.ToInt32(model.WindDirCode36).ToString());
                    model.WindDirName48 = WeatherUtility.FXCodeToText(Convert.ToInt32(model.WindDirCode48).ToString());
                    model.WindDirName60 = WeatherUtility.FXCodeToText(Convert.ToInt32(model.WindDirCode60).ToString());
                    model.WindDirName72 = WeatherUtility.FXCodeToText(Convert.ToInt32(model.WindDirCode72).ToString());


                    model.WindSpeName12 = WeatherUtility.FSCodeToText(Convert.ToInt32(model.WindSpeCode12).ToString());
                    model.WindSpeName24 = WeatherUtility.FSCodeToText(Convert.ToInt32(model.WindSpeCode24).ToString());
                    model.WindSpeName36 = WeatherUtility.FSCodeToText(Convert.ToInt32(model.WindSpeCode36).ToString());
                    model.WindSpeName48 = WeatherUtility.FSCodeToText(Convert.ToInt32(model.WindSpeCode48).ToString());
                    model.WindSpeName60 = WeatherUtility.FSCodeToText(Convert.ToInt32(model.WindSpeCode60).ToString());
                    model.WindSpeName72 = WeatherUtility.FSCodeToText(Convert.ToInt32(model.WindSpeCode72).ToString());


                    bll.Add(model, false);
                    //var aa = model;
                }
                return bll.SaveChange();
            }
            catch (Exception ex)
            {

                return 0;
            }

        }
    }

}

