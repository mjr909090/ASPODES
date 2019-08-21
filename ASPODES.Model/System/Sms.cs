using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace ASPODES.Model
{
    /// <summary>
    /// 通知
    /// </summary>
    public class Sms
    {
        /// <summary>
        /// 自增主键
        /// </summary>
        public int? SmsId{get;set;}
        /// <summary>
        /// 通知类型
        /// </summary>
        [StringLength(128)]
        public string SmsType{get;set;}
        /// <summary>
        /// 手机
        /// </summary>
        [Required,StringLength(20)]
        public string Phone{get;set;}
        /// <summary>
        /// 通知内容
        /// </summary>
        [Required, StringLength(250)]
        public string SmsContent{get;set;}
        /// <summary>
        /// 发送时间
        /// </summary>
        public DateTime SendTime{get;set;}
        /// <summary>
        /// 发送状态
        /// </summary>
        [StringLength(32)]
        public string Status{get;set;}
        /// <summary>
        /// 接受用户ID
        /// </summary>
        public string UserId{get;set;}
        /// <summary>
        /// 接受用户名
        /// </summary>
        [StringLength(256)]
        public string UserName { get; set; }
        /// <summary>
        /// 导航属性，User
        /// </summary>
        public virtual User User { get; set; }
    }
}
