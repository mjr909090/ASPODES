using ASPODES.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ASPODES.DTO.AnnualTask
{
    /// <summary>
    /// 获取项目任务书DTO
    /// </summary>
    public class GetProjectAnnualTaskDTO
    {
        public GetProjectAnnualTaskDTO()
        {
            Previous = new List<GetAnnualTaskVO>();
        }
        /// <summary>
        /// 当前的项目任务书
        /// </summary>
        public GetAnnualTaskVO Current { get; set; }
        /// <summary>
        /// 之前的项目任务书
        /// </summary>
        public List<GetAnnualTaskVO> Previous { get; set; }
    }

    /// <summary>
    /// 获取年度任务书DTO
    /// </summary>
    public class GetAnnualTaskVO
    {
        /// <summary>
        /// 任务书名称
        /// </summary>
        public string Name { get { return ProjectName + "第" + Year + "年任务书";} }

        /// <summary>
        /// 项目名称
        /// </summary>
        public string ProjectName { get; set; }

        /// <summary>
        /// 年度任务书编号
        /// </summary>
        public int AnnualTaskId { get; set; }

        /// <summary>
        /// 年份,和申请年度相同，即System.ApplyYear
        /// </summary>
        public int Year { get; set; }
        /// <summary>
        /// 年度任务书状态
        /// </summary>
        public AnnualTaskStatus Status { get; set; }

        /// <summary>
        /// 编辑时间
        /// </summary>
        public DateTime EditTime { get; set; }
    }
}


