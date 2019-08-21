using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ASPODES.Model;


namespace ASPODES.Database
{
    public class AnnouncementAttachmentMap : EntityTypeConfiguration<AnnouncementAttachment>
    {
        public AnnouncementAttachmentMap()
        {
            ToTable("AnnouncementAttachment");

            HasKey(aa => aa.AnnouncementAttachmentId);

            //HasRequired(aa => aa.Announcement)
            //    .WithMany( a=>a.AnnouncementAttachments )
            //    .HasForeignKey(a => a.AnnouncementId)
            //    .WillCascadeOnDelete(true);
 
        }
    }
}
