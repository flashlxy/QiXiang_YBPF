using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PF.ViewModels
{
    public class BaoWen_Check_ViewModel
    {
        public string YbUserName { get; set; }
        public string Work { get; set; }
        public DateTime Date { get; set; }
        public bool IsMiss { get; set; }

        public string Message { get; set; }
    }
}
