using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Newtonsoft.Json;

namespace ASPODES.Model
{
    /// <summary>
    /// 申请书的文档
    /// </summary>
    public class AnnualTaskDoc
    {
        /// <summary>
        /// 自增主键
        /// </summary>
        public int? AnnualTaskDocId { get; set; }
        /// <summary>
        /// 文档名
        /// </summary>
        [Required,StringLength(256)]
        public string Name { get; set; }

        /// <summary>
        /// 文档显示名称
        /// </summary>
        public string DisplayName { get; set; }

        /// <summary>
        /// 文档类型
        /// </summary>
        [Required]
        public AnnualTaskDocType Type { get; set; }
        /// <summary>
        /// 文档路径
        /// </summary>
        [Required,StringLength(256)]
        public string RelativeURL { get; set; }
        /// <summary>
        /// 所属任务书，外键,参考AnnualTask表
        /// </summary>
        public int? AnnualTaskId { get; set; }
        /// <summary>
        /// 导航属性，所属任务书
        /// </summary>
        [JsonIgnore]
        public AnnualTask AnnualTask { get; set; }
    }

    /// <summary>
    /// 年度任务书关联文档的类型
    /// </summary>
    public enum AnnualTaskDocType
    {
        /// <summary>
        /// 年度任务的年度报告
        /// </summary>
        ANNUAL_REPORT,
        /// <summary>
        /// 年度任务书正文
        /// </summary>
        BODY,
        /// <summary>
        /// 附件
        /// </summary>
        ATTACHMENT,
        /// <summary>
        /// PDF格式的年度任务书
        /// </summary>
        PDF
    }
}
