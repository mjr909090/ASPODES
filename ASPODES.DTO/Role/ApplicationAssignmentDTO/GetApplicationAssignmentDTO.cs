using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ASPODES.DTO.Role
{
    /// <summary>
    /// 获取申请书分配DTO
    /// </summary>
    public class GetApplicationAssignmentDTO
    {
        /// <summary>
        /// 申请书分配ID
        /// </summary>
        public int? ApplicationAssignmentId { get; set; }

        /// <summary>
        /// 用户ID
        /// </summary>
        public string UserId { get; set; }

        /// <summary>
        /// 项目类型ID
        /// </summary>
        public int? ProjectTypeId { get; set; }

        /// <summary>
        /// 用户名
        /// </summary>
        public string UserName { get; set; }

        /// <summary>
        /// 项目类型名
        /// </summary>
        public string ProjectTypeName { get; set; }

    }
}
