//------------------------------------------------------------------------------
// <auto-generated>
//    此代码是根据模板生成的。
//
//    手动更改此文件可能会导致应用程序中发生异常行为。
//    如果重新生成代码，则将覆盖对此文件的手动更改。
// </auto-generated>
//------------------------------------------------------------------------------

namespace PF.Models.Oracle
{
    using System;
    using System.Collections.Generic;
    
    public partial class DATAMINUTE
    {
        public System.Guid FID { get; set; }
        public short STAID { get; set; }
        public string STANAME { get; set; }
        public System.DateTime FDATE { get; set; }
        public Nullable<short> DWDIRECTION { get; set; }
        public Nullable<decimal> DWSPEED { get; set; }
        public Nullable<short> TWDIRECTION { get; set; }
        public Nullable<decimal> TWSPEED { get; set; }
        public Nullable<short> MAXWDIRECTION { get; set; }
        public Nullable<decimal> MAXWSPEED { get; set; }
        public Nullable<System.DateTime> MAXWSPEEDTIME { get; set; }
        public Nullable<short> INSDIRECTION { get; set; }
        public Nullable<decimal> INSSPEED { get; set; }
        public Nullable<short> MAXDIRECTION { get; set; }
        public Nullable<decimal> MAXSPEED { get; set; }
        public Nullable<System.DateTime> MAXSPEEDTIME { get; set; }
        public Nullable<decimal> HRAIN { get; set; }
        public Nullable<decimal> MAXMRAIN { get; set; }
        public Nullable<System.DateTime> MAXMRAINTIME { get; set; }
        public Nullable<decimal> TEMPERATURE { get; set; }
        public Nullable<decimal> MAXTEMP { get; set; }
        public Nullable<System.DateTime> MAXTEMPTIME { get; set; }
        public Nullable<decimal> MINTEMP { get; set; }
        public Nullable<System.DateTime> MINTEMPTIME { get; set; }
        public Nullable<decimal> HUMTEMP { get; set; }
        public Nullable<short> HUMCAP { get; set; }
        public Nullable<short> COMHUM { get; set; }
        public Nullable<short> MINCOMHUM { get; set; }
        public Nullable<System.DateTime> MINCOMHUMTIME { get; set; }
        public Nullable<decimal> WATPRE { get; set; }
        public Nullable<decimal> DEWTEMP { get; set; }
        public Nullable<decimal> STAPRES { get; set; }
        public Nullable<decimal> MAXSTAPRES { get; set; }
        public Nullable<System.DateTime> MAXSTAPRESTIME { get; set; }
        public Nullable<decimal> MINSTAPRES { get; set; }
        public Nullable<System.DateTime> MINSTAPRESTIME { get; set; }
        public Nullable<decimal> SURTEMP { get; set; }
        public Nullable<decimal> MAXSURTEMP { get; set; }
        public Nullable<System.DateTime> MAXSURTEMPTIME { get; set; }
        public Nullable<decimal> MINSURTEMP { get; set; }
        public Nullable<System.DateTime> MINSURTEMPTIME { get; set; }
        public Nullable<decimal> GTEMP5 { get; set; }
        public Nullable<decimal> GTEMP10 { get; set; }
        public Nullable<decimal> GTEMP15 { get; set; }
        public Nullable<decimal> GTEMP20 { get; set; }
        public Nullable<decimal> GTEMP40 { get; set; }
        public Nullable<decimal> GTEMP80 { get; set; }
        public Nullable<decimal> GTEMP160 { get; set; }
        public Nullable<decimal> GTEMP320 { get; set; }
        public Nullable<decimal> EVAPORATION { get; set; }
        public Nullable<short> SUNLIGHT { get; set; }
        public string OTHERNAME { get; set; }
        public string BELONGTO { get; set; }
        public Nullable<short> VISIBILITY { get; set; }
        public Nullable<short> MINVISIBILITY { get; set; }
        public Nullable<System.DateTime> MINVISIBILITYTIME { get; set; }
        public Nullable<decimal> SID { get; set; }
    }
}
