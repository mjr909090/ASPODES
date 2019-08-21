using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using ASPODES.Common.Util;
using ASPODES.DTO.Inst_Person_User;
using ASPODES.Database;
using ASPODES.Model;
using ASPODES.WebAPI.Common;
using ASPODES.WebAPI.Filter;
using ASPODES.WebAPI.Models;
using Newtonsoft.Json;
using ASPODES.WebAPI.Security;
using ASPODES.WebAPI.Repository.System;

namespace ASPODES.WebAPI.Controllers
{
    /// <summary>
    /// 用户登录控制器
    /// </summary>
    [ActionTrack]
    public class LoginController : ApiController
    {
        //private AspodesDB _context = new AspodesDB();
        // POST api/login
        /// <summary>
        /// 用户登录
        /// </summary>
        /// <param name="dto">用户名及密码</param>
        /// <returns>HttpResponseMessage</returns>
        [Validation]
        public HttpResponseMessage Post( LoginDTO dto )
        {
            try 
            {
                using (AspodesDB _context = new AspodesDB())
                {
                    string pwd = HashHelper.IntoMd5(dto.Password);
                    //User user = _context.Users.FirstOrDefault(u => u.UserId == dto.Username && u.Password == pwd && u.Person.Status != "D");

                    //20170114 为演示方便，更改登录验证为 EnglishName 登录（英文名默认为汉字转拼音）
                    string username = dto.Username.ToLower().Replace("@qq.com","").ToUpper();
                    User user = _context.Users.FirstOrDefault(u => (u.Person.EnglishName == username || u.UserId == dto.Username) && u.Password == pwd && u.Person.Status != "D");

                    if (null == user)
                    {
                        return ResponseWrapper.ExceptionResponse(new OtherException("用户名或密码错误！"));
                    }
                    var roles = (from r in _context.Roles
                        join a in _context.Authorizes
                        on r.RoleId equals a.RoleId
                        join u in _context.Users
                        on a.UserId equals u.UserId
                        where u.UserId == user.UserId
                        select r.Name).ToArray();

                    LoginData ld=new LoginData();
                    ld.UserId = user.UserId;
                    ld.UserName = user.Name;
                    ld.UserTimeStamp = HashHelper.GetTimestamp();
                    ld.Roles = roles;
                    
                    if (user.Person.PersonId.HasValue)
                    {
                        ld.PersonId = user.Person.PersonId.Value;
                    }
                    else
                    {
                        ld.PersonId = 0;
                    }
                    
                    LoginLog log = new LoginLog();
                    log.LoginLogId = Guid.NewGuid().ToString();
                    log.UserId = user.UserId;
                    log.Roles = string.Join(",", roles); ;
                    log.LoginIP = Helper.GetClientIpAddress(Request);
                    log.LoginTime = DateTime.Now;
                    log.LastActiveTime = DateTime.Now;
                    log.IsLogout = false;
                    log.LoginTimeStamp = HashHelper.GetTimestamp(DateTime.Now.AddMinutes(HashHelper.LoginDuration));
                    log.Token = HashHelper.Encrypt(JsonConvert.SerializeObject(ld));
                    ld.Token = log.Token;

                    _context.LoginLogs.Add(log);

                    _context.SaveChanges();

                    CurrentUser current = new CurrentUser 
                    {
                        UserId = user.UserId,
                        PersonId = user.PersonId.Value,
                        InstId = user.Person.InstituteId.Value,
                        Name = user.Name,
                        Roles = roles
                    };
                    if (roles.Contains("院管理员"))
                    {
                        var projectTypes = from aa in _context.ApplicationAssignments where aa.UserId == user.UserId select aa.ProjectTypeId;
                        current.ProjectTypeIds = projectTypes.ToArray();
                    }

                    var cookie = new HttpCookie("user", UserHelper.SerializeUserInfo(current));
                    HttpContext.Current.Response.AppendCookie(cookie);

                    return ResponseWrapper.SuccessResponse( ld);
                }
            }
            catch (DbEntityValidationException e)
            {
                foreach (var eve in e.EntityValidationErrors)
                {
                    Console.WriteLine("Entity of type \"{0}\" in state \"{1}\" has the following validation errors:",
                        eve.Entry.Entity.GetType().Name, eve.Entry.State);
                    foreach (var ve in eve.ValidationErrors)
                    {
                        Console.WriteLine("- Property: \"{0}\", Error: \"{1}\"",
                            ve.PropertyName, ve.ErrorMessage);
                    }
                }
                return ResponseWrapper.ExceptionResponse(new OtherException("数据库连接错误"));
            }
            catch (Exception e)
            {
                var a = e.Message;
                return ResponseWrapper.ExceptionResponse(e);
            }
        }

        /// <summary>
        /// 用户忘记密码
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [Validation]
        [HttpPost]
        [Route("api/forgetPassword")]
        public HttpResponseMessage ForgetPassword(ForgetPasswordDTO dto)
        {
            try
            {
                using (AspodesDB _context = new AspodesDB())
                {
                    User user = _context.Users.FirstOrDefault(u => u.IDCard == dto.IDCard && u.Name == dto.Name && u.Person.Status != "D");
                    if (null == user)
                    {
                        return ResponseWrapper.ExceptionResponse(new OtherException("填入信息有误"));
                    }
                    string IdentifyCode = StringExtensions.GetRandomString(6, true, true);
                    Email email = new Email();
                    email.SenderId = SystemConfig.SenderAddress;
                    email.ReceiverId = user.UserId;
                    email.Content = "[农科院]验证码："+ IdentifyCode + "，农科院用户，您正在通过邮箱重置您的密码[验证码告知他人将导致账号被盗，请勿泄露]";
                    email.ReciveAddress = user.Person.Email;
                    email.Subject = "农科院";
                    email.IdentifyCode = IdentifyCode;
                    EmailRepository mailRepository = new EmailRepository();
                    mailRepository.AddEmail(email);
                    return ResponseWrapper.SuccessResponse("邮件已发送成果，请注意查收，部分邮件会被自动删除，请在回收箱中仔细查找");
                }
            }
            catch (DbEntityValidationException e)
            {
                foreach (var eve in e.EntityValidationErrors)
                {
                    Console.WriteLine("Entity of type \"{0}\" in state \"{1}\" has the following validation errors:",
                        eve.Entry.Entity.GetType().Name, eve.Entry.State);
                    foreach (var ve in eve.ValidationErrors)
                    {
                        Console.WriteLine("- Property: \"{0}\", Error: \"{1}\"",
                            ve.PropertyName, ve.ErrorMessage);
                    }
                }
                return ResponseWrapper.ExceptionResponse(new OtherException("数据库连接错误"));
            }
            catch (Exception e)
            {
                var a = e.Message;
                return ResponseWrapper.ExceptionResponse(e);
            }
        }

        /// <summary>
        /// 用户重置密码
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [Validation]
        [HttpPost]
        [Route("api/resetPassword")]
        public HttpResponseMessage ResetPassword(ResetPasswordDTO dto)
        {
            try
            {
                using (AspodesDB _context = new AspodesDB())
                {
                    User user = _context.Users.FirstOrDefault(u => u.IDCard == dto.IDCard && u.Name == dto.Name && u.Person.Status != "D");
                    if (null == user)
                    {
                        return ResponseWrapper.ExceptionResponse(new OtherException("填入信息有误"));
                    }

                    Email email = _context.Emails.OrderBy(e => e.SendTime).FirstOrDefault(e => e.ReceiverId == user.UserId && e.IdentifyCode.Equals(dto.IdentifyCode));
                    if(null == email) return ResponseWrapper.ExceptionResponse(new OtherException("验证码有误或已过期"));
                    if (dto.IdentifyCode != email.IdentifyCode) return ResponseWrapper.ExceptionResponse(new OtherException("验证码有误或已过期"));

                    user.Password = HashHelper.IntoMd5(dto.Password);

                    _context.SaveChanges();

                    return ResponseWrapper.SuccessResponse("重置密码成功，请使用新密码登录");
                }
            }
            catch (DbEntityValidationException e)
            {
                foreach (var eve in e.EntityValidationErrors)
                {
                    Console.WriteLine("Entity of type \"{0}\" in state \"{1}\" has the following validation errors:",
                        eve.Entry.Entity.GetType().Name, eve.Entry.State);
                    foreach (var ve in eve.ValidationErrors)
                    {
                        Console.WriteLine("- Property: \"{0}\", Error: \"{1}\"",
                            ve.PropertyName, ve.ErrorMessage);
                    }
                }
                return ResponseWrapper.ExceptionResponse(new OtherException("数据库连接错误"));
            }
            catch (Exception e)
            {
                var a = e.Message;
                return ResponseWrapper.ExceptionResponse(e);
            }
        }
    }
}
