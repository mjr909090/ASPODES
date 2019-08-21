using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ASPODES.DTO.Application
{
    /// <summary>
    /// 获取预算科目DTO
    /// </summary>
    public class GetAnnualBudgetItemDTO
    {
        /// <summary>
        /// 预算科目ID
        /// </summary>
        public int? AnnualBudgetItemId { get; set; }

        /// <summary>
        /// 会计科目
        /// </summary>
        public string SubjectId { get; set; }

        /// <summary>
        /// 会计科目名称
        /// </summary>
        public string SubjectName { get; set; }

        /// <summary>
        /// 额度
        /// </summary>
        public double? Amount { get; set; }

        /// <summary>
        /// 经费支出依据
        /// </summary>
        public string Reason { get; set; }

        /// <summary>
        /// 所属年度经费预算ID
        /// </summary>
        public int? AnnualBudgetId { get; set; }
        
    }
}
