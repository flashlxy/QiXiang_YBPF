using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using Limilabs.FTP.Client;
using PF.BLL.SQL;
using PF.Models.SQL;
using PF.Utility;

namespace PF.CA_LiveData_Rain
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
    }
}
