//------------------------------------------------------------------------------
// <auto-generated>
//    此代码是根据模板生成的。
//
//    手动更改此文件可能会导致应用程序中发生异常行为。
//    如果重新生成代码，则将覆盖对此文件的手动更改。
// </auto-generated>
//------------------------------------------------------------------------------

namespace PF.Models.SQL
{
    using System;
    using System.Collections.Generic;
    
    public partial class LiveData
    {
        public System.Guid LDID { get; set; }
        public Nullable<System.DateTime> FDate { get; set; }
        public string CountryCode { get; set; }
        public string CountryName { get; set; }
        public Nullable<decimal> Rain { get; set; }
        public Nullable<decimal> MaxTemp { get; set; }
        public Nullable<decimal> MinTemp { get; set; }
        public string Category { get; set; }
        public Nullable<System.DateTime> CreateTime { get; set; }
    }
}