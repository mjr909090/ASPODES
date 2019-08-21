using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ASPODES.DTO.Application
{
    /// <summary>
    /// 单位管理员导出已通过或既往申请书DTO
    /// </summary>
    public class InstExportAppDTO
    {
        /// <summary>
        /// 序号
        /// </summary>
        public string Order { get; set; }
        /// <summary>
        /// 名称
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
        /// 负责人
        /// </summary>
        public string Leader { get; set; }
        /// <summary>
        /// 状态
        /// </summary>
        public string Status { get; set; }
        /// <summary>
        /// 选择导出的大类ID
        /// </summary>
        public int CategoryId { get; set; }
        /// <summary>
        /// 要搜索的年份
        /// </summary>
        public int Year { get; set; }
    }
}
