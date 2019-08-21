using ASPODES.DTO.AnnualTask;
using ASPODES.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ASPODES.DTO.Project
{
    /// <summary>
    /// 获取项目DTO
    /// </summary>
    public class GetProjectDTO
    {
        /// <summary>
        /// 项目ID
        /// </summary>
        public string ProjectId { get; set; }

        /// <summary>
        /// 项目名称
        /// </summary>
        public string Name { get; set; }

        //public int? LeaderId { get; set; }

        //public string LeaderName { get; set; }

        // public int? InstituteId { get; set; }

        //public string InstituteName { get; set; }

        /// <summary>
        /// 执行期限
        /// </summary>
        public int? Period { get; set; }

        /// <summary>
        /// 项目开始执行的时间
        /// </summary>
        public DateTime StartDate { get; set; }

        /// <summary>
        /// 项目执行结束的时间
        /// </summary>
        public DateTime EndDate { get; set; }

        /// <summary>
        /// 资助额度
        /// </summary>
        public double? TotalBudget { get; set; }

        /// <summary>
        /// 项目类型ID
        /// </summary>
        public int? ProjectTypeId { get; set; }

        /// <summary>
        /// 项目类型名称
        /// </summary>
        public string ProjectTypeName { get; set; }

        /// <summary>
        /// 委托类型
        /// </summary>
        public string DelegateType { get; set; }

        /// <summary>
        /// 项目状态
        /// </summary>
        public ProjectStatus Status { get; set; }

        /// <summary>
        /// 项目文档
        /// </summary>
        public List<GetProjectDocDTO> Docs { get; set; }

        /// <summary>
        /// 主持单位项目成员
        /// </summary>
        public List<GetProjectMemberVO> HostDepartMembers { get; set; }

        /// <summary>
        /// 领导
        /// </summary>
        public GetProjectMemberVO Leader { get; set; }

        /// <summary>
        /// 其他单位项目成员
        /// </summary>
        public List<GetProjectMemberVO> OtherDepartMembers { get; set; }

        /// <summary>
        /// 项目类型
        /// </summary>
        public List<GetAnnualTaskVO> AnnualTasks { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        public string Comment { get; set; }

        /// <summary>
        /// 是否提交
        /// </summary>
        public bool IsSubmit { get; set; }
    }
}
