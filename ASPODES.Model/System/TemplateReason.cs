using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace ASPODES.Model
{
    /// <summary>
    /// 原因模板
    /// </summary>
    public class TemplateReason
    {
        /// <summary>
        /// 代理主键，自增
        /// </summary>
        public int? ReasonId { get; set; }
        /// <summary>
        /// 编辑人
        /// </summary>
        public string EditorId { get; set; }
        /// <summary>
        /// 原因类型
        /// </summary>
        [Required]
        public int? Type { get; set; }
        /// <summary>
        /// 内容
        /// </summary>
        [Required,StringLength(256)]
        public string Content { get; set; }

        /// <summary>
        /// 添加/修改时间
        /// </summary>
        public DateTime? EditTime { get; set; }
    }

    /// <summary>
    /// 原因模板
    /// </summary>
    public enum TemplateReasonType
    {
        /// <summary>
        /// 驳回理由
        /// </summary>
        REJECT,
        /// <summary>
        /// 撤回理由
        /// </summary>
        CANCEL,
        /// <summary>
        /// 不受理理由
        /// </summary>
        DECLINE,
        /// <summary>
        /// 拒审理由
        /// </summary>
        REFUSE_REVIEW,
        /// <summary>
        /// 不资助理由
        /// </summary>
        NON_FUND,

    }
}
