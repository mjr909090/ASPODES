using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ASPODES.DTO.Review
{
    /// <summary>
    /// 添加初审专家评审意见DTO
    /// </summary>
    public class AddReviewCommentDTO
    {
        /// <summary>
        /// 初审指派ID
        /// </summary>
        public int ReviewAssignmentId { get; set; }

        /// <summary>
        /// 专家初审意见
        /// </summary>
        public string Comment { get; set; }

        /// <summary>
        /// 专家评定等级
        /// </summary>
        public string Level { get; set; }

        /// <summary>
        /// 资助意见
        /// </summary>
        public string Imburse { get; set; }

        /// <summary>
        /// 专家评分
        /// </summary>
        public int? Score { get; set; }

        /// <summary>
        /// 拟资助金额
        /// </summary>
        public double Amount{get;set;}
    }
}
