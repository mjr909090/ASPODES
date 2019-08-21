using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ASPODES.DTO.Application
{
    /// <summary>
    /// 获取项目成员DTO
    /// </summary>
    public class GetMemberDTO
    {
        /// <summary>
        /// 所属申请书ID
        /// </summary>
        public string ApplicationId { get; set; }

        /// <summary>
        /// 人员ID
        /// </summary>
        public int? PersonId { get; set; }

        /// <summary>
        /// 成员责任分工
        /// </summary>
        public string Task { get; set; }

        /// <summary>
        /// 人员名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 部门名称
        /// </summary>
        public string InstName { get; set; }

        /// <summary>
        /// 身份证号
        /// </summary>
        public string IDCard { get; set; }

        /// <summary>
        /// 性别
        /// </summary>
        public string Male { get; set; }

        /// <summary>
        /// 职务
        /// </summary>
        public string Duty { get; set; }

        /// <summary>
        /// 职称
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// 电话号码
        /// </summary>
        public string Phone { get; set; }

        /// <summary>
        /// 专业
        /// </summary>
        public string Major { get; set; }
    }
}
