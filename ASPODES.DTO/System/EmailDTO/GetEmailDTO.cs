﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ASPODES.DTO.System
{
    /// <summary>
    /// 获取邮件DTO
    /// </summary>
    public class GetEmailDTO
    {
        /// <summary>
        /// 邮件ID
        /// </summary>
        public int? EmailId { get; set; }

        /// <summary>
        /// 发送者ID
        /// </summary>
        public string SenderId { get; set; }

        /// <summary>
        /// 接受者ID
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
