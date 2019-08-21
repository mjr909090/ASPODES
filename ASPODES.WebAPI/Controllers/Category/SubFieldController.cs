using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

using ASPODES.WebAPI.Repository;
using ASPODES.WebAPI.Authorize;
using ASPODES.WebAPI.Common;

namespace ASPODES.WebAPI.Controllers
{
    /// <summary>
    /// 研究子领域操作
    /// </summary>
    [ASPODES.WebAPI.Filter.ActionTrack]
    [AspodesAuthorize]
    public class SubFieldController : ApiController
    {
        private SubFieldRepository repository = new SubFieldRepository();
        /// <summary>
        /// 获取研究子领域列表
        /// </summary>
        /// <returns></returns>
        public HttpResponseMessage Get()
        {
            try
            {
                return ResponseWrapper.SuccessResponse(repository.GetSubFields(sf => true));
            }
            catch (Exception e)
            {
                return ResponseWrapper.ExceptionResponse(e);
            }
        }

        /// <summary>
        /// 获取研究领域的子领域
        /// </summary>
        /// <param name="fieldName">学科名</param>
        /// <returns></returns>
        [Route("api/subfield/{fieldName}")]
        public HttpResponseMessage GetSubField( string fieldName )
        {
            try
            {
                return ResponseWrapper.SuccessResponse(repository.GetSubFields(sf => sf.ParentName == fieldName));
            }
            catch (Exception e)
            {
                return ResponseWrapper.ExceptionResponse(e);
            }
        }
    }
}
