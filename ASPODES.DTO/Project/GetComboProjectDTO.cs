using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ASPODES.DTO.Project
{
    /// <summary>
    /// 获取项目简化DTO
    /// </summary>
    public class GetComboProjectDTO
    {
        /// <summary>
        /// 项目ID
        /// </summary>
        public string ProjectId { get; set; }

        /// <summary>
        /// 项目名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 领导人ID
        /// </summary>
        public string LeaderId { get; set; }

        /// <summary>
        /// 领导人姓名
        /// </summary>
        public string LeaderName { get; set; }

        /// <summary>
        /// 项目开始执行的时间
        /// </summary>
        public DateTime StartDate { get; set; }

        /// <summary>
        /// 项目执行结束的时间
        /// </summary>
        public DateTime EndDate { get; set; }
    }
}
