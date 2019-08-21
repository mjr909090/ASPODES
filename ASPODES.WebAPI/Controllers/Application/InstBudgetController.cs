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

namespace ASPODES.WebAPI.Controllers
{
    /// <summary>
    /// 申请书预算信息
    /// </summary>
    [ActionTrack]
    [Authorize(Roles = "用户")]
    public class InstBudgetController : ApiController
    {
        public InstBudgetRepository repository = new InstBudgetRepository();
        private Privilege privilege = new Privilege();
        /// <summary>
        /// 获取申请书的单位预算列表
        /// </summary>
        /// <param name="id">申请书ID</param>
        /// <returns></returns>
        public HttpResponseMessage Get(string id )
        {
            try
            {
                return ResponseWrapper.SuccessResponse(repository.GetInstBudgets(id));
            }
            catch (Exception e)
            {
                return ResponseWrapper.ExceptionResponse(e);
            }
        }

        /// <summary>
        /// 获取申请书某一单位的预算
        /// </summary>
        /// <param name="applicationId">申请书ID</param>
        /// <param name="instId">ID</param>
        /// <returns></returns>
        [Route("api/instbudget/{applicationId}/{instId}")]
        public HttpResponseMessage Get( string applicationId, int instId )
        {
            try
            {
                return ResponseWrapper.SuccessResponse(repository.GetInstBudget(applicationId, instId));
            }
            catch (Exception e)
            {
                return ResponseWrapper.ExceptionResponse(e);
            }
        }

        /// <summary>
        /// 添加或者替换单位预算信息
        /// </summary>
        /// <param name="budget">单位预算信息</param>
        /// <returns></returns>
        public HttpResponseMessage Post( AddInstTotalWithAnnualBudget budget)
        {
            try
            {
                var instBudget = repository.AddInstBudget(budget, privilege.UserEditApplication);
                return ResponseWrapper.SuccessResponse();
            }
            catch (Exception e)
            {
                return ResponseWrapper.ExceptionResponse(e);
            }
        }

        /// <summary>
        /// 删除单位预算
        /// </summary>
        /// <param name="id">单位预算ID</param>
        /// <returns></returns>
        public HttpResponseMessage Delete( int id )
        {
            try
            {
                repository.Delete(id, privilege.UserEditApplication);
                return ResponseWrapper.SuccessResponse("删除成功");
            }
            catch (Exception e)
            {
                return ResponseWrapper.ExceptionResponse(e);
            }
        }
    }
}
