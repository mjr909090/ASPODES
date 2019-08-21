using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using ASPODES.WebAPI.Repository;
using ASPODES.Model;
using Newtonsoft.Json;
using ASPODES.WebAPI.Common;
using ASPODES.WebAPI.Service;

namespace ASPODES.WebAPI.Controllers.Review
{
    /// <summary>
    /// 申请书指派专家控制器
    /// </summary>
    [ASPODES.WebAPI.Filter.ActionTrack]
    public class ReviewAssignmentController : ApiController
    {

        public ReviewAssignmentRepository repository = new ReviewAssignmentRepository();
        private ApplicationService _applicationService;
        //private CreateNoticeService _noticeService;

        /// <summary>
        /// 依赖注入形成通知信息的service层
        /// </summary>
        /// <param name="noticeService"></param>
        /// <param name="applicationService"></param>
        public ReviewAssignmentController(//CreateNoticeService noticeService,
            ApplicationService applicationService)
        {
            //this._noticeService = noticeService;
            this._applicationService = applicationService;
        }

        /// <summary>
        /// 给一份申请书自动指派专家
        /// </summary>
        /// <param name="applicationId"></param>
        /// <returns></returns>
        [HttpPost, Route("api/reviewassignmeng/application/{applicationId}"), Authorize(Roles = "院管理员")]
        public HttpResponseMessage AutoApplicationExpertAssignment(string applicationId)
        {
            try
            {
                Func<Application, bool> privilege = a => UserHelper.GetCurrentUser().ProjectTypeIds.Contains(a.ProjectTypeId);
                var result = repository.ApplicationExpertAssignment(applicationId, privilege);

                // 当某一个申请书自动指派没有指派完整的时候，形成消息通知管理员
                //if ( !repository.CompleteAssignment(applicationId))
                //{
                //    //院管理员受理申请书,并自动指派没有完整指派
                //    //通知打点:院管理员
                //    _noticeService.AddNoticeList(
                //        _noticeService.GetDepartManagerIdListByApplicationId(applicationId), 16);
                //}

                // 当所有的申请书都已经指派专家完成后发通知
                //if ( !_applicationService.ExistUnAssignmentApplication(applicationId))
                //{
                //    //自动指派专家，申请书所在领域的申请书全部指派
                //    //通知打点:院管理员
                //    _noticeService.AddNoticeList(
                //        _noticeService.GetDepartManagerIdListByApplicationId(applicationId), 17);
                //}

                return ResponseWrapper.SuccessResponse(result);
            }
            catch (Exception e)
            {
                return ResponseWrapper.ExceptionResponse(e);
            }

        }

        /// <summary>
        /// 手动指派
        /// </summary>
        /// <returns></returns>
        [HttpPost, Route("api/reviewassignment/manual"), Authorize(Roles = "院管理员")]
        public HttpResponseMessage ManualApplicationAssignment()
        {
            try
            {
                string content = Request.Content.ReadAsStringAsync().Result;
                var data = JsonConvert.DeserializeAnonymousType(content, new { applications = new List<string>(), experts = new List<string>() });
                Func<Application, bool> privilege = a => UserHelper.GetCurrentUser().ProjectTypeIds.Contains(a.ProjectTypeId);
                var result = repository.AddReviewAssignments(data.applications, data.experts, privilege);
                
                // 当所有的申请书都已经指派专家完成后发通知
                //if (data.applications.First() != null 
                //    && !_applicationService.ExistUnAssignmentApplication(data.applications.First()))
                //{
                //    //自动指派专家，申请书所在领域的申请书全部指派
                //    //通知打点:院管理员
                //    _noticeService.AddNoticeList(
                //        _noticeService.GetDepartManagerIdListByApplicationId(data.applications.First()), 18);
                //}

                return ResponseWrapper.SuccessResponse(result);
            }
            catch (Exception e)
            {
                return ResponseWrapper.ExceptionResponse(e);
            }
        }

        /// <summary>
        /// 替换专家
        /// </summary>
        /// <param name="applicationId"></param>
        /// <param name="oldExpertId"></param>
        /// <param name="newExpertId"></param>
        /// <returns></returns>
        [HttpPut, Route("api/reviewassignment/changeexpert/{applicationId}"), Authorize(Roles = "院管理员")]
        public HttpResponseMessage ChangeExpert(string applicationId)
        {
            try
            {
                string oldExpertId = Request.GetQueryNameValuePairs().FirstOrDefault(p => p.Key == "oldExpertId").Value;
                string newExpertId = Request.GetQueryNameValuePairs().FirstOrDefault(p => p.Key == "newExpertId").Value;
                Func<Application, bool> privilege = a => UserHelper.GetCurrentUser().ProjectTypeIds.Contains(a.ProjectTypeId);
                var result = repository.ChangeExpert(applicationId, oldExpertId, newExpertId, privilege);

                // 当所有的申请书都已经指派专家完成后发通知
                //if (!_applicationService.ExistUnAssignmentApplication(applicationId))
                //{
                //    //自动指派专家，申请书所在领域的申请书全部指派
                //    //通知打点:院管理员
                //    _noticeService.AddNoticeList(
                //        _noticeService.GetDepartManagerIdListByApplicationId(applicationId), 18);
                //}

                return ResponseWrapper.SuccessResponse(result);
            }
            catch (Exception e)
            {
                return ResponseWrapper.ExceptionResponse(e);
            }
        }

        /// <summary>
        /// 获取等待发送专家指派的列表
        /// </summary>
        /// <returns></returns>
        [Route("api/reviewassignment/sendassignment/{instId}/{page}"), Authorize(Roles = "院管理员")]
        public HttpResponseMessage GetSendAssignmentList(int instId, int page)
        {
            try
            {
                var userInfo = UserHelper.GetCurrentUser();

                Func<Application, bool> predicate = a => a.CurrentYear == SystemConfig.ApplicationStartYear
                                                   && a.Status == ApplicationStatus.SEND_ASSIGNMENT
                                                   && userInfo.ProjectTypeIds.Contains(a.ProjectTypeId)
                                                   && ((instId == 0) || a.InstituteId == instId);

                string fieldId = Request.GetQueryNameValuePairs().FirstOrDefault(p => p.Key == "fieldId").Value;
                string subFieldId = Request.GetQueryNameValuePairs().FirstOrDefault(p => p.Key == "subFieldId").Value;
                return ResponseWrapper.SuccessResponse(repository.GetPagingReviewAssignmentList(fieldId, subFieldId, predicate, page));
            }
            catch (Exception e)
            {
                return ResponseWrapper.ExceptionResponse(e);
            }
        }

        /// <summary>
        /// 获取等待手动指派专家的列表
        /// </summary>
        /// <returns></returns>
        [Route("api/reviewassignment/manualassignment/{instId}/{page}"), Authorize(Roles = "院管理员")]
        public HttpResponseMessage GetManualAssignmentList(int instId, int page)
        {
            try
            {
                var userInfo = UserHelper.GetCurrentUser();

                Func<Application, bool> predicate = a => a.CurrentYear == SystemConfig.ApplicationStartYear
                                                   && a.Status == ApplicationStatus.MANUAL_ASSIGNMENT
                                                   && userInfo.ProjectTypeIds.Contains(a.ProjectTypeId)
                                                   && ((instId == 0) || a.InstituteId == instId);

                string fieldId = Request.GetQueryNameValuePairs().FirstOrDefault(p => p.Key == "fieldId").Value;
                string subFieldId = Request.GetQueryNameValuePairs().FirstOrDefault(p => p.Key == "subFieldId").Value;

                fieldId = fieldId == null ? "all" : fieldId;
                subFieldId = subFieldId == null ? "all" : subFieldId;

                return ResponseWrapper.SuccessResponse(repository.GetPagingReviewAssignmentList(fieldId, subFieldId, predicate, page));
            }
            catch (Exception e)
            {
                return ResponseWrapper.ExceptionResponse(e);
            }
        }

        /// <summary>
        /// 获取专家评审中的列表
        /// </summary>
        /// <returns></returns>
        [Route("api/reviewassignment/review/{instId}/{page}"), Authorize(Roles = "院管理员")]
        public HttpResponseMessage GetReviewList(int instId, int page)
        {
            try
            {
                var userInfo = UserHelper.GetCurrentUser();

                Func<Application, bool> predicate = a => a.CurrentYear == SystemConfig.ApplicationStartYear
                                                   && a.Status == ApplicationStatus.REVIEW
                                                   && userInfo.ProjectTypeIds.Contains(a.ProjectTypeId)
                                                   && ((instId == 0) || a.InstituteId == instId);

                string fieldId = Request.GetQueryNameValuePairs().FirstOrDefault(p => p.Key == "fieldId").Value;
                string subFieldId = Request.GetQueryNameValuePairs().FirstOrDefault(p => p.Key == "subFieldId").Value;
                return ResponseWrapper.SuccessResponse(repository.GetPagingReviewAssignmentList(fieldId, subFieldId, predicate, page));
            }
            catch (Exception e)
            {
                return ResponseWrapper.ExceptionResponse(e);
            }
        }

        /// <summary>
        /// 发送初审指派信息
        /// </summary>
        /// <returns></returns>
        [HttpPut, Route("api/reviewassignment/sendassignment"), Authorize(Roles = "院管理员")]
        public HttpResponseMessage SendReviewAssignment()
        {
            try
            {
                repository.SendReviewAssignment();

                //确认后通知要审核的专家
                //通知打点:专家
                //_noticeService.AddNoticeList(_noticeService.GetExpertIdList(), 21);

                return ResponseWrapper.SuccessResponse("发送成功");
            }
            catch (Exception e)
            {
                return ResponseWrapper.ExceptionResponse(e);
            }
        }

        /// <summary>
        /// 专家获取指派给他的申请书
        /// </summary>
        /// <returns></returns>
        [Route("api/reviewassignment/expert"), Authorize(Roles = "专家")]
        public HttpResponseMessage GetExpertReviewAssignment()
        {
            try
            {
                var userInfo = UserHelper.GetCurrentUser();
            Func<ReviewAssignment, bool> predicate = ra => !ra.Overdue.Value //未过期的
                && ra.ExpertId == userInfo.UserId //指派给他的
                && ra.Status == ReviewAssignmentStatus.WAITE_REVIEW; //状态筛选

            return ResponseWrapper.SuccessResponse(repository.GetReviewAssignmentList(predicate));
            }
            catch (Exception e)
            {
                return ResponseWrapper.ExceptionResponse(e);
            }
        }

    }
}
