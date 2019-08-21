using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace ASPODES.Model
{
    public class Announcement
    {
        public Announcement() 
        {
            PublishDate = DateTime.Now;
            Status = "C";
        }

        public int? AnnouncementId { get; set; }
        [Required]
        public string Title { get; set; }

        public string AbstractContent { get; set; }

        [MaxLength,Required]
        public string Content { get; set; }

        public DateTime PublishDate { get; set; }

        public string PublisherId { get; set; }

        //单位ID
        public int? InstituteId { get; set; }

        [StringLength(4)]
        public string Type { get; set; }

        [StringLength(4)]
        public string Status { get; set; }

        public virtual User Publisher { get; set; }

        /// <summary>
        /// 导航属性,公告附件
        /// </summary>
        public virtual ICollection<AnnouncementAttachment> AnnouncementAttachments { get; set; }
    }
}
