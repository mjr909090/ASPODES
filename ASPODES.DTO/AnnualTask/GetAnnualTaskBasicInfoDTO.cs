using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ASPODES.DTO.AnnualTask
{
    /// <summary>
    /// 获取年度任务书基本信息DTO
    /// </summary>
    public class GetAnnualTaskBasicInfoDTO
    {
        /// <summary>
        /// 年度任务书编号
        /// </summary>
        public int? AnnualTaskId { get; set; }

        /// <summary>
        /// 年度任务书名称
        /// </summary>
        public string Name { get { return ProjectName + "第" + Year + "年度任务"; } }

        /// <summary>
        /// 项目ID
        /// </summary>
        public string ProjectId { get; set; }

        /// <summary>
        /// 牵头负责人ID
        /// </summary>
        public int? LeaderId { get; set; }

        /// <summary>
        /// 牵头负责人名称
        /// </summary>
        public string LeaderName { get; set; }

        /// <summary>
        /// 承担单位ID
        /// </summary>
        public int? InstituteId { get; set; }

        /// <summary>
        /// 承担单位名称
        /// </summary>
        public string InstituteName { get; set; }

        /// <summary>
        /// 项目名称
        /// </summary>
        public string ProjectName { get; set; }

        /// <summary>
        /// 项目类型
        /// </summary>
        public string ProjectTypeName { get; set; }

        /// <summary>
        /// 年份,和申请年度相同，即System.ApplyYear
        /// </summary>
        public int? Year { get; set; }

        /// <summary>
        /// 总预算
        /// </summary>
        public double? TotalBudget { get; set; }

        /// <summary>
        /// 当前预算
        /// </summary>
        public double? CurrentBudget { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        public string Comment { get; set; }

        /// <summary>
        /// 年度任务书状态
        /// </summary>
        public int Status { get; set; }
    }
}
