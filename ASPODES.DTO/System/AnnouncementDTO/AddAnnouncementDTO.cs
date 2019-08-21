using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ASPODES.DTO.System
{
    /// <summary>
    /// 添加公告DTO
    /// </summary>
    public class AddAnnouncementDTO
    {
        /// <summary>
        /// 公告ID
        /// </summary>
        public int? AnnouncementId { get; set; }

        /// <summary>
        /// 标题
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// 简介
        /// </summary>
        public string AbstractContent { get; set; }

        /// <summary>
        /// 内容
        /// </summary>
        public string Content { get; set; }

        /// <summary>
        /// 类型
        /// </summary>
        public string Type { get; set; }

        /// <summary>
        /// 公告所包含的附件的ID数组
        /// </summary>
        public int[] Attachments { get; set; }
    }
}
