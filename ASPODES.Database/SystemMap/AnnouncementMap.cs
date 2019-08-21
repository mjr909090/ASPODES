using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ASPODES.Model;
namespace ASPODES.Database
{
    public class AnnouncementMap : EntityTypeConfiguration<Announcement>
    {
        public AnnouncementMap()
        {
            ToTable("Announcement");
            HasKey(a => a.AnnouncementId);

            HasRequired(a => a.Publisher)
                .WithMany()
                .HasForeignKey(a => a.PublisherId)
                .WillCascadeOnDelete(false);
        }
    }
}
