using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PF.ViewModels
{
    public  class LiveData_Check
    {
        public DateTime Date { get; set; }
        public string DataMiss { get; set; }
        //public List<LiveData_Miss> DataMiss { get; set; }
    }

    public class LiveData_Miss
    {
        public string CityName { get; set; }
        public string DataMiss { get; set; }

    }

}
