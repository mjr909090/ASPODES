using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ASPODES.Model;
namespace ASPODES.Database
{
    public class NoticeMap : EntityTypeConfiguration<Notice>
    {
        public NoticeMap()
        {
            ToTable("Notice");

            HasKey(n => n.NoticeID);

            HasRequired(n => n.NoticeTemplate)
              .WithMany()
              .HasForeignKey(n => n.NoticeTemplateId)
              .WillCascadeOnDelete(false);

            HasRequired(n => n.Receive)
              .WithMany()
              .HasForeignKey(n => n.ReceiveId)
              .WillCascadeOnDelete(false);

        }
    }

}
