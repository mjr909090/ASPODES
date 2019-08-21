using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;

namespace ASPODES.Model
{
    /// <summary>
    /// 出库项目文档
    /// </summary>
    public class ProjectDoc
    {
        /// <summary>
        /// 自增主键
        /// </summary>
        public int ProjectDocId { get; set; }
        /// <summary>
        /// 所属项目的ID，外键
        /// </summary>
        [StringLength(64)]
        public string ProjectId { get; set; }
        /// <summary>
        /// 文档类型
        /// </summary>
        [Required]
        public ProjectDocType Type { get; set; }
        /// <summary>
        /// 文档名称
        /// </summary>
        [Required,StringLength(256)]
        public string Name { get; set; }

        /// <summary>
        /// 文档显示名称
        /// </summary>
        public string DisplayName { get; set; }

        /// <summary>
        /// 文档保存路径
        /// </summary>
        [Required,StringLength(256)]
        public string RelativeURL { get; set; }
        /// <summary>
        /// 导航属性，出库项目
        /// </summary>
        [JsonIgnore]
        public virtual Project Project { get; set; }
    }

    /// <summary>
    /// 出库项目文档类型
    /// </summary>
    public enum ProjectDocType
    {
        /// <summary>
        /// 年度任务书
        /// </summary>
        //TASK,
        /// <summary>
        /// 结题报告
        /// </summary>
        FINISH_REPORT,
        /// <summary>
        /// 年度报告
        /// </summary>
        //ANUAL_REPORT
        /// <summary>
        /// 附件（其他）
        /// </summary>
        OTHER
    }
}
