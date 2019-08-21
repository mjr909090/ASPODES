using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ASPODES.Model;
namespace ASPODES.Database
{
    public class MemberMap : EntityTypeConfiguration<Member>
    {
        public MemberMap()
        {
            ToTable("Member");

            HasKey(m => new { m.PersonId, m.ApplicationId });

            HasRequired(m => m.Person )
                .WithMany()
                .HasForeignKey( m=>m.PersonId )
                .WillCascadeOnDelete(true);

            HasRequired(m => m.Application)
                .WithMany()
                .HasForeignKey(m => m.ApplicationId)
                .WillCascadeOnDelete(true);

        }
    }
}
