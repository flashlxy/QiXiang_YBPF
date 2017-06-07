using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using PF.BLL;
using PF.Models.SQL;
using PF.BLL.SQL;
using PF.Utility;

namespace PF.Web.Score
{
    public partial class ScoreDay : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                InitDateTime();
                InitYBUser();


            }
        }
        public void InitDateTime()
        {
            int startYear = 2016;

            int countYear = DateTime.Now.Year - startYear;

            for (int i = 0; i <= countYear; i++)
            {
                int cYear = startYear + i;
                ListItem li = new ListItem() { Text = cYear.ToString() + "年", Value = cYear.ToString() };
                DropDownList_Year.Items.Add(li);
            }

            foreach (ListItem item in DropDownList_Year.Items)
            {
                if (item.Value == DateTime.Now.ToString("yyyy"))
                {
                    item.Selected = true;
                }
                else
                {
                    item.Selected = false;
                }
            }
            foreach (ListItem item in DropDownList_Month.Items)
            {
                if (item.Value == DateTime.Now.ToString("MM"))
                {
                    item.Selected = true;
                }
                else
                {
                    item.Selected = false;
                }
            }
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
            List<PF.Models.SQL.Score_Day> list = new List<Score_Day>();
            if (DropDownList_YBUser.SelectedItem.Value == "全部")
            {
                 list = bll.GetList(a => a.YBDate >= startTime && a.YBDate <= endTime && a.YBTime == DropDownList_YBTime.SelectedItem.Value).OrderBy(a => a.YBDate).ToList();

            }
            else
            {
                 list = bll.GetList(a => a.YBDate >= startTime && a.YBDate <= endTime && a.YBTime == DropDownList_YBTime.SelectedItem.Value&&a.YBUserName==DropDownList_YBUser.SelectedItem.Value).OrderBy(a => a.YBDate).ToList();

            }
            GridView_List.DataSource = list;
            GridView_List.DataBind();
        }

        protected void GridView_List_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            GridView_List.PageIndex = e.NewPageIndex;
            BindData();
        }

        public void InitYBUser()
        {
            YbUsers_BLL bll = new YbUsers_BLL();
            List<YbUsers> ulist = bll.GetList(a=>a.Work=="预报").OrderBy(a=>a.Order).ToList();
            ListItem allli = new ListItem() { Text = "全部", Value = "全部",Selected=true };
            DropDownList_YBUser.Items.Add(allli);

            foreach (var item in ulist)
            {
                ListItem li = new ListItem() {Text=item.YBUserName,Value=item.YBUserName};
                DropDownList_YBUser.Items.Add(li);
            }
        }
        //计算全部在下面
        protected void Button_Calculate_Click(object sender, EventArgs e)
        {

            LiveData_BLL liveBll = new LiveData_BLL();
            DateTime startTime = DateTime.Parse(DropDownList_Year.SelectedItem.Value + "-" + DropDownList_Month.SelectedItem.Value + "-01");
            DateTime endTime = startTime.AddMonths(1);

            if (liveBll.DataCheck(startTime, DropDownList_YBTime.SelectedItem.Value))
            {
                JavaScriptHelper.Loading("大人请稍后，奴才正在拼命计算...");


                Score_Day_BLL bll = new Score_Day_BLL();
                if (DropDownList_YBTime.SelectedItem.Value == "08时")
                {

                    bll.Caculate08_ByYbUser(startTime, endTime);
                }
                else
                {

                    bll.Caculate20_ByYbUser(startTime, endTime);
                }
                JavaScriptHelper.UnLoading();

                Response.Write("<script language=javascript defer>alert('计算完成！');</script>");
            }
            else
            {
                Response.Write("<script language=javascript defer>alert('抱歉，该月实况数据不完整，请校验！');</script>");
            }





        }

        protected void Button_Add08_Click(object sender, EventArgs e)
        {
            Score_Day_BLL bll = new Score_Day_BLL();
            DateTime startTime = DateTime.Parse(DropDownList_Year.SelectedItem.Value + "-" + DropDownList_Month.SelectedItem.Value + "-01");
            DateTime endTime = startTime.AddMonths(1);
            JavaScriptHelper.Loading("大人请稍后，奴才正在拼命计算...");

            int count = bll.Add08_User(startTime, endTime);
            JavaScriptHelper.UnLoading();

            Response.Write("<script language=javascript defer>alert('计算完成,共计："+count.ToString()+"条数据！');</script>");


        }
    }
}