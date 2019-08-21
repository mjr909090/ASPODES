using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace ASPODES.Model
{
    /// <summary>
    /// 系统文档模板
    /// </summary>
    public class TemplateDoc
    {
        /// <summary>
        /// 代理主键，自增
        /// </summary>
        public int? DocId { get; set; }
        /// <summary>
        /// 模板类型
        /// </summary>
        [Required, StringLength(256)]
        public string Type { get; set; }
        /// <summary>
        /// 文档名称 
        /// </summary>
        [Required,StringLength(256)]
        public string Name { get; set; }
        /// <summary>
        /// 文档URL相对路径
        /// </summary>
        [Required,StringLength(256)]
        public string RelativeURL { get; set; }
        /// <summary>
        /// 说明项
        /// </summary>
        [StringLength(256)]
        public string Remark { get; set; }

    }
}
