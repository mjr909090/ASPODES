using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity.ModelConfiguration;
using ASPODES.Model;

namespace ASPODES.Database
{
    public class AnnualTaskDocMap:EntityTypeConfiguration<AnnualTaskDoc>
    {
        public AnnualTaskDocMap()
        {
            ToTable("AnnualTaskDoc");
            HasKey(atd => atd.AnnualTaskDocId);

            HasRequired(atd => atd.AnnualTask)
                .WithMany( at=>at.AnnualTaskDocs)
                .HasForeignKey(atd => atd.AnnualTaskId)
                .WillCascadeOnDelete(true);
        }
    }
}
