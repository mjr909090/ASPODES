using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ASPODES.DTO.Review
{
    /// <summary>
    /// 添加初审指派DTO
    /// </summary>
    public class AddReviewAssignmentDTO
    {
        /// <summary>
        /// 待审申请书的ID
        /// </summary>
        public int? ApplicationId { get; set; }

        /// <summary>
        /// 初审专家的ID
        /// </summary>
        public string ExpertId { get; set; }

        /// <summary>
        /// 年份
        /// </summary>
        public int? Year { get; set; }
    }
}
