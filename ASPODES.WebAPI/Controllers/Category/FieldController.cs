using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

using ASPODES.WebAPI.Repository;
using ASPODES.WebAPI.Filter;
using ASPODES.WebAPI.Authorize;
using ASPODES.WebAPI.Common;

namespace ASPODES.WebAPI.Controllers
{
    /// <summary>
    /// 研究领域操作
    /// </summary>
    [ActionTrack]
    [AspodesAuthorize]
    public class FieldController : ApiController
    {
        private FieldRepository repository = new FieldRepository();

        /// <summary>
        /// 获取研究领域列表
        /// </summary>
        /// <returns></returns>
        public HttpResponseMessage Get()
        {
            try
            {
                return ResponseWrapper.SuccessResponse(repository.GetFields());
            }
            catch (Exception e)
            {
                return ResponseWrapper.ExceptionResponse(e);
            }
        }
    }
}
