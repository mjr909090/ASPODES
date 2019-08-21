using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace ASPODES.Model
{
    /// <summary>
    /// 邮件通知
    /// </summary>
    public class Email
    {
        public Email()
        {
            Status = EmailStatus.SEND_FIRST;
        }
        /// <summary>
        /// 代理主键，自增
        /// </summary>
        public int? EmailId { get; set; }
        /// <summary>
        /// 此处是发信人的邮箱
        /// </summary>
        [StringLength(256)]
        public string SenderId { get; set; }
        /// <summary>
        /// 收信人,外键，参照User
        /// </summary>
        [StringLength(256)]
        public string ReceiverId { get; set; }
        /// <summary>
        /// 接收邮箱
        /// </summary>
        [Required, StringLength(256)]
        public string ReciveAddress { get; set; }

        public string Subject { get; set; }
        /// <summary>
        /// 邮件内容
        /// </summary>
        [Required, StringLength(256)]
        public string Content { get; set; }
        /// <summary>
        /// 验证码
        /// </summary>
        public string IdentifyCode { get; set; }
        /// <summary>
        /// 发送时间
        /// </summary>
        [Required]
        public DateTime SendTime { get; set; }

        /// <summary>
        /// 状态
        /// </summary>
        public EmailStatus Status { get; set; }

        /// <summary>
        /// 导航属性，收信人
        /// </summary>
        public User Receiver { get; set; }
    }

    /// <summary>
    /// 邮件发送状态
    /// </summary>
    public enum EmailStatus
    {
        /// <summary>
        /// 第一次发送
        /// </summary>
        SEND_FIRST,
        /// <summary>
        /// 第二次发送
        /// </summary>
        SEND_SECOND,
        /// <summary>
        /// 第三次发送
        /// </summary>
        SEND_THIRD,
        /// <summary>
        /// 失败
        /// </summary>
        FAIL,
        /// <summary>
        /// 成功
        /// </summary>
        SUCCESS
    }
}
