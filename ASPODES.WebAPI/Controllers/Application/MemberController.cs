using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

using ASPODES.DTO.Application;
using ASPODES.WebAPI.Repository;
using ASPODES.WebAPI.Common;
using ASPODES.WebAPI.Filter;
using ASPODES.WebAPI.Security;
using ASPODES.Model;

namespace ASPODES.WebAPI.Controllers
{
    /// <summary>
    /// 申请书的成员操作
    /// </summary>
    [ActionTrack]
    [Authorize(Roles="用户")]
    public class MemberController : ApiController
    {
        private MemberRepository repository = new MemberRepository();
        private Privilege privilege = new Privilege();

        /// <summary>
        /// 获取申请书的一个成员
        /// </summary>
        /// <param name="applicationId">申请书ID</param>
        /// <param name="personId">人员ID</param>
        /// <returns></returns>
        [Route("api/member/{applicationId}/{personId}")]
        public HttpResponseMessage GetApplicationMember( string applicationId, int personId)
        {
            try
            {
                return ResponseWrapper.SuccessResponse(repository.GetMemeber(applicationId, personId));
            }
            catch (Exception e)
            {
                return ResponseWrapper.ExceptionResponse(e);
            }
        }

        /// <summary>
        /// 获取申请书的所有成员
        /// UTL:api/member
        /// </summary>
        /// <param name="id">申请书ID</param>
        /// <returns></returns>
        public HttpResponseMessage GetApplicationMembers(string id)
        {
            try
            {
                return ResponseWrapper.SuccessResponse(repository.GetApplicationMembersList(m => m.ApplicationId == id));
            }
            catch (Exception e)
            {
                return ResponseWrapper.ExceptionResponse(e);
            }
        }

        /// <summary>
        /// 获取申请书承担单位的成员
        /// </summary>
        /// <param name="applicationId">申请书ID</param>
        /// <returns></returns>
        [Route("api/member/leaderInst/{applicationId}")]
        public HttpResponseMessage GetApplicationLeaderInstMember(string applicationId)
        {
            try
            {
                return ResponseWrapper.SuccessResponse(repository.GetApplicationLeaderInstMembers(applicationId));
            }
            catch (Exception e)
            {
                return ResponseWrapper.ExceptionResponse(e);
            }
        }

        /// <summary>
        /// 获取申请书非承担单位的成员
        /// </summary>
        /// <param name="applicationId">申请书ID</param>
        /// <returns></returns>
        [Route("api/member/assistInst/{applicationId}")]
        public HttpResponseMessage GetApplicationAssistInstMember(string applicationId)
        {
            try
            {
                return ResponseWrapper.SuccessResponse(repository.GetApplicationAssistInstMembers(applicationId));
            }
            catch (Exception e)
            {
                return ResponseWrapper.ExceptionResponse(e);
            }
        }

        /// <summary>
        /// 获取申请书某一单位的成员
        /// </summary>
        /// <param name="instId">单位ID</param>
        /// <param name="applicationId">申请书ID</param>
        /// <returns></returns>
        [Route("api/member/instmember/{instId}/{applicationId}")]
        public HttpResponseMessage GetApplicationInstMember(int instId, string applicationId)
        {
            try
            {
                Func<Member, bool> predicate = m => m.ApplicationId == applicationId && m.Person.InstituteId == instId;
                return ResponseWrapper.SuccessResponse(repository.GetApplicationMembersList(predicate));
            }
            catch (Exception e)
            {
                return ResponseWrapper.ExceptionResponse(e);
            }
        }

        /// <summary>
        /// 添加申请书成员
        /// </summary>
        /// <param name="memberDTO">成员信息</param>
        /// <returns></returns>
        public HttpResponseMessage Post(AddMemberDTO memberDTO)
        {
            try
            {
                return ResponseWrapper.SuccessResponse(repository.AddMember(memberDTO, privilege.UserEditApplication));
            }
            catch (Exception e)
            {
                return ResponseWrapper.ExceptionResponse(e);
            }
        }

        /// <summary>
        /// 更新成员信息
        /// </summary>
        /// <param name="memberDTO">成员信息</param>
        /// <returns></returns>
        public HttpResponseMessage Put(AddMemberDTO memberDTO)
        {
            try
            {
                return ResponseWrapper.SuccessResponse(repository.UpdateMember(memberDTO, privilege.UserEditApplication));
            }
            catch (Exception e)
            {
                return ResponseWrapper.ExceptionResponse(e);
            }
        }

        /// <summary>
        /// 删除申请书成员
        /// </summary>
        /// <param name="applicationId">申请书ID</param>
        /// <param name="personId">人员ID</param>
        /// <returns></returns>
        [Route("api/member/{applicationId}/{personId}")]
        public HttpResponseMessage Delete(string applicationId, int personId)
        {
            try
            {
                repository.Delete(applicationId, personId, privilege.UserEditApplication);
                return ResponseWrapper.SuccessResponse("删除成功");
            }
            catch (Exception e)
            {
                return ResponseWrapper.ExceptionResponse(e);
            }
        }
    }
}
