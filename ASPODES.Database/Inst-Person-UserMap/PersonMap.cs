using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity.ModelConfiguration;
using ASPODES.Model;

namespace ASPODES.Database
{
    public class PersonMap:EntityTypeConfiguration<Person>
    {
        public PersonMap()
        {
            ToTable("Person");

            HasKey( p=>p.PersonId );

            HasRequired(p => p.Institute)
                .WithMany()
                .HasForeignKey(p => p.InstituteId)
                .WillCascadeOnDelete(false);

        }
    }
}
