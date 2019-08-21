using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace ASPODES.Model
{
    /// <summary>
    /// 角色类
    /// </summary>
    public class Role
    {
        /// <summary>
        /// 自增主键
        /// </summary>
        public int? RoleId { get; set; }
        /// <summary>
        /// 角色名
        /// </summary>
        [Required,StringLength(128)]
        public string Name { get; set; }
       
    }
}
