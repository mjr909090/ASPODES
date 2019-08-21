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
    /// 研究领域，子领域
    /// </summary>
    public class SubField
    {
        /// <summary>
        /// 领域名称，主键
        /// </summary>
        [StringLength(256)]
        public string SubFieldName { get; set; }

        /// <summary>
        /// 上级领域名称，外键，参照Field
        /// </summary>
        [StringLength(256)]
        public string ParentName { get; set; }

        /// <summary>
        /// 导航属性，上级领域
        /// </summary>
        [JsonIgnore]
        public virtual Field ParentField { get; set; }
    }
}
