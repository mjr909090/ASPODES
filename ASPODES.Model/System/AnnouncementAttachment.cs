using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ASPODES.Model
{
    /// <summary>
    /// 公告附件
    /// </summary>
    public class AnnouncementAttachment
    {
        /// <summary>
        /// 附件ID
        /// </summary>
        public int? AnnouncementAttachmentId { get; set; }

        /// <summary>
        /// 公告ID，自增
        /// </summary>
        public int? AnnouncementId { get; set; }

        /// <summary>
        /// 附件名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 相对路径
        /// </summary>
        public string RelativeURL { get; set; }

        /// <summary>
        /// 导航属性
        /// </summary>
        public Announcement Announcement { get; set; }
    }
}
