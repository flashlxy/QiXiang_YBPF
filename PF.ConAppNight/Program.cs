using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PF.BLL.SQL;
using PF.Models.SQL;

namespace PF.ConAppNight
{
    class Program
    {
        static void Main(string[] args)
        {

            BaoWenFile bf = new BaoWenFile();
            bf.BaoWen_Import();
            BaoWens_BLL bwbll = new BaoWens_BLL();
            List<BaoWens> list = bwbll.GetList(a=>a.BWType=="晚间报文"&&a.IsTranslate==false).OrderBy(a => a.YBDateTime).ThenBy(a => a.YBUserName).ToList();
           
            foreach (BaoWens baoWens in list)
            {
                if (bf.JieXiBaoWen(baoWens) > 0)
                {
                    baoWens.IsTranslate = true;
                    bwbll.Update(baoWens);
                }
            }
         
          
        }
    }
}
