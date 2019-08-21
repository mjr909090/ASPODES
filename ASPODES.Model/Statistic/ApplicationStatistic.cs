using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ASPODES.Model
{
    /// <summary>
    /// 用来接收根据大类和状态统计的申请书
    /// </summary>
    public class ApplicationStatistic
    {
        /// <summary>
        /// 项目类别
        /// </summary>
        public string Category { get; set; }

        /// <summary>
        /// 等待审核的项目数量
        /// </summary>
        public int? Checking { get; set; }

        /// <summary>
        /// 审核不通过的申请书数量
        /// </summary>
        public int? Reject { get; set; }

        /// <summary>
        /// 审核通过的申请书数量
        /// </summary>
        public int? Accept { get; set; }

        /// <summary>
        /// 三种状态的申请书总数
        /// </summary>
        public int? Total { get; set; }
    }
    
    /// <summary>
    /// 根据大类统计资助项目数量和项目总数
    /// </summary>
    public class FundAndAcceptByCate
    {
        /// <summary>
        /// 大类名称
        /// </summary>
        public string Category { get; set; }

        /// <summary>
        /// 资助数量
        /// </summary>
        public int FundNumber { get; set; }

        /// <summary>
        /// 总数
        /// </summary>
        public int AcceptNumber { get; set; }
    }
}
