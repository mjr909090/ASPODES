using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Net.Http;
using System.Web.Http;

using ASPODES.DTO.Application;
using ASPODES.WebAPI.Repository;
using ASPODES.WebAPI.Common;
using System.Security.Principal;
using ASPODES.WebAPI.Authorize;
using ASPODES.Model;
using ASPODES.WebAPI.Filter;
using ASPODES.WebAPI.Security;
using ASPODES.WebAPI.Service;

namespace ASPODES.WebAPI.Controllers
{
    /// <summary>
    /// 申请书操作
    /// </summary>
    [ActionTrack, Authorize(Roles="用户")]
    public class ApplicationController : ApiController
    {
        private ApplicationRepository repository = new ApplicationRepository();
        private Privilege privilege = new Privilege();
        //private CreateNoticeService _noticeService;

        //public ApplicationController(CreateNoticeService noticeService)
        //{
        //    this._noticeService = noticeService;
        //}

        /// <summary>
        /// 使用UUID生成一个新的申请书ID
        /// </summary>
        /// <returns></returns>
        [Route("api/application/applicationId")]
        public HttpResponseMessage GetNewApplicationId()
        {
            var guid = Guid.NewGuid();
            return ResponseWrapper.SuccessResponse(new Application { ApplicationId = guid.ToString() });
        }

        /// <summary>
        /// 获取一份申请书的基本信息
        /// </summary>
        /// <param name="id">申请书ID</param>
        /// <returns></returns>
        public HttpResponseMessage Get( string id )
        {
            try
            {
                return ResponseWrapper.SuccessResponse(repository.GetOneApplication(id));
            }
            catch(Exception e)
            {
                return ResponseWrapper.ExceptionResponse(e);
            }
        }

        /// <summary>
        /// 获取申请书草稿
        /// </summary>
        /// <returns></returns>
        [Route("api/application/draft")]
        public HttpResponseMessage GetDraftApplications()
        {
            
            try
            {
                var userInfo = UserHelper.GetCurrentUser();
                Func<Application, bool> predicate = a => a.LeaderId == userInfo.PersonId //他自己的
                                                   && a.CurrentYear == SystemConfig.ApplicationStartYear //本申请年份的
                                                   && (a.Status == ApplicationStatus.NEW_ONE //草稿
                                                        || a.Status == ApplicationStatus.NEW_TWO
                                                        || a.Status == ApplicationStatus.NEW_THREE
                                                        || a.Status == ApplicationStatus.NEW_FOUR
                                                        || a.Status == ApplicationStatus.NEW
                                                        || a.Status == ApplicationStatus.CANCEL
                                                        || a.Status == ApplicationStatus.REJECT);
                return ResponseWrapper.SuccessResponse( repository.GetApplicationList(predicate));
            }
            catch( Exception e)
            {
                return ResponseWrapper.ExceptionResponse(e);
            }
        }

        /// <summary>
        /// 获取用户已经提交的申请书
        /// </summary>
        /// <returns></returns>
        [Route("api/applciation/submited")]
        public HttpResponseMessage GetSubmitedApplications()
        {
            try
            {
                var userInfo = UserHelper.GetCurrentUser();
                Func<Application, bool> predicate = a => a.LeaderId == userInfo.PersonId //他自己的
                                                   && a.CurrentYear == SystemConfig.ApplicationStartYear //本申请年份的
                                                   && ((int)a.Status == (int)ApplicationStatus.CHECK
                                                   || (int)a.Status > (int)ApplicationStatus.CANCEL);
                return ResponseWrapper.SuccessResponse( repository.GetApplicationList(predicate));
            }
            catch( Exception e )
            {
                return ResponseWrapper.ExceptionResponse(e);
            }
            
        }

        /// <summary>
        /// 获取申请项目的参与单位
        /// </summary>
        /// <param name="applicationId">申请书ID</param>
        /// <returns></returns>
        [Route("api/application/participateinst/{applicationId}")]
        public HttpResponseMessage GetParticipateInst(string applicationId)
        {
            try
            {
                return ResponseWrapper.SuccessResponse( repository.GetParticipateInst(applicationId) ); 
            }
            catch( Exception e)
            {
                return ResponseWrapper.ExceptionResponse(e);
            }
            
        }

        /// <summary>
        /// 添加申请书
        /// </summary>
        /// <param name="dto">待添加的申请书信息</param>
        /// <returns></returns>
        [Authorize(Roles="用户")]
        public HttpResponseMessage Post( AddApplicationDTO  dto)
        {
            try
            {
                return ResponseWrapper.SuccessResponse(repository.AddOrUpdateApplication(dto, privilege.UserEditApplication));
            }
            catch( Exception e)
            {
                return ResponseWrapper.ExceptionResponse(e);
            }

        }

        /// <summary>
        /// 保存申请书，主要操作是生成申请书PDF文档，将申请书的状态修改为ApplicationStatus.NEW
        /// </summary>
        /// <param name="applicationId">申请书ID</param>
        /// <returns></returns>
        [HttpPut]
        [Route("api/application/save/{applicationId}")]
        public HttpResponseMessage SaveApplication(string applicationId)
        {
            try
            {
                return ResponseWrapper.SuccessResponse(repository.SaveApplication(applicationId, privilege.UserEditApplication));
            }
            catch (Exception e)
            {
                return ResponseWrapper.WarningResponse(e);
            }
        }

        /// <summary>
        /// 保存申请书，主要操作是生成申请书PDF文档，将申请书的状态修改为ApplicationStatus.NEW
        /// </summary>
        /// <param name="applicationId">申请书ID</param>
        /// <returns></returns>
        [HttpPut]
        [Route("api/application/createPDF/{applicationId}")]
        public HttpResponseMessage CreateApplicationPDF(string applicationId)
        {
            try
            {
                repository.CreateApplicationPDF(applicationId);
                return ResponseWrapper.SuccessResponse();
            }
            catch (Exception e)
            {
                return ResponseWrapper.WarningResponse(e);
            }
        }

        /// <summary>
        /// 提交申请书，先检查申请书的完整性，如通过检查将申请书状态修改为ApplicationStatus.CHECK，否则返回不允许此操作
        /// </summary>
        /// <param name="applicationId"></param>
        /// <returns></returns>
        [HttpPut]
        [Authorize(Roles = "用户")]
        [Route("api/application/submit/{applicationId}")]
        public HttpResponseMessage SubmitApplication( string applicationId )
        {
            try
            {
                var userInfo = UserHelper.GetCurrentUser();
                //用户不能提交多封申请书
                if (repository.IsResubmitted()) throw new OtherException("您已提交一封申请书，请不要重复提交");
                repository.ChangeApplicationStatus(applicationId, ApplicationStatus.NEW, ApplicationStatus.CHECK, a => a.LeaderId == userInfo.PersonId && DateTime.Now <= SystemConfig.ApplicationSubmitDeadline && DateTime.Now >= SystemConfig.ApplicationSubmitBeginTime);

                //用户提交申请书
                //通知打点:发给所在单位所有管理员
                //_noticeService.AddNoticeList(
                //    _noticeService.GetInstituteManagerIdList(userInfo.InstId), 9);
                //通知信打点:发给自己
                //_noticeService.AddNotice(userInfo.UserId, 1);

                return ResponseWrapper.SuccessResponse();
            }
            catch (Exception e)
            {
                return ResponseWrapper.ExceptionResponse(e);
            }
            
        }
        
        /// <summary>
        /// 获取既往申请书
        /// </summary>
        /// <returns></returns>
        [Authorize(Roles="用户")]
        [Route("api/application/previous")]
        public HttpResponseMessage GetPreviousApplication()
        {
            try
            {
                var userInfo = UserHelper.GetCurrentUser();
                Func<Application, bool> predicate = a => a.LeaderId == userInfo.PersonId
                                                      && a.CurrentYear < SystemConfig.ApplicationStartYear;
                return ResponseWrapper.SuccessResponse(repository.GetApplicationList(predicate));
            }
            catch (Exception e)
            {
                return ResponseWrapper.ExceptionResponse(e);
            }
            
        }
        
        /// <summary>
        /// 删除申请书草稿
        /// </summary>
        /// <param name="id">申请书ID</param>
        /// <returns></returns>
        public HttpResponseMessage Delete(string id)
        {
            try
            {
                var userInfo = UserHelper.GetCurrentUser();
                repository.Delete(id, a => a.LeaderId == userInfo.PersonId);
                return ResponseWrapper.SuccessResponse();
            }
            catch (Exception e)
            {
                return ResponseWrapper.ExceptionResponse(e);
            }
            
        }


        //-----------------------------------------------
        /// <summary>
        /// 获取申请书第一步左侧栏的信息
        /// </summary>
        /// <param name="id">申请书ID</param>
        /// <returns></returns>
        [Route("api/application/step/one/left/{id}")]
        public HttpResponseMessage GetStepOneLeft(string id)
        {
            try
            {
                return ResponseWrapper.SuccessResponse(repository.GetStepOneLeft(id));
            }
            catch (Exception e)
            {
                return ResponseWrapper.ExceptionResponse(e);
            }
        }
    
    
        private ApplicationDoc reciveFile()
        {
            return null;
        }
    }
}
