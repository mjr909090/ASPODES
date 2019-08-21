using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ASPODES.DTO.Inst_Person_User
{
    /// <summary>
    /// 获取专家研究领域DTO
    /// </summary>
    public class GetExpertFieldDTO
    {
        /// <summary>
        /// 专家研究领域ID
        /// </summary>
        public int? ExpertFieldId { get; set; }

        /// <summary>
        /// 研究领域ID
        /// </summary>
        public string FieldId { get; set; }

        /// <summary>
        /// 研究子领域ID
        /// </summary>
        public string SubFieldId { get; set; }

        /// <summary>
        /// 专家ID
        /// </summary>
        public string UserId { get; set; }

        /// <summary>
        /// 专家名称
        /// </summary>
        public string UserName { get; set; }

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
