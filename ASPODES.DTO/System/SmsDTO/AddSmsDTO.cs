using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ASPODES.DTO.System
{
    /// <summary>
    /// 添加通知DTO
    /// </summary>
    public class AddSmsDTO
    {
        /// <summary>
        /// 通知类型
        /// </summary>
        public string SmsType { get; set; }

        /// <summary>
        /// 手机
        /// </summary>
        public string Phone { get; set; }

        /// <summary>
        /// 通知内容
        /// </summary>
        public string SmsContent { get; set; }

        /// <summary>
        /// 发送时间
        /// </summary>
        public DateTime SendTime { get; set; }

        /// <summary>
        /// 接收用户ID
        /// </summary>
        public string UserId { get; set; }

        /// <summary>
        /// 接收用户名
        /// </summary>
        public string UserName { get; set; }


    }
}
