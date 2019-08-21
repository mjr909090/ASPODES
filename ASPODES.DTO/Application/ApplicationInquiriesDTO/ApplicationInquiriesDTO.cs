using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ASPODES.DTO.Application
{
    public class ApplicationInquiriesDTO
    {
        /// <summary>
        /// 申请书名称
        /// </summary>
        public string ProjectName { get; set; }
        /// <summary>
        /// 项目周期
        /// </summary>
        public int[] Period { get; set; }
        /// <summary>
        /// 申请书大类
        /// </summary>
        public int[] ProjectType { get; set; }
        /// <summary>
        /// 单位ID
        /// </summary>
        public int[] Institute { get; set; }
        /// <summary>
        /// leader名字
        /// </summary>
        public string LeaderName { get; set; }
        /// <summary>
        /// 项目开始年份
        /// </summary>
        public int? StartYearCreated { get; set; }
        /// <summary>
        /// 项目开始年份
        /// </summary>
        public int? EndYearCreated { get; set; }
        /// <summary>
        /// 总经费
        /// </summary>
        public double? StartTotalBudget { get; set; }
        /// <summary>
        /// 总经费
        /// </summary>
        public double? EndTotalBudget { get; set; }
        /// <summary>
        /// 申请书状态
        /// </summary>
        public int[] Status { get; set; }
        /// <summary>
        /// 委托类型 1-委托；0-非委托
        /// </summary>
        public int? DelegateType { get; set; }
        /// <summary>
        /// 总分
        /// </summary>
        public int? StartTotalScore { get; set; }
        /// <summary>
        /// 总分
        /// </summary>
        public int? EndTotalScore { get; set; }
    }
}
