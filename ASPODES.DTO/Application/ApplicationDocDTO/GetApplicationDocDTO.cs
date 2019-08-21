using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ASPODES.DTO.Application
{
    /// <summary>
    /// 获取申请书文档DTO
    /// </summary>
    public class GetApplicationDocDTO
    {
        /// <summary>
        /// 申请书文档ID
        /// </summary>
        public int? ApplicationDocId { get; set; }

        /// <summary>
        /// 所属申请书
        /// </summary>
        public string ApplicationId { get; set; }

        /// <summary>
        /// 文档显示名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 文档类型
        /// </summary>
        public int Type { get; set; }
        /// <summary>
        /// 文档路径
        /// </summary>
        public string RelativeURL { get; set; }
    }
}
