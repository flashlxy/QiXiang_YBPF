﻿//------------------------------------------------------------------------------
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
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    
    public partial class OracleSAEntities : DbContext
    {
        public OracleSAEntities()
            : base("name=OracleSAEntities")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public DbSet<DATAHOUR> DATAHOUR { get; set; }
        public DbSet<EARLY_WARNING_COUNTRIES> EARLY_WARNING_COUNTRIES { get; set; }
        public DbSet<YTHPT_WEATHER_DICTIONARY> YTHPT_WEATHER_DICTIONARY { get; set; }
        public DbSet<DATAMINUTE> DATAMINUTE { get; set; }
    }
}
