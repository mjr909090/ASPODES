using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ASPODES.Model;
using System.Data.Entity.ModelConfiguration;

namespace ASPODES.Database
{
    public class ApplicationAssignmentMap : EntityTypeConfiguration<ApplicationAssignment>
    {
        public ApplicationAssignmentMap()
        {
            ToTable("ApplicationAssignment");

            HasKey(aa => aa.ApplicationAssignmentId);

            HasRequired( aa=>aa.Authorize)
                .WithMany()
                .HasForeignKey(aa => new {aa.RoleId , aa.UserId })
                .WillCascadeOnDelete(true);

            HasRequired(aa => aa.ProjectType)
                .WithMany()
                .HasForeignKey(aa => aa.ProjectTypeId)
                .WillCascadeOnDelete(true);

        }
    }
}
