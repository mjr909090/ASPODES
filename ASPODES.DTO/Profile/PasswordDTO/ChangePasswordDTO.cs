using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ASPODES.DTO.Profile
{
    /// <summary>
    /// 更换密码DTO
    /// </summary>
    public class ChangePasswordDTO
    {
        /// <summary>
        /// 旧密码
        /// </summary>
        [Required(AllowEmptyStrings = false, ErrorMessage = "原密码不能为空！")]
        [StringLength(50, ErrorMessage = "密码长度不能小于6位", MinimumLength = 6)]
        public string OldPassword { get; set; }

        /// <summary>
        /// 新密码
        /// </summary>
        [Required(AllowEmptyStrings = false, ErrorMessage = "新密码不能为空！")]
        [StringLength(50, ErrorMessage = "密码长度不能小于6位", MinimumLength = 6)]
        public string NewPassword { get; set; }

        /// <summary>
        /// 再次输入的新密码
        /// </summary>
        [Required(AllowEmptyStrings = false, ErrorMessage = "重复新密码不能为空！")]
        [StringLength(50, ErrorMessage = "密码长度不能小于6位", MinimumLength = 6)]
        [Compare("NewPassword", ErrorMessage = "两次密码必须一致")]
        public string RepeatPassword { get; set; }

    }
}
