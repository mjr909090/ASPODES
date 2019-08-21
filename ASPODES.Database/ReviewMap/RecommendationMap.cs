using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity.ModelConfiguration;
using ASPODES.Model;

namespace ASPODES.Database
{
    public class RecommendationMap:EntityTypeConfiguration<Recommendation>
    {
        public RecommendationMap()
        {
            ToTable("Recommendation");
            HasKey(r => r.RecommendationId);
            HasRequired(r => r.Recommender)
                .WithMany()
                .HasForeignKey(r => r.RecommenderId)
                .WillCascadeOnDelete(false);

            HasRequired(r => r.Candidate)
                .WithMany()
                .HasForeignKey(r => r.CandidateId)
                .WillCascadeOnDelete(false);


            HasRequired(r => r.Institute)
                .WithMany()
                .HasForeignKey(r => r.InstituteId)
                .WillCascadeOnDelete(false);
        }
    }
}
