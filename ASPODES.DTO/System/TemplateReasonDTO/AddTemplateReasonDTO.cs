using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ASPODES.DTO.System
{
    /// <summary>
    /// 添加原因模板DTO
    /// </summary>
    public class AddTemplateReasonDTO
    {
        /// <summary>
        /// 编辑人
        /// </summary>
        public string EditorId { get; set; }

        /// <summary>
        /// 原因类型
        /// </summary>
        public int? Type { get; set; }

        /// <summary>
        /// 内容
        /// </summary>
        public string Content { get; set; }
    }
}
