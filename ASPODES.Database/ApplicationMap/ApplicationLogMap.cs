using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity.ModelConfiguration;
using ASPODES.Model;

namespace ASPODES.Database
{
    public class ApplicationLogMap:EntityTypeConfiguration<ApplicationLog>
    {
        public ApplicationLogMap()
        {
            ToTable("ApplicationLog");

            HasKey( al=>al.ApplicationLogId );

            HasRequired(al => al.Application)
                .WithMany()
                .HasForeignKey(al => al.ApplicationId)
                .WillCascadeOnDelete(true);

            HasRequired(al => al.User)
                .WithMany()
                .HasForeignKey(al => al.UserId)
                .WillCascadeOnDelete(false);

        }

    }
}
