using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using PF.Models.SQL;
using PF.BLL.SQL;

namespace PF.Web.Warn
{
    public partial class WarnStatistics : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void Button_Query_Click(object sender, EventArgs e)
        {
            int year = int.Parse(DropDownList_Year.SelectedItem.Value); 
            int month = int.Parse(DropDownList_Month.SelectedItem.Value);
            WarnStatistics_BLL bll = new WarnStatistics_BLL();
          List<PF.Models.SQL.WarnStatistics>  list= bll.GetList(a => a.Year == year && a.Month == month).OrderBy(a=>a.WarnCategory).ThenBy(a=>a.LevelOrder).ToList();
            Repeater_List.DataSource = list;
            Repeater_List.DataBind();

        }
    }
}