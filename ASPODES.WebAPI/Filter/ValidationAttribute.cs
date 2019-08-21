using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http.Filters;
using System.Web.Http.Controllers;
using System.Net;
using System.Net.Http;
using ASPODES.WebAPI.Common;
using System.Text;

namespace ASPODES.WebAPI.Filter
{
    /// <summary>
    /// 通过过滤器实现模型验证
    /// </summary>
    public class ValidationAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting( HttpActionContext actionContext)
        {
            if (actionContext.ActionArguments.Any(kv => kv.Value == null))
            {
                actionContext.Response = ResponseWrapper.ExceptionResponse( new OtherException( "参数不能为NULL"));
            }

            if (actionContext.ModelState.IsValid == false)
            {
                StringBuilder msg = new StringBuilder();
                foreach (var key in actionContext.ModelState.Keys)
                {
                    var state = actionContext.ModelState[key];
                    foreach (var error in state.Errors)
                    {
                        msg.Append(error.ErrorMessage);
                        msg.Append("<br/>");
                    }
                }
                
                actionContext.Response = ResponseWrapper.ExceptionResponse( new OtherException(  msg.ToString()));
            }
        }
    }
}