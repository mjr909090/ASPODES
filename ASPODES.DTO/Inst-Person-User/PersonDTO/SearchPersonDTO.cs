using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ASPODES.DTO.Inst_Person_User
{
    /// <summary>
    /// 关键字搜索用户DTO
    /// </summary>
    public class SearchPersonDTO
    {
        public SearchPersonDTO() 
        {
            Types = "";
            KeyWords = "";
        }

        /// <summary>
        /// 用户类型
        /// </summary>
        [Required]
        public string Types { get; set; }

        /// <summary>
        /// 关键字
        /// </summary>
        [Required]
        public string KeyWords { get; set; }
    }
}
