using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace ASPODES.Model
{
    /// <summary>
    /// 专家研究领域
    /// </summary>
    public class ExpertField
    {
        /// <summary>
        /// 主键，自增
        /// </summary>
        public int? ExpertFieldId { get; set; }
        /// <summary>
        /// 研究领域名称,外键，参照SubField
        /// </summary>
        [StringLength(256)]
        public string SubFieldId { get; set; }
        /// <summary>
        /// 专家信息，外键，参照ExpertInfo
        /// </summary>
        [StringLength(256)]
        public string UserId { get; set; }
        /// <summary>
        /// 关键字，中文
        /// </summary>
        [StringLength(256)]
        public string KeyWordsCN { get; set; }
        /// <summary>
        /// 关键字英文
        /// </summary>
        [StringLength(256)]
        public string KeyWordsEN { get; set; }
        /// <summary>
        /// 导航属性，研究子领域
        /// </summary>
        public virtual SubField SubField { get; set; }
        /// <summary>
        /// 导航属性，专家信息
        /// </summary>
        public virtual User User { get; set; }
    }
}
