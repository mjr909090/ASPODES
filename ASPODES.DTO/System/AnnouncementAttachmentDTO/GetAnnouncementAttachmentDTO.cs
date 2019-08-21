using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ASPODES.DTO.System
{
    /// <summary>
    /// 获取公告附件DTO
    /// </summary>
    public class GetAnnouncementAttachmentDTO
    {
        /// <summary>
        /// 附件ID
        /// </summary>
        public int? AnnouncementAttachmentId { get; set; }

        /// <summary>
        /// 公告ID
        /// </summary>
        //public int? AnnouncementId { get; set; }

        /// <summary>
        /// 附件名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 相对路径
        /// </summary>
        public string RelativeURL { get; set; }
    }
}
