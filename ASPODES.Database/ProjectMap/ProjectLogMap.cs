using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ASPODES.Model;
using System.Data.Entity.ModelConfiguration;

namespace ASPODES.Database
{
    public class ProjectLogMap : EntityTypeConfiguration<ProjectLog>
    {
        public ProjectLogMap() 
        {
            ToTable("ProjectLog");

            HasRequired(pl => pl.Project)
                .WithMany()
                .HasForeignKey(pl => pl.ProjectId)
                .WillCascadeOnDelete(false);
        }
    }
}
