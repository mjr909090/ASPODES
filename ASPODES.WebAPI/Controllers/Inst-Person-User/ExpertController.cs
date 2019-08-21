using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Net.Http;
using System.Web.Http;
using ASPODES.Common.Util;
using ASPODES.DTO.Inst_Person_User;
using ASPODES.WebAPI.Repository;
using ASPODES.WebAPI.Common;
using ASPODES.Model;
using ASPODES.WebAPI.Service;

namespace ASPODES.WebAPI.Controllers
{
    /// <summary>
    /// 专家操作
    /// </summary>
    [AllowAnonymous]
    [ASPODES.WebAPI.Filter.ActionTrack]
    public class ExpertController : ApiController
    {
        private PersonRepository repository = new PersonRepository();
        private RecommendationRepository recommendationrepository = new RecommendationRepository();
        //private CreateNoticeService _noticeService;

        //public ExpertController(CreateNoticeService noticeService)
        //{
        //    this._noticeService = noticeService;
        //}

        /// <summary>
        /// 获取专家基本信息
        /// </summary>
        /// <param name="id">专家ID</param>
        /// <returns></returns>
        [Authorize(Roles="院管理员"),Route("api/expert/{id}")]
        public HttpResponseMessage Get(string id)
        {
            try
            {
                return ResponseWrapper.SuccessResponse(repository.GetOnePersonByUserId(id));

            }
            catch (Exception e)
            {
                return ResponseWrapper.ExceptionResponse(e);
            }
        }

        /// <summary>
        /// 获取评审密码
        /// </summary>
        /// <returns></returns>
        [Authorize(Roles="专家"),Route("api/expert/review/password")]
        public string GetReviewPassword()
        {
            return UserHelper.GetReviewPassword();
        }

        /// <summary>
        /// 将人员设置为专家
        /// </summary>
        /// <param name="id">人员ID</param>
        /// <returns></returns>
        [HttpPost]
        [Authorize(Roles = "院管理员")]
        [Route("api/expert")]
        public HttpResponseMessage Post(int id)
        {
            try
            {
                //检查专家研究领域完整性
                if (PersonRepository.ExpertFieldComplete(id))
                {
                    PersonRepository.GrantRole(id, 5, p => true);
                    return ResponseWrapper.SuccessResponse();
                }
                else
                {
                    return ResponseWrapper.ExceptionResponse(new OtherException("请完善研究领域信息"));
                }
            }
            catch (Exception e)
            {
                return ResponseWrapper.ExceptionResponse(e);
            }
        }

        /// <summary>
        /// 将院外人员设置为专家
        /// </summary>
        /// <param name="id">人员ID</param>
        /// <returns></returns>
        [HttpPost]
        [Authorize(Roles = "院管理员")]
        [Route("api/expert/partners")]
        public HttpResponseMessage PostPartners(int id)
        {
            try
            {
                //检查专家研究领域完整性
                //if (PersonRepository.ExpertFieldComplete(id))
                //{
                //    PersonRepository.GrantRole(id, 5, p => true);
                //    return ResponseWrapper.SuccessResponse();
                //}
                //else
                //{
                //    return ResponseWrapper.ExceptionResponse(new OtherException("请完善研究领域信息"));
                //}
                //完善校外专家信息
                PersonRepository.ExpertFieldCompletePartners(id);
                PersonRepository.GrantRole(id, 5, p => true);
                return ResponseWrapper.SuccessResponse();
            }
            catch (Exception e)
            {
                return ResponseWrapper.ExceptionResponse(e);
            }
        }

        /// <summary>
        /// 删除专家
        /// </summary>
        /// <returns></returns>
        [Authorize(Roles="院管理员")]
        public HttpResponseMessage Delete( int id )
        {
            try
            {
                PersonRepository.RevokeRole(id, 5, p => true);
                recommendationrepository.RefuseRecommendation(recommendationrepository.GetRecommendation(id).RecommendationId.Value);

                //院管理员取消专家资格
                //通知打点:发给单位管理员
                //_noticeService.AddNoticeList(
                //    _noticeService.GetInstMListByPersonId(id), 115,
                //    new Dictionary<string, string>
                //    { {"UserName", _noticeService.GetNameByPersionId(id)} });

                return ResponseWrapper.SuccessResponse();
            }
            catch (Exception e)
            {
                return ResponseWrapper.ExceptionResponse(e);
            }
        }


        /// <summary>
        /// 院管理员获取专家列表，按单位，研究领域筛选，分页
        /// </summary>
        /// <param name="instId"></param>
        /// <param name="page"></param>
        /// <returns></returns>
        [Route("api/expert/depart/{instId}/{page}"), Authorize(Roles = "院管理员")]
        public HttpResponseMessage GetPagingExpertList(int instId, int page)
        {
            try
            {
                var items = Request.GetQueryNameValuePairs();
                string fieldId = items.FirstOrDefault(p => p.Key == "fieldId").Value;
                string subFieldId = items.FirstOrDefault(p => p.Key == "subFieldId").Value;
                return ResponseWrapper.SuccessResponse(repository.GetPagingExpertList(instId, fieldId, subFieldId, page));
            }
            catch (Exception e)
            {
                return ResponseWrapper.ExceptionResponse(e);
            }
            
        }

        /// <summary>
        /// 单位管理员获取专家列表，按研究领域筛选，分页
        /// </summary>
        /// <param name="page"></param>
        /// <returns></returns>
        [Route("api/expert/inst/{page}"), Authorize(Roles = "单位管理员")]
        public HttpResponseMessage GetInstExpertList(int page)
        {
            try
            {
                var userInfo = UserHelper.GetCurrentUser();
                var items = Request.GetQueryNameValuePairs();
                string fieldId = items.FirstOrDefault(p => p.Key == "fieldId").Value;
                string subFieldId = items.FirstOrDefault(p => p.Key == "subFieldId").Value;
                return ResponseWrapper.SuccessResponse(repository.GetPagingExpertList(userInfo.InstId, fieldId, subFieldId, page));
            }
            catch (Exception e)
            {
                return ResponseWrapper.ExceptionResponse(e);
            }
        }
    }
}
