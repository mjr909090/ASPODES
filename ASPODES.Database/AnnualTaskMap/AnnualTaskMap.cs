using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity.ModelConfiguration;
using ASPODES.Model;

namespace ASPODES.Database
{
    public class AnnualTaskMap:EntityTypeConfiguration<AnnualTask>
    {
        public AnnualTaskMap()
        {
            ToTable("AnnualTask");

            HasKey(at => at.AnnualTaskId);

            HasRequired(at => at.Project)
                .WithMany(at => at.AnnualTasks)
                .HasForeignKey(at => at.ProjectId)
                .WillCascadeOnDelete(true);

            HasRequired(at => at.Leader)
                .WithMany()
                .HasForeignKey(at => at.LeaderId)
                .WillCascadeOnDelete(false);

            HasRequired(at => at.Institute)
                .WithMany()
                .HasForeignKey(at => at.InstituteId)
                .WillCascadeOnDelete(false);
            
        }
    }
}
