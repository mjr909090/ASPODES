using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ASPODES.DTO.Role
{
    /// <summary>
    /// 申请书分配DTO
    /// </summary>
    public class ApplicationAssignmentDTO
    {
        /// <summary>
        /// 项目类型ID
        /// </summary>
        public int ProjectTypeId { get; set; }

        /// <summary>
        /// 项目类型名称
        /// </summary>
        public string ProjectTypeName { get; set; }

        /// <summary>
        /// 是否Checked
        /// </summary>
        public bool Checked { get; set; }

    }
}
