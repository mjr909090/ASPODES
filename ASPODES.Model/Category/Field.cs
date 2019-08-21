using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations;

namespace ASPODES.Model
{
    /// <summary>
    /// 研究领域
    /// </summary>
    public class Field
    {
        
        /// <summary>
        /// 领域名称,主键
        /// </summary>
        [StringLength(256)]
        public string FieldName { get; set; }

        ///// <summary>
        ///// 导航属性，子领域
        ///// </summary>
        //public virtual ICollection<SubField> SubFields { get; set; }
    }
}
