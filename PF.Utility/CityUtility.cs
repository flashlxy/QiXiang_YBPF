using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PF.Utility
{
    public class CityUtility
    {
        public static string GetName(string code)
        {
            switch (code)
            {
                case "54842":
                    return "平度";
                case "54849":
                    return "胶州";
                case "54851":
                    return "莱西";
                case "54853":
                    return "崂山";
                case "54855":
                    return "即墨";
                case "54857":
                    return "青岛";
                case "54943":
                    return "黄岛";
                default:
                    return "未知";

            }
        }
        public static string GetCode(string name)
        {
            switch (name)
            {
                case "平度":
                    return "54842";
                case "胶州":
                    return "54849";
                case "莱西":
                    return "54851";
                case "崂山":
                    return "54853";
                case "即墨":
                    return "54855";
                case "青岛":
                    return "54857";
                case "黄岛":
                    return "54943";
                default:
                    return "未知";

            }
        }

        public static List<string> AllCodeList()

        {
            List<string> codes = new List<string>();
            codes.Add("54842");
            codes.Add("54849");
            codes.Add("54851");
            codes.Add("54853");
            codes.Add("54855");
            codes.Add("54857");
            codes.Add("54943");
            return codes;
        }

        public static List<string> AllNameList()

        {
            List<string> names = new List<string>();
            names.Add("平度");
            names.Add("胶州");
            names.Add("莱西");
            names.Add("崂山");
            names.Add("即墨");
            names.Add("青岛");
            names.Add("黄岛");
            return names;
        }
    }
}