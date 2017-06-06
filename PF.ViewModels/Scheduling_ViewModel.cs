using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PF.ViewModels
{
   public  class Day_Scheduling_ViewModel
    {
        public int Week { get; set; }
        public bool IsCurrentMonth { get; set; }
        public DateTime DayTime { get; set; }
        public string DayTimeString { get; set; }
        public string User1 { get; set; }
        public string User2 { get; set; }
        public string User3 { get; set; }
    }
}
