using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ASPODES.Model
{
    /// <summary>
    /// 申请书的文档
    /// </summary>
    public class ApplicationDoc
    {
        /// <summary>
        /// 自增主键
        /// </summary>
        public int? ApplicationDocId { get; set; }
        /// <summary>
        /// 文档名
        /// </summary>
        [Required, StringLength(256)]
        public string Name { get; set; }

        /// <summary>
        /// 文档显示名称
        /// </summary>
        public string DisplayName { get; set; }

        /// <summary>
        /// 文档类型
        /// </summary>
        [Required]
        public ApplicationDocType Type { get; set; }
        /// <summary>
        /// 文档路径
        /// </summary>
        [Required,StringLength(256)]
        public string RelativeURL { get; set; }
        /// <summary>
        /// 所属申请书，外键,参考Application表
        /// </summary>
        [StringLength(64)]
        public string ApplicationId { get; set; }
        /// <summary>
        /// 导航属性，所属申请书
        /// </summary>
        public Application Application { get; set; }
    }

    /// <summary>
    /// 申请书关联文档的类型
    /// </summary>
    public enum ApplicationDocType
    {
        /// <summary>
        /// 申请书正文
        /// </summary>
        BODY,
        /// <summary>
        /// 附件
        /// </summary>
        ATTACHMENT,
        /// <summary>
        /// PDF格式的申请书全文
        /// </summary>
        PDF
    }
}
