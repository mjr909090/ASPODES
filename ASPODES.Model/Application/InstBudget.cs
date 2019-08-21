using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ASPODES.Model
{
    /// <summary>
    /// 单位经费预算
    /// </summary>
    public class InstBudget
    {
        /// <summary>
        /// 自增主键
        /// </summary>
        public int? InstBudgetId { get; set; }

        /// <summary>
        /// 所属申请书的ID，外键
        /// </summary>
        [StringLength(64)]
        public string ApplicationId { get; set; }

        /// <summary>
        /// 单位ID，外键
        /// </summary>
        public int? InstituteId { get; set; }

        /// <summary>
        /// 额度
        /// </summary>
        [Required]
        public double? Amount { get; set; }

        /// <summary>
        /// 导航属性，所属申请书
        /// </summary>
        public virtual Application Application { get; set; }

        /// <summary>
        /// 导航属性，单位
        /// </summary>
        public virtual Institute Institute { get; set; }

        /// <summary>
        /// 导航属性，单位年度预算
        /// </summary>
        public ICollection<InstAnnualBudget> InstAnnualBudgets { get; set; }

    }
}