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
    /// 初审专家评审意见
    /// </summary>
    public class ReviewComment
    {
        
        /// <summary>
        /// 自增，主键
        /// </summary>
        public int? ReviewCommentId { get; set; }

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
        /// 专家初审意见
        /// </summary>
        [StringLength(1024)]
        public string Comment { get; set; }

        /// <summary>
        /// 资助意见
        /// </summary>
        [MaxLength(8)]
        public string Imburse { get; set; }

        /// <summary>
        /// 专家评定等级
        /// </summary>
        [StringLength(8)]
        public string Level { get; set; }

        /// <summary>
        /// 拟资助金额
        /// </summary>
        public double? Amount { get; set; }

        /// <summary>
        /// 专家评分
        /// </summary>
        public int? Score { get; set; }

        /// <summary>
        /// 评审意见的状态：未评议、保存、提交
        /// 默认值，未评议
        /// </summary>
        public ReviwerCommentStatus Status { get; set; }

        /// <summary>
        /// 评审的年度，和申请书的提交年度相同
        /// </summary>
        public int Year { get; set; }

        public int ReviewAssignmentId { get; set; }

        /// <summary>
        /// 导航属性，所属的初审指派
        /// </summary>
        public virtual ReviewAssignment ReviewAssignment { get; set; }

       
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
    /// 评审意见状态
    /// </summary>
    public enum ReviwerCommentStatus
    {
        /// <summary>
        /// 未评审
        /// </summary>
        NULL,
        /// <summary>
        /// 保存
        /// </summary>
        SAVE,
        /// <summary>
        /// 提交
        /// </summary>
        LOGIN
    }
}
