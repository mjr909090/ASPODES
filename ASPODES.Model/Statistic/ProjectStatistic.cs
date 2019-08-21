using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ASPODES.Model
{
    /// <summary>
    /// 用来接收根据大类统计的项目
    /// </summary>
    public class ProjectStatistic
    {
        /// <summary>
        /// 项目类别
        /// </summary>
        public string Category { get; set; }
        
        /// <summary>
        /// 项目数量
        /// </summary>
        public int Number { get; set; }

    }

    /// <summary>
    /// 通过单位分组的在研项目统计模型
    /// </summary>
    public class ProjectStatisticByInst
    {
        /// <summary>
        /// 单位名称
        /// </summary>
        public string InstName { get; set; }

        /// <summary>
        /// 项目数量
        /// </summary>
        public int Number { get; set; }
    }
}
