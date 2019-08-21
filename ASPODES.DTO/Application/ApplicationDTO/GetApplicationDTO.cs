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
    /// 获取申请书DTO
    /// </summary>
    public class GetApplicationDTO
    {
        /// <summary>
        /// 申请书ID
        /// </summary>
        public string ApplicationId { get; set; }

        /// <summary>
        /// 工作任务名称
        /// </summary>
        public string ProjectName { get; set; }

        /// <summary>
        /// 执行期限
        /// </summary>
        public int? Period { get; set; }

        /// <summary>
        /// 样品类型ID
        /// </summary>
        public int? ProjectTypeId { get; set; }

        /// <summary>
        /// 样品类型名称
        /// </summary>
        public string ProjectTypeName { get; set; }

        /// <summary>
        /// 资助类别ID
        /// </summary>
        public int? SupportCategoryId { get; set; }

        /// <summary>
        /// 资助类别名称
        /// </summary>
        public string SupportCategoryName { get; set; }

        /// <summary>
        /// 承担单位ID
        /// </summary>
        public int? InstituteId { get; set; }

        /// <summary>
        /// 承担单位名称
        /// </summary>
        public string InstituteName { get; set; }

        /// <summary>
        /// 项目负责人ID
        /// </summary>
        public int? LeaderId { get; set; }

        /// <summary>
        /// 项目负责人名称
        /// </summary>
        public string LeaderName { get; set; }

        /// <summary>
        /// 项目负责人电话
        /// </summary>
        public string LeaderPhone { get; set; }

        /// <summary>
        /// 项目负责人邮箱
        /// </summary>
        public string LeaderEmail { get; set; }
        
        public string ContactId { get; set; }

        public string ContactPhone { get; set; }

        public string ContactEmail { get; set; }

        /// <summary>
        /// 总预算额
        /// </summary>
        public double? TotalBudget { get; set; }

        /// <summary>
        /// 当年预算额
        /// </summary>
        public double? FirstYearBudget { get; set; }

        /// <summary>
        /// 首次申报年份
        /// </summary>
        public int? YearCreated { get; set; }

        /// <summary>
        /// 本次申报年份
        /// </summary>
        public int? CurrentYear { get; set; }

        /// <summary>
        /// 摘要
        /// </summary>
        public string AbstractContent { get; set; }

        /// <summary>
        /// 委托类型
        /// </summary>
        public DelegateType DeleageType { get; set; }

        /// <summary>
        /// 申请书的状态
        /// </summary>
        public int Status { get; set; }

        public string PDF { get; set; }
    }
}
