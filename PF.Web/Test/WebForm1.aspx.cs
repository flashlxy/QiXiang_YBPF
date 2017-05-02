using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using PF.BLL.Oracle;
using PF.BLL.SQL;
using PF.Models.Oracle;
using PF.Models.SQL;


namespace PF.Web.Test
{
    public partial class WebForm1 : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        public void Add()
        {
            YTHPT_WEATHER_DICTIONARY_BLL obll = new YTHPT_WEATHER_DICTIONARY_BLL();

            obll.Delete(a => a.CATEGORY == "温度");

            //WeatherDictionary_BLL sbll = new WeatherDictionary_BLL();
          //  List<WeatherDictionary> slist= sbll.GetList(a => a.Type == "天气").OrderBy(a=>a.Type).ThenBy(a=>a.Code).ToList();

            //foreach (WeatherDictionary item in slist)
            //{
            //    YTHPT_WEATHER_DICTIONARY model = new YTHPT_WEATHER_DICTIONARY()
            //    {
            //       FID=Guid.NewGuid().ToString(),
            //         CATEGORY=item.Type,
            //         CODE=item.Code.ToString().PadLeft(2,'0'),
            //         NAME=item.Name,
            //         CREATETIME=DateTime.Now
                     
            //    };
            //    obll.Add(model);
            //}





            for (int i = 0; i < 100; i++)
            {
                YTHPT_WEATHER_DICTIONARY model = new YTHPT_WEATHER_DICTIONARY()
                {
                    FID = Guid.NewGuid().ToString(),
                    CATEGORY ="温度",
                    CODE = i.ToString().PadLeft(2, '0'),
                    NAME = Temp(i).ToString(),
                    CREATETIME = DateTime.Now

                };
                obll.Add(model);
            }


        }


        public int Temp(int code)
        {
            if (code <= 50)
            {
                return code;

            }
            return 50-code  ;
        }

        protected void Button1_Click(object sender, EventArgs e)
        {
            Add();
            Response.Write("asdfasdf");
        }
    }
}