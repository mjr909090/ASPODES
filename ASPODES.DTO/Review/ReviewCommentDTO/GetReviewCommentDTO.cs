﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ASPODES.DTO.Review
{
    /// <summary>
    /// 获取初审专家评审意见DTO
    /// </summary>
    public class GetReviewCommentDTO
    {
        /// <summary>
        /// 初审专家评审意见ID
        /// </summary>
        public int ReviewCommentId { get; set; }

        /// <summary>
        /// 初审指派ID
        /// </summary>
        public int ReviewAssignmentId { get; set; }

        /// <summary>
        /// 待审申请书的ID
        /// </summary>
        public string ApplicationId { get; set; }

        /// <summary>
        /// 项目名称
        /// </summary>
        public string ProjectName { get; set; }

        /// <summary>
        /// 初审专家的ID
        /// </summary>
        public string ExpertId { get; set; }

        /// <summary>
        /// 初审专家的名称
        /// </summary>
        public string ExpertName { get; set; }

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
        /// 评审意见的状态：未评议、保存、提交
        /// </summary>
        public int Status { get; set; }

        /// <summary>
        /// 拟资助金额
        /// </summary>
        public double Amount { get; set; }

    }
}
