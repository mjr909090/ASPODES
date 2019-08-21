using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ASPODES.DTO.System
{
    /// <summary>
    /// 通知DTO
    /// </summary>
    public class NoticeDTO
    {
        /// <summary>
        /// 通知ID
        /// </summary>
        public int? NoticeID { get; set; }

        /// <summary>
        /// 收信人
        /// </summary>
        public string ReceiveId { get; set; }

        /// <summary>
        /// 消息类型
        /// </summary>
        public string Type { get; set; }

        /// <summary>
        /// 通知内容
        /// </summary>
        public string NoticeContent { get; set; }

        /// <summary>
        /// 发送时间
        /// </summary>
        public DateTime SendTime { get; set; }

        /// <summary>
        /// 跳转URL
        /// </summary>
        public string URL { get; set; }

        /// <summary>
        /// 消息接受者类型
        /// </summary>
        public int ReciverType { get; set; }

        /// <summary>
        /// 消息是否已读
        /// </summary>
        public bool Read { get; set; }
    }
}
