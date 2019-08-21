using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

using ASPODES.WebAPI.Repository;
using ASPODES.WebAPI.Authorize;
using ASPODES.WebAPI.Common;

namespace ASPODES.WebAPI.Controllers.Category
{
    /// <summary>
    /// 资助类别操作
    /// </summary>
    [ASPODES.WebAPI.Filter.ActionTrack]
    [AspodesAuthorize]
    public class SupportCategoryController : ApiController
    {
        private SupportCategoryRepository repository = new SupportCategoryRepository();
        
        /// <summary>
        /// 获取资助类别列表
        /// </summary>
        /// <returns></returns>
        public HttpResponseMessage Get()
        {
            try
            {
                return ResponseWrapper.SuccessResponse(repository.GetSupportCategorys(sc => true));
            }
            catch (Exception e)
            {
                return ResponseWrapper.ExceptionResponse(e);
            }
        }

        /// <summary>
        /// 获取某一项目类型的资助类别
        /// </summary>
        /// <param name="projectTypeId">项目类型ID</param>
        /// <returns></returns>
        [Route("api/supportcategory/{projectTypeId}")]
        public HttpResponseMessage Get( int projectTypeId )
        {
            try
            {
                return ResponseWrapper.SuccessResponse(repository.GetSupportCategorys(sc => sc.ProjectTypeId == projectTypeId));
            }
            catch (Exception e)
            {
                return ResponseWrapper.ExceptionResponse(e);
            }
        }
    }
}
