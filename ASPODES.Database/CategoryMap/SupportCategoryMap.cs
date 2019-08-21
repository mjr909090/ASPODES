using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ASPODES.Model;
namespace ASPODES.Database
{
    public class SupportCategoryMap : EntityTypeConfiguration<SupportCategory>
    {

        public SupportCategoryMap()
        {
            ToTable("SupportCategory");

            HasKey(s => s.SupportCategoryId);

            HasRequired(sc => sc.ProjectType)
                .WithMany()
                .HasForeignKey(sc => sc.ProjectTypeId)
                .WillCascadeOnDelete(true);
        }
    }

}
