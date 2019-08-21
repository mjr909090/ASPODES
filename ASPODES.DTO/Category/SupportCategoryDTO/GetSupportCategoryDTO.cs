using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ASPODES.DTO.Category
{
    /// <summary>
    /// 获取资助类别DTO
    /// </summary>
    public class GetSupportCategoryDTO
    {
        /// <summary>
        /// 资助类别ID
        /// </summary>
        public int? SupportCategoryId { get; set; }

        /// <summary>
        /// 资助类别名称
        /// </summary>
        public string Name { get; set; }
    }
}
