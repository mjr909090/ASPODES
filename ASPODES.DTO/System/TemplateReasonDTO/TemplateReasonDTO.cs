using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ASPODES.DTO.System
{
    /// <summary>
    /// 原因模板DTO
    /// </summary>
    public class TemplateReasonDTO
    {
        /// <summary>
        /// 原因模板ID
        /// </summary>
        [Required]
        public int ReasonId { get; set; }

        /// <summary>
        /// 内容
        /// </summary>
        [Required]
        public string ReasonContent { get; set; }
    }
}
