using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ASPODES.Model;
namespace ASPODES.Database
{
    public class SysSettingMap : EntityTypeConfiguration<SysSetting>
    {
        public SysSettingMap()
        {
            ToTable("SysSetting");

            HasKey(s => s.SetId);

         
        }
    }

}
