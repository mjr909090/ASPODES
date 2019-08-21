using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ASPODES.DTO.Inst_Person_User
{
    /// <summary>
    /// 获取单位简化DTO
    /// </summary>
    public class GetComboInstDTO
    {
        /// <summary>
        /// 单位ID
        /// </summary>
        public int? InstituteId { get; set; }

        /// <summary>
        /// 名称
        /// </summary>
        public string Name { get; set; }
    }
}
