using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PF.ViewModels
{
    class YBUsers_ViewModel
    {
    }

    public class YBUsers_Date_ViewModel
    {
        public DateTime Date { get; set; }
        public DateTime Next_Date { get; set; }
    }
    public class YBUsers_Name_Day_ViewModel
    {
        //public DateTime Date { get; set; }
       
        public Guid YBUserID { get; set; }


        public string YBUserName { get; set; }

        private DateTime _date;

        public DateTime Date
        {
            set { _date =DateTime.Parse(value.ToShortDateString()); }
            get { return _date; }

        }
    }


    public class YBUsers_Name_ViewModel
    {
        //public DateTime Date { get; set; }

        public Guid YBUserID { get; set; }


        public string YBUserName { get; set; }

      
    }
}
