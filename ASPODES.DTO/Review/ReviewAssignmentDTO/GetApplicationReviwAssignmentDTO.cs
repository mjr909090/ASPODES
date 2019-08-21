using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ASPODES.DTO.Review
{
    /// <summary>
    /// 获取申请书初审专家评审意见DTO
    /// </summary>
    public class GetApplicationReviwAssignmentDTO
    {
        public GetApplicationReviwAssignmentDTO()
        {
            ReviewAssignments = new List<GetReviewAssignmentVO>();
        }

        /// <summary>
        /// 当前年份
        /// </summary>
        public int? CurrentYear { get; set; }

        /// <summary>
        /// 待审申请书的ID
        /// </summary>
        public string ApplicationId { get; set; }

        /// <summary>
        /// 项目名称
        /// </summary>
        public string ProjectName { get; set; }

        /// <summary>
        /// 醒目类型ID
        /// </summary>
        public int? ProjectTypeId { get; set; }

        /// <summary>
        /// 项目类型名称
        /// </summary>
        public string ProjectTypeName { get; set; }

        /// <summary>
        /// 所属单位ID
        /// </summary>
        public int? InstituteId { get; set; }

        /// <summary>
        /// 所属单位名称
        /// </summary>
        public string InstituteName { get; set; }

        /// <summary>
        /// 领导人ID
        /// </summary>
        public int? LeaderId { get; set; }

        /// <summary>
        /// 领导人名称
        /// </summary>
        public string LeaderName { get; set; }

        /// <summary>
        /// 初审指派列表
        /// </summary>
        public List<GetReviewAssignmentVO> ReviewAssignments { get; set; }
    }

    /// <summary>
    /// 获取初审指派列表DTO
    /// </summary>
    public class GetReviewAssignmentVO
    {
        /// <summary>
        /// 初审指派ID
        /// </summary>
        public int ReviewAssignmentId { get; set; }

        /// <summary>
        /// 初审专家的ID
        /// </summary>
        public string ExpertId { get; set; }

        /// <summary>
        /// 初审专家的名称
        /// </summary>
        public string ExpertName { get; set; }

        /// <summary>
        /// 初审专家是否接受指派
        /// </summary>
        public bool? Accept { get; set; }

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
