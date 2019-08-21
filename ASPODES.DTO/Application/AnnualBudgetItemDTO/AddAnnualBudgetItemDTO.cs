using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace ASPODES.DTO.Application
{
    /// <summary>
    /// 添加预算科目DTO
    /// </summary>
    public class AddAnnualBudgetItemDTO
    {
        /// <summary>
        /// 预算科目ID
        /// </summary>
        public int? AnnualBudgetItemId { get; set; }

        /// <summary>
        /// 会计科目
        /// </summary>
        [Required]
        public string SubjectId { get; set; }

        /// <summary>
        /// 额度
        /// </summary>
        [Required]
        public double? Amount { get; set; }

        /// <summary>
        /// 经费支出依据
        /// </summary>
        public string Reason { get; set; }

        /// <summary>
        /// 所属申请书的ID
        /// </summary>
        [Required]
        public string ApplicationId { get; set; }

        /// <summary>
        /// 年份
        /// </summary>
        [Required]
        public int? Year { get; set; }
    }
}
