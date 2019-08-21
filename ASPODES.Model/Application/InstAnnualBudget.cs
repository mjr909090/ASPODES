using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace ASPODES.Model
{
    /// <summary>
    /// 单位年度预算
    /// </summary>
    public class InstAnnualBudget
    {
        /// <summary>
        /// 主键，自增
        /// </summary>
        public int? InstAnnualBudgetId { get; set; }

        /// <summary>
        /// 所属单位预算ID，外键
        /// </summary>
        public int? InstBudgetId { get; set; }
        /// <summary>
        /// 年份
        /// </summary>
        [Required]
        public int? Year { get; set; }
        /// <summary>
        /// 机构年度预算额
        /// </summary>
        [Required]
        public double? Amount { get; set; }
        /// <summary>
        /// 导航属性，所属单位预算
        /// </summary>
        public InstBudget InstBudget { get; set; }

    }
}
