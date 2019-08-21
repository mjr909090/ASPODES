using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ASPODES.DTO.System
{
    /// <summary>
    /// 添加邮件DTO
    /// </summary>
    public class AddEmailDTO
    {
        /// <summary>
        /// 发送者ID
        /// </summary>
        public string SenderId { get; set; }

        /// <summary>
        /// 接收者ID
        /// </summary>
        public string ReceiverId { get; set; }

        /// <summary>
        /// 接收地址
        /// </summary>
        public string ReciveAddress { get; set; }

        /// <summary>
        /// 内容
        /// </summary>
        public string Content { get; set; }

        /// <summary>
        /// 发送时间
        /// </summary>
        public DateTime SendTime { get; set; }
    }
}
