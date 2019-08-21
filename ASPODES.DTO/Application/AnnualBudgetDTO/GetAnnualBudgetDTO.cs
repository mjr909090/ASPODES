using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ASPODES.DTO.Application
{
    /// <summary>
    /// 获取年度预算DTO
    /// </summary>
    public class GetAnnualBudgetDTO
    {
        public GetAnnualBudgetDTO()
        {
            Items = new List<GetAnnualBudgetItemDTO>();
        }

        /// <summary>
        /// 年度预算ID
        /// </summary>
        public int? AnnualBudgetId { get; set; }

        /// <summary>
        /// 所属申请书的ID
        /// </summary>
        public string ApplicationId { get; set; }

        /// <summary>
        /// 年度预算额度
        /// </summary>
        public double? Amount { get; set; }

        /// <summary>
        /// 年份
        /// </summary>
        public int? Year { get; set; }

        /// <summary>
        /// 预算科目DTO列表
        /// </summary>
        public List<GetAnnualBudgetItemDTO> Items { get; set; }
    }
}
