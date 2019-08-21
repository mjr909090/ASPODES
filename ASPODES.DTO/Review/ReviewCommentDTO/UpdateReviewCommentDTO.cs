using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ASPODES.DTO.Review
{
    /// <summary>
    /// 修改初审专家评审意见DTO
    /// </summary>
    public class UpdateReviewCommentDTO
    {
        /// <summary>
        /// 初审专家评审意见ID
        /// </summary>
        public int ReviewCommentId { get; set; }

        /// <summary>
        /// 待审申请书的ID
        /// </summary>
        public int? ApplicationId { get; set; }

        /// <summary>
        /// 初审专家的ID
        /// </summary>
        public string ExpertId { get; set; }

        /// <summary>
        /// 专家初审意见
        /// </summary>
        public string Comment { get; set; }

        /// <summary>
        /// 资助意见
        /// </summary>
        public string Imburse { get; set; }

        /// <summary>
        /// 专家评定等级
        /// </summary>
        public string Level { get; set; }

        /// <summary>
        /// 专家评分
        /// </summary>
        public int? Score { get; set; }

        /// <summary>
        /// 评审意见的状态：未评议、保存、提交
        /// </summary>
        public int Status { get; set; }

        /// <summary>
        /// 拟资助金额
        /// </summary>
        public double Amount { get; set; }
        
    }
}
