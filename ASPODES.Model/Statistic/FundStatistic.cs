using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ASPODES.Model
{
    /// <summary>
    /// 根据大类统计项目经费
    /// </summary>
    public class FundStatisticByCate
    {
        /// <summary>
        /// 类别名称
        /// </summary>
        public string Category { get; set; }

        /// <summary>
        /// 经费总量
        /// </summary>
        public double Amount { get; set; }
    }

    /// <summary>
    /// 统计资助金额前十的项目
    /// </summary>
    public class Top10FundProject
    {
        /// <summary>
        /// 项目名称
        /// </summary>
        public string ProjectName { get; set; }

        /// <summary>
        /// 项目总金额
        /// </summary>
        public double Amount { get; set; }

        /// <summary>
        /// 项目结题日期
        /// </summary>
        public DateTime EndDate { get; set; }

        /// <summary>
        /// 大类名
        /// </summary>
        public string Category { get; set; }
        /// <summary>
        /// 公司名
        /// </summary>
        public string InstName { get; set; }

    }
}
