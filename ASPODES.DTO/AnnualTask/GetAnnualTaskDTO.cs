using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ASPODES.DTO.AnnualTask
{
    /// <summary>
    /// 获取年度任务书DTO
    /// </summary>
    public class GetAnnualTaskDTO
    {
        /// <summary>
        /// 获取年度任务书基本信息
        /// </summary>
        public GetAnnualTaskBasicInfoDTO Bsic { get; set; }

        /// <summary>
        /// 科目预算项
        /// </summary>
        public List<GetAnnualTaskBudgetItemDTO> BudgetItems { get; set; }

        /// <summary>
        /// 单位预算项
        /// </summary>
        public List<GetAnnualTaskInstBudgetDTO> InstBudgets { get; set; }

        /// <summary>
        /// 文档
        /// </summary>
        public List<GetAnnualTaskDocDTO> Docs { get; set; }
    }
}
