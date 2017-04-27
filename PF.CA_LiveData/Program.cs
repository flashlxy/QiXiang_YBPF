using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PF.CA_LiveData
{
    class Program
    {
        static void Main(string[] args)
        {
            ZFile zfile = new ZFile();
            zfile.Copy();
            zfile.Calculate20();
            zfile.Calculate08();
            LiveTemp lt = new LiveTemp();
            lt.Temp08();
            lt.Temp20();

         
        }
    }
}
