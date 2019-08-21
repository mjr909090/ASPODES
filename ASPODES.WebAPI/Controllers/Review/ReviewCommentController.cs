using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

using ASPODES.WebAPI.Repository;
using ASPODES.DTO.Review;
using System.Web;
using System.IO;
using ASPODES.WebAPI.Authorize;
using Newtonsoft.Json;
using ASPODES.Model;
using ASPODES.WebAPI.Common;
using ASPODES.WebAPI.Service;

namespace ASPODES.WebAPI.Controllers.Review
{

    /// <summary>
    /// 专家评审结果路由
    /// </summary>
    [ASPODES.WebAPI.Filter.ActionTrack]
    [AllowAnonymous]
    public class ReviewCommentController : ApiController
    {
        private ReviewCommentRepository repository = new ReviewCommentRepository();
        //private CreateNoticeService _noticeService;

        /// <summary>
        /// 构造函数，依赖注入CreateNoticeService服务
        /// </summary>
        /// <param name="noticeService"></param>
        //public ReviewCommentController(CreateNoticeService noticeService)
        //{
        //    this._noticeService = noticeService;
        //}

        /// <summary>
        /// 添加或者更新评审意见
        /// </summary>
        /// <param name="dto">AddReviewCommentDTO</param>
        /// <returns></returns>
        [Authorize(Roles = "专家")]
        public HttpResponseMessage Post(AddReviewCommentDTO dto)
        {
            try
            {
                var userInfo = UserHelper.GetCurrentUser();
                var result = repository.AddOrUpdateReviewComment(dto, ra => ra.ExpertId == userInfo.UserId);

                ////当一封申请书的指派专家全部评审完毕后
                //if (_noticeService.CompleteReview(
                //        _noticeService.GetApplicationIdByAssignmentId(
                //            dto.ReviewAssignmentId)))
                //{
                //    //申请书指派的所有专家评审结束
                //    //通知打点:院管理员
                //    _noticeService.AddNoticeList(
                //        _noticeService.GetDeptMIdListByAssignmentId(dto.ReviewAssignmentId), 19);
                //}
                
                return ResponseWrapper.SuccessResponse(result);
            }
            catch (Exception e)
            {
                return ResponseWrapper.ExceptionResponse(e);
            }

        }

        /// <summary>
        /// 批量添加评审意见
        /// </summary>
        /// <param name="dtos">AddReviewCommentDTO</param>
        /// <returns></returns>
        [Route("api/reviewcomment/multy")]
        [Authorize(Roles = "专家")]
        public HttpResponseMessage MultyReview(AddReviewCommentDTO[] dtos)
        {
            try
            {
                var userInfo = UserHelper.GetCurrentUser();
                foreach(AddReviewCommentDTO dto in dtos)
                {
                    repository.AddOrUpdateReviewComment(dto, ra => ra.ExpertId == userInfo.UserId);
                }
                var result = "success";

                ////当一封申请书的指派专家全部评审完毕后
                //if (_noticeService.CompleteReview(
                //        _noticeService.GetApplicationIdByAssignmentId(
                //            dto.ReviewAssignmentId)))
                //{
                //    //申请书指派的所有专家评审结束
                //    //通知打点:院管理员
                //    _noticeService.AddNoticeList(
                //        _noticeService.GetDeptMIdListByAssignmentId(dto.ReviewAssignmentId), 19);
                //}

                return ResponseWrapper.SuccessResponse(result);
            }
            catch (Exception e)
            {
                return ResponseWrapper.ExceptionResponse(e);
            }

        }

        /// <summary>
        /// 根据评审意见的ID获取一条评审意见
        /// </summary>
        /// <param name="id">ReviewAssignmentId</param>
        /// <returns></returns>
        [Authorize(Roles = "专家,院管理员")]
        public HttpResponseMessage Get(int id)
        {
            try
            {
                return ResponseWrapper.SuccessResponse(repository.GetOneReviewComment(rc => rc.ReviewCommentId == id));
            }
            catch (Exception e)
            {
                return ResponseWrapper.ExceptionResponse(e);
            }
        }

        /// <summary>
        /// 根据指派的ID获取一条评审意见
        /// </summary>
        /// <param name="id">ReviewAssignmentId</param>
        /// <returns></returns>
        [Route("api/reviewcomment/reviewassignment/{id}"), Authorize(Roles = "专家,院管理员")]
        public HttpResponseMessage GetCommentByAssignmentId(int id)
        {
            try
            {
                return ResponseWrapper.SuccessResponse(repository.GetOneReviewComment(rc => rc.ReviewAssignmentId == id));
            }
            catch (Exception e)
            {
                return ResponseWrapper.ExceptionResponse(e);
            }
        }

        /// <summary>
        /// 获取申请书评审结果
        /// </summary>
        /// <param name="applicationId"></param>
        /// <returns></returns>
        [Route("api/reviewComment/user/{applicationId}")]
        public HttpResponseMessage GetApplicationReviewCommnet( string applicationId )
        {
            try
            {
                var res = repository.GetApplicationReviewComment( applicationId );
                return ResponseWrapper.SuccessResponse(res);
            }
            catch( Exception e )
            {
                return ResponseWrapper.ExceptionResponse(e);
            }
        }

        /// <summary>
        /// 专家获取评审记录，按评审年度筛选，分页
        /// </summary>
        /// <param name="year">评审年份，year=-1本年度评审结果， year=-2除去本年度外的评审结果 获取当前评审年度的评审意见(代码未实现)，year等于0获取所有年度的评审意见</param>
        /// <param name="page">页码</param>
        /// <returns></returns>
        [Authorize(Roles = "专家"), Route("api/reviewcomment/expert/{year}/{page}")]
        public HttpResponseMessage GetPagingReviewCommentList(int year, int page)
        {
            try
            {
                year = year < 0 ? SystemConfig.ApplicationStartYear : year;
                var userInfo = UserHelper.GetCurrentUser();
                Func<ReviewAssignment, bool> predicate = ra => ra.ExpertId == userInfo.UserId
                                                          && ((year == 0) || (ra.Year == year));
                return ResponseWrapper.SuccessResponse(repository.GetPagingReviewCommentList(predicate, page));
            }
            catch (Exception e)
            {
                return ResponseWrapper.ExceptionResponse(e);
            }

        }

        /// <summary>
        /// 院管理员或单位管理员获取某位专家的既往评审信息
        /// </summary>
        /// <param name="expertId">专家ID</param>
        /// <param name="page">页码</param>
        /// <returns></returns>
        [Authorize(Roles = "单位管理员,院管理员"), Route("api/reviewcommentlist/{expertId}/{page}")]
        public HttpResponseMessage Getreviewcommentlist(string expertId, int page)
        {
            try
            {
                Func<ReviewAssignment, bool> predicate = ra => ra.ExpertId == expertId;
                return ResponseWrapper.SuccessResponse(repository.GetPagingReviewCommentList(predicate, page));
            }
            catch (Exception e)
            {
                return ResponseWrapper.ExceptionResponse(e);
            }

        }

        /// <summary>
        /// 单位管理员获取本单位的项目评审
        /// </summary>
        /// <param name="projectTypeId"></param>
        /// <param name="year"></param>
        /// <param name="page"></param>
        /// <returns></returns>
        [Authorize(Roles = "单位管理员"), Route("api/reviewcomment/inst/{projectTypeId}/{year}/{page}")]
        public HttpResponseMessage GetPagingApplicationReviewCommentList(int projectTypeId, int year, int page)
        {
            try
            {
                /*
                year = year <= 0 ? SystemConfig.ApplicationStartYear : year;
                var userInfo = UserHelper.GetCurrentUser();
                Func<Application, bool> predicate = a => userInfo.ProjectTypeIds.Contains(a.ProjectTypeId) //院管理员只能看到他分管的类型
                                                     && ((instId == 0) || a.InstituteId == instId)//单位筛选
                                                     && ((projectTypeId == 0) || a.ProjectTypeId == projectTypeId)//项目类型筛选
                                                     && (a.CurrentYear >= year);//年份筛选
                */

                var userInfo = UserHelper.GetCurrentUser();
                var predicate = RetrievalConditions(userInfo.InstId, projectTypeId, 0, a => a.HasReview());
                var res = repository.GetPagingApplicationReviewCommentList(predicate, page);
                return ResponseWrapper.SuccessResponse(res);
            }
            catch (Exception e)
            {
                return ResponseWrapper.ExceptionResponse(e);
            }

        }

        /// <summary>
        /// 院管理员获取专家评审意见的列表，根据单位、项目类型、评审年度进行筛选，分页
        /// </summary>
        /// <param name="instId">单位</param>
        /// <param name="projectTypeId">项目类型</param>
        /// <param name="year">年份</param>
        /// <param name="page">页码</param>
        /// <param name="status">状态</param>
        /// <returns></returns>
        [Authorize(Roles = "院管理员"), Route("api/reviewcomment/depart/{instId}/{projectTypeId}/{year}/{status}/{page}")]
        public HttpResponseMessage GetPagingApplicationReviewCommentList(int instId, int projectTypeId, int year, int page, ApplicationStatus status)
        {
            try
            {
                //year = year <= 0 ? SystemConfig.ApplicationStartYear : year;
                //var userInfo = UserHelper.GetCurrentUser();
                //Func<Application, bool> predicate = a => userInfo.ProjectTypeIds.Contains(a.ProjectTypeId) //院管理员只能看到他分管的类型
                //                                     && ((instId == 0) || a.InstituteId == instId)//单位筛选
                //                                     && ((projectTypeId == 0) || a.ProjectTypeId == projectTypeId)//项目类型筛选
                //                                     && (a.CurrentYear >= year);//年份筛选
                Func<Application, bool> predicate;
                if (status == 0)
                {
                    predicate = RetrievalConditions(instId, projectTypeId, year, a => a.HasReview());
                }else
                {
                    predicate = RetrievalConditions(instId, projectTypeId, year, a => a.Status == status);
                }
                
                return ResponseWrapper.SuccessResponse(repository.GetPagingApplicationReviewCommentList(predicate, page));
            }
            catch (Exception e)
            {
                return ResponseWrapper.ExceptionResponse(e);
            }

        }

        /// <summary>
        /// 计算总分
        /// </summary>
        /// <returns></returns>
        [HttpGet, Route("api/reviewComment/calScore")]
        public HttpResponseMessage CalculateApplicationReviewScore()
        {
            try
            {
                ReviewCommentRepository.ReviewCommentTotalScore();
                return ResponseWrapper.SuccessResponse("计算完成");
            }
            catch( Exception e)
            {
                return ResponseWrapper.ExceptionResponse(e);
            }
        }

        /// <summary>
        /// 导出初审结果
        /// </summary>
        /// <param name="instId"></param>
        /// <param name="projectTypeId"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        [HttpGet, Route("api/reviewcomment/export/{instId}/{projectTypeId}/{count}"), Authorize(Roles = "院管理员")]
        public HttpResponseMessage ExportReviewComment(int instId, int projectTypeId, int count)
        {
            try
            {
                var userInfo = UserHelper.GetCurrentUser();

                /*
                Func<Application, bool> predicate = a => a.CurrentYear == SystemConfig.ApplicationStartYear
                                                   && userInfo.ProjectTypeIds.Contains(a.ProjectTypeId)
                                                   && a.Status == ApplicationStatus.FINISH_REVIEW
                                                   && ((instId == 0) || (a.InstituteId == instId))
                                                   && ((projectTypeId == 0) || (a.ProjectTypeId == projectTypeId));
                 * */

                var predicate = RetrievalConditions(instId, projectTypeId, 0, a => a.Status == ApplicationStatus.FINISH_REVIEW);
                return repository.ExportReviewComment(predicate, count, userInfo.Name);
            }
            catch (Exception e)
            {
                return ResponseWrapper.ExceptionResponse(e);
            }
        }



        public static Func<Application,bool> RetrievalConditions( int instId, int projectTypeId, int year, Func<Application,bool> status )
        {
            year = year == 0 ? SystemConfig.ApplicationStartYear : year;
            List<int> projectTypeIds = new List<int>();
            if (projectTypeId < 0) 
            {
                projectTypeIds.AddRange(UserHelper.GetCurrentUser().ProjectTypeIds.Select(p => p.Value).ToList());
            }
            else if( projectTypeId > 0 )
            {
                projectTypeIds.Add(projectTypeId);
            }

            Func<Application, bool> predicate = a => 
                (year < 0 || a.CurrentYear == year)
                && status(a)
                && ((instId == 0) || (a.InstituteId == instId))
                && ((projectTypeId == 0) || projectTypeIds.Contains(a.ProjectTypeId.Value));
            return predicate;
        }


        //[AllowAnonymous]
        //[HttpGet, Route("api/reviewcomment/testexport/{instId}/{projectTypeId}/{count}")]
        //public HttpResponseMessage TestExportReviewComment(int instId, int projectTypeId, int count)
        //{
        //    //var userInfo = UserHepler.GetCurrentUser();
        //    Func<Application, bool> predicate = a => a.CurrentYear == SystemConfig.ApplicationStartYear
        //        //&& userInfo.ProjectTypeIds.Contains(a.ProjectTypeId)
        //                                             && a.Status == ApplicationStatus.FINISH_REVIEW
        //                                             && ((instId == 0) || (a.InstituteId == instId))
        //                                             && ((projectTypeId == 0) || (a.ProjectTypeId == projectTypeId));
        //    //return repository.ExportReviewComment(predicate, count, userInfo.Name);
        //    return repository.ExportReviewComment(predicate, count, "测试");

        //}
    }
}
