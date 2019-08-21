using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ASPODES.Model
{
    /// <summary>
    /// 初审指派，为申请书指派初审专家
    /// </summary>
    public class ReviewAssignment
    {
        /// <summary>
        /// 自增主键
        /// </summary>
        public int ReviewAssignmentId { get; set; }
        
        /// <summary>
        /// 待审申请书的ID，外键
        /// </summary>
        [StringLength(64)]
        public string ApplicationId { get; set; }
        
        /// <summary>
        /// 初审专家的ID,即初审专家的注册邮箱，外键
        /// </summary>
        [StringLength(256)]
        public string ExpertId { get; set; }
        /// <summary>
        /// 初审专家是否接受指派
        /// </summary>
        public bool? Accept { get; set; }

        /// <summary>
        /// 年份
        /// </summary>
        [Required]
        public int? Year { get; set; }

        /// <summary>
        /// 初审指派的状态，新建、待确认、拒绝、接受
        /// 默认值，新建
        /// </summary>
        [Required]
        public ReviewAssignmentStatus Status { get; set; }

        /// <summary>
        /// 是否过期失效.默认值FALSE
        /// </summary>
        [Required]
        public bool? Overdue { get; set; }

        /// <summary>
        /// 导航属性，初审专家
        /// </summary>
        public virtual User Expert { get; set; }

        /// <summary>
        /// 导航属性，待评审申请书
        /// </summary>
        public virtual Application Application { get; set; }
    }

    /// <summary>
    /// 初审指派状态
    /// </summary>
    public enum ReviewAssignmentStatus
    {
        /// <summary>
        /// 新建
        /// </summary>
        NEW,

        /// <summary>
        /// 待确认
        /// </summary>
        REPLY,

        /// <summary>
        /// 接受
        /// </summary>
        ACCEPT,

        /// <summary>
        /// 拒绝
        /// </summary>
        REFUSE,

        /// <summary>
        /// 调整
        /// </summary>
        CHANGE,

        /// <summary>
        /// 待评审
        /// </summary>
        WAITE_REVIEW,//5

        /// <summary>
        /// 已评审
        /// </summary>
        ALREADY_REVIEW

    }
}
