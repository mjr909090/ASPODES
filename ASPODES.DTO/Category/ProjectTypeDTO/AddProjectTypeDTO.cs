using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ASPODES.DTO.Category
{
    /// <summary>
    /// 添加项目类型DTO
    /// </summary>
    public class AddProjectTypeDTO
    {
        /// <summary>
        /// 项目类型名
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 限项数目
        /// </summary>
        public int? Limit { get; set; }
    }
}
