using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ASPODES.Model;
namespace ASPODES.Database
{
    public class SmsMap : EntityTypeConfiguration<Sms>
    {
        public SmsMap()
        {
            ToTable("Sms");

            HasKey(f => f.SmsId);

            HasRequired(f => f.User)
               .WithMany()
               .HasForeignKey(f => f.UserId)
               .WillCascadeOnDelete(true);
        }
    }

}
