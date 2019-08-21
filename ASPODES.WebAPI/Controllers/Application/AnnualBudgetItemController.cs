using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;

using ASPODES.DTO.Application;
using ASPODES.WebAPI.Authorize;
using ASPODES.WebAPI.Repository;
using ASPODES.WebAPI.Common;
using ASPODES.WebAPI.Filter;
using ASPODES.Model;
using ASPODES.WebAPI.Security;

namespace ASPODES.WebAPI.Controllers
{
    /// <summary>
    /// 年度预算的详细条目信息
    /// </summary>
    [AspodesAuthorize(Roles="用户")]
    [ActionTrack]
    public class AnnualBudgetItemController : ApiController
    {
        private AnnualBudgetItemRepository repository = new AnnualBudgetItemRepository();
        private Privilege privilege = new Privilege();
        /// <summary>
        /// 为年度预算增加一条预算条目,或者更改一条项目预算
        /// </summary>
        /// <param name="itemDTO">预算条目信息</param>
        /// <returns></returns>
        public HttpResponseMessage Post( AddAnnualBudgetItemDTO　itemDTO )
        {
            try
            {
                return ResponseWrapper.SuccessResponse(repository.AddOrUpdateAnnualBudgetItem(itemDTO, privilege.UserEditApplication));
            }
            catch (Exception e)
            {
                return ResponseWrapper.ExceptionResponse(e);
            }
        }

        /// <summary>
        /// 删除一条预算条目
        /// </summary>
        /// <param name="id">预算条目ID</param>
        /// <returns></returns>
        public HttpResponseMessage Delete( int id )
        {
            try
            {
                repository.Delete(id, privilege.UserEditApplication);
                return ResponseWrapper.SuccessResponse();
            }
            catch (Exception e)
            {
                return ResponseWrapper.ExceptionResponse(e);
            }
        }
    }
}