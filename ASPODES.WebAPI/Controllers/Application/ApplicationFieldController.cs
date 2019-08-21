 using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web;
using System.Web.Http;

using ASPODES.DTO.Application;
using ASPODES.WebAPI.Repository;
using ASPODES.WebAPI.Common;
using ASPODES.WebAPI.Filter;
using ASPODES.WebAPI.Security;

namespace ASPODES.WebAPI.Controllers
{
    /// <summary>
    /// 申请书领域信息
    /// </summary>
    [Authorize(Roles = "用户")]
    [ActionTrack]
    public class ApplicationFieldController:ApiController
    {
        private Privilege privilege = new Privilege();
        private ApplicationFieldRepository repository = new ApplicationFieldRepository();
       
        /// <summary>
        /// 获取申请书的领域列表
        /// </summary>
        /// <param name="applicationId">申请书ID</param>
        /// <returns></returns>
        public HttpResponseMessage Get( string id)
        {
            try
            {
                return ResponseWrapper.SuccessResponse(repository.GetApplicationFields(id));
            }
            catch (Exception e)
            {
                return ResponseWrapper.ExceptionResponse(e);
            }
        }

        /// <summary>
        /// 添加或者更新一条申请书领域信息
        /// </summary>
        /// <param name="fieldDTO">需要更新的领域内容</param>
        /// <returns></returns>
        public HttpResponseMessage Post( List<AddApplicationFieldDTO> fieldDTOs  )
        {
            try
            {
                return ResponseWrapper.SuccessResponse(repository.AddOrUpdateApplicationField(fieldDTOs, privilege.UserEditApplication));
            }
            catch (Exception e)
            {
                return ResponseWrapper.ExceptionResponse(e);
            }
        }

        /// <summary>
        /// 删除申请书领域
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public HttpResponseMessage Delete( int id )
        {
            try
            {
                repository.DeleteApplicationField(id, privilege.UserEditApplication);
                return ResponseWrapper.SuccessResponse();
            }
            catch (Exception e)
            {
                return ResponseWrapper.ExceptionResponse(e);
            }
        }
    }
}