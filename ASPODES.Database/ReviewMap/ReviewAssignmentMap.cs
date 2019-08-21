using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity.ModelConfiguration;
using ASPODES.Model;

namespace ASPODES.Database
{
    public class ReviewAssignmentMap:EntityTypeConfiguration<ReviewAssignment>
    {
        public ReviewAssignmentMap()
        {
            ToTable("ReviewAssignment");

            HasKey(ra => ra.ReviewAssignmentId);

            HasRequired(ra => ra.Application)
                .WithMany( a=>a.ReviewAssignments )
                .HasForeignKey(ra => ra.ApplicationId)
                .WillCascadeOnDelete(false);

            HasRequired(ra => ra.Expert)
                .WithMany()
                .HasForeignKey(ra => ra.ExpertId)
                .WillCascadeOnDelete(false);

        }
    }
}
