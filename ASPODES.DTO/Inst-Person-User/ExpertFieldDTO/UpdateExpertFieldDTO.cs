using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ASPODES.DTO.Inst_Person_User
{
    /// <summary>
    /// 更新专家研究领域DTO
    /// </summary>
    public class UpdateExpertFieldDTO
    {
        /// <summary>
        /// 专家研究领域ID
        /// </summary>
        public int? ExpertFieldId { get; set; }

        /// <summary>
        /// 研究子领域ID
        /// </summary>
        public string SubFieldId { get; set; }

        /// <summary>
        /// 专家ID
        /// </summary>
        public string UserId { get; set; }

        /// <summary>
        /// 中文关键字
        /// </summary>
        public string KeyWordsCN { get; set; }

        /// <summary>
        /// 英文关键字
        /// </summary>
        public string KeyWordsEN { get; set; }
    }
}
