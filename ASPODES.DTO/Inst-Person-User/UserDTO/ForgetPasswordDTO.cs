using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Net.Configuration;
using System.Text;
using System.Threading.Tasks;

namespace ASPODES.DTO.Inst_Person_User
{
    /// <summary>
    /// 登录DTO
    /// </summary>
    public class ForgetPasswordDTO
    {
        /// <summary>
        /// 身份证号
        /// </summary>
        [Required]
        public string IDCard { get; set; }

        /// <summary>
        /// 姓名
        /// </summary>
        [Required]
        public string Name { get; set; }
    }
}
