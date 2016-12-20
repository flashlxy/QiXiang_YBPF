using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using PF.BLL.SQL;
using PF.Models.SQL;
using PF.Utility;

namespace PF.Web.AFile
{
    public partial class AFileToLive : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                DataImport();
            }
        }

        public void DataImport()
        {
            LiveData_BLL bll = new LiveData_BLL();
            List<FileInfo> allFileList = FileHelper.GetShareFileInfos(@"\\172.18.226.109\市县一体化平台文档\检验\A","A*", "administrator", "yubk0501!");
            foreach (FileInfo fileInfo in allFileList)
            {
                List<LiveData> sklist = new List<LiveData>();

                //string contents = File.ReadAllText(@"\\172.18.226.10\nt40\zdzh\" + item.Value);
                string countrycode = fileInfo.Name.Substring(1, 5);
                //string apath = @"P:\zdzh\" + item.Value;
                string contents =FileHelper.GetShareTextContent(@"\\172.18.226.109\市县一体化平台文档\检验\A\"+fileInfo.Name,"Administrator","yubk0501!",Encoding.Default);
                string tbcontents =
                    contents.Substring(contents.IndexOf("TB") + 4,
                        (contents.IndexOf("IB") - contents.IndexOf("TB") - 7)).Replace("\r\n", "");
                string[] tb = tbcontents.Split('.');


                for (int i = 0; i < tb.Length; i++)
                {
                    LiveData shikuang = new LiveData();
                    shikuang.LDID = Guid.NewGuid();
                    shikuang.FDate =DateTime.ParseExact(fileInfo.Name.Substring(7, 6) + (i + 1).ToString("00"), "yyyyMMdd",CultureInfo.InvariantCulture) ;
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
                  int count=  bll.GetCount(a => a.CountryCode == item.CountryCode && a.FDate == item.FDate&&a.Category=="20时");
                    if (count <= 0)
                    {
                        bll.Add(item);
                    }
                }

                Response.Write(countrycode + " "+fileInfo.Name.Substring(7,6) + sklist.Count().ToString());

            }

            Response.Write(allFileList.Count().ToString());

        }


    }
}