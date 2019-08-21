using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ASPODES.DTO.Inst_Person_User
{
    /// <summary>
    /// 忘记密码后重置密码DTO
    /// </summary>
    public class ResetPasswordDTO
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

        /// <summary>
        /// 验证码
        /// </summary>
        [Required]
        public string IdentifyCode { get; set; }

        /// <summary>
        /// 密码
        /// </summary>
        [Required]
        public string Password { get; set; }
    }
}
