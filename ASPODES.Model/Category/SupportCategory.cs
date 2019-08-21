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
    /// 资助类别，8大项里的小项
    /// </summary>
    public class SupportCategory
    {
        /// <summary>
        /// 资助类别ID，主键，自增
        /// </summary>
        public int? SupportCategoryId { get; set; }
        /// <summary>
        /// 资助类别名称
        /// </summary>
        [Required,StringLength(128),Index(IsUnique=true)]
        public string Name { get; set; }
        /// <summary>
        /// 项目类型ID，外键， 参照ProjectType
        /// </summary>
        public int? ProjectTypeId { get; set; }
        /// <summary>
        /// 导航属性，项目类型
        /// </summary>
        [JsonIgnore]
        public virtual ProjectType ProjectType { get; set; }

    }
}
