
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ASPODES.Model
{
    /// <summary>
    /// 年度预算
    /// </summary>
    public class AnnualBudget
    {
        /// <summary>
        /// 自增主键
        /// </summary>
        public int? AnnualBudgetId { get; set; }
        /// <summary>
        /// 所属申请书的ID，外键
        /// </summary>
        [StringLength(64)]
        public string ApplicationId { get; set; }
        /// <summary>
        /// 年度预算额度
        /// </summary>
        [Required]
        public double? Amount { get; set; }
        /// <summary>
        /// 年份
        /// </summary>
        [Required]
        public int? Year { get; set; }
        /// <summary>
        /// 导航属性，所属申请书
        /// </summary>
        public virtual Application Application { get; set; }
        ///// <summary>
        ///// 预算科目
        ///// </summary>
        public ICollection<AnnualBudgetItem> Items { get; set; }

    }
}