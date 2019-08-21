using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace ASPODES.Model
{
    /// <summary>
    /// 角色和资源的中间表
    /// </summary>
    public class Permission
    {
        /// <summary>
        /// 主键，自增
        /// </summary>
        //public int? PermissionId { get; set; }
        /// <summary>
        /// 角色,外键，参照Role
        /// </summary>
        public int? RoleId { get; set; }
       
        /// <summary>
        /// 资源URL，外键，参照Resource
        /// </summary>
        public int? ResourceId { get; set; }
        /// <summary>
        /// 导航属性，资源
        /// </summary>
        public Resource Resource { get; set; }

        /// <summary>
        /// 导航属性，角色
        /// </summary>
        public Role Role { get; set; }
    }
}
