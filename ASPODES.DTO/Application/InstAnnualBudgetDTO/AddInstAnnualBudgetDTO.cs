using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace ASPODES.DTO.Application
{
    /// <summary>
    /// 添加单位年度预算DTO
    /// </summary>
    public class AddInstAnnualBudgetDTO
    {
        /// <summary>
        /// 单位年度预算ID
        /// </summary>
        public int? InstAnnualBudgetId { get; set; }

        /// <summary>
        /// 所属单位预算ID
        /// </summary>
        [Required]
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
    }
}
