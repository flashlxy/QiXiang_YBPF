using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PF.BLL.SQL;
using PF.Models.SQL;

namespace PF.CA_BW_Morning
{
    class Program
    {
        static void Main(string[] args)
        {

            BaoWenFile bf = new BaoWenFile();
            bf.BaoWen_Import();
            BaoWens_BLL bwbll = new BaoWens_BLL();
            List<BaoWens> list = bwbll.GetList(a => a.BWType == "早间报文" && a.IsTranslate == false).OrderBy(a => a.YBDateTime).ThenBy(a => a.YBUserName).ToList();

            foreach (BaoWens baoWens in list)
            {
                if (bf.JieXiBaoWen(baoWens) > 0)
                {
                    baoWens.IsTranslate = true;
                    bwbll.Update(baoWens);
                    Console.WriteLine("早间间报文翻译成功：" + baoWens.YBUserName+" "+baoWens.YBDateTime);

                }
            }
            Console.WriteLine("早间间报文翻译成功,总数：" + list.Count);
    
        }
    }
}
