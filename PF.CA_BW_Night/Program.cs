using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml;
using PF.BLL.SQL;
using PF.Models.SQL;

namespace PF.CA_BW_Night
{
    class Program
    {
        static void Main(string[] args)
        {
            //Console.WriteLine(File.ReadAllText(@"data\tq.xml"));
            BaoWenFile bf = new BaoWenFile();
            bf.BaoWen_Import();
            BaoWens_BLL bwbll = new BaoWens_BLL();
            List<BaoWens> list = bwbll.GetList(a => a.BWType == "晚间报文" && a.IsTranslate == false).OrderBy(a => a.YBDateTime).ThenBy(a => a.YBUserName).ToList();

            foreach (BaoWens baoWens in list)
            {
                if (bf.JieXiBaoWen(baoWens) > 0)
                {
                    baoWens.IsTranslate = true;
                    bwbll.Update(baoWens);
                }
            }
            Console.WriteLine("晚间报文翻译成功,总数：" + list.Count);
            //Console.Read();

        }
    }
}