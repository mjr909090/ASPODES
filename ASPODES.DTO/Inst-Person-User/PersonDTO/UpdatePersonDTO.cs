using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace ASPODES.DTO.Inst_Person_User
{
    /// <summary>
    /// 更新人员DTO
    /// </summary>
    public class UpdatePersonDTO
    {
        /// <summary>
        /// 人员ID
        /// </summary>
        [Required]
        public int PersonId { get; set; }

        /// <summary>
        /// 身份证号
        /// </summary>
        [Required]
        public string IDCard { get; set; }

        /// <summary>
        /// 性
        /// </summary>
        [Required]
        public string FirstName { get; set; }

        /// <summary>
        /// 名
        /// </summary>
        [Required]
        public string LastName { get; set; }

        /// <summary>
        /// 姓名
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 英文名
        /// </summary>
        [Required]
        public string EnglishName { get; set; }

        /// <summary>
        /// 生日
        /// </summary>
        public DateTime Birthday { get; set; }

        /// <summary>
        /// 性别
        /// </summary>
        [Required]
        public string Male { get; set; }

        /// <summary>
        /// 电话
        /// </summary>
        [Required]
        public string Phone { get; set; }

        /// <summary>
        /// 职务
        /// </summary>
        public string Duty { get; set; }

        /// <summary>
        /// 职称
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// 通信地址
        /// </summary>
        public string Address { get; set; }

        /// <summary>
        /// 邮政编码
        /// </summary>
        public string Zip { get; set; }

        /// <summary>
        /// 办公室电话
        /// </summary>
        public string OfficePhone { get; set; }

        /// <summary>
        /// 专业
        /// </summary>
        public string Major { get; set; }

        /// <summary>
        /// 教育经历
        /// </summary>
        public string EducationHistor { get; set; }

        /// <summary>
        /// 工作经历
        /// </summary>
        public string WorkingHistory { get; set; }

        /// <summary>
        /// 成果
        /// </summary>
        public string Achievements { get; set; }
    }
}
