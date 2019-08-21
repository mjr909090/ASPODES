using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ASPODES.Database;
using System.Net.Http;
using ASPODES.Model;
using ASPODES.WebAPI.Common;
using ASPODES.DTO.Project;
using ASPODES.DTO.Inst_Person_User;

namespace ASPODES.WebAPI.Repository
{
    /// <summary>
    /// 项目操作类
    /// </summary>
    public class ProjectRepository : IProjectRepository
    {
        private AspodesDB _ctx;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="ctx">DBContext</param>
        public ProjectRepository(AspodesDB ctx)
        {
            _ctx = ctx;
        }

        /// <summary>
        /// 负责人获取项目
        /// </summary>
        /// <param name="leaderId">负责人Id</param>
        /// <param name="Status">项目状态</param>
        /// <returns></returns>
        public IOrderedQueryable<Project> GetLeaderProjectList(int leaderId, ProjectStatus Status)
        {
            return _ctx.Projects.Include("Docs").Include("Members").Include("AnnualTasks")
                   .Where(p => p.LeaderId == leaderId && (Status==ProjectStatus.ACTIVE ? (p.Status!=ProjectStatus.FINISH): (p.Status == Status)))
                   .OrderBy(p => p.StartDate);
        }

        /// <summary>
        /// 个人获取参与项目
        /// </summary>
        /// <param name="personId">参与人Id</param>
        /// <param name="Status">项目状态</param>
        /// <returns></returns>
        public IOrderedQueryable<Project> GetPersonJoinComboProjectList(int personId, ProjectStatus Status)
        {
            return _ctx.ProjectMembers
                   .Where(pm => pm.PersonId == personId)
                   .Select(pm => pm.Project)
                   .Where(p => p.LeaderId != personId && (Status == ProjectStatus.ACTIVE ? (p.Status != ProjectStatus.FINISH) : (p.Status == Status)))
                   .OrderBy(p => p.StartDate);
        }

        /// <summary>
        /// 单位获取项目
        /// </summary>
        /// <param name="instituteId">单位Id</param>
        /// <param name="Status">项目状态</param>
        /// <returns></returns>
        public IOrderedQueryable<Project> GetInstituteProjectList(int instituteId, ProjectStatus Status)
        {
            return _ctx.Projects
                  .Where(p => p.InstituteId == instituteId 
                         && (Status == ProjectStatus.ACTIVE ? (p.Status == Status || p.Status == ProjectStatus.DEPART_REJECT || p.Status == ProjectStatus.INST_REJECT) : (p.Status == Status)))
                  .OrderBy(p => p.StartDate);
        }


        public IQueryable<Project> GetProjectList( int instId, int year, int status, int?[] projectTypes)
        {
            int typeCount = _ctx.ProjectTypes.Count( pt=>pt.Enable );
            
            IQueryable<Project> resultSet = _ctx.Projects;
            if( null == projectTypes || typeCount == projectTypes.Length )
            {
                
            }
            else if( projectTypes.Length == 0 )
            {
                return Queryable.AsQueryable<Project>(new List<Project>() );
            }
            else if( projectTypes.Length == 1 )
            {
                int projectType = projectTypes[0].Value;
                resultSet = resultSet.Where( p=>p.ProjectTypeId == projectType );
            }
            else 
            {
                resultSet = resultSet.Where(p => projectTypes.Contains(p.ProjectTypeId.Value));
            }

            if( instId > 0 )
            {
                resultSet = resultSet.Where( p=>p.InstituteId == instId );
            }

            if( year > 0 )
            {
                resultSet = resultSet.Where(p => p.StartDate.Year == year);
            }

            if( status == 1 )
            {
                resultSet = resultSet.Where(p => p.Status == ProjectStatus.FINISH);
            }
            else
            {
                resultSet = resultSet.Where(p => p.Status != ProjectStatus.FINISH );
            }

            return resultSet;
        }
        /// <summary>
        /// 单位获取参与项目
        /// </summary>
        /// <param name="instituteId">单位Id</param>
        /// <param name="Status">项目状态</param>
        /// <returns></returns>
        public IOrderedQueryable<Project> GetInstituteJoinComboProjectList(int instituteId, ProjectStatus Status)
        {
            return _ctx.ProjectMembers
                   .Where(pm => pm.Person.InstituteId == instituteId)
                   .Select(pm => pm.Project)
                   .Distinct()
                   .Where(p => p.InstituteId != instituteId && (Status == ProjectStatus.ACTIVE ? (p.Status != ProjectStatus.FINISH) : (p.Status == Status)))
                   .OrderBy(p => p.StartDate);
        }

        /// <summary>
        /// 院获取项目
        /// </summary>
        /// <returns></returns>
        public IOrderedQueryable<Project> GetProjectList(ProjectStatus Status)
        {
            return _ctx.Projects.Where(p => Status == ProjectStatus.ACTIVE ? 
                                        (p.Status == Status || p.Status == ProjectStatus.DEPART_REJECT || p.Status == ProjectStatus.INST_REJECT) : 
                                        (p.Status == Status)).OrderBy(p => p.InstituteId).OrderBy(p => p.LeaderId);
        }

        /// <summary>
        /// 获取项目详细信息
        /// </summary>
        /// <param name="projectId">项目Id</param>
        /// <returns></returns>
        public Project GetProjectDetailList(string projectId)
        {
            return _ctx.Projects.Include("Docs").Include("Members").Include("AnnualTasks")
                   .FirstOrDefault(p => p.ProjectId == projectId);
        }

        /// <summary>
        /// 添加一个项目
        /// </summary>
        /// <param name="project">需要添加的实体</param>
        /// <returns></returns>
        public Project Add(Project project)
        {
            var entity = _ctx.Projects.Add(project);
            _ctx.SaveChanges();
            return entity;
        }

        /// <summary>
        /// 更新一个项目
        /// </summary>
        /// <param name="project">更新的值</param>
        /// <returns></returns>
        public Project Update(Project project)
        {
            var invalid = _ctx.Projects.FirstOrDefault(p => p.ProjectId == project.ProjectId);
            if (invalid == null) throw new NotFoundException();
            _ctx.Entry<Project>(invalid).CurrentValues.SetValues(project);
            _ctx.SaveChanges();
            return invalid;
        }

        /// <summary>
        /// 删除一个项目
        /// </summary>
        /// <param name="id">需要删除的项目ID</param>
        public void Delete(string id)
        {
            var project = _ctx.Projects.FirstOrDefault(p => p.ProjectId == id);
            if (project == null) throw new NotFoundException();
            _ctx.Projects.Remove(project);
            _ctx.SaveChanges();
        }

        public List<GetComboInstDTO> GetJionInsts(string projectId)
        {
            var project = _ctx.Projects.FirstOrDefault(p => p.ProjectId == projectId);
            if (project == null) throw new NotFoundException();
            ApplicationRepository rep = new ApplicationRepository();
            return rep.GetParticipateInst(project.ApplicationId);
        }
    }
}