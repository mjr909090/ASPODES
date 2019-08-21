using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Net.Http;
using System.Web.Http;

using ASPODES.DTO.Application;
using ASPODES.WebAPI.Filter;
using ASPODES.WebAPI.Repository;
using ASPODES.WebAPI.Common;

namespace ASPODES.WebAPI.Controllers
{
    /// <summary>
    /// 申请书日志操作
    /// </summary>

    [ActionTrack]
    [Authorize(Roles = "用户")]
    public class ApplicationLogController : ApiController
    {
        private ApplicationLogRepository repository = new ApplicationLogRepository();

        /// <summary>
        /// 获取一个申请书某个类型的操作日志
        /// </summary>
        /// <param name="applicationId">申请书ID</param>
        /// <param name="actionType">日志类型</param>
        /// <returns></returns>
        [Route("api/applicationlog/{applicationId}/{actionType}")]
        public HttpResponseMessage GetApplicationLog(string applicationId, int actionType)
        {
            try
            {
                return ResponseWrapper.SuccessResponse(repository.GetApplicationLog(applicationId, actionType));
            }
            catch (Exception e)
            {
                return ResponseWrapper.ExceptionResponse(e);
            }
        }
    }
}
