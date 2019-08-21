using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ASPODES.DTO.Review
{
    /// <summary>
    /// 获取初审专家推荐DTO
    /// </summary>
    public class GetRecommendationDTO
    {
        /// <summary>
        /// 初审专家推荐ID
        /// </summary>
        public int? RecommendationId { get; set; }

        /// <summary>
        /// 单位管理员推荐的专家候选人ID
        /// </summary>
        public string CandidateId { get; set; }

        /// <summary>
        /// 单位管理员推荐的专家候选人人员ID
        /// </summary>
        public int CandidatePersonId { get; set; }

        /// <summary>
        /// 单位管理员推荐的专家候选人名称
        /// </summary>
        public string CandidateName { get; set; }

        /// <summary>
        /// 推荐人ID
        /// </summary>
        public string RecommenderId { get; set; }

        /// <summary>
        /// 推荐人姓名
        /// </summary>
        public string RecommenderName { get; set; }

        /// <summary>
        /// 所在单位ID
        /// </summary>
        public int InstituteId { get; set; }

        /// <summary>
        /// 所在单位名称
        /// </summary>
        public string InstituteName { get; set; }

        /// <summary>
        /// 被推荐人是否同意成为初审专家
        /// NULL：未处理
        /// TRUE:同意
        /// FALSE：拒绝
        /// 被推荐人在同意前需要完善自己的专家信息
        /// </summary>
        public bool? Agree { get; set; }

        /// <summary>
        /// 院管理员是否采纳建议
        /// NULL：院管理员未处理
        /// TRUE：院管理员采纳建议，被推荐人成为初审专家
        /// FALSE：院管理员拒绝采纳
        /// </summary>
        public bool? Adopt { get; set; }

        /// <summary>
        /// 推荐时间
        /// </summary>
        public DateTime Time { get; set; }
        
    }
}
