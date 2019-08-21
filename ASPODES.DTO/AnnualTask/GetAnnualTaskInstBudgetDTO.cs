using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ASPODES.DTO.AnnualTask
{
    /// <summary>
    /// 获取年度任务书单位预算DTO
    /// </summary>
    public class GetAnnualTaskInstBudgetDTO
    {
        /// <summary>
        /// 年度任务书ID
        /// </summary>
        public int? AnnualTaskId { get; set; }

        /// <summary>
        /// 单位ID
        /// </summary>
        public int? InstituteId { get; set; }

        /// <summary>
        /// 单位名称
        /// </summary>
        public string InstituteName { get; set; }

        /// <summary>
        /// 预算额度
        /// </summary>
        public double? Amount { get; set; }
    }
}
