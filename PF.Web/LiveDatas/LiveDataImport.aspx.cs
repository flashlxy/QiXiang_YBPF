﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using PF.BLL.SQL;
using PF.Models.SQL;
using PF.Utility;
using PF.ViewModels;

namespace PF.Web.LiveDatas
{
    public partial class LiveDataImport : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            Panel_DataMiss.Visible = false;
        }

        protected void Btn_DataCheck_Click(object sender, EventArgs e)
        {
            DateTime startTime = DateTime.Parse(DropDownList_Year.SelectedItem.Value + "-" + DropDownList_Month.SelectedItem.Value + "-01");
            DateTime endTime = startTime.AddMonths(1);
            LiveData_BLL bll = new LiveData_BLL();
            List<LiveData> list =
                bll.GetList(
                    a =>
                        a.FDate >= startTime & a.FDate < endTime && a.Category == DropDownList_YBTime.SelectedItem.Value)
                    .OrderBy(a => a.FDate).ThenBy(a => a.CountryCode)
                    .ToList();
            //GridView1.DataSource = list;
            //GridView1.DataBind();

            TimeSpan timeSpan = endTime - startTime;

            List<LiveData_Check> ldcList = new List<LiveData_Check>();
            List<string> citys = CityUtility.AllNameList();
            for (int i = 0; i < timeSpan.Days; i++)
            {
                DateTime currentDate = startTime.AddDays(i);

                List<LiveData> daylist = list.Where(a => a.FDate == currentDate).ToList();

                LiveData_Check ldc = new LiveData_Check();
                ldc.Date = currentDate;
                StringBuilder dataMissStr = new StringBuilder();
                foreach (string city in citys)
                {
                    LiveData dayCity = daylist.Where(a => a.CountryName == city).FirstOrDefault();

                    if (dayCity != null)
                    {
                        if (dayCity.MaxTemp == null || dayCity.MinTemp == null || dayCity.Rain == null)
                        {
                            dataMissStr.Append("<li style='color:#ff6a00;'>"+city+" 缺少 ");
                            string missElement = string.Empty;
                            if (dayCity.MaxTemp == null)
                            {
                                missElement += "最高温度 ";
                            }
                            if(dayCity.MinTemp==null)
                            {
                                missElement += "最低温度 ";
                            }
                            if (dayCity.Rain == null)
                            {
                                missElement += "降水 ";
                            }

                            dataMissStr.Append(missElement); 
                            dataMissStr.Append("数据<br/></li>");

                        }
                    }
                    else
                    {
                        dataMissStr.Append("<li  style='color:#ff0000;'>" + city + " 缺少 最高温度 最低温度 降水 数据<br/></li>");

                    }


                }
                ldc.DataMiss = dataMissStr.ToString();
                if (!String.IsNullOrWhiteSpace(ldc.DataMiss))
                {
                    ldcList.Add(ldc);

                }
            }
            Repeater_DataCheck.DataSource = ldcList;
            Repeater_DataCheck.DataBind();
            if (ldcList.Count() > 0)
            {
                Label_DataMiss.Text = "数据有缺失，请及时补充！";
                Panel_DataMiss.Visible = true;
            }
            else
            {
                Label_DataMiss.Text = "数据齐全！";
                Panel_DataMiss.Visible = false;

            }


        }
    }
}