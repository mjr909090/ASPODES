using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ASPODES.Model;

namespace ASPODES.Database
{
    public class SysSettingHistoryMap : EntityTypeConfiguration<SysSettingHistory>
    {
        public SysSettingHistoryMap()
        {
            ToTable("SysSettingHistory");
            HasKey(s => s.Id);
        }
    }
}
