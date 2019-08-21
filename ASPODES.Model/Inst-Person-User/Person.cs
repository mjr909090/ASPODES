using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;

namespace ASPODES.Model
{
    /// <summary>
    /// 人员信息
    /// </summary>
    public class Person
    {
        public Person()
        {
            Status = "C";
        }
        /// <summary>
        /// 自增主键
        /// </summary>
        public int? PersonId { get; set; }

        /// <summary>
        /// 身份证号
        /// </summary>
        [RegularExpression(@"^[1-9]\d{5}(18|19|([23]\d))\d{2}((0[1-9])|(10|11|12))(([0-2][1-9])|10|20|30|31)\d{3}[0-9Xx]$",ErrorMessage = "身份证号码错误"),
        Required(), StringLength(32)]
        public string IDCard { get; set; }

        /// <summary>
        /// 姓名
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 姓拼音
        /// </summary>
        [Required(ErrorMessage = "姓不能为空"), StringLength(64)]
        public string FirstName { get; set; }

        /// <summary>
        /// 名拼音
        /// </summary>
        [Required(ErrorMessage = "名不能为空"), StringLength(64)]
        public string LastName { get; set; }

        /// <summary>
        /// 其他英文名
        /// </summary>
        [StringLength(64)]
        public string EnglishName { get; set; }

        /// <summary>
        /// 出生日期
        /// </summary>
        public DateTime? Birthday { get; set; }

        /// <summary>
        /// 性别
        /// </summary>
        [StringLength(4)]
        public string Male { get; set; }

        /// <summary>
        /// 电话号码
        /// </summary>
        [StringLength(32)]
        public string Phone { get; set; }
        /// <summary>
        /// 邮箱
        /// </summary>
        [RegularExpression(@"^[a-zA-Z0-9_-]+@[a-zA-Z0-9_-]+(\.[a-zA-Z0-9_-]+)+$",ErrorMessage = "邮箱错误"),
        Required(), StringLength(256)]
        public string Email { get; set; }
        /// <summary>
        /// 所属单位ID
        /// </summary>
        public int? InstituteId { get; set; }

        /// <summary>
        /// 职务
        /// </summary>
        [StringLength(256)]
        public string Duty { get; set; }

        /// <summary>
        /// 职称
        /// </summary>
        [StringLength(256)]
        public string Title { get; set; }

        /// <summary>
        /// 通信地址
        /// </summary>
        [StringLength(256)]
        public string Address { get; set; }

        /// <summary>
        /// 邮政编码
        /// </summary>
        [StringLength(32)]
        public string Zip { get; set; }

        /// <summary>
        /// 办公室电话
        /// </summary>
        [StringLength(32)]
        public string OfficePhone { get; set; }
        /// <summary>
        /// 专业
        /// </summary>
        [StringLength(256)]
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
        //[StringLength(512)]
        public string Achievements { get; set; }

        /// <summary>
        ///  C 正常、L-锁定、D-删除,默认值C
        /// </summary>
        [StringLength(4)]
        public string Status { get; set; }
        
        /// <summary>
        /// 导航属性，单位
        /// </summary>
        [JsonIgnore]
        public virtual Institute Institute { get; set; }


    }
}