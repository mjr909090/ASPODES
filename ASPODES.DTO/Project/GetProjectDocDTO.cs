using ASPODES.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ASPODES.DTO.Project
{
    /// <summary>
    /// 获取出库项目文档DTO
    /// </summary>
    public class GetProjectDocDTO
    {
        /// <summary>
        /// 出库项目文档ID
        /// </summary>
        public int ProjectDocId { get; set; }

        /// <summary>
        /// 出库项目文档类型
        /// </summary>
        public ProjectDocType Type { get; set; }

        /// <summary>
        /// 出库项目文档名称
        /// </summary>
        public string Name { get; set; }

    }


}
