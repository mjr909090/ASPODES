using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ASPODES.DTO.Application
{
    /// <summary>
    /// 获取申请书领域DTO
    /// </summary>
    public class GetApplicationFieldDTO
    {
        /// <summary>
        /// 申请书领域ID
        /// </summary>
        public int? ApplicationFieldId { get; set; }

        /// <summary>
        /// 申请书ID
        /// </summary>
        public string ApplicationId { get; set; }

        /// <summary>
        /// 父领域名称
        /// </summary>
        public string ParentFieldName { get; set; }

        /// <summary>
        /// 研究领域ID
        /// </summary>
        public string SubFieldId { get; set; }

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
