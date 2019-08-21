using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace ASPODES.DTO.Application
{
    /// <summary>
    /// 添加项目成员DTO
    /// </summary>
    public class AddMemberDTO
    {
        /// <summary>
        /// 所属申请书ID
        /// </summary>
        [Required]
        public string ApplicationId { get; set; }

        /// <summary>
        /// 人员ID
        /// </summary>
        [Required]
        public int? PersonId { get; set; }

        /// <summary>
        /// 成员责任分工
        /// </summary>
        public string Task { get; set; }
    }
}
