using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace ASPODES.DTO.Application
{
    /// <summary>
    /// 添加年度预算DTO
    /// </summary>
    public class AddAnnualBudgetDTO
    {
        /// <summary>
        /// 年度预算ID
        /// </summary>
        public int? AnnualBudgetId { get; set; }

        /// <summary>
        /// 所属申请书ID
        /// </summary>
        [Required]
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

    }
}
