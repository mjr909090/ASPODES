using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ASPODES.DTO.Project
{
    /// <summary>
    /// 获取项目成员DTO
    /// </summary>
    public class GetProjectMemberDTO
    {
        /// <summary>
        /// 主持单位人员列表
        /// </summary>
        public List<GetProjectMemberVO> HostDepartMember { get; set; }

        /// <summary>
        /// 其他单位人员列表
        /// </summary>
        public List<GetProjectMemberVO> OtherDepartMember { get; set; }
    }

    /// <summary>
    /// 获取详细项目人员DTO
    /// </summary>
    public class GetProjectMemberVO
    {
        /// <summary>
        /// 项目ID
        /// </summary>
        public string ProjectId { get; set; }

        /// <summary>
        /// 用户名
        /// </summary>
        public string UserName { get; set; }

        /// <summary>
        /// 人员名
        /// </summary>
        public string PersonName { get; set; }

        /// <summary>
        /// 成员责任分工
        /// </summary>
        public string Task { get; set; }

        /// <summary>
        /// 所属单位名称
        /// </summary>
        public string InstituteName { get; set; }

    }
}


