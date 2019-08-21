using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ASPODES.DTO.System
{
    /// <summary>
    /// 添加页面通知DTO
    /// </summary>
    public class AddNoticeDTO
    {
        /// <summary>
        /// 发信人ID
        /// </summary>
        public string SenderId { get; set; }

        /// <summary>
        /// 收信人ID
        /// </summary>
        public int Receiver { get; set; }

        /// <summary>
        /// 通知内容
        /// </summary>
        public string Content { get; set; }

        /// <summary>
        /// 发送时间
        /// </summary>
        public DateTime SendTime { get; set; }
    }
}
