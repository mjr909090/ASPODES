using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ASPODES.DTO.Category
{
    /// <summary>
    /// 获取会计科目DTO
    /// </summary>
    public class GetAccountingSubjectDTO
    {
        /// <summary>
        /// 科目代码
        /// </summary>
        public string SubjectCode { get; set; }

        /// <summary>
        /// 科目名称
        /// </summary>
        public string SubjectName { get; set; }
    }
}
