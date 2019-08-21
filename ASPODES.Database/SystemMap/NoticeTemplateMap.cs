using ASPODES.Model;
using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ASPODES.Database
{
    public class NoticeTemplateMap : EntityTypeConfiguration<NoticeTemplate>
    {
        public NoticeTemplateMap()
        {
            ToTable("NoticeTemplate");
        }
    }
}
