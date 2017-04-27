using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using Limilabs.FTP.Client;
using PF.BLL.SQL;
using PF.Models.SQL;
using PF.Utility;

namespace PF.CA_LiveData
{
    public class ZFile
    {
        public void Copy()
        {



            using (Ftp client = new Ftp())
            {
                client.Connect("172.18.200.251");
                client.Login("x25bejn", "bejn90");
                var list = client.GetList("/user/BEJNUP/SURF/ST_DAY/");
                List<string> citycodes = CityUtility.AllCodeList();

                List<FtpItem> totalList = new List<FtpItem>();
                foreach (string citycode in citycodes)
                {
                    totalList.AddRange(list.Where(a => a.Name.Contains(citycode)).ToList());


                }
                int fileSuccessCount = 0;


                File_Z_BLL bll = new File_Z_BLL();

                foreach (FtpItem ftpItem in totalList)
                {
                    string romoteName = @"user\BEJNUP\SURF\ST_DAY\" + ftpItem.Name;
                    string month = ftpItem.Name.Substring(15, 6);

                    string localPath = @"D:\市县一体化平台文档\检验\日z文件\" + month;
                    if (!Directory.Exists(localPath))
                    {
                        Directory.CreateDirectory(localPath);
                    }

                    string localName = localPath + @"\" + ftpItem.Name;
                    try
                    {
                        client.Download(romoteName, localName);
                        fileSuccessCount++;
                        Console.WriteLine("成功复制：" + ftpItem.Name);



                        ///存取数据库
                        string name = ftpItem.Name;
                        string content = File.ReadAllText(localName);

                        string[] contentLine = File.ReadAllLines(localName);

                        string countrycode = contentLine[0].Substring(0, 5);
                        string countryname = CityUtility.GetName(countrycode);

                        DateTime date = DateTime.ParseExact(contentLine[1].Split(' ').ToList().ElementAt(0).Substring(0, 8), "yyyyMMdd", CultureInfo.InvariantCulture);

                        string rain1_string = contentLine[1].Split(' ').ToList().ElementAt(1);
                        string rain2_string = contentLine[1].Split(' ').ToList().ElementAt(2);
                        decimal rain1 = 0;
                        decimal rain2 = 0;
                        if (rain1_string == ",,,,,")
                        {
                            rain1 = (decimal)0.01;
                        }
                        else
                        {
                            rain1 = decimal.Parse(rain1_string) / 10;
                        }
                        if (rain2_string == ",,,,,")
                        {
                            rain2 = (decimal)0.01;
                        }
                        else
                        {
                            rain2 = decimal.Parse(rain2_string) / 10;
                        }


                        File_Z model = bll.Get(a => a.Date == date && a.CountryCode == countrycode);
                        if (model != null)
                        {
                            model.Twenty_Eight = rain1;
                            model.Eight_Twenty = rain2;
                            model.FileContent = content;
                            model.FileName = name;
                            model.CreateTime = DateTime.Now;
                            bll.Update(model);
                        }
                        else
                        {
                            model = new File_Z()
                            {
                                FileID = Guid.NewGuid(),
                                CountryCode = countrycode,
                                CountryName = countryname,
                                CreateTime = DateTime.Now,
                                Date = date,
                                Twenty_Eight = rain1,
                                Eight_Twenty = rain2,
                                FileContent = content,
                                FileName = name
                            };
                            bll.Add(model);
                        }

                        Console.WriteLine("成功入库：" + ftpItem.Name);


                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex);

                    }


                }
                Console.WriteLine("成功复制：" + fileSuccessCount + "个文件，共有：" + totalList.Count() + "个文件。");


            }



        }

        public void CopyAll()
        {
            List<FileInfo> flist = FileHelper.GetShareFileInfos(@"\\172.18.226.109\市县一体化平台文档\检验\日z文件\201704", "administrator", "yubk0501!");
            File_Z_BLL bll = new File_Z_BLL();

            foreach (FileInfo fileInfo in flist)
            {
                ///存取数据库
                string name = fileInfo.Name;
                string content = FileHelper.GetShareTextContent(@"\\172.18.226.109\市县一体化平台文档\检验\日z文件\201704\" + fileInfo.Name, "administrator", "yubk0501!", Encoding.Default);

                string[] contentLine = FileHelper.GetShareTextLines(@"\\172.18.226.109\市县一体化平台文档\检验\日z文件\201704\" + fileInfo.Name, "administrator", "yubk0501!");

                string countrycode = contentLine[0].Substring(0, 5);
                string countryname = CityUtility.GetName(countrycode);

                DateTime date = DateTime.ParseExact(contentLine[1].Split(' ').ToList().ElementAt(0).Substring(0, 8), "yyyyMMdd", CultureInfo.InvariantCulture);

                string rain1_string = contentLine[1].Split(' ').ToList().ElementAt(1);
                string rain2_string = contentLine[1].Split(' ').ToList().ElementAt(2);
                decimal rain1 = 0;
                decimal rain2 = 0;
                if (rain1_string == ",,,,,")
                {
                    rain1 = (decimal)0.01;
                }
                else
                {
                    rain1 = decimal.Parse(rain1_string) / 10;
                }
                if (rain2_string == ",,,,,")
                {
                    rain2 = (decimal)0.01;
                }
                else
                {
                    rain2 = decimal.Parse(rain2_string) / 10;
                }


                File_Z model = bll.Get(a => a.Date == date && a.CountryCode == countrycode);
                if (model != null)
                {
                    //model.Twenty_Eight = rain1;
                    //model.Eight_Twenty = rain2;
                    //model.FileContent = content;
                    //model.FileName = name;
                    //model.CreateTime = DateTime.Now;
                    //bll.Update(model);
                }
                else
                {
                    model = new File_Z()
                    {
                        FileID = Guid.NewGuid(),
                        CountryCode = countrycode,
                        CountryName = countryname,
                        CreateTime = DateTime.Now,
                        Date = date,
                        Twenty_Eight = rain1,
                        Eight_Twenty = rain2,
                        FileContent = content,
                        FileName = name
                    };
                    bll.Add(model);
                }

                Console.WriteLine("成功入库：" + fileInfo.Name);
            }
        }


        public void Calculate20()
        {
            File_Z_BLL zbll = new File_Z_BLL();
            LiveData_BLL lbll = new LiveData_BLL();
            DateTime lastDay = DateTime.Now.AddDays(-5);

            List<File_Z> zlist = zbll.GetList(a => a.Date >= lastDay).OrderBy(a => a.Date).ToList();
            foreach (File_Z fileZ in zlist)
            {
                LiveData liveData = lbll.Get(a => a.FDate == fileZ.Date && a.CountryCode == fileZ.CountryCode && a.Category == "20时");
                if (liveData != null)
                {
                    if (liveData.Rain == null)
                    {
                        liveData.Rain = fileZ.Twenty_Eight + fileZ.Eight_Twenty;
                        lbll.Update(liveData);
                    }
                }
                else
                {
                    liveData = new LiveData()
                    {
                        LDID = Guid.NewGuid(),
                        Category = "20时",
                        CountryCode = fileZ.CountryCode,
                        CountryName = fileZ.CountryName,
                        CreateTime = DateTime.Now,
                        FDate = fileZ.Date,
                        Rain = fileZ.Twenty_Eight + fileZ.Eight_Twenty
                    };
                    lbll.Add(liveData);
                }

                Console.WriteLine("成功计算20时：" + fileZ.CountryName + fileZ.Date.ToString());

            }
        }

        public void Calculate08()
        {
            File_Z_BLL zbll = new File_Z_BLL();
            LiveData_BLL lbll = new LiveData_BLL();
            DateTime lastDay = DateTime.Now.AddDays(-5);
            List<File_Z> zlist = zbll.GetList(a => a.Date >= lastDay).ToList();

            DateTime newDay = (DateTime)zlist.Max(a => a.Date);

            List<File_Z> prelist = zlist.Where(a => a.Date != newDay).OrderBy(a => a.Date).ToList();


            foreach (File_Z fileZ in prelist)
            {
                DateTime nextDay = ((DateTime)fileZ.Date).AddDays(1);

                File_Z nextFileZ = zlist.Where(a => a.Date == nextDay && a.CountryCode == fileZ.CountryCode).FirstOrDefault();


                LiveData liveData = lbll.Get(a => a.FDate == fileZ.Date && a.CountryCode == fileZ.CountryCode && a.Category == "08时");
                if (liveData != null)
                {
                    if (liveData.Rain == null)
                    {
                        liveData.Rain = fileZ.Eight_Twenty + nextFileZ.Twenty_Eight;
                        lbll.Update(liveData);
                    }
                }
                else
                {
                    liveData = new LiveData()
                    {
                        LDID = Guid.NewGuid(),
                        Category = "08时",
                        CountryCode = fileZ.CountryCode,
                        CountryName = fileZ.CountryName,
                        CreateTime = DateTime.Now,
                        FDate = fileZ.Date,
                        Rain = fileZ.Eight_Twenty + nextFileZ.Twenty_Eight
                    };
                    lbll.Add(liveData);
                }
                Console.WriteLine("成功计算08时：" + fileZ.CountryName + fileZ.Date.ToString());

            }
        }
    }
}
