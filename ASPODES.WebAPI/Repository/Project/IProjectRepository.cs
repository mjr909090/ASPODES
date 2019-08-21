using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ASPODES.Model;
using ASPODES.DTO.Project;
using ASPODES.DTO.Inst_Person_User;

namespace ASPODES.WebAPI.Repository
{
    public interface IProjectRepository
    {
        /// <summary>
        /// 院获取项目
        /// </summary>
        /// <returns></returns>
        IOrderedQueryable<Project> GetProjectList(ProjectStatus Status);

        /// <summary>
        /// 负责人获取项目
        /// </summary>
        /// <param name="leaderId">负责人Id</param>
        /// <param name="Status">项目状态</param>
        /// <returns></returns>
        IOrderedQueryable<Project> GetLeaderProjectList(int leaderId, ProjectStatus Status);

        /// <summary>
        /// 单位获取项目
        /// </summary>
        /// <param name="instituteId">单位Id</param>
        /// <param name="Status">项目状态</param>
        /// <returns></returns>
        IOrderedQueryable<Project> GetInstituteProjectList(int instituteId, ProjectStatus Status);

        /// <summary>
        /// 个人获取参与项目
        /// </summary>
        /// <param name="personId">参与人Id</param>
        /// <param name="Status">项目状态</param>
        /// <returns></returns>
        IOrderedQueryable<Project> GetPersonJoinComboProjectList(int personId, ProjectStatus Status);

        /// <summary>
        /// 单位获取参与项目
        /// </summary>
        /// <param name="instituteId">单位Id</param>
        /// <param name="Status">项目状态</param>
        /// <returns></returns>
        IOrderedQueryable<Project> GetInstituteJoinComboProjectList(int instituteId, ProjectStatus Status);


        /// <summary>
        /// 获取项目，按条件筛选
        /// </summary>
        /// <param name="instId">单位ID</param>
        /// <param name="year">立项年度</param>
        /// <param name="page">页码</param>
        /// <param name="projectTypes">项目类型，projectTypes=null选择所有类型</param>
        /// <returns></returns>
        IQueryable<Project> GetProjectList(int instId, int year, int status, int?[] projectTypes);

        /// <summary>
        /// 获取项目详细信息
        /// </summary>
        /// <param name="projectId">项目Id</param>
        /// <returns></returns>
        Project GetProjectDetailList(string projectId);

        /// <summary>
        /// 添加一个项目
        /// </summary>
        /// <param name="project">需要添加的实体</param>
        /// <returns></returns>
        Project Add(Project project);

        /// <summary>
        /// 更新一个项目
        /// </summary>
        /// <param name="project">更新的值</param>
        /// <returns></returns>
        Project Update(Project project);

        /// <summary>
        /// 删除一个项目
        /// </summary>
        /// <param name="id">需要删除的项目ID</param>
        void Delete(string id);


        List<GetComboInstDTO> GetJionInsts(string projectId);
    }
}
