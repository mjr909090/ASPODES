using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ASPODES.DTO.Application
{
    /// <summary>
    /// 院管理员导出申请书DTO
    /// </summary>
    public class DeptExportAppDTO
    {
        /// <summary>
        /// 申请书编号
        /// </summary>
        public string Order { get; set; }
        /// <summary>
        /// 申请书名称
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 委托
        /// </summary>
        public string Deleage { get; set; }
        /// <summary>
        /// 类别
        /// </summary>
        public string Category { get; set; }
        /// <summary>
        /// 单位
        /// </summary>
        public string Institute { get; set; }
        /// <summary>
        /// 负责人
        /// </summary>
        public string Leader { get; set; }
        /// <summary>
        /// 要搜索的大类ID
        /// </summary>
        public int CategoryId { get; set; }
        /// <summary>
        /// 要搜索的单位ID
        /// </summary>
        public int InstituteId { get; set; }
        /// <summary>
        /// 要搜索的年份
        /// </summary>
        public int Year { get; set; }
    }
}
