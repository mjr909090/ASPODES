using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ASPODES.DTO.Application
{
    /// <summary>
    /// 获取单位经费预算DTO
    /// </summary>
    public class GetInstBudgetDTO
    {
        public GetInstBudgetDTO()
        {
            AnnualBudgets = new List<GetInstAnnualBudgetDTO>();
        }

        /// <summary>
        /// 单位经费预算ID
        /// </summary>
        public int? InstBudgetId { get; set; }

        /// <summary>
        /// 单位名称
        /// </summary>
        public string InstName { get; set; }

        /// <summary>
        /// 所属申请书的ID
        /// </summary>
        public string ApplicationId { get; set; }

        /// <summary>
        /// 单位ID
        /// </summary>
        public int? InstituteId { get; set; }

        /// <summary>
        /// 额度
        /// </summary>
        public double? Amount { get; set; }

        /// <summary>
        /// 单位年度预算列表
        /// </summary>
        public List<GetInstAnnualBudgetDTO> AnnualBudgets { get; set; }
    }
}
