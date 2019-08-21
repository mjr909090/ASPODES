using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ASPODES.DTO.System
{
    /// <summary>
    /// 获取系统文档模板DTO
    /// </summary>
    public class GetTemplateDocDTO
    {
        /// <summary>
        /// 系统文档模板ID
        /// </summary>
        public int? DocId { get; set; }

        /// <summary>
        /// 模板类型
        /// </summary>
        public string Type { get; set; }

        /// <summary>
        /// 文档名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 文档URL相对路径
        /// </summary>
        public string RelativeURL { get; set; }

        /// <summary>
        /// 说明项
        /// </summary>
        public string Remark { get; set; }
    }
}
