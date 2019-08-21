using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

using ASPODES.WebAPI.Repository;
using ASPODES.WebAPI.Common;
using ASPODES.WebAPI.Authorize;
using ASPODES.WebAPI.Filter;

namespace ASPODES.WebAPI.Controllers
{
    /// <summary>
    /// 支出科目信息
    /// </summary>
    [ActionTrack]
    [AspodesAuthorize]
    public class AccountingSubjectController : ApiController
    {
        private AccountSubjectRepository repository = new AccountSubjectRepository();

        /// <summary>
        /// 获取支出科目列表
        /// </summary>
        /// <returns></returns>
        public HttpResponseMessage Get()
        {
            try
            {
                return ResponseWrapper.SuccessResponse(repository.GetAccountSubjects());
            }
            catch (Exception e)
            {
                return ResponseWrapper.ExceptionResponse(e);
            }
        }
    }
}
