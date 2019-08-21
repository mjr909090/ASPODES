using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ASPODES.DTO.Inst_Person_User
{
    /// <summary>
    /// 获取人员信息DTO
    /// </summary>
    public class GetPersonDTO
    {
        /// <summary>
        /// 人员ID
        /// </summary>
        public int? PersonId { get; set; }

        /// <summary>
        /// 身份证号
        /// </summary>
        public string IDCard { get; set; }

        /// <summary>
        /// 姓名
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 姓
        /// </summary>
        public string FirstName { get; set; }

        /// <summary>
        /// 名
        /// </summary>
        public string LastName { get; set; }

        /// <summary>
        /// 英文名
        /// </summary>
        public string EnglishName { get; set; }

        /// <summary>
        /// 生日
        /// </summary>
        public DateTime Birthday { get; set; }

        /// <summary>
        /// 性别
        /// </summary>
        public string Male { get; set; }

        /// <summary>
        /// 电话
        /// </summary>
        public string Phone { get; set; }

        /// <summary>
        /// 邮箱
        /// </summary>
        public string Email { get; set; }

        /// <summary>
        /// 所属单位ID
        /// </summary>
        public int? InstituteId { get; set; }

        /// <summary>
        /// 所属单位名称
        /// </summary>
        public string InstituteName { get; set; }

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
        /// 教育背景
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

        /// <summary>
        /// C 正常、L-锁定、D-删除,默认值C
        /// </summary>
        public string Status { get; set; }
    }
}
