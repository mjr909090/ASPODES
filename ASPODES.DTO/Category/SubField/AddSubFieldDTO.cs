using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ASPODES.DTO.Category
{
    /// <summary>
    /// 添加子领域DTO
    /// </summary>
    public class AddSubFieldDTO
    {
        /// <summary>
        /// 子领域ID
        /// </summary>
        public string SubFieldId { get; set; }

        /// <summary>
        /// 上级领域名称
        /// </summary>
        public string ParentName { get; set; }
    }
}
