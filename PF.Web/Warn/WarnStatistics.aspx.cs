using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using PF.Models.SQL;
using PF.BLL.SQL;
using Aspose.Words;
using Aspose.Words.Tables;
using System.Drawing;

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
            List<PF.Models.SQL.WarnStatistics> list = bll.GetList(a => a.Year == year && a.Month == month).OrderBy(a => a.WarnCategory).ThenBy(a => a.LevelOrder).ToList();
            Repeater_List.DataSource = list;
            Repeater_List.DataBind();

        }

        protected void Button_Export_Click(object sender, EventArgs e)
        {
            int year = int.Parse(DropDownList_Year.SelectedItem.Value);
            int month = int.Parse(DropDownList_Month.SelectedItem.Value);

            DateTime startDate=DateTime.Parse(year.ToString()+"-"+month.ToString()+"-01");
            DateTime endDate= startDate.AddMonths(1).AddDays(-1);



            WarnStatistics_BLL bll = new WarnStatistics_BLL();
            List<PF.Models.SQL.WarnStatistics> list = bll.GetList(a => a.Year == year && a.Month == month).OrderBy(a => a.WarnCategory).ThenBy(a => a.LevelOrder).ToList();

            ///////////
            //string fileName = @"D:\市县一体化平台文档\预报评分\预警信号质量检验报表-" + startDate.ToString("yyyy年MM月") + ".docx";
            string fileName = Server.MapPath("~/Data/预警信号检验/预警信号质量检验报表-" + startDate.ToString("yyyy年MM月") + ".doc");
            Document doc = new Document();
            DocumentBuilder builder = new DocumentBuilder(doc);
            //builder.MoveToDocumentEnd();
            builder.InsertParagraph();
            builder.Font.Size = 15;
            builder.Font.Name = "黑体";
            builder.Font.Bold = true;

            // Specify linespacing 1.5 line  
            builder.ParagraphFormat.LineSpacingRule = LineSpacingRule.Multiple;
            // The line spacing is specified in the LineSpacing property as the number of lines.  
            // One line equals 12 points. so 1.5 lines = 18 points  
            builder.ParagraphFormat.LineSpacing = 15.6;
            builder.ParagraphFormat.Alignment = ParagraphAlignment.Center;
            builder.Write("青岛市气象台气象灾害预警信号质量检验报表");


            builder.InsertParagraph();
            builder.Font.Size = 15;
            builder.Font.Name = "宋体";
            builder.Font.Bold = true;
            builder.ParagraphFormat.Alignment = ParagraphAlignment.Right;
            // Specify linespacing 1.5 line  
            builder.ParagraphFormat.LineSpacingRule = LineSpacingRule.Multiple;
            // The line spacing is specified in the LineSpacing property as the number of lines.  
            // One line equals 12 points. so 1.5 lines = 18 points  
            builder.ParagraphFormat.LineSpacing = 15.6;
            builder.Write(startDate.ToString("yyyy年MM月dd日")+"至"+ endDate.ToString("yyyy年MM月dd日"));











            builder.InsertParagraph();
            builder.Font.Name = "宋体";
            builder.ParagraphFormat.Alignment = ParagraphAlignment.Center;
            builder.Font.Bold = false;
            // Specify linespacing 1 line  
            builder.ParagraphFormat.LineSpacingRule = LineSpacingRule.Multiple;
            // The line spacing is specified in the LineSpacing property as the number of lines.  
            // One line equals 12 points. so 1.5 lines = 18 points  
            builder.ParagraphFormat.LineSpacing = 12;
            builder.StartTable();
            //builder.CellFormat.Width = 20;

            //builder.RowFormat.Alignment = RowAlignment.Center;
            //builder.CellFormat.Borders.LineStyle = LineStyle.Single;
            //builder.CellFormat.Borders.Color = Color.Black;
            //builder.CellFormat.BottomPadding = 10;
            //builder.CellFormat.TopPadding = 10;
            builder.CellFormat.VerticalAlignment = CellVerticalAlignment.Center;

            builder.ParagraphFormat.Alignment = ParagraphAlignment.Center;//水平居中对齐
                                                                          ////////////////////
            builder.Font.Size = 11;

            builder.InsertCell();
            builder.CellFormat.VerticalMerge = CellMerge.First;
            builder.CellFormat.HorizontalMerge = CellMerge.None;
            builder.Write("预警信号类别");
            builder.InsertCell();
            builder.CellFormat.VerticalMerge = CellMerge.First;
            builder.CellFormat.HorizontalMerge = CellMerge.None;
            builder.Write("预警信号级别");
            builder.InsertCell();
            builder.CellFormat.VerticalMerge = CellMerge.First;
            builder.CellFormat.HorizontalMerge = CellMerge.None;
            builder.Write("TS评分(正确率)");
            builder.InsertCell();
            builder.CellFormat.VerticalMerge = CellMerge.First;
            builder.CellFormat.HorizontalMerge = CellMerge.None;
            builder.Write("命中率");
            builder.InsertCell();
            builder.CellFormat.VerticalMerge = CellMerge.First;
            builder.CellFormat.HorizontalMerge = CellMerge.None;
            builder.Write("漏报率");
            builder.InsertCell();
            builder.CellFormat.VerticalMerge = CellMerge.First;
            builder.CellFormat.HorizontalMerge = CellMerge.None;
            builder.Write("空报率");

            builder.InsertCell();
            builder.CellFormat.VerticalMerge = CellMerge.None;
            builder.CellFormat.HorizontalMerge = CellMerge.First;
            builder.Write("时间提前量(分钟)");
            builder.InsertCell();
            builder.CellFormat.VerticalMerge = CellMerge.None;
            builder.CellFormat.HorizontalMerge = CellMerge.Previous;
            builder.InsertCell();
            builder.CellFormat.VerticalMerge = CellMerge.None;
            builder.CellFormat.HorizontalMerge = CellMerge.Previous;
            builder.EndRow();
            ////////////////////////
            builder.InsertCell();
            builder.CellFormat.VerticalMerge = CellMerge.Previous;
            builder.CellFormat.HorizontalMerge = CellMerge.None;
            builder.Write("时间");
            builder.InsertCell();
            builder.CellFormat.VerticalMerge = CellMerge.Previous;
            builder.CellFormat.HorizontalMerge = CellMerge.None;
            builder.Write("时间");
            builder.InsertCell();
            builder.CellFormat.VerticalMerge = CellMerge.Previous;
            builder.CellFormat.HorizontalMerge = CellMerge.None;
            builder.Write("时间");

            builder.InsertCell();
            builder.CellFormat.VerticalMerge = CellMerge.Previous;
            builder.CellFormat.HorizontalMerge = CellMerge.None;
            builder.Write("时间");

            builder.InsertCell();
            builder.CellFormat.VerticalMerge = CellMerge.Previous;
            builder.CellFormat.HorizontalMerge = CellMerge.None;
            builder.Write("时间");

            builder.InsertCell();
            builder.CellFormat.VerticalMerge = CellMerge.Previous;
            builder.CellFormat.HorizontalMerge = CellMerge.None;
            builder.Write("时间");


            builder.InsertCell();
            builder.CellFormat.VerticalMerge = CellMerge.None;
            builder.CellFormat.HorizontalMerge = CellMerge.None;
            builder.Write("T1");

            builder.InsertCell();
            builder.CellFormat.VerticalMerge = CellMerge.None;
            builder.CellFormat.HorizontalMerge = CellMerge.None;
            builder.Write("T2");
            builder.InsertCell();
            builder.CellFormat.VerticalMerge = CellMerge.None;
            builder.CellFormat.HorizontalMerge = CellMerge.None;
            builder.Write("T3");
            builder.EndRow();





            var cates = list.GroupBy(a => new { a.WarnCategory }).ToList();

            foreach (var cate in cates)
            {
                var catelist = list.Where(a => a.WarnCategory == cate.Key.WarnCategory).ToList();
                if (catelist.Count() > 2)
                {
                    for (int i = 0; i < catelist.Count(); i++)
                    {
                        if (i == 0)
                        {
                            builder.InsertCell();
                            builder.CellFormat.VerticalMerge = CellMerge.First;
                            builder.Write(catelist.ElementAt(i).WarnCategory);

                            builder.InsertCell();
                            builder.CellFormat.VerticalMerge = CellMerge.None;
                            builder.Write(catelist.ElementAt(i).WarnLevel);

                            builder.InsertCell();
                            builder.CellFormat.VerticalMerge = CellMerge.None;
                            builder.Write(catelist.ElementAt(i).TSScore.ToString());
                            builder.InsertCell();
                            builder.CellFormat.VerticalMerge = CellMerge.None;
                            builder.Write(catelist.ElementAt(i).HitRate.ToString());
                            builder.InsertCell();
                            builder.CellFormat.VerticalMerge = CellMerge.None;
                            builder.Write(catelist.ElementAt(i).MissRate.ToString());
                            builder.InsertCell();
                            builder.CellFormat.VerticalMerge = CellMerge.None;
                            builder.Write(catelist.ElementAt(i).EmptyRate.ToString());
                            builder.InsertCell();
                            builder.CellFormat.VerticalMerge = CellMerge.None;
                            builder.Write(catelist.ElementAt(i).ReachSpendMinute1.ToString());
                            builder.InsertCell();
                            builder.CellFormat.VerticalMerge = CellMerge.None;
                            builder.Write(catelist.ElementAt(i).ReachSpendMinute1.ToString());
                            builder.InsertCell();
                            builder.CellFormat.VerticalMerge = CellMerge.None;
                            builder.Write(catelist.ElementAt(i).ReachSpendMinute1.ToString());

                            builder.EndRow();
                        }
                        else
                        {
                            builder.InsertCell();
                            builder.CellFormat.VerticalMerge = CellMerge.Previous;
                            builder.Write(catelist.ElementAt(i).WarnCategory);

                            builder.InsertCell();
                            builder.CellFormat.VerticalMerge = CellMerge.None;
                            builder.Write(catelist.ElementAt(i).WarnLevel);

                            builder.InsertCell();
                            builder.CellFormat.VerticalMerge = CellMerge.None;
                            builder.Write(catelist.ElementAt(i).TSScore.ToString());
                            builder.InsertCell();
                            builder.CellFormat.VerticalMerge = CellMerge.None;
                            builder.Write(catelist.ElementAt(i).HitRate.ToString());
                            builder.InsertCell();
                            builder.CellFormat.VerticalMerge = CellMerge.None;
                            builder.Write(catelist.ElementAt(i).MissRate.ToString());
                            builder.InsertCell();
                            builder.CellFormat.VerticalMerge = CellMerge.None;
                            builder.Write(catelist.ElementAt(i).EmptyRate.ToString());
                            builder.InsertCell();
                            builder.CellFormat.VerticalMerge = CellMerge.None;
                            builder.Write(catelist.ElementAt(i).ReachSpendMinute1.ToString());
                            builder.InsertCell();
                            builder.CellFormat.VerticalMerge = CellMerge.None;
                            builder.Write(catelist.ElementAt(i).ReachSpendMinute1.ToString());
                            builder.InsertCell();
                            builder.CellFormat.VerticalMerge = CellMerge.None;
                            builder.Write(catelist.ElementAt(i).ReachSpendMinute1.ToString());
                            builder.EndRow();
                        }
                    }
                }

            }


            builder.EndTable();

            builder.InsertBreak(BreakType.PageBreak);
            builder.InsertParagraph();
            builder.Font.Size = 15;
            builder.Font.Name = "黑体";
            builder.Font.Bold = true;
            builder.ParagraphFormat.Alignment = ParagraphAlignment.Center;
            // Specify linespacing 1.5 line  
            builder.ParagraphFormat.LineSpacingRule = LineSpacingRule.Multiple;
            // The line spacing is specified in the LineSpacing property as the number of lines.  
            // One line equals 12 points. so 1.5 lines = 18 points  
            builder.ParagraphFormat.LineSpacing = 15.6;
            builder.Write("青岛市气象台气象灾害预警信号质量检验报表2");


            builder.InsertParagraph();
            builder.Font.Size = 15;
            builder.Font.Name = "宋体";
            builder.Font.Bold = true;
            builder.ParagraphFormat.Alignment = ParagraphAlignment.Right;
            // Specify linespacing 1.5 line  
            builder.ParagraphFormat.LineSpacingRule = LineSpacingRule.Multiple;
            // The line spacing is specified in the LineSpacing property as the number of lines.  
            // One line equals 12 points. so 1.5 lines = 18 points  
            builder.ParagraphFormat.LineSpacing = 15.6;
            builder.Write(startDate.ToString("yyyy年MM月dd日") + "至" + endDate.ToString("yyyy年MM月dd日"));











            builder.InsertParagraph();
            builder.Font.Name = "宋体";
            builder.ParagraphFormat.Alignment = ParagraphAlignment.Center;
            builder.Font.Bold = false;
            // Specify linespacing 1 line  
            builder.ParagraphFormat.LineSpacingRule = LineSpacingRule.Multiple;
            // The line spacing is specified in the LineSpacing property as the number of lines.  
            // One line equals 12 points. so 1.5 lines = 18 points  
            builder.ParagraphFormat.LineSpacing = 12;


            builder.StartTable();
            //builder.CellFormat.Width = 20;

            //builder.RowFormat.Alignment = RowAlignment.Center;
            //builder.CellFormat.Borders.LineStyle = LineStyle.Single;
            //builder.CellFormat.Borders.Color = Color.Black;
            //builder.CellFormat.BottomPadding = 10;
            //builder.CellFormat.TopPadding = 10;
            builder.CellFormat.VerticalAlignment = CellVerticalAlignment.Center;

            builder.ParagraphFormat.Alignment = ParagraphAlignment.Center;//水平居中对齐
            builder.Font.Size = 11;
            ////////////////////

            builder.InsertCell();
            builder.CellFormat.VerticalMerge = CellMerge.None;
            builder.CellFormat.HorizontalMerge = CellMerge.None;
            builder.Write("预警信号类别");
            builder.InsertCell();
            builder.CellFormat.VerticalMerge = CellMerge.None;
            builder.CellFormat.HorizontalMerge = CellMerge.None;
            builder.Write("预警信号级别");
            builder.InsertCell();
            builder.CellFormat.VerticalMerge = CellMerge.None;
            builder.CellFormat.HorizontalMerge = CellMerge.None;
            builder.Write("预报正确次数");
            builder.InsertCell();
            builder.CellFormat.VerticalMerge = CellMerge.None;
            builder.CellFormat.HorizontalMerge = CellMerge.None;
            builder.Write("预报漏报次数");
            builder.InsertCell();
            builder.CellFormat.VerticalMerge = CellMerge.None;
            builder.CellFormat.HorizontalMerge = CellMerge.None;
            builder.Write("预报空报次数");
          
            builder.EndRow();



            






            foreach (var cate in cates)
            {
                var catelist = list.Where(a => a.WarnCategory == cate.Key.WarnCategory).ToList();
                if (catelist.Count() > 2)
                {
                    for (int i = 0; i < catelist.Count(); i++)
                    {
                        if (i == 0)
                        {
                            builder.InsertCell();
                            builder.CellFormat.VerticalMerge = CellMerge.First;
                            builder.Write(catelist.ElementAt(i).WarnCategory);

                            builder.InsertCell();
                            builder.CellFormat.VerticalMerge = CellMerge.None;
                            builder.Write(catelist.ElementAt(i).WarnLevel);

                            builder.InsertCell();
                            builder.CellFormat.VerticalMerge = CellMerge.None;
                            builder.Write(catelist.ElementAt(i).HitCount.ToString());
                            builder.InsertCell();
                            builder.CellFormat.VerticalMerge = CellMerge.None;
                            builder.Write(catelist.ElementAt(i).MissCount.ToString());
                            builder.InsertCell();
                            builder.CellFormat.VerticalMerge = CellMerge.None;
                            builder.Write(catelist.ElementAt(i).EmptyCount.ToString());
                            builder.EndRow();
                        }
                        else
                        {
                            builder.InsertCell();
                            builder.CellFormat.VerticalMerge = CellMerge.Previous;
                            builder.Write(catelist.ElementAt(i).WarnCategory);

                            builder.InsertCell();
                            builder.CellFormat.VerticalMerge = CellMerge.None;
                            builder.Write(catelist.ElementAt(i).WarnLevel);

                            builder.InsertCell();
                            builder.CellFormat.VerticalMerge = CellMerge.None;
                            builder.Write(catelist.ElementAt(i).HitCount.ToString());
                            builder.InsertCell();
                            builder.CellFormat.VerticalMerge = CellMerge.None;
                            builder.Write(catelist.ElementAt(i).MissCount.ToString());
                            builder.InsertCell();
                            builder.CellFormat.VerticalMerge = CellMerge.None;
                            builder.Write(catelist.ElementAt(i).EmptyCount.ToString());
                           
                            builder.EndRow();
                        }
                    }
                }

            }



            builder.EndTable();



            builder.InsertParagraph();
            builder.Font.Size = 15;
            builder.Font.Name = "宋体";
            builder.Font.Bold = true;
            builder.ParagraphFormat.Alignment = ParagraphAlignment.Right;
            // Specify linespacing 1.5 line  
            builder.ParagraphFormat.LineSpacingRule = LineSpacingRule.Multiple;
            // The line spacing is specified in the LineSpacing property as the number of lines.  
            // One line equals 12 points. so 1.5 lines = 18 points  
            builder.ParagraphFormat.LineSpacing = 15.6;
            builder.Write("审核：任兆鹏                  日期："+DateTime.Now.ToString("yyyy年M月d日"));









            doc.Save(fileName, SaveFormat.Doc);



            Response.ContentType = "application/x-zip-compressed";
            Response.AddHeader("Content-Disposition", "attachment;filename="+System.IO.Path.GetFileName(fileName));
          
            Response.TransmitFile(fileName);
        }
    }
}