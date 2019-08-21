using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ASPODES.Model;
using ASPODES.Database;
using ASPODES.WebAPI.Common;

namespace ASPODES.WebAPI.Repository
{
    /// <summary>
    /// 项目成员操作类
    /// </summary>
    public class ProjectMemberRepository:IProjectMemberRepository
    {
        private AspodesDB _ctx;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="ctx">DBContext</param>
        public ProjectMemberRepository( AspodesDB ctx )
        {
            _ctx = ctx;
        }

       
        /// <summary>
        /// 获得项目成员
        /// </summary>
        public IOrderedQueryable<ProjectMember> GerProjectMemberList( string projectId )
        {
            return _ctx.ProjectMembers.Include("Person").Where( m=>m.ProjectId == projectId).OrderBy(pm => pm.ProjectId);
        }

        /// <summary>
        /// 添加项目成员
        /// </summary>
        public ProjectMember Add( ProjectMember member )
        {
            var entity = _ctx.ProjectMembers.Add(member);
            _ctx.SaveChanges();
            return entity;
        }

        /// <summary>
        /// 修改项目成员
        /// </summary>
        public ProjectMember Update( ProjectMember member )
        {

            var invalid = _ctx.ProjectMembers.FirstOrDefault(pm => pm.ProjectId == member.ProjectId && pm.PersonId == member.PersonId);
            if (null == invalid) throw new NotFoundException();
            _ctx.Entry<ProjectMember>(invalid).CurrentValues.SetValues(member);
            return invalid;
        }

        /// <summary>
        /// 删除项目成员
        /// </summary>
        public void Delete( string projectId, int? personId )
        {
            var entity = _ctx.ProjectMembers.FirstOrDefault(pm => pm.ProjectId == projectId && pm.PersonId == personId);
            if (null == entity) throw new NotFoundException();
            _ctx.ProjectMembers.Remove(entity);
            _ctx.SaveChanges();
        }
    }
}