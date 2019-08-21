using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace ASPODES.Model
{
    /// <summary>
    /// 页面通知
    /// </summary>
    public class Notice
    {
        /// <summary>
        /// 代理主键，自增
        /// </summary>
        public int? NoticeID { get; set; }
        
        /// <summary>
        /// 消息通知内容的ID
        /// </summary>
        public int NoticeTemplateId { get; set; }

        /// <summary>
        /// 收信人
        /// </summary>
        [Required, StringLength(256)]
        public string ReceiveId { get; set; }
        
        /// <summary>
        /// 发送时间
        /// </summary>
        [Required]
        public DateTime SendTime { get; set; }

        /// <summary>
        /// 消息是否已读
        /// </summary>
        public bool Read { get; set; }

        /// <summary>
        /// 非模板的通知内容
        /// </summary>
        public string Content { get; set; }

        /// <summary>
        /// 导航属性，通知模板
        /// </summary>
        public virtual NoticeTemplate NoticeTemplate { get; set; }

        /// <summary>
        /// 导航属性， 收信人
        /// </summary>
        public virtual User Receive { get; set; }
                                          
    }

    /// <summary>
    /// 管理员统计申请书信息的存储过程返回的对象
    /// </summary>
    public class SpNotice
    {
        /// <summary>
        /// 生情书数量
        /// </summary>
        public int Number { get; set; }
        
        /// <summary>
        /// 模板内容
        /// </summary>
        public string Content { get; set; }

        /// <summary>
        /// 通知类别
        /// </summary>
        public int NoticeType { get; set; }

        /// <summary>
        /// 接受者类型
        /// </summary>
        public int ReceiverType { get; set; }

        /// <summary>
        /// 跳转链接
        /// </summary>
        public string URL { get; set; }
    }

    /// <summary>
    /// 页面通知状态
    /// </summary>
    public enum NoticeTypes
    {
        /// <summary>
        /// 正常
        /// </summary>
        Normal,
        /// <summary>
        /// 成功
        /// </summary>
        Success,
        /// <summary>
        /// 失败
        /// </summary>
        Fail,
        /// <summary>
        /// 拒绝
        /// </summary>
        Refuse,
        /// <summary>
        /// 警告
        /// </summary>
        Warning,
        /// <summary>
        /// 错误
        /// </summary>
        Error,
        /// <summary>
        /// 等待操作
        /// </summary>
        Pending
    }
}
