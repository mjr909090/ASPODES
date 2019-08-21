using ASPODES.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ASPODES.DTO.AnnualTask
{
    /// <summary>
    /// 获取申请书的文档DTO
    /// </summary>
    public class GetAnnualTaskDocDTO
    {
        /// <summary>
        /// 主键
        /// </summary>
        public int? AnnualTaskDocId { get; set; }

        /// <summary>
        /// 文档显示名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 文档类型
        /// </summary>
        public AnnualTaskDocType Type { get; set; }
    }
}
