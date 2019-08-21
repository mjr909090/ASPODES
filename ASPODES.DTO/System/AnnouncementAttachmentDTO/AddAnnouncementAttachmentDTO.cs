using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ASPODES.DTO.System
{
    /// <summary>
    /// 添加公告附件DTO
    /// </summary>
    public class AddAnnouncementAttachmentDTO
    {
        /// <summary>
        /// 公告ID
        /// </summary>
        [Required]
        public int? AnnouncementId { get; set; }

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
