using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ASPODES.DTO.System
{
    public class GetAnnouncementDTO
    {
        /// <summary>
        /// 获取公告DTO
        /// </summary>
        public GetAnnouncementDTO()
        {
            Attachments = new List<GetAnnouncementAttachmentDTO>();
        }

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
        /// 发布日期
        /// </summary>
        public DateTime PublishDate { get; set; }

        /// <summary>
        /// 发布者ID
        /// </summary>
        public string PublisherId { get; set; }

        /// <summary>
        /// 发布者名称
        /// </summary>
        public string PublisherName { get; set; }

        /// <summary>
        /// 类型
        /// </summary>
        public string Type { get; set; }

        /// <summary>
        /// 附件列表
        /// </summary>
        public List<GetAnnouncementAttachmentDTO> Attachments { get; set; }

    }
}
