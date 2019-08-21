using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

using ASPODES.Model;
using ASPODES.WebAPI.Repository;
using ASPODES.DTO.Review;
using System.Web;
using System.IO;
using ASPODES.WebAPI.Authorize;
using Newtonsoft.Json;
using ASPODES.WebAPI.Common;
using ASPODES.WebAPI.Service;

namespace ASPODES.WebAPI.Controllers.Review
{
    /// <summary>
    /// 专家推荐路由
    /// </summary>
    [AspodesAuthorize]
    [ASPODES.WebAPI.Filter.ActionTrack]
    public class RecommendationController : ApiController
    {

        private RecommendationRepository repository = new RecommendationRepository();
        //private CreateNoticeService _noticeService;

        /// <summary>
        /// 构造函数，注入CreateNoticeService
        /// </summary>
        /// <param name="noticeService"></param>
        //public RecommendationController(CreateNoticeService noticeService)
        //{
        //    this._noticeService = noticeService;
        //}

        /// <summary>
        /// 院管理员的推荐专家列表 status=adopt 获取已经通过的推荐列表，status=refuse获取已拒绝的推荐列表，
        /// status=handle获取未处理的推荐列表，其他值都会返回所有的推荐
        /// instId = 0获取所有单位的推荐
        /// </summary>
        /// <param name="status">申请书处理状态</param>
        /// <param name="instId">单位的ID</param>
        /// <param name="page">页码</param>
        /// <returns></returns>
        [Route("api/recomendation/depart/{status}/{instId}/{page}"),Authorize(Roles="院管理员")]
        public HttpResponseMessage GetRecommendations( string status, int instId, int page )
        {
            try
            {
                bool? adopt = null;
                string statusQuery = status;
                var userInfo = UserHelper.GetCurrentUser();
                RecommendationStatus(ref statusQuery, ref adopt);
                Func<Recommendation, bool> predicate = r => ((status == "all") || (r.Adopt == adopt))
                                                        && ((instId == 0) || (r.InstituteId == instId));
                return ResponseWrapper.SuccessResponse(repository.GetPagingRecommendationList(predicate, page));
            }
            catch(Exception e)
            {
                return ResponseWrapper.ExceptionResponse(e);
            }
            
        }
        
        /// <summary>
        /// 获取单位的专家推荐列表
        /// </summary>
        [Route("api/recommendation/inst/{status}/{page}"), Authorize(Roles = "单位管理员")]
        public HttpResponseMessage GetInstRecommendations(string status, int page)
        {
            try
            {
                bool? adopt = null;
                string statusQuery = status;
                var userInfo = UserHelper.GetCurrentUser();
                RecommendationStatus(ref statusQuery, ref adopt);
                Func<Recommendation, bool> predicate = r => ((statusQuery == "all") || (r.Adopt == adopt))
                                                        && (r.InstituteId == userInfo.InstId);
                return ResponseWrapper.SuccessResponse(repository.GetPagingRecommendationList(predicate, page));
            }
            catch (Exception e)
            {
                return ResponseWrapper.ExceptionResponse(e);
            }
        }

        /// <summary>
        /// 获取单位为推荐人员列表
        /// </summary>
        [Route("api/unRecommendation/inst/{page}"), Authorize(Roles = "单位管理员")]
        public HttpResponseMessage GetInstUnRecommendations(int page)
        {
            try
            {
                //bool? adopt = null;
                //string statusQuery = status;
                var userInfo = UserHelper.GetCurrentUser();
                //RecommendationStatus(ref statusQuery, ref adopt);
                //Func<Recommendation, bool> predicate = r => ((statusQuery == "all") || (r.Adopt == adopt))
                //                                        && (r.InstituteId == userInfo.InstId);
                return ResponseWrapper.SuccessResponse(repository.GetPagingUnRecommendationList(userInfo.InstId, page));
            }
            catch (Exception e)
            {
                return ResponseWrapper.ExceptionResponse(e);
            }
        }


        /// <summary>
        /// 单位管理员的推荐专家
        /// </summary>
        [Authorize(Roles="单位管理员")]
        public HttpResponseMessage Post( int id )
        {
            try
            {
                var userInfo = UserHelper.GetCurrentUser();
                var result = repository.AddRecommendation(userInfo.UserId, id, r => r.InstituteId == userInfo.InstId);

                //单位管理员推荐某用户成为专家
                //通知打点:发给所有院管理员
                //_noticeService.AddNoticeList(
                //    _noticeService.GetAllDepartManagerIdList(), 120);
                //通知打点:发给用户
                //_noticeService.AddNotice(
                //    _noticeService.GetUserIdByPersonId(id), 103);

                return ResponseWrapper.SuccessResponse(result);
            }
            catch(Exception e)
            {
                return ResponseWrapper.ExceptionResponse(e);
            }
        }

        /// <summary>
        /// 院管理员通过专家推荐
        /// </summary>
        /// <param name="recommendationId"></param>
        /// <returns></returns>
        [HttpPut]
        [Route("api/recommendation/adopt/{recommendationId}"), Authorize(Roles = "院管理员")]
        public HttpResponseMessage AdoptRecommendation( int recommendationId )
        {
            try
            {
                var result = repository.AdoptRecommendation(recommendationId, r => true);

                //院管理员同意专家的推荐
                //通知打点:发给单位管理员
                //_noticeService.AddNoticeList(
                //    _noticeService.GetInstMListByAdoptRecommendation(recommendationId), 114,
                //    new Dictionary<string, string>
                //    { {"UserName", _noticeService.GetCandidateNameByRecommendation(recommendationId)} });
                //通知打点:发给用户
                //_noticeService.AddNotice(
                //    _noticeService.GetCandidateIdByRecommendation(recommendationId), 105);

                return ResponseWrapper.SuccessResponse(result);
            }
            catch (Exception e)
            {
                return ResponseWrapper.ExceptionResponse(e);
            }
        }


        /// <summary>
        /// 院管理员驳回专家推荐
        /// </summary>
        /// <param name="recommendationId"></param>
        /// <returns></returns>
        [HttpPut]
        [Route("api/recommendation/refuse/{recommendationId}"), Authorize(Roles = "院管理员")]
        public HttpResponseMessage RefuseRecommendation(int recommendationId)
        {
            try
            {
                var result = repository.RefuseRecommendation(recommendationId);

                //院管理员驳回专家的推荐
                //通知打点:发给单位管理员
                //_noticeService.AddNoticeList(
                //    _noticeService.GetInstMListByAdoptRecommendation(recommendationId), 113,
                //    new Dictionary<string, string>
                //    { {"UserName", _noticeService.GetCandidateNameByRecommendation(recommendationId)} });
                //通知打点:发给用户
                //_noticeService.AddNotice(
                //    _noticeService.GetCandidateIdByRecommendation(recommendationId), 106);

                return ResponseWrapper.SuccessResponse(result);
            }
            catch (Exception e)
            {
                return ResponseWrapper.ExceptionResponse(e);
            }
        }

        /// <summary>
        /// 删除专家推荐
        /// </summary>
        /// <param name="recommendationId"></param>
        /// <returns></returns>
        [Authorize(Roles="单位管理员")]
        public HttpResponseMessage Delete(int id)
        {
            try
            {
                var userInfo = UserHelper.GetCurrentUser();
                repository.DeleteRecommendation(id, r => r.InstituteId == userInfo.InstId);

                //单位管理员撤回推荐某用户成为专家
                //通知打点:发给所有院管理员
                //_noticeService.AddNoticeList(
                //    _noticeService.GetAllDepartManagerIdList(), 121,
                //    new Dictionary<string, string>
                //    { {"DepartmentName",_noticeService.GetInstituteNameById(userInfo.InstId)},
                //      {"UserName",  _noticeService.GetNameByPersionId(id)} });
                //通知打点:发给用户
                //_noticeService.AddNotice(
                //    _noticeService.GetUserIdByPersonId(id), 104);

                return ResponseWrapper.SuccessResponse("删除成功");
            }
            catch (Exception e)
            {
                return ResponseWrapper.ExceptionResponse(e);
            }
        }


        private void RecommendationStatus(ref string status, ref bool? adopt)
        {
            if (status == "adopt")
            {
                adopt = true;
            }
            else if (status == "refuse")
            {
                adopt = false;
            }
            else if (status == "handle")
            {
                adopt = null;
            }
            else
            {
                status = "all";
            }
        }
    }
}
