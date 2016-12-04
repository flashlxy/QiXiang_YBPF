using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows;
using PF.BLL.SQL;
using PF.Utility;
using PF.Models.SQL;

namespace PF.Wpf.BaoWen
{
    public class BaoWenFile
    {
        public void BaoWen_Import()
        {
            BaoWens_BLL bll = new BaoWens_BLL();



            List<FileInfo> list = FileHelper.GetShareFileInfos(@"\\172.18.226.109\市县一体化平台文档\基本预报\城镇报文\下午16点15前", "Z_SEVP_C_BEQD_*16812*", "administrator", "yubk0501!");

            List<Models.SQL.BaoWens> blist = new List<Models.SQL.BaoWens>();

            foreach (FileInfo fileInfo in list)
            {
                if(!fileInfo.Name.Contains("-16812_."))
                {
                    Models.SQL.BaoWens baowen = new Models.SQL.BaoWens();
                    baowen.BWID = Guid.NewGuid();
                    baowen.BWType = "晚间报文";
                    baowen.CreateTime = DateTime.Now;
                    baowen.YBDateTime = DateTime.ParseExact(fileInfo.Name.Substring(14, 12), "yyyyMMddHHmm", CultureInfo.InvariantCulture).AddHours(8);
                    //int leftcount = fileInfo.Name.Length - fileInfo.Name.IndexOf("-16812");

                    string fileName = fileInfo.Name.ToUpper();
                    if (fileName.Contains("-16812_") && !fileName.Contains("-16812_."))
                    {


                        baowen.YBUserName = fileName.Substring(fileName.IndexOf("-16812_") + 7, fileName.Length - fileName.IndexOf("-16812_") - 7 - 4);

                    }
                    else
                    {
                       
                            baowen.YBUserName = "集体";
                       
                    }
                    baowen.Content = FileHelper.GetShareTextContent(@"\\172.18.226.109\市县一体化平台文档\基本预报\城镇报文\下午16点15前\"+fileInfo.Name, "administrator", "yubk0501!", Encoding.Default);
                    baowen.FileName = @"\\172.18.226.109\市县一体化平台文档\基本预报\城镇报文\下午16点15前\" + fileInfo.Name;
                    blist.Add(baowen);
                    bll.Add(baowen);
                }
                
            }

            var aaa = blist;

        }

        public void BaoWen_ImportYbUser()
        {
            BaoWens_BLL bll = new BaoWens_BLL();
            YbUsers_BLL ubll = new YbUsers_BLL();
            List<BaoWens> blist = bll.GetList().ToList();
            List<YbUsers> ulist = ubll.GetList().ToList();

            foreach (BaoWens baoWens in blist)
            {
                baoWens.YBUserID = ulist.Where(a => a.YBUserName == baoWens.YBUserName).FirstOrDefault().YBUserID;
                bll.Update(baoWens);
            }
            //MessageBox.Show("sdafd");
        }
    }
}

