using System;
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

namespace PF.Web.R248
{
    public partial class Rain08ToData : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            DataImport();
        }

        public void DataImport()
        {
            LiveData_BLL bll = new LiveData_BLL();
            List<FileInfo> allFileList = FileHelper.GetShareFileInfos(@"\\172.18.226.109\市县一体化平台文档\检验\r24-8-p", "*.000", "administrator", "yubk0501!");
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

            Response.Write(allFileList.Count.ToString());
        }
    }
}