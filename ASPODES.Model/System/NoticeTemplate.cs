using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ASPODES.Model
{
    /// <summary>
    /// 通知模板
    /// </summary>
    public class NoticeTemplate
    {
        /// <summary>
        /// 通知消息模板的ID，自增
        /// </summary>
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]//不自动增长
        public int Id { get; set; }

        /// <summary>
        /// 通知消息模板的内容 
        /// </summary>
        [Required]
        public string Content { get; set; }

        /// <summary>
        /// 消息类型
        /// </summary>
        public NoticeTypes NoticeType { get; set; }

        /// <summary>
        /// 跳转url
        /// </summary>
        [StringLength(512)]
        public string URL { get; set; }

        /// <summary>
        /// 接收此消息的用户类型
        /// </summary>
        public ReceiverType ReceiverType { get; set; }
    }

    /// <summary>
    /// 接收此消息的用户类型
    /// </summary>
    public enum ReceiverType
    {
        /// <summary>
        /// 所有用户 0
        /// </summary>
        All,
        /// <summary>
        /// 普通用户 1
        /// </summary>
        User,

        /// <summary>
        /// 单位管理员 2
        /// </summary>
        InstManager,

        /// <summary>
        /// 院管理员 3
        /// </summary>
        DeptManager
    }
}
