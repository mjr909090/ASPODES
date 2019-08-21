using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ASPODES.Model;
namespace ASPODES.Database
{
    public class EmailMap : EntityTypeConfiguration<Email>
    {
        public EmailMap()
        {
            ToTable("Email");

            HasKey(e => e.EmailId);

            HasRequired(e => e.Receiver)
                .WithMany()
                .HasForeignKey(e => e.ReceiverId)
                .WillCascadeOnDelete(false);
 
        }
    }

}
