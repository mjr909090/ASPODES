using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;
using ASPODES.Database;
using ASPODES.Model;
using ASPODES.WebAPI.Service;

namespace ASPODES.WebAPI.Filter
{
    /// <summary>
    /// 通过过滤器实现记录
    /// </summary>
    public class ActionTrackAttribute : ActionFilterAttribute
    {
        public ActionTrackAttribute()
        {

        }
        
        private static readonly string key = "enterTime";
        /// <summary>
        /// 请求执行前的异步操作
        /// </summary>
        /// <param name="actionContext"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public override Task OnActionExecutingAsync(HttpActionContext actionContext, CancellationToken cancellationToken)
        {
            if (SkipLogging(actionContext))
            {
                return base.OnActionExecutingAsync(actionContext, cancellationToken);

            }
            //记录进入请求的时间
            actionContext.Request.Properties[key] = DateTime.Now.ToBinary();

            return base.OnActionExecutingAsync(actionContext, cancellationToken);
        }

        /// <summary>
        /// 在请求执行完后 记录请求的数据以及返回数据
        /// </summary>
        /// <param name="actionExecutedContext"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public override Task OnActionExecutedAsync(HttpActionExecutedContext actionExecutedContext, CancellationToken cancellationToken)
        {
            object beginTime = null;
            if (actionExecutedContext.Request.Properties.TryGetValue(key, out beginTime))
            {
                DateTime time = DateTime.FromBinary(Convert.ToInt64(beginTime));
                HttpRequest request = HttpContext.Current.Request;
                UAParserUserAgent userAgent = new UAParserUserAgent((HttpContextBase)actionExecutedContext.Request.Properties["MS_HttpContext"]);
                DateTime nowTime=DateTime.Now;

                //VisitLog apiActionLog = new VisitLog
                //{
                //    //VisitId = Guid.NewGuid(),
                //    //获取action名称
                //    ActionName = actionExecutedContext.ActionContext.ActionDescriptor.ActionName,
                //    //获取Controller 名称
                //    ControllerName = actionExecutedContext.ActionContext.ActionDescriptor.ControllerDescriptor.ControllerName,
                //    //获取action开始执行的时间
                //    BeginTime = time,
                //    EndTime = nowTime,
                //    //获取执行action的耗时
                //    CostTime = (nowTime - time).TotalMilliseconds,
                //    UsersAgents = request.UserAgent,
                //    //获取用户
                //    UserId = getUserID(),
                //    //获取访问的ip
                //    IPAddress = request.UserHostAddress,
                //    UserHostName = request.UserHostName,
                //    UrlReferrer = request.UrlReferrer != null ? request.UrlReferrer.AbsoluteUri : "",
                //    //获取request提交的参数
                //    RequestParamaters = GetRequestValues(actionExecutedContext),
                //    //获取response响应的结果
                //    ResponseResult = GetResponseValues(actionExecutedContext),
                //    RequestUri = request.Url.AbsoluteUri,

                //};
                using (var db = new AspodesDB())
                {
                    VisitLog apiActionLog = new VisitLog();
                    //获取action名称
                    apiActionLog.ActionName = actionExecutedContext.ActionContext.ActionDescriptor.ActionName;
                    //获取Controller 名称
                    apiActionLog.ControllerName = actionExecutedContext.ActionContext.ActionDescriptor.ControllerDescriptor.ControllerName;
                    //获取action开始执行的时间
                    apiActionLog.BeginTime = time;
                    apiActionLog.EndTime = nowTime;
                    //获取执行action的耗时
                    apiActionLog.CostTime = Math.Round((nowTime - time).TotalMilliseconds,2);
                    apiActionLog.UsersAgents = request.UserAgent;
                    //获取用户
                    apiActionLog.UserId = getUserID();
                    //获取访问的ip
                    apiActionLog.IPAddress = request.UserHostAddress;
                    apiActionLog.UserHostName = request.UserHostName;
                    apiActionLog.UrlReferrer = request.UrlReferrer != null ? request.UrlReferrer.AbsoluteUri : "";
                    //获取request提交的参数
                    apiActionLog.RequestParamaters = GetRequestValues(actionExecutedContext);
                    //获取response响应的结果
                    apiActionLog.ResponseResult = GetResponseValues(actionExecutedContext);
                    apiActionLog.RequestUri = request.Url.AbsoluteUri;
                    apiActionLog.UserDevice = userAgent.Device.ToString().Trim();
                    apiActionLog.UserBrowser = userAgent.UserAgent.ToString().Trim();
                    apiActionLog.UserOS = userAgent.OS.ToString().Trim();

                    if (userAgent.IsMobileDevice)
                    {
                        apiActionLog.UserPartform = "手机";
                    }

                    else if (userAgent.IsTablet)
                    {
                        apiActionLog.UserPartform = "平板";
                    }
                    else
                    {
                        apiActionLog.UserPartform = "普通电脑";
                    }


                    db.VisitLogs.Add(apiActionLog);
                    db.SaveChanges();
                }
            }
            return base.OnActionExecutedAsync(actionExecutedContext, cancellationToken);

        }

        public static string getUserID()
        {
            if (HttpContext.Current.User.Identity.IsAuthenticated)
            {
                return HttpContext.Current.User.Identity.Name;
            }
            else
            {
                return "未登录";
            }
        }

        /// <summary>
        /// 读取request 的提交内容
        /// </summary>
        /// <param name="actionExecutedContext"></param>
        /// <returns></returns>
        public string GetRequestValues(HttpActionExecutedContext actionExecutedContext)
        {

            Stream stream = actionExecutedContext.Request.Content.ReadAsStreamAsync().Result;
            Encoding encoding = Encoding.UTF8;
            /*
                这个StreamReader不能关闭，也不能dispose
                因为你关掉后，后面的管道  或拦截器就没办法读取了
            */
            var reader = new StreamReader(stream, encoding);
            string result = reader.ReadToEnd();
            /*
            这里也要注意：   stream.Position = 0;
            当你读取完之后必须把stream的位置设为开始
            因为request和response读取完以后Position到最后一个位置，交给下一个方法处理的时候就会读不到内容了。
            */
            stream.Position = 0;
            return result;
        }

        /// <summary>
        /// 读取action返回的result
        /// </summary>
        /// <param name="actionExecutedContext"></param>
        /// <returns></returns>
        public string GetResponseValues(HttpActionExecutedContext actionExecutedContext)
        {
            Stream stream = actionExecutedContext.Response.Content.ReadAsStreamAsync().Result;
            Encoding encoding = Encoding.UTF8;
            /*
            这个StreamReader不能关闭，也不能dispose
            因为你关掉后，后面的管道  或拦截器就没办法读取了
            */
            var reader = new StreamReader(stream, encoding);
            string result = reader.ReadToEnd();
            /*
            这里也要注意：   stream.Position = 0; 
            当你读取完之后必须把stream的位置设为开始
            因为request和response读取完以后Position到最后一个位置，交给下一个方法处理的时候就会读不到内容了。
            */
            stream.Position = 0;
            return result;
        }
        /// <summary>
        /// 判断类和方法头上的特性是否要进行Action拦截
        /// </summary>
        /// <param name="actionContext"></param>
        /// <returns></returns>
        private static bool SkipLogging(HttpActionContext actionContext)
        {
            return actionContext.ActionDescriptor.GetCustomAttributes<NoLogAttribute>().Any() || actionContext.ActionDescriptor.ControllerDescriptor.GetCustomAttributes<NoLogAttribute>().Any();
        }
    }
}
