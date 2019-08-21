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
    /// 更新年度任务书单位预算DTO
    /// </summary>
    public class UpdateAnnualTaskInstBudgetDTO
    {
        /// <summary>
        /// 年度任务书ID
        /// </summary>
        [Required(ErrorMessage="AnnualTaskId")]
        public int? AnnualTaskId { get; set; }

        /// <summary>
        /// 单位ID
        /// </summary>
        [Required(ErrorMessage = "InstituteId")]
        public int? InstituteId { get; set; }

        /// <summary>
        /// 预算额度
        /// </summary>
        [PositiveNumber(ErrorMessage = "Amount")]
        public double? Amount { get; set; }
    }
}
