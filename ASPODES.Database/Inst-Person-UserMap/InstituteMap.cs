using ASPODES.Model;
using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ASPODES.Database
{
    public class InstituteMap : EntityTypeConfiguration<Institute>
    {
        public InstituteMap()
        {
            ToTable("Institute");

            HasKey(i => i.InstituteId);

            HasOptional(i => i.Contact)
                .WithMany()
                .HasForeignKey(i => i.ContactId)
                .WillCascadeOnDelete(false);
        }
    }
}
