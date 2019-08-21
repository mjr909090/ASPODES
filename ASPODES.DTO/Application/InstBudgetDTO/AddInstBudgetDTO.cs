using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace ASPODES.DTO.Application
{
    /// <summary>
    /// 添加单位经费预算DTO
    /// </summary>
    public class AddInstBudgetDTO
    {
        /// <summary>
        /// 单位经费预算ID
        /// </summary>
        public int? InstBudgetId { get; set; }

        /// <summary>
        /// 所属申请书的ID
        /// </summary>
        [Required]
        public string ApplicationId { get; set; }

        /// <summary>
        /// 单位ID
        /// </summary>
        [Required]
        public int? InstituteId { get; set; }

        /// <summary>
        /// 额度
        /// </summary>
        [Required]
        public double? Amount { get; set; }
    }
}
