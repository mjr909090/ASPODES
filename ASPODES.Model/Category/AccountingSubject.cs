using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace ASPODES.Model
{
    /// <summary>
    /// 会计科目
    /// </summary>
    public class AccountingSubject
    {
        /// <summary>
        /// 科目代码，无二级科目的用一级科目、有二级科目的用二级科目
        /// 会议差旅费的二级科目，其他的均为一级科目
        /// </summary>
        [StringLength(30)]
        public string SubjectCode { get; set; }
        /// <summary>
        /// 科目名称，UNIQUE
        /// </summary>
        [Required,StringLength(100)]
        public string SubjectName { get; set; }
    }
}
