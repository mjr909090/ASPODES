using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

using ASPODES.DTO.Application;
using ASPODES.WebAPI.Authorize;
using ASPODES.WebAPI.Repository;
using ASPODES.WebAPI.Common;
using ASPODES.WebAPI.Filter;

namespace ASPODES.WebAPI.Controllers
{
    /// <summary>
    /// 预算信息
    /// </summary>
    [AspodesAuthorize(Roles="用户")]
    [ActionTrack]
    public class AnnualBudgetController : ApiController
    {
        /// <summary>
        /// AnnualBudgetRepository对象
        /// </summary>
        private AnnualBudgetRepository repository = new AnnualBudgetRepository();

        /// <summary>
        /// 获取申请书的年度预算
        /// </summary>
        /// <param name="applicationId">申请书的ID</param>
        /// <returns>
        /// 成功返回ResponseStatus.success和年度预算列表，年度预算对象内含有预算条目列表
        /// 失败返回ResponseStatus.unkown_error和错误信息
        /// </returns>
        public HttpResponseMessage Get(string id)
        {
            try
            {
                return ResponseWrapper.SuccessResponse(repository.GetApplicationAnnualBudgets(id));
            }
            catch (Exception e)
            {
                return ResponseWrapper.ExceptionResponse(e);
            }
        }
        /// <summary>
        /// 根据申请书ID和年度，获取一个申请书某年的年底预算AnnualBudget
        /// </summary>
        /// <param name="applicationId">年度预算所属的申请书ID</param>
        /// <param name="year">第几年的预算，执行期为3年的项目年度预算的year值分别是1，2，3</param>
        /// <returns></returns>
        [Route("api/annualbudget/{applicationId}/{year}")]
        public HttpResponseMessage Get(string applicationId, int year)
        {
            try
            {
                return ResponseWrapper.SuccessResponse(repository.GetOneAnnualBudget(applicationId, year));
            }
            catch (Exception e)
            {
                return ResponseWrapper.ExceptionResponse(e);
            }
        }

    }
}
