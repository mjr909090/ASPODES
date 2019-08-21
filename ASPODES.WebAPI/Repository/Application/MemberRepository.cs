using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using System.Data.Entity.Migrations;

using System.Net;
using System.Net.Http;
using AutoMapper;

using ASPODES.WebAPI.Common;
using ASPODES.Database;
using ASPODES.Model;
using ASPODES.DTO.Application;
using ASPODES.DTO.Inst_Person_User;
using ASPODES.WebAPI.Security;


namespace ASPODES.WebAPI.Repository
{
    /// <summary>
    /// 申请书成员操作类
    /// </summary>
    public class MemberRepository
    {
        /// <summary>
        /// 获取一个申请书成员
        /// </summary>
        /// <param name="applicationId">申请书ID</param>
        /// <param name="personId">人员ID</param>
        /// <returns></returns>
        public GetMemberDTO GetMemeber(string applicationId, int personId)
        {

            using (var ctx = new AspodesDB())
            {
                var member = ctx.Members.FirstOrDefault(m => m.ApplicationId == applicationId && m.PersonId == personId);
                if (member == null) throw new NotFoundException("未找到该成员");
                var memberDTO = Mapper.Map<GetMemberDTO>(member);
                return memberDTO;
            }
        }

        /// <summary>
        /// 获取成员列表
        /// </summary>
        /// <param name="predicate">检索条件</param>
        /// <returns></returns>
        public List<GetMemberDTO> GetApplicationMembersList(Func<Member, bool> predicate)
        {
            using (var ctx = new AspodesDB())
            {
                return ctx.Members.Where(predicate).Select(Mapper.Map<GetMemberDTO>).ToList();
            }
        }

        /// <summary>
        /// 获取协作单位的成员
        /// </summary>
        /// <param name="applicationId">申请书ID</param>
        /// <returns></returns>
        public IEnumerable<GetMemberDTO> GetApplicationAssistInstMembers(string applicationId)
        {
            using (var ctx = new AspodesDB())
            {
                var application = ctx.Applications.FirstOrDefault(a => a.ApplicationId == applicationId);
                if (application == null)
                    throw new NotFoundException();
                var members = ctx.Members
                    .Where(m => (m.ApplicationId == applicationId) && (m.Person.InstituteId.Value != application.InstituteId.Value))
                    .Select(Mapper.Map<GetMemberDTO>)
                    .ToList();
                return members;
            }

        }

        /// <summary>
        /// 获取项目主持单位的申请书成员，不包含申请人
        /// </summary>
        /// <param name="applicationId">申请书ID</param>
        /// <returns></returns>
        public List<GetMemberDTO> GetApplicationLeaderInstMembers(string applicationId)
        {
            using (var ctx = new AspodesDB())
            {
                var application = ctx.Applications.FirstOrDefault(a => a.ApplicationId == applicationId);
                if (application == null)
                    throw new NotFoundException();
                var members = ctx.Members
                    .Where(m => m.ApplicationId == applicationId && m.Person.InstituteId == application.InstituteId)
                    .Select(Mapper.Map<GetMemberDTO>)
                    .ToList();
                //删除申请人
                GetMemberDTO leader = members.FirstOrDefault(m => m.PersonId == application.LeaderId);
                members.Remove(leader);
                return members;
            }
        }

        /// <summary>
        /// 更新成员信息
        /// </summary>
        /// <param name="memberDTO">更新的成员信息</param>
        /// <returns></returns>
        public GetMemberDTO UpdateMember(AddMemberDTO memberDTO, Func<Application, CurrentUser, bool> privilege)
        {
            using (var ctx = new AspodesDB())
            {
                var member = ctx.Members.FirstOrDefault(
                    m => m.ApplicationId == memberDTO.ApplicationId && m.PersonId == memberDTO.PersonId);
                if (member == null)
                    throw new NotFoundException("未找到成员信息");
                if (!privilege(member.Application, UserHelper.GetCurrentUser()))
                {
                    throw new UnauthorizationException();
                }
                member.Task = memberDTO.Task;
                ctx.SaveChanges();
                return Mapper.Map<GetMemberDTO>(member);
            }
        }


        /// <summary>
        /// 添加申请书成员
        /// </summary>
        /// <param name="memberDTO">成员信息</param>
        /// <returns></returns>
        public GetMemberDTO AddMember(AddMemberDTO memberDTO, Func<Application, CurrentUser, bool> privilege)
        {
            using (var ctx = new AspodesDB())
            {
                var application = ctx.Applications.FirstOrDefault(a => a.ApplicationId == memberDTO.ApplicationId);
                if (null == application) throw new NotFoundException();
                if (!privilege(application, UserHelper.GetCurrentUser()))
                {
                    throw new UnauthorizationException();
                }
                var old = ctx.Members.FirstOrDefault(m => m.ApplicationId == memberDTO.ApplicationId && m.PersonId == memberDTO.PersonId);
                if (old != null)
                {
                    throw new OtherException("人员已添加");
                }
                var member = ctx.Members.Add(Mapper.Map<Member>(memberDTO));
                ctx.SaveChanges();
                return Mapper.Map<GetMemberDTO>(member);
            }
        }

        /// <summary>
        /// 删除申请书成员
        /// </summary>
        /// <param name="applicationId">申请书ID</param>
        /// <param name="personId">人员ID</param>
        /// <returns></returns>
        public void Delete(string applicationId, int personId, Func<Application, CurrentUser, bool> privilege)
        {
            using (var ctx = new AspodesDB())
            {
                var member = ctx.Members.FirstOrDefault(m => m.ApplicationId == applicationId && m.PersonId == personId);
                if (member == null)
                    throw new NotFoundException();
                if (!privilege(member.Application, UserHelper.GetCurrentUser()))
                {
                    throw new UnauthorizationException();
                }
                ctx.Members.Remove(member);
                ctx.SaveChanges();
            }
        }
    }
}