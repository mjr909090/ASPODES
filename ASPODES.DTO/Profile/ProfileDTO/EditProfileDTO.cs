using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ASPODES.DTO.Profile
{
    /// <summary>
    /// 编辑个人信息DTO
    /// </summary>
    public class EditProfileDTO
    {
        /// <summary>
        /// 邮箱
        /// </summary>
        [Required(AllowEmptyStrings = false, ErrorMessage = "邮箱不能为空！")]
        [RegularExpression(@"^[0-9A-Za-zd]+([-_.][0-9A-Za-zd]+)*@([0-9A-Za-zd]+[-.])+[0-9A-Za-zd]{2,5}$", ErrorMessage = "请输入正确的电子邮箱地址！")]
        public string Email { get; set; }

        /// <summary>
        /// 电话
        /// </summary>
        [Required(AllowEmptyStrings = false, ErrorMessage = "联系电话不能为空！")]
        [RegularExpression(@"^(13[0-9]|17[0-9]|14[5|7]|15[0|1|2|3|5|6|7|8|9]|18[0|1|2|3|5|6|7|8|9])\d{8}$", ErrorMessage = "请输入正确的手机号！")]
        public string Phone { get; set; }

        /// <summary>
        /// 通讯地址
        /// </summary>
        [Required(AllowEmptyStrings = false, ErrorMessage = "地址不能为空！")]
        [StringLength(128, ErrorMessage = "地址不能超过128位")]
        public string Address { get; set; }
    }
}
