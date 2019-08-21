using ASPODES.Model;
using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ASPODES.Database
{
    public class ApplicationFieldMap : EntityTypeConfiguration<ApplicationField>
    {
        public ApplicationFieldMap() 
        {
            ToTable("ApplicationField");

            HasKey(af => af.ApplicationFieldId);

            HasRequired(af => af.SubField)
                .WithMany()
                .HasForeignKey(af => af.SubFieldId)
                .WillCascadeOnDelete(false);

            HasRequired(af => af.Application)
                .WithMany()
                .HasForeignKey(af => af.ApplicationId)
                .WillCascadeOnDelete(true);

        }
    }
}
