using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Net.Http.Headers;
using System.Security.Principal;
using System.Web.UI.WebControls;
using ASPODES.Common.Util;
using ASPODES.Database;
using ASPODES.Model;
using ASPODES.WebAPI.Common;
using ASPODES.WebAPI.Models;
using Newtonsoft.Json;
using System.Security.Claims;
using System.Runtime.Serialization;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace ASPODES.WebAPI.Security
{

    public class AuthorizationMessageHandler : DelegatingHandler
    {
        protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request,CancellationToken cancellationToken)
        {

            HttpRequestHeaders headers = request.Headers;

            //登录，忘记密码，重置密码
            if (request.RequestUri.AbsolutePath.Equals("/api/login") 
                || request.RequestUri.AbsolutePath.Equals("/api/forgetPassword") 
                || request.RequestUri.AbsolutePath.Equals("/api/resetPassword"))
            {
                return base.SendAsync(request, cancellationToken);
            }
            if (request.RequestUri.AbsolutePath.Equals("/api/Notice/StartIIS"))
            {
                return base.SendAsync(request, cancellationToken);
            }

            if (headers.Authorization != null)
            {
                //含有Token
                if (headers.Authorization.Scheme=="Basic")
                {
                    try
                    {
                        LoginData ld = JsonConvert.DeserializeObject<LoginData>(HashHelper.Decrypt(headers.Authorization.Parameter.Trim()));
                        using (var db = new AspodesDB())
                        {
                            LoginLog llog = db.LoginLogs.Where(c => c.UserId == ld.UserId && c.IsLogout == false)
                                .OrderByDescending(c => c.LoginTime)
                                .FirstOrDefault();
                            if (llog != null)
                            {
                                if (headers.Authorization.Parameter.Trim() == llog.Token)
                                {
                                    //验证数据正确
                                    //判断时间是否有效
                                    if (llog.LoginTimeStamp>HashHelper.GetTimestamp())
                                    {
                                        llog.LastActiveTime=DateTime.Now;
                                        llog.LoginTimeStamp = HashHelper.GetTimestamp(DateTime.Now.AddMinutes(HashHelper.LoginDuration));

                                        db.SaveChanges();

                                        var roles = (from r in db.Roles
                                            join a in db.Authorizes
                                            on r.RoleId equals a.RoleId
                                            join u in db.Users
                                            on a.UserId equals u.UserId
                                                     where u.UserId == llog.UserId
                                            select r.Name).ToArray();

                                        
                                        //生成Principal
                                        var identity = new GenericIdentity(llog.UserId);
                                        UserPrincipal principal;

                                        if (roles.Length>0)
                                        {
                                            principal = new UserPrincipal(identity, roles);
                                        }
                                        else
                                        {
                                            principal = new UserPrincipal(identity, new string[] {""});
                                        }

                                        CurrentUser userInfo = null;
                                        try
                                        {
                                            HttpCookie userCookie = HttpContext.Current.Request.Cookies["user"];
                                            byte[] buffer = Convert.FromBase64String(userCookie.Value);
                                            IFormatter formatter = new BinaryFormatter();
                                            MemoryStream stream = new MemoryStream(buffer);
                                            userInfo = (CurrentUser)formatter.Deserialize(stream);
                                            principal.UserInfo = userInfo;
                                        }
                                        catch( Exception e)
                                        {
                                            var projectTypes =  db.ApplicationAssignments
                                                .Where( aa=>aa.UserId == llog.UserId )
                                                .Select( aa=>aa.ProjectTypeId)
                                                .ToArray();
                                            //测试时自动补全数据
                                            userInfo=new CurrentUser
                                            {
                                                UserId = llog.UserId,
                                                InstId = llog.User.InstituteId.Value,
                                                Roles = roles,
                                                PersonId = llog.User.PersonId.Value,
                                                Name = llog.User.Name,
                                                ProjectTypeIds = projectTypes,
                                            };
                                            principal.UserInfo = userInfo;
                                        }

                                        //登录身份不相符
                                        if (userInfo.UserId != llog.UserId)
                                        {
                                            return Task.FromResult(ResponseWrapper.ExceptionResponse(new UnauthorizationException("登录身份不相符")));
                                        }

                                        Thread.CurrentPrincipal = principal;
                                        if (HttpContext.Current != null)
                                        {
                                            HttpContext.Current.User = principal;
                                        }
                                    }
                                    else
                                    {
                                        //登录过期
                                        return Task.FromResult(ResponseWrapper.ExceptionResponse(new UnauthorizationException("登录已过期")));
                                    }
                                }
                                else
                                {
                                    //token错误，在其他位置登录
                                    return Task.FromResult(ResponseWrapper.ExceptionResponse(new UnauthorizationException("用户在其他位置登录")));
                                }
                            }
                            else
                            {
                                //找不到登录用户信息
                                return Task.FromResult(ResponseWrapper.ExceptionResponse(new UnauthorizationException("无效的用户信息")));

                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        return Task.FromResult(ResponseWrapper.ExceptionResponse(new UnauthorizationException("认证错误")));
                    }
                }
                else
                {
                    return Task.FromResult(ResponseWrapper.ExceptionResponse(new UnauthorizationException("授权验证错误")));
                }
                return base.SendAsync(request, cancellationToken);
            }

            //Tester();
            //return base.SendAsync(request, cancellationToken);
            return Task.FromResult(ResponseWrapper.ExceptionResponse(new UnauthorizationException("请先登录")));
        }

        private void Tester()
        {
            
            using( var ctx = new AspodesDB() )
            {
                var user = ctx.Users.FirstOrDefault(u => u.UserId == "zhangjiangli@777.com");
                var projectTypes = ctx.ApplicationAssignments
                                                .Where(aa => aa.UserId == user.UserId)
                                                .Select(aa => aa.ProjectTypeId)
                                                .ToArray();
                var roles = ctx.Authorizes.Where( a=>a.UserId == user.UserId ).Select( a=>a.Role.Name ).ToArray();

                var identity = new GenericIdentity(user.UserId);
                var principal = new UserPrincipal(identity, roles);

                //测试时自动补全数据
                var userInfo = new CurrentUser
                {
                    UserId = user.UserId,
                    InstId = user.InstituteId.Value,
                    Roles = roles,
                    PersonId = user.PersonId.Value,
                    Name = user.Name,
                    ProjectTypeIds = projectTypes,
                };
                principal.UserInfo = userInfo;
                Thread.CurrentPrincipal = principal;
                if (HttpContext.Current != null)
                {
                    HttpContext.Current.User = principal;
                }
            }
        }
    }

}