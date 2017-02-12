using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using PF.Models.SQL;
using PF.BLL.SQL;

namespace PF.Web.Score
{
    public partial class ScoreDay : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
              
            }
        }
        public void Init()
        {
            Score_Day_BLL bll = new Score_Day_BLL();
            
        }

        protected void Button_Query_Click(object sender, EventArgs e)
        {
            BindData();


        }

        

        public void BindData()
        {
            Score_Day_BLL bll = new Score_Day_BLL();
            DateTime startTime = DateTime.Parse(DropDownList_Year.SelectedItem.Value + "-" + DropDownList_Month.SelectedItem.Value + "-01");
            DateTime endTime = startTime.AddMonths(1).AddDays(-1);
            List<PF.Models.SQL.Score_Day> list = bll.GetList(a => a.YBDate >= startTime && a.YBDate <= endTime && a.YBTime == DropDownList_YBTime.SelectedItem.Value).OrderBy(a => a.YBDate).ToList();

            GridView_List.DataSource = list;
            GridView_List.DataBind();
        }

        protected void GridView_List_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            GridView_List.PageIndex = e.NewPageIndex;
            BindData();
        }
    }
}