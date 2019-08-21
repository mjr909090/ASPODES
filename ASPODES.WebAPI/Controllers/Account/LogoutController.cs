using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using ASPODES.Common.Util;
using ASPODES.Model;
using ASPODES.Database;
using ASPODES.WebAPI.Authorize;
using ASPODES.WebAPI.Common;
using ASPODES.WebAPI.Filter;

namespace ASPODES.WebAPI.Controllers
{
    /// <summary>
    /// 用户注销控制器
    /// </summary>
    [AspodesAuthorize]
    public class LogoutController : ApiController
    {
        // PUT api/logout
        /// <summary>
        /// 用户注销
        /// </summary>
        /// <returns>HttpResponseMessage</returns>
        [AspodesAuthorize]
        [ActionTrack]
        public HttpResponseMessage Put()
        {
            try 
            {
                using (var context = new AspodesDB())
                {
                    string userid = User.Identity.Name;
                    var log = context.LoginLogs.Where(c => c.UserId == userid && c.IsLogout==false).OrderByDescending(c=>c.LoginTime).FirstOrDefault();
                    if (log != null)
                    {
                        log.IsLogout = true;
                        log.User.LastLogin = DateTime.Now;
                        log.LoginTimeStamp = HashHelper.GetTimestamp();
                        context.SaveChanges();
                    }
                    else
                    {
                        return ResponseWrapper.ExceptionResponse(new OtherException( "无效的用户信息") );
                    }
                }
            }
            catch( Exception e )
            {
                return ResponseWrapper.ExceptionResponse( e);
            }
            return ResponseWrapper.SuccessResponse("注销成功");
        }

    }
}
