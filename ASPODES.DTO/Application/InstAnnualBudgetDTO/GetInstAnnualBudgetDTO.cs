using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ASPODES.DTO.Application
{
    /// <summary>
    /// 获取单位年度预算DTO
    /// </summary>
    public class GetInstAnnualBudgetDTO
    {
        /// <summary>
        /// 单位年度预算ID
        /// </summary>
        public int? InstAnnualBudgetId { get; set; }

        /// <summary>
        /// 所属单位预算ID
        /// </summary>
        public int? InstBudgetId { get; set; }

        /// <summary>
        /// 年份
        /// </summary>
        public int? Year { get; set; }

        /// <summary>
        /// 机构年度预算额
        /// </summary>
        public double? Amount { get; set; }
    }
}
