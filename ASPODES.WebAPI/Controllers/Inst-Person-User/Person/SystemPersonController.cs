using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using ASPODES.DTO.Category;
using ASPODES.Model;
using ASPODES.WebAPI.Repository;
using ASPODES.DTO.Inst_Person_User;
using ASPODES.WebAPI.Authorize;
using ASPODES.WebAPI.Filter;
using ASPODES.WebAPI.Common;
using ASPODES.WebAPI.Service;

namespace ASPODES.WebAPI.Controllers
{
    /// <summary>
    /// 系统管理员控制器
    /// </summary>
    [ActionTrack]
    [AspodesAuthorize(Roles="系统管理员")]
    public class SystemPersonController : ApiController
    {
        private PersonRepository repository = new PersonRepository();
        //private CreateNoticeService _noticeService;

        //public SystemPersonController(CreateNoticeService noticeService)
        //{
        //    this._noticeService = noticeService;
        //}
        /// <summary>
        /// 添加院管理员
        /// </summary>
        /// <param name="personDTO"></param>
        /// <returns></returns>
        [HttpPost, Route("api/systemperson")]
        public HttpResponseMessage AddDepartAdmin( AddPersonDTO personDTO )
        {
            try
            {
                var result = repository.AddDepartAdmin(personDTO);

                //系统管理员任命院管理员
                //通知打点:发给用户
                //_noticeService.AddNotice(
                //    _noticeService.GetUserIdByPersonId(personDTO.PersonId.Value), 108);

                return ResponseWrapper.SuccessResponse(result);
            }
            catch (Exception e)
            {
                return ResponseWrapper.ExceptionResponse(e);
            }
        }
        
        /// <summary>
        /// 给person赋予院管理员角色
        /// </summary>
        /// <param name="personId"></param>
        /// <returns></returns>
        [HttpPut,Route("api/systemperson/appoint/{personId}")]
        public HttpResponseMessage AppointDepartAdmin( int personId )
        {
            try
            {
                PersonRepository.GrantRole(personId, 3, p => true);

                //系统管理员任命院管理员
                //通知打点:发给用户
                //_noticeService.AddNotice(
                //    _noticeService.GetUserIdByPersonId(personId), 107);

                return ResponseWrapper.SuccessResponse();
            }
            catch (Exception e)
            {
                return ResponseWrapper.ExceptionResponse(e);
            }
        }

        /// <summary>
        /// 取消院管理员角色
        /// </summary>
        /// <param name="personId"></param>
        /// <returns></returns>
        [HttpPut, Route("api/systemperson/dismiss/{personId}")]
        public HttpResponseMessage DismissDepartAdmin( int personId )
        {
            try
            {
                PersonRepository.RevokeRole(personId, 3, p => true);

                //系统管理员任命院管理员
                //通知打点:发给用户
                //_noticeService.AddNotice(
                //    _noticeService.GetUserIdByPersonId(personId), 109);

                return ResponseWrapper.SuccessResponse();
            }
            catch (Exception e)
            {
                return ResponseWrapper.ExceptionResponse(e);
            }
        }

        /// <summary>
        /// 获取院管理员列表
        /// </summary>
        /// <returns></returns>
        [Route("api/systemperson/departadmins/{page}")]
        public HttpResponseMessage GetDepartAdmins( int page )
        {
            try
            {
                return ResponseWrapper.SuccessResponse(repository.GetPagingPersonListByRole(3, 0, page));
            }
            catch (Exception e)
            {
                return ResponseWrapper.ExceptionResponse(e);
            }
        }

        /// <summary>
        /// 获取可以设置为院管理员的人员列表,简单信息，用于ComboBox下拉列表
        /// </summary>
        /// <returns></returns>
        [Route("api/systemperson/combo/departadmin/candidate")]
        public HttpResponseMessage GetHeadQuaterPersons()
        {
            try
            {
                return ResponseWrapper.SuccessResponse(repository.GetDedpartAdminCandidate());
            }
            catch (Exception e)
            {
                return ResponseWrapper.ExceptionResponse(e);
            }
        }

        /// <summary>
        /// 获取项目分类列表
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("api/systemperson/supportcategory")]
        public HttpResponseMessage GetSupportCategorys()
        {
            try
            {
                return ResponseWrapper.SuccessResponse(repository.GetSupportCategorys());
            }
            catch (Exception e)
            {
                return ResponseWrapper.ExceptionResponse(e);
            }
        }

        /// <summary>
        /// 添加项目分类
        /// </summary>
        /// <param name="supportCategoryData">项目分类信息，ID为0时为主分类，其他值时为父分类ID</param>
        /// <returns></returns>
        [Validation]
        [HttpPost]
        [Route("api/systemperson/supportcategory")]
        public HttpResponseMessage PostSupportCategorys(SysSupportCategoryAddDTO supportCategoryData)
        {
            try
            {
                repository.GreateSupportCategorys(supportCategoryData);
                return ResponseWrapper.SuccessResponse();
            }
            catch (Exception e)
            {
                return ResponseWrapper.ExceptionResponse(e);
            }
        }

        /// <summary>
        /// 获取单位管理员列表
        /// </summary>
        /// <returns></returns>
        [Route("api/systemperson/instadmins/{instId}/{page}")]
        public HttpResponseMessage GetInstAdmins(int instId, int page)
        {
            try
            {
                return ResponseWrapper.SuccessResponse(repository.GetPagingPersonListByRole(2, instId, page));
            }
            catch (Exception e)
            {
                return ResponseWrapper.ExceptionResponse(e);
            }
        }

        /// <summary>
        /// 设置用户为单位管理员
        /// </summary>
        /// <param name="userId">用户ID</param>
        /// <returns></returns>
        [HttpPut]
        [Route("api/systemperson/appoint/instadmin/{personId}")]
        public HttpResponseMessage AppointInstAdmin(int personId)
        {
            try
            {
                var result = PersonRepository.GrantRole(personId, 2, p => true);

                //系统管理员任命单位管理员
                //通知打点:发给用户
                //_noticeService.AddNotice(
                //    _noticeService.GetUserIdByPersonId(personId), 111);

                return ResponseWrapper.SuccessResponse(result);
            }
            catch (Exception e)
            {
                return ResponseWrapper.ExceptionResponse(e);
            }

        }

        /// <summary>
        /// 取消用户为单位管理员
        /// </summary>
        /// <param name="userId">用户ID</param>
        /// <returns></returns>
        [HttpPut]
        [Route("api/systemperson/dismiss/instadmin/{personId}")]
        public HttpResponseMessage DismissInstAdmin(int personId)
        {
            try
            {
                var result = PersonRepository.RevokeRole(personId, 2, p => true);

                //系统管理员取消单位管理员资格
                //通知打点:发给用户
                //_noticeService.AddNotice(
                //    _noticeService.GetUserIdByPersonId(personId), 112);

                return ResponseWrapper.SuccessResponse(result);
            }
            catch (Exception e)
            {
                return ResponseWrapper.ExceptionResponse(e);
            }
        }

        /// <summary>
        /// 设置单位联系人
        /// </summary>
        /// <param name="personId"></param>
        /// <returns></returns>
        [HttpPut, Route("api/systemperson/instcontact/{personId}")]
        public HttpResponseMessage SetInstContact(int personId)
        {
            try
            {
                repository.SetInstContact(personId);
                return ResponseWrapper.SuccessResponse();
            }
            catch (Exception e)
            {
                return ResponseWrapper.ExceptionResponse(e);
            }
        }
    }
}
