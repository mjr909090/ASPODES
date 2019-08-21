using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ASPODES.DTO.AnnualTask
{
    /// <summary>
    /// 获取出库项目年度预算DTO
    /// </summary>
    public class GetAnnualTaskBudgetItemDTO
    {
        /// <summary>
        /// ID
        /// </summary>
        public int? AnnualTaskBudgetItemId { get; set; }

        /// <summary>
        /// 支出科目ID
        /// </summary>
        public string SubjectId { get; set; }

        /// <summary>
        /// 支出科目名称
        /// </summary>
        public string SubjectName { get; set; }

        /// <summary>
        /// 年度预算额度
        /// </summary>
        public double? Amount { get; set; }

        /// <summary>
        /// 经费支出依据
        /// </summary>
        public string Reason { get; set; }
    }
}
