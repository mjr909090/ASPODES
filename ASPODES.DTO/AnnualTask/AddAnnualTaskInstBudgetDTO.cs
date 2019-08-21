using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ASPODES.Common;
using System.ComponentModel.DataAnnotations;

namespace ASPODES.DTO.AnnualTask
{
    /// <summary>
    /// 年度任务书单位预算DTO
    /// </summary>
    public class AddAnnualTaskInstBudgetDTO
    {
        /// <summary>
        /// 年度任务书ID
        /// 组合主键
        /// </summary>
        [Required]
        public int? AnnualTaskId { get; set; }

        /// <summary>
        /// 单位ID
        /// 组合主键
        /// </summary>
        [Required]
        public int? InstituteId { get; set; }

        /// <summary>
        /// 预算额度
        /// </summary>
        [PositiveNumber]
        public double? Amount { get; set; }
    }
}
