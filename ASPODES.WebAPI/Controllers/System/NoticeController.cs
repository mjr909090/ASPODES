using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using ASPODES.Database;
using ASPODES.WebAPI.Authorize;
using ASPODES.WebAPI.Common;
using ASPODES.WebAPI.Filter;
using ASPODES.WebAPI.Repository;
using ASPODES.WebAPI.Service;
using ASPODES.WebAPI.Repository.System;
using System.Net.Mail;
using ASPODES.Model;

namespace ASPODES.WebAPI.Controllers
{
    /// <summary>
    /// 用户通知
    /// </summary>
    [AspodesAuthorize]
    [ActionTrack]
    public class NoticeController : ApiController
    {
        //private NoticeRepository repository = new NoticeRepository();
        private OperateNoticeService _noticeService;


        public NoticeController(OperateNoticeService noticeService)
        {
            this._noticeService = noticeService;
        }
        /// <summary>
        /// 获取用户通知列表
        /// </summary>
        /// <param name="page">页码</param>
        /// <param name="read">已读未读0-未读，1-已读，2-全部</param>
        /// <param name="userType">用户类型0-不对用户进行分类，1-普通用户和专家，2-单位管理员，3-院管理员</param>
        /// <returns></returns>
        [HttpGet]
        [Route("api/Notice/{page}/{read}/{userType}")]
        public HttpResponseMessage GetNoticeList(int page, int read, int userType)
        {
            try
            {
                //return ResponseWrapper.SuccessResponse(_noticeService.GetNotice(page, read,userType));
                return ResponseWrapper.SuccessResponse(_noticeService.GetNotice(page));
            }
            catch (Exception e)
            {
                return ResponseWrapper.ExceptionResponse(e);
            }

        }


        /// <summary>
        /// 发送邮件测试
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("api/email/send")]
        public HttpResponseMessage SendEmail()
        {
            try
            {
                Email email = new Email();
                email.SenderId = SystemConfig.SenderAddress;
                email.ReceiverId = "2803000719@qq.com";
                email.Content = "邮件测试，看看是否能发送成功aaaaaassssssss";
                email.ReciveAddress = "2803000719@qq.com";
                email.Subject = "邮件的主题";
                EmailRepository mailRepository = new EmailRepository();
                mailRepository.AddEmail(email);
                return ResponseWrapper.SuccessResponse("aaaaaaaaaaaaaaaaaaaaa");
            }
            catch (Exception e)
            {
                return ResponseWrapper.ExceptionResponse(e);
            }

        }

        ///// <summary>
        ///// 未读变已读
        ///// </summary>
        ///// <param name="noticeId">通知的Id</param>
        ///// <returns></returns>
        //[HttpPost]
        //[Route("api/Notice/{noticeId}")]
        //public HttpResponseMessage ReadNotice(int? noticeId)
        //{
        //    try
        //    {
        //        _noticeService.ReadNotice(noticeId);
        //        return ResponseWrapper.SuccessResponse("success");
        //    }
        //    catch (Exception e)
        //    {
        //        return ResponseWrapper.ExceptionResponse(e);
        //    }

        //}

        ///// <summary>
        ///// 获取用户通知列表，默认返回第一页
        ///// </summary>
        ///// <returns></returns>
        //public HttpResponseMessage Get()
        //{
        //    try
        //    {
        //        return ResponseWrapper.SuccessResponse(repository.GetNotice(1));
        //    }
        //    catch (Exception e)
        //    {
        //        return ResponseWrapper.ExceptionResponse(e);
        //    }
        //}

        ///// <summary>
        ///// 获取用户未读通知数量，并返回最近5条通知
        ///// </summary>
        ///// <returns></returns>
        //[HttpGet]
        //[Route("api/Notice/DropDown")]
        //public HttpResponseMessage GetDropdownNotifications()
        //{
        //    try
        //    {
        //        return ResponseWrapper.SuccessResponse(repository.GetDropDownNotice());
        //    }
        //    catch (Exception e)
        //    {
        //        return ResponseWrapper.ExceptionResponse(e);
        //    }
        //}

        //[HttpGet]
        //[AllowAnonymous]
        //[Route("api/Notice/StartIIS")]
        //public HttpResponseMessage GetStartIIS()
        //{
        //    try
        //    {
        //        using (var db=new AspodesDB())
        //        {
        //            var roles = db.Roles.ToList();
        //        }
        //    }
        //    catch (Exception e)
        //    {
        //        return ResponseWrapper.ExceptionResponse(e);
        //    }
        //    return ResponseWrapper.SuccessResponse("启动成功");
        //}
    }
}
