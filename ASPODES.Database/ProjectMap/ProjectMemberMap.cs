using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity.ModelConfiguration;
using ASPODES.Model;

namespace ASPODES.Database
{
    public class ProjectMemberMap:EntityTypeConfiguration<ProjectMember>
    {
        public ProjectMemberMap() 
        {
            ToTable("ProjectMember");
            HasKey(pm => new { pm.ProjectId, pm.PersonId });

            HasRequired(pm => pm.Project)
                .WithMany( p=>p.Members )
                .HasForeignKey(pm => pm.ProjectId)
                .WillCascadeOnDelete(true);

            HasRequired(pm => pm.Person)
                .WithMany()
                .HasForeignKey(pm => pm.PersonId)
                .WillCascadeOnDelete(false);
        }
    }
}
