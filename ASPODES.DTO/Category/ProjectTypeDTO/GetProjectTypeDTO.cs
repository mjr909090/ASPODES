using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ASPODES.DTO.Category
{
    /// <summary>
    /// 获取项目类型DTO
    /// </summary>
    public class GetProjectTypeDTO
    {
        /// <summary>
        /// 项目类型ID
        /// </summary>
        public int? ProjectTypeId { get; set; }

        /// <summary>
        /// 项目类型名
        /// </summary>
        public string Name { get; set; }

    }
}
