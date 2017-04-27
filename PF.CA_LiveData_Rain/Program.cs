using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PF.CA_LiveData_Rain
{
    class Program
    {
        static void Main(string[] args)
        {
            ZFile zfile = new ZFile();
            zfile.Copy();
            zfile.Calculate20();
            zfile.Calculate08();
            //Console.ReadLine();
        }
    }
}
