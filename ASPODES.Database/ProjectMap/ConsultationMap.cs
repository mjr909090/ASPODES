using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity.ModelConfiguration;
using ASPODES.Model;

namespace ASPODES.Database
{
    public class ConsultationMap:EntityTypeConfiguration<Consultation>
    {
        /*
        public ConsultationMap()
        {
            ToTable("Consultation");

            HasKey(c => c.ConsultationId);
            HasRequired(c => c.Application)
                .WithMany()
                .HasForeignKey(c => c.ApplicationId)
                .WillCascadeOnDelete(true);

            HasRequired(c => c.Leader)
                .WithMany()
                .HasForeignKey(c => c.LeaderId)
                .WillCascadeOnDelete(false);

            HasRequired(c => c.Institude)
                .WithMany()
                .HasForeignKey(c => c.InstituteId)
                .WillCascadeOnDelete(false);

            HasRequired(c => c.ProjectType)
                .WithMany()
                .HasForeignKey(c => c.ProjectTypeId)
                .WillCascadeOnDelete(false);
        }
         * */
    }
}
