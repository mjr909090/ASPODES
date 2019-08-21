using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ASPODES.Model;
using System.ComponentModel.DataAnnotations;

namespace ASPODES.DTO.Inst_Person_User
{
    /// <summary>
    /// 添加单位DTO
    /// </summary>
    public class AddInstDTO
    {
        /// <summary>
        /// 机构编码
        /// </summary>
        [Required( ErrorMessage="机构代码不能为空")]
        public string Code { get; set; }

        /// <summary>
        /// 单位名称
        /// </summary>
        [Required(ErrorMessage = "单位名称不能为空")]
        public string Name { get; set; }

        /// <summary>
        /// 简称
        /// </summary>
        public string ShortName { get; set; }

        /// <summary>
        /// 地址
        /// </summary>
        public string Address { get; set; }

        /// <summary>
        /// 邮编
        /// </summary>
        public string Zip { get; set; }

        /// <summary>
        /// 网址
        /// </summary>
        public string WebSite { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        public string Comment { get; set; }

        private string _type;

        /// <summary>
        /// 单位类型
        /// </summary>
        [Required(ErrorMessage = "单位类型不能为空")]
        public string Type {
            get { return _type; }
            
            set{
                if (value == "外单位")
                {
                    _type = InstituteType.PARTNER.ToString();
                }
                else if (value == "院机关")
                {
                    _type = InstituteType.HEADQUATER.ToString();
                }
                else
                {
                    _type = InstituteType.INSTITUTE.ToString();
                }
            }
        }

        /// <summary>
        /// 联系人姓氏
        /// </summary>
        [Required(ErrorMessage = "联系人姓氏不能为空")]
        public string FirstName { get; set; }

        /// <summary>
        /// 联系人名
        /// </summary>
        [Required(ErrorMessage = "联系人名不能为空")]
        public string LastName { get; set; }

        /// <summary>
        /// 联系人身份证号
        /// </summary>
        [Required(ErrorMessage = "联系人身份证不能为空")]
        public string IDCard { get; set; }

        /// <summary>
        /// 联系人邮箱
        /// </summary>
        [Required(ErrorMessage = "联系人邮箱不能为空")]
        public string Email { get; set; }

        /// <summary>
        /// 联系人电话
        /// </summary>
        public string Phone { get; set; }
    }
}
