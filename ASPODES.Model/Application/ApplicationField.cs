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
    /// 申请书领域
    /// </summary>
    public class ApplicationField
    {
        /// <summary>
        /// 主键，自增
        /// </summary>
        public int? ApplicationFieldId { get; set; }
        /// <summary>
        /// 申请书ID，外键，组合主键，参照Application表
        /// </summary>
        [StringLength(64)]
        public string ApplicationId { get; set; }
        /// <summary>
        /// 研究领域ID，外键，组合主键，参照SubField表
        /// </summary>
        [StringLength(256)]
        public string SubFieldId { get; set; }
        /// <summary>
        /// 中文关键字
        /// </summary>
        [StringLength(256)]
        public string KeyWordsCN { get; set; }
        /// <summary>
        /// 英文关键字
        /// </summary>
        [StringLength(256)]
        public string KeyWordsEN { get; set; }
        /// <summary>
        /// 导航属性，申请书
        /// </summary>
        [JsonIgnore]
        public virtual Application Application { get; set; }
        /// <summary>
        /// 导航属性，研究领域
        /// </summary>
        [JsonIgnore]
        public virtual SubField SubField { get; set; }

    }
}
