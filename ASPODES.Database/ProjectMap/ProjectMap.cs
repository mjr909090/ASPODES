using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity.ModelConfiguration;
using ASPODES.Model;

namespace ASPODES.Database
{
    public class ProjectMap:EntityTypeConfiguration<Project>
    {
        public ProjectMap()
        {
            ToTable("Project");

            HasKey(p => p.ProjectId);

            HasRequired(p => p.Application)
                .WithOptional()
                .WillCascadeOnDelete(false);

            HasRequired(p => p.Leader)
                .WithMany()
                .HasForeignKey(p => p.LeaderId)
                .WillCascadeOnDelete(false);

            HasRequired(p => p.Institude)
                .WithMany()
                .HasForeignKey(p => p.InstituteId)
                .WillCascadeOnDelete(false);

            HasRequired(p => p.ProjectType)
                .WithMany()
                .HasForeignKey(p => p.ProjectTypeId)
                .WillCascadeOnDelete(false);

        }
    }
}
