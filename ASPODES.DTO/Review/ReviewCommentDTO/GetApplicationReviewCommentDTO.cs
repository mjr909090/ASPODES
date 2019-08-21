using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ASPODES.DTO.Review
{
    /// <summary>
    /// 获取申请书评审意见DTO
    /// </summary>
    public class GetApplicationReviewCommentDTO
    {
        public GetApplicationReviewCommentDTO()
        {
            ReviewComments = new List<GetReviewCommentVO>();
        }

        /// <summary>
        /// 申请书ID
        /// </summary>
        public string ApplicationId { get; set; }

        /// <summary>
        /// 项目名称
        /// </summary>
        public string ProjectName { get; set; }

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
        /// 执行期限
        /// </summary>
        public int? Period { get; set; }

        /// <summary>
        /// 总预算
        /// </summary>
        public double? TotalBudget { get; set; }

        /// <summary>
        /// 第一年支出预算
        /// </summary>
        public double? FirstYearBudget { get; set; }

        /// <summary>
        /// 申请书初审评分
        /// </summary>
        public double? TotalScore { get; set; }

        /// <summary>
        /// 初审专家评审意见列表
        /// </summary>
        public ICollection<GetReviewCommentVO> ReviewComments { get; set; } 
    }

    /// <summary>
    /// 获取初审专家评审意见DTO
    /// </summary>
    public class GetReviewCommentVO
    {
        /// <summary>
        /// 初审专家评审意见ID
        /// </summary>
        public int ReviewCommentId { get; set; }

        /// <summary>
        /// 初审专家的ID
        /// </summary>
        public string ExpertId { get; set; }

        /// <summary>
        /// 初审专家的名称
        /// </summary>
        public string ExpertName { get; set; }

        /// <summary>
        /// 资助意见
        /// </summary>
        public string Imburse { get; set; }

        /// <summary>
        /// 专家评分
        /// </summary>
        public int? Score { get; set; }

        /// <summary>
        /// 专家初审意见
        /// </summary>
        public string Comment { get; set; }

        /// <summary>
        /// 专家评定等级
        /// </summary>
        public string Level { get; set; }

        /// <summary>
        /// 拟资助金额
        /// </summary>
        public double? Amount { get; set; }
    }
}
