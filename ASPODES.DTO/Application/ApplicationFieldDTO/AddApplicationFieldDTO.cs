using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace ASPODES.DTO.Application
{
    /// <summary>
    /// 添加申请书领域DTO
    /// </summary>
    public class AddApplicationFieldDTO
    {
        /// <summary>
        /// 申请书领域ID
        /// </summary>
        public int? ApplicationFieldId { get; set; }

        /// <summary>
        /// 申请书ID
        /// </summary>
        [Required]
        public string ApplicationId { get; set; }

        /// <summary>
        /// 研究领域ID
        /// </summary>
        [Required]
        public string SubFieldId { get; set; }

        /// <summary>
        /// 中文关键字
        /// </summary>
        [Required]
        public string KeyWordsCN { get; set; }

        /// <summary>
        /// 英文关键字
        /// </summary>
        [Required]
        public string KeyWordsEN { get; set; }
    }
}
