using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity.ModelConfiguration;
using ASPODES.Model;
using System.ComponentModel.DataAnnotations.Schema;

namespace ASPODES.Database
{
    public class ReviewCommentMap:EntityTypeConfiguration<ReviewComment>
    {
        public ReviewCommentMap()
        {
            ToTable("ReviewComment");

            HasKey(rc => rc.ReviewCommentId);

            Property( rc=>rc.ReviewCommentId ).HasDatabaseGeneratedOption( DatabaseGeneratedOption.Identity );

            HasRequired(rc => rc.ReviewAssignment)
                .WithOptional()
                .WillCascadeOnDelete(true);

            HasRequired(rc => rc.Application)
                .WithMany( a=>a.ReviewComments )
                .HasForeignKey(rc=>rc.ApplicationId)
                .WillCascadeOnDelete(false);

            HasRequired(rc => rc.Expert)
                .WithMany()
                .HasForeignKey(rc=>rc.ExpertId)
                .WillCascadeOnDelete(false);

        }
    }
}
