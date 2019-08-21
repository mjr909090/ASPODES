using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ASPODES.DTO.Inst_Person_User
{
    /// <summary>
    /// 获取简化人员信息
    /// </summary>
    public class GetComboPersonDTO
    {
        /// <summary>
        /// 人员ID
        /// </summary>
        public int? PersonId { get; set; }

        /// <summary>
        /// 姓名
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 邮箱
        /// </summary>
        public string Email { get; set; }

        /// <summary>
        /// 所属单位ID
        /// </summary>
        public int? InstituteId { get; set; }
    }
}
