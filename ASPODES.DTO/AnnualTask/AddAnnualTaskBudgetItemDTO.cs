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
    /// 添加出库项目年度预算DTO
    /// </summary>
    public class AddAnnualTaskBudgetItemDTO
    {
        /// <summary>
        /// 所属出库项目的ID
        /// </summary>
        [Required]
        public int? AnnualTaskId { get; set; }

        /// <summary>
        /// 支出科目
        /// </summary>
        [Required]
        public string SubjectId { get; set; }

        /// <summary>
        /// 年度预算额度
        /// </summary>
        [PositiveNumber()]
        public double? Amount { get; set; }

        /// <summary>
        /// 经费支出依据
        /// </summary>
        public string Reason { get; set; }
    }
}
