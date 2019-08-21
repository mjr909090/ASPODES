using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace ASPODES.DTO.Role
{
    /// <summary>
    /// 增加申请书分配DTO
    /// </summary>
    public class AddApplicationAssignmentDTO
    {
        /// <summary>
        /// 角色ID
        /// </summary>
        public int? RoleId { get; set; }

        /// <summary>
        /// 用户ID
        /// </summary>
        public string UserId { get; set; }

        /// <summary>
        /// 项目类型ID
        /// </summary>
        public int? ProjectTypeId { get; set; }
    }
}
