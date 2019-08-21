using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using ASPODES.WebAPI.Authorize;
using ASPODES.WebAPI.Repository;
using ASPODES.WebAPI.Filter;
using ASPODES.WebAPI.Common;

namespace ASPODES.WebAPI.Controllers.Category
{
    /// <summary>
    /// 项目类型操作
    /// </summary>
    [ActionTrack]
    [AspodesAuthorize]
    public class ProjectTypeController : ApiController
    {
        private ProjectTypeRepository repository = new ProjectTypeRepository();

        /// <summary>
        /// 获取项目类型列表
        /// </summary>
        /// <returns></returns>
        public HttpResponseMessage Get()
        {
            try
            {
                return ResponseWrapper.SuccessResponse(repository.GetProjectTypes(pt => true));
            }
            catch (Exception e)
            {
                return ResponseWrapper.ExceptionResponse(e);
            }

        }

        /// <summary>
        /// 院管理员获取项目类型列表（只返回院管理员分管的项目类型）
        /// </summary>
        /// <returns></returns>
        [Route("api/ProjectType/Deaprt")]
        public HttpResponseMessage GetDepart()
        {
            try
            {
                var userInfo = UserHelper.GetCurrentUser();
                return ResponseWrapper.SuccessResponse(repository.GetProjectTypes(pt => userInfo.ProjectTypeIds.Contains(pt.ProjectTypeId)));
            }
            catch (Exception e)
            {
                return ResponseWrapper.ExceptionResponse(e);
            }

        }
    }
}
