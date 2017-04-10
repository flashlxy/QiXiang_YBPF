using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Limilabs.FTP.Client;
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
                foreach (FtpItem ftpItem in totalList)
                {
                    string romoteName = @"user\BEJNUP\SURF\ST_DAY\" + ftpItem.Name;
                    string month = ftpItem.Name.Substring(15, 6);

                    string localPath = @"D:\市县一体化平台文档\检验\日z文件\" + month;
                    if (!Directory.Exists(localPath))
                    {
                        Directory.CreateDirectory(localPath);
                    }

                    string localName= localPath +@"\"+ ftpItem.Name;
                    try
                    {
                        client.Download(romoteName, localName);
                        fileSuccessCount++;
                        Console.WriteLine("成功复制："+ftpItem.Name);
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex);
                      
                    }


                }
                Console.WriteLine("成功复制："+fileSuccessCount+"个文件，共有："+totalList.Count()+"个文件。");


            }
        }
    }
}
