using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace ASPODES.Model
{
    /// <summary>
    /// 用户和角色的中间表
    /// </summary>
    public class Authorize
    {
        
        /// <summary>
        /// 角色ID，参照Role的外键
        /// </summary>
        public int? RoleId { get; set; }
       
        /// <summary>
        /// 用户ID，参照User的外键
        /// </summary>
        [StringLength(256)]
        public string UserId { get; set; }
        /// <summary>
        /// 导航属性，用户
        /// </summary>
        public virtual User User { get; set; }
        /// <summary>
        /// 导航属性
        /// </summary>
        public virtual Role Role { get; set; }
        
    }
}
