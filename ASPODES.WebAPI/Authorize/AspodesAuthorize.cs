using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web.Helpers;
using System.Web.Http;
using System.Web.Http.Controllers;
using ASPODES.WebAPI.Common;

namespace ASPODES.WebAPI.Authorize
{
    public class AspodesAuthorize : AuthorizeAttribute
    {
        /// <summary>
        /// 处理没有权限的请求
        /// </summary>
        /// <param name="filterContext"></param>
        protected override void HandleUnauthorizedRequest(HttpActionContext filterContext)
        {
            base.HandleUnauthorizedRequest(filterContext);

            var response = filterContext.Response = filterContext.Response ?? new HttpResponseMessage();
            response.StatusCode = HttpStatusCode.Forbidden;
            var content = new Result
            {
                status = 2,
                errorMsg = "您无权进行此操作"
            };
            response.Content = new StringContent(Json.Encode(content), Encoding.UTF8, "application/json");

            //response = Response.Create(HttpStatusCode.Unauthorized, ResponseStatus.identity_error, "您不具有该操作权限");
        }

    }

    public class Result
    {
        public int status { get; set; }
        public string errorMsg { get; set; }
    }
}
