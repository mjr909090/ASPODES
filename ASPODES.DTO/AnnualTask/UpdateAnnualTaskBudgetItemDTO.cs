using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using ASPODES.Common;

namespace ASPODES.DTO.AnnualTask
{
    /// <summary>
    /// 更新出库项目年度预算
    /// </summary>
    public class UpdateAnnualTaskBudgetItemDTO
    {
        /// <summary>
        /// 出库项目年度预算ID
        /// </summary>
        [Required]
        public int? AnnualTaskBudgetItemId { get; set; }

        /// <summary>
        /// 年度预算额度
        /// </summary>
        [PositiveNumber]
        public double? Amount { get; set; }

        /// <summary>
        /// 经费支出依据
        /// </summary>
        public string Reason { get; set; }
    }
}
