using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.ComponentModel.DataAnnotations;
using ASPODES.Model;

namespace ASPODES.DTO.Application
{
    /// <summary>
    /// 添加申请书DTO
    /// </summary>
    public class AddApplicationDTO
    {
        /// <summary>
        /// 申请书ID
        /// </summary>
        [Required]
        public string ApplicationId { get; set; }

        /// <summary>
        /// 工作任务名称
        /// </summary>
        [Required]
        public string ProjectName { get; set; }

        /// <summary>
        /// 执行期限
        /// </summary>
        [Required]
        public int? Period { get; set; }

        /// <summary>
        /// 样品类型ID
        /// </summary>
        [Required]
        public int? ProjectTypeId { get; set; }

        /// <summary>
        /// 资助类别
        /// </summary>
        [Required]
        public int? SupportCategoryId { get; set; }

        /// <summary>
        /// 总预算额
        /// </summary>
        [Required]
        public double? TotalBudget { get; set; }

        /// <summary>
        /// 当年预算额
        /// </summary>
        [Required]
        public double? FirstYearBudget { get; set; }

        /// <summary>
        /// 摘要
        /// </summary>
        public string AbstractContent { get; set; }

        /// <summary>
        /// 研究领域1
        /// </summary>
        public string SubFieldId1 { get; set; }

        /// <summary>
        /// 研究领域2
        /// </summary>
        public string SubFieldId2 { get; set; }

        /// <summary>
        /// 委托类型
        /// </summary>
        public DelegateType DeleageType { get; set; }
    }
}
