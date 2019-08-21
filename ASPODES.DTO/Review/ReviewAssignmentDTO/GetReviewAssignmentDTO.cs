using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ASPODES.DTO.Review
{
    /// <summary>
    /// 获取初审指派DTO
    /// </summary>
    public class GetReviewAssignmentDTO
    {
        /// <summary>
        /// 初审指派ID
        /// </summary>
        public int? ReviewAssignmentId { get; set; }

        /// <summary>
        /// 待审申请书的ID
        /// </summary>
        public string ApplicationId { get; set; }

        /// <summary>
        /// 单位名称
        /// </summary>
        public string InstititeName { get; set; }

        /// <summary>
        /// 领导人名称
        /// </summary>
        public string LeaderName { get; set; }

        /// <summary>
        /// 项目类型名称
        /// </summary>
        public string ProjectTypeName { get; set; }

        /// <summary>
        /// 项目名
        /// </summary>
        public string ProjectName { get; set; }

        /// <summary>
        /// 初审专家的ID
        /// </summary>
        public string ExpertId { get; set; }

        /// <summary>
        /// 初审专家名称
        /// </summary>
        public string ExpertName { get; set; }

        /// <summary>
        /// 初审专家是否接受指派
        /// </summary>
        public bool? Accept { get; set; }

        /// <summary>
        /// 年份
        /// </summary>
        public int? Year { get; set; }

        /// <summary>
        /// 是否过期失效
        /// </summary>
        public bool? Overdue { get; set; }

        /// <summary>
        /// 初审指派的状态，新建、待确认、拒绝、接受
        /// </summary>
        public int Status { get; set; }

    }
}
