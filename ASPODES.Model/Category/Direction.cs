using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace ASPODES.Model
{
    /// <summary>
    /// 研究方向
    /// </summary>
    public class Direction
    {
        /// <summary>
        /// 研究方向名称，主键
        /// </summary>
        [StringLength(256)]
        public string DirectId { get; set; }
        /// <summary>
        /// 研究子领域名称，外键，参照SubField表
        /// </summary>
        [StringLength(256)]
        public string SubFieldId { get; set; }
        /// <summary>
        /// 导航属性，子领域
        /// </summary>
        public SubField SubjectField { get; set; }
    }
}
