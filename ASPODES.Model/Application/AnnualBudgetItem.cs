
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ASPODES.Model
{
    /// <summary>
    /// 预算科目
    /// </summary>
    public class AnnualBudgetItem
    {
        /// <summary>
        /// 自增主键
        /// </summary>
        public int? AnnualBudgetItemId { get; set; }

        /// <summary>
        /// 会计科目,外键，参照AccountingSubject
        /// </summary>
        [StringLength(30)]
        public string SubjectId { get; set; }
        /// <summary>
        /// 额度
        /// </summary>
        [Required]
        public double? Amount { get; set; }
        /// <summary>
        /// 经费支出依据
        /// </summary>
        [StringLength(1024)]
        public string Reason { get; set; }
        /// <summary>
        /// 所属年度经费预算ID，外键
        /// </summary>
        public int? AnnualBudgetId { get; set; }
        /// <summary>
        /// 导航属性，所属年度经费预算
        /// </summary>
        public virtual AnnualBudget AnnualBudget { get; set; }
        /// <summary>
        /// 导航属性，会计科目
        /// </summary>
        public virtual AccountingSubject Subject { get; set; }
    }
}
