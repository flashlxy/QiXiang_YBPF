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
                List<WeatherDictionary> weathercodes = new WeatherDictionary_BLL().GetList().ToList();

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


                    
                    model.TianQiName12 = weathercodes.Where(a => a.Code == model.TianQiCode12&&a.Type=="天气").FirstOrDefault().Name;
                    model.TianQiName24 = weathercodes.Where(a => a.Code == model.TianQiCode24&&a.Type=="天气").FirstOrDefault().Name;
                    model.TianQiName36 = weathercodes.Where(a => a.Code == model.TianQiCode36&&a.Type=="天气").FirstOrDefault().Name;
                    model.TianQiName48 = weathercodes.Where(a => a.Code == model.TianQiCode48&&a.Type=="天气").FirstOrDefault().Name;
                    model.TianQiName60 = weathercodes.Where(a => a.Code == model.TianQiCode60&&a.Type=="天气").FirstOrDefault().Name;
                    model.TianQiName72 = weathercodes.Where(a => a.Code == model.TianQiCode72&&a.Type=="天气").FirstOrDefault().Name;


                    model.WindDirName12 = weathercodes.Where(a => a.Code == model.WindDirCode12 && a.Type == "风向").FirstOrDefault().Name;
                    model.WindDirName24 = weathercodes.Where(a => a.Code == model.WindDirCode24 && a.Type == "风向").FirstOrDefault().Name;
                    model.WindDirName36 = weathercodes.Where(a => a.Code == model.WindDirCode36 && a.Type == "风向").FirstOrDefault().Name;
                    model.WindDirName48 = weathercodes.Where(a => a.Code == model.WindDirCode48 && a.Type == "风向").FirstOrDefault().Name;
                    model.WindDirName60 = weathercodes.Where(a => a.Code == model.WindDirCode60 && a.Type == "风向").FirstOrDefault().Name;
                    model.WindDirName72 = weathercodes.Where(a => a.Code == model.WindDirCode72 && a.Type == "风向").FirstOrDefault().Name;
                  


                    model.WindSpeName12 = weathercodes.Where(a => a.Code == model.WindSpeCode12 && a.Type == "风速").FirstOrDefault().Name;
                    model.WindSpeName24 = weathercodes.Where(a => a.Code == model.WindSpeCode24 && a.Type == "风速").FirstOrDefault().Name;
                    model.WindSpeName36 = weathercodes.Where(a => a.Code == model.WindSpeCode36 && a.Type == "风速").FirstOrDefault().Name;
                    model.WindSpeName48 = weathercodes.Where(a => a.Code == model.WindSpeCode48 && a.Type == "风速").FirstOrDefault().Name;
                    model.WindSpeName60 = weathercodes.Where(a => a.Code == model.WindSpeCode60 && a.Type == "风速").FirstOrDefault().Name;
                    model.WindSpeName72 = weathercodes.Where(a => a.Code == model.WindSpeCode72 && a.Type == "风速").FirstOrDefault().Name;



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

