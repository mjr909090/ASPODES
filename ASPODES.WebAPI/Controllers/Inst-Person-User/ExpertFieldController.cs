using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Net.Http;
using System.Web.Http;
using ASPODES.WebAPI.Repository;
using ASPODES.DTO.Inst_Person_User;
using ASPODES.WebAPI.Authorize;
using ASPODES.WebAPI.Common;

namespace ASPODES.WebAPI.Controllers.Inst_Person_User
{
    /// <summary>
    /// 专家所属领域
    /// </summary>
    [ASPODES.WebAPI.Filter.ActionTrack]
    [AspodesAuthorize]
    public class ExpertFieldController : ApiController
    {
        private ExpertFieldRepository repository = new ExpertFieldRepository();
        
        /// <summary>
        /// 获取专家的研究领域列表
        /// Get:api/expertfield?userId={userId}
        /// </summary>
        /// <returns></returns>
        [Route("api/expertfield/{personId}")]
        public HttpResponseMessage Get( int personId )
        {
            try
            {
                return ResponseWrapper.SuccessResponse(repository.GetExpertField(personId));
            }
            catch (Exception e)
            {
                return ResponseWrapper.ExceptionResponse(e);
            }
        }

        /// <summary>
        /// 通过Id获取专家的研究领域列表
        /// Get:api/expertfield?userId={userId}
        /// </summary>
        /// <returns></returns>
        [Route("api/expertfieldById/{userId}")]
        public HttpResponseMessage Get(string UserId)
        {
            try
            {
                return ResponseWrapper.SuccessResponse(repository.GetExpertFieldByUserId(UserId));
            }
            catch (Exception e)
            {
                return ResponseWrapper.ExceptionResponse(e);
            }
        }

        /// <summary>
        /// 用户添加研究领域
        /// </summary>
        /// <param name="fieldDTO"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("api/expertfield/user"), Authorize(Roles="用户")]
        public HttpResponseMessage UserAddField( List<AddExpertFieldDTO> fieldDTOs )
        {
            try
            {
                var userInfo = UserHelper.GetCurrentUser();
                return ResponseWrapper.SuccessResponse(repository.AddOrUpdateExpertField(fieldDTOs, u => u.PersonId == userInfo.PersonId));
            }
            catch (Exception e)
            {
                return ResponseWrapper.ExceptionResponse(e);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="fieldDTO"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("api/expertfield/inst"), Authorize(Roles = "单位管理员")]
        public HttpResponseMessage InstAddField(List<AddExpertFieldDTO> fieldDTOs)
        {
            try
            {
                var userInfo = UserHelper.GetCurrentUser();
                return ResponseWrapper.SuccessResponse(repository.AddOrUpdateExpertField(fieldDTOs, u => u.InstituteId == userInfo.InstId));
            }
            catch (Exception e)
            {
                return ResponseWrapper.ExceptionResponse(e);
            }
        }

        /// <summary>
        /// 院管理员添加研究领域
        /// </summary>
        /// <param name="fieldDTO"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("api/expertfield/depart"), Authorize(Roles = "院管理员")]
        public HttpResponseMessage DepartAddField(List<AddExpertFieldDTO> fieldDTOs)
        {
            try
            {
                return ResponseWrapper.SuccessResponse(repository.AddOrUpdateExpertField(fieldDTOs, u => true));
            }
            catch (Exception e)
            {
                return ResponseWrapper.ExceptionResponse(e);
            }
        }

        /// <summary>
        /// 用户删除研究领域
        /// </summary>
        /// <param name="fieldId"></param>
        /// <returns></returns>
        [HttpDelete]
        [Route("api/expertfield/user/{fieldId}"),Authorize(Roles="用户")]
        public HttpResponseMessage UserDeleteField( int fieldId )
        {
            try
            {
                var userInfo = UserHelper.GetCurrentUser();
                repository.DeleteExpertField(fieldId, u => u.PersonId == userInfo.PersonId);
                return ResponseWrapper.SuccessResponse();
            }
            catch (Exception e)
            {
                return ResponseWrapper.ExceptionResponse(e);
            }
        }

        /// <summary>
        /// 单位管理员删除研究领域
        /// </summary>
        /// <param name="fieldId"></param>
        /// <returns></returns>
        [HttpDelete]
        [Route("api/expertfield/inst/{fieldId}"),Authorize(Roles="单位管理员")]
        public HttpResponseMessage InstDeleteField(int fieldId)
        {

            try
            {
                var userInfo = UserHelper.GetCurrentUser();
                repository.DeleteExpertField(fieldId, u => u.InstituteId == userInfo.InstId);
                return ResponseWrapper.SuccessResponse();
            }
            catch (Exception e)
            {
                return ResponseWrapper.ExceptionResponse(e);
            }
            
        }

        /// <summary>
        /// 院管理员删除研究领域
        /// </summary>
        /// <param name="fieldId"></param>
        /// <returns></returns>
        [HttpDelete]
        [Route("api/expertfield/depart/{fieldId}"),Authorize(Roles="院管理员")]
        public HttpResponseMessage DepartDeleteField(int fieldId)
        {
            try
            {
                repository.DeleteExpertField(fieldId, u => true);
                return ResponseWrapper.SuccessResponse();
            }
            catch (Exception e)
            {
                return ResponseWrapper.ExceptionResponse(e);
            }
            
        }
    }
}
