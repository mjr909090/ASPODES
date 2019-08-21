using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity.ModelConfiguration;
using ASPODES.Model;

namespace ASPODES.Database
{
    public class ApplicationDocMap : EntityTypeConfiguration<ApplicationDoc>
    {
        public ApplicationDocMap()
        {
            ToTable("ApplicationDoc");

            HasKey(ad => ad.ApplicationDocId );

            HasRequired(ad => ad.Application)
                .WithMany()
                .HasForeignKey(ad => ad.ApplicationId)
                .WillCascadeOnDelete(true);
        }
    }
}
