using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace ASPODES.DTO.Application
{
    /// <summary>
    /// 添加申请书文档DTO
    /// </summary>
    public class AddApplicationDocDTO
    {
        /// <summary>
        /// 所属申请书
        /// </summary>
        [Required]
        public string ApplicationId { get; set; }

        /// <summary>
        /// 文档显示名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 文档类型
        /// </summary>
        [Required]
        public int Type { get; set; }

        /// <summary>
        /// 文档路径
        /// </summary>
        public string RelativeURL { get; set; }
    }
}
