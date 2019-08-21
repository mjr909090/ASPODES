using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ASPODES.Model;
namespace ASPODES.Database
{
    public class ApplicationMap : EntityTypeConfiguration<Application>
    {
        public ApplicationMap()
        {
            ToTable("Application");
            
            HasKey( a => a.ApplicationId );

            HasRequired(a => a.ProjectType)
                .WithMany()
                .HasForeignKey(a => a.ProjectTypeId)
                .WillCascadeOnDelete(false);

            HasRequired(a => a.SupportCategory)
                .WithMany()
                .HasForeignKey( a=>a.SupportCategoryId )
                .WillCascadeOnDelete(false);

            HasRequired(a => a.Institute)
                .WithMany()
                .HasForeignKey( a=>a.InstituteId )
                .WillCascadeOnDelete(false);

            HasRequired(a => a.Leader)
                .WithMany()
                .HasForeignKey( a=>a.LeaderId )
                .WillCascadeOnDelete(false);

            HasRequired(a => a.Contact)
                .WithMany()
                .HasForeignKey(a => a.ContactId)
                .WillCascadeOnDelete(false);
        }
    }

}
