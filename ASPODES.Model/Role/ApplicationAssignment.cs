using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace ASPODES.Model
{
    public class ApplicationAssignment
    {
        public int? ApplicationAssignmentId { get; set; }
        /// <summary>
        /// 角色ID，参照Role的外键
        /// </summary>
        public int? RoleId { get; set; }

        /// <summary>
        /// 用户ID，参照User的外键
        /// </summary>
        [StringLength(256)]
        public string UserId { get; set; }

        public int? ProjectTypeId { get; set; }

        public virtual Authorize Authorize { get; set; }

        public virtual ProjectType ProjectType { get; set; }
    }
}
