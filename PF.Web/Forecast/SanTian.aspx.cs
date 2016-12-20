using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using PF.BLL.SQL;
using PF.Models.SQL;


namespace PF.Web.Forecast
{
    public partial class SanTian : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            InitData();
        }

        public void InitData()
        {
            BwYbs_BLL bll = new BwYbs_BLL();
            List<BwYbs> list = bll.GetPageListOrderBy(1, 50, a => a.YBUserName == "任兆鹏", a => a.YBDateTime).ToList();
           GridView1.DataSource = list;
            GridView1.DataBind();
        }
    }
}