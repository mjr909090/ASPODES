using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace ASPODES.Model
{
    /// <summary>
    /// 项目成员
    /// </summary>
    public class ProjectMember
    {
        /// <summary>
        /// 项目ID，外键，参照Project
        /// </summary>
        [StringLength(64)]
        public string ProjectId { get; set; }
        /// <summary>
        /// 人员ID，外键，参照Person
        /// </summary>
        public int? PersonId { get; set; }
        /// <summary>
        /// 成员责任分工
        /// </summary>
        [StringLength(1024)]
        public string Task { get; set; }
        /// <summary>
        /// 导航属性，项目
        /// </summary>
        public virtual Project Project { get; set; }
        /// <summary>
        /// 导航属性，人员
        /// </summary>
        public virtual Person Person { get; set; }

    }
}
