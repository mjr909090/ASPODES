using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Net.Http;
using System.Web.Http;

using ASPODES.DTO.Inst_Person_User;
using ASPODES.WebAPI.Repository;
using System.Security.Principal;
using ASPODES.WebAPI.Authorize;
using ASPODES.WebAPI.Common;
using ASPODES.Database;
using ASPODES.Model;
using ASPODES.DTO;
using ASPODES.WebAPI.Service;

namespace ASPODES.WebAPI.Controllers
{
    /// <summary>
    /// 单位管理员人员操作
    /// </summary>
    //[AspodesAuthorize]
    [Authorize(Roles = "单位管理员")]
    [ASPODES.WebAPI.Filter.ActionTrack]
    public class InstPersonController : ApiController
    {

        private PersonRepository repository = new PersonRepository();
        //private CreateNoticeService _noticeService;

        /// <summary>
        /// 构造函数，注入CreateNoticeService
        /// </summary>
        /// <param name="createNoticeService"></param>
        //public InstPersonController(CreateNoticeService createNoticeService)
        //{
        //    this._noticeService = createNoticeService;
        //}
        /// <summary>
        /// 获取当前用户所属单位的人员信息,分页
        /// </summary>
        /// <returns></returns>
        //[Route("api/InstPerson/self/{page}")]
        [Route("api/instperson/normal/{page}")]
        public HttpResponseMessage GetInstPersons(int page)
        {
            try
            {
                var user = UserHelper.GetCurrentUser();
                var pagingList = repository.GetPagingPersonList(p => p.InstituteId == user.InstId && p.Status == "C" && p.Email != "system@126.com", page);
                return ResponseWrapper.SuccessResponse(pagingList);
            }
            catch (Exception e)
            {
                return ResponseWrapper.ExceptionResponse(e);
            }
        }

        /// <summary>
        /// 查找院内的人员
        /// </summary>
        /// <param name="page"></param>
        /// <param name="PersonDTO"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("api/instperson/search/{page}")]
        public HttpResponseMessage GetInstPersonsSearch(int page, SearchPersonDTO PersonDTO)
        {

            try
            {
                var user = UserHelper.GetCurrentUser();
                PagingListDTO<GetPersonDTO> pagingList = null;
                if (!string.IsNullOrEmpty(PersonDTO.Types.Trim()) && !string.IsNullOrEmpty(PersonDTO.KeyWords.Trim()))
                {
                    switch (PersonDTO.Types.Trim().ToLower())
                    {
                        case "name"://姓名
                            pagingList = repository.GetPagingPersonList(p => p.InstituteId == user.InstId && p.Status == "C" && p.Email != "system@126.com" && p.Name.Contains(PersonDTO.KeyWords), page);
                            break;
                        case "idcard"://身份证
                            pagingList = repository.GetPagingPersonList(p => p.InstituteId == user.InstId && p.Status == "C" && p.Email != "system@126.com" && p.IDCard.ToLower().Contains(PersonDTO.KeyWords.ToLower()), page);
                            break;
                        case "phone"://手机
                            pagingList = repository.GetPagingPersonList(p => p.InstituteId == user.InstId && p.Status == "C" && p.Email != "system@126.com" && p.Phone.Contains(PersonDTO.KeyWords), page);
                            break;
                        case "email"://邮箱
                            pagingList = repository.GetPagingPersonList(p => p.InstituteId == user.InstId && p.Status == "C" && p.Email != "system@126.com" && p.Email.ToLower().Contains(PersonDTO.KeyWords.ToLower()), page);
                            break;
                        default:
                            pagingList = repository.GetPagingPersonList(p => p.InstituteId == user.InstId && p.Status == "C" && p.Email != "system@126.com", page);
                            break;
                    }
                }
                else
                {
                    pagingList = repository.GetPagingPersonList(p => p.InstituteId == user.InstId && p.Status == "C" && p.Email != "system@126.com", page);
                }
                return ResponseWrapper.SuccessResponse(pagingList);
            }
            catch (Exception e)
            {
                return ResponseWrapper.ExceptionResponse(e);
            }


        }

        /*
        /// <summary>
        /// 获取当前用户所属单位的简单人员信息,ComboBox下拉框
        /// </summary>
        /// <returns></returns>
        [Route("api/InstPerson/combo/self")]
        public HttpResponseMessage GetInstPersons()
        {
            var user = UserHelper.GetCurrentUser();
            return repository.GetNormalPersonList<GetComboPersonDTO>(p => p.InstituteId == user.InstId);
        }
        */

        /// <summary>
        /// 获取单位被删的人员,分页
        /// </summary>
        /// <returns></returns>
        //[Route("api/instperson/delete")]
        [Route("api/instperson/delete/{page}")]
        public HttpResponseMessage GetInstDeletePersons(int page)
        {
            try
            {
                var user = UserHelper.GetCurrentUser();
                return ResponseWrapper.SuccessResponse(repository.GetPagingPersonList(p => p.InstituteId == user.InstId && p.Status == "D", page));
            }
            catch (Exception e)
            {
                return ResponseWrapper.ExceptionResponse(e);
            }
        }


        /// <summary>
        /// 添加人员
        /// </summary>
        /// <param name="dto">添加人员信息</param>
        /// <returns></returns>
        public HttpResponseMessage Post(AddPersonDTO personDTO)
        {
            try
            {
                var user = UserHelper.GetCurrentUser();
                var person = repository.AddPerson(personDTO, p => p.InstituteId == user.InstId);
                return ResponseWrapper.SuccessResponse(person);
            }
            catch (Exception e)
            {
                return ResponseWrapper.ExceptionResponse(e);
            }

        }

        /// <summary>
        /// 单位管理员更新人员信息
        /// </summary>
        /// <param name="personDTO">人员信息</param>
        /// <returns></returns>
        public HttpResponseMessage Put(UpdatePersonDTO personDTO)
        {
            try
            {
                var user = UserHelper.GetCurrentUser();
                var result = repository.UpdatePerson(personDTO, p => p.InstituteId == user.InstId);

                //单位管理员修改个人信息
                //通知打点:发给用户
                //_noticeService.AddNotice(
                //    _noticeService.GetUserIdByPersonId(personDTO.PersonId), 101);

                return ResponseWrapper.SuccessResponse(result);
            }
            catch (Exception e)
            {
                return ResponseWrapper.ExceptionResponse(e);
            }


        }

        /// <summary>
        /// 重置密码
        /// </summary>
        /// <param name="personId"></param>
        /// <returns></returns>
        [HttpPut, Route("api/instperson/resetpwd/{personId}")]
        public HttpResponseMessage ResetPassword(int personId)
        {
            try
            {
                var user = UserHelper.GetCurrentUser();
                repository.ResetPassword(personId, p => p.InstituteId == user.InstId);

                //单位管理员重置个人密码
                //通知打点:发给用户
                //_noticeService.AddNotice(
                //    _noticeService.GetUserIdByPersonId(personId), 102);

                return ResponseWrapper.SuccessResponse("修改成功");
            }
            catch (Exception e)
            {
                return ResponseWrapper.ExceptionResponse(e);
            }


        }

        /// <summary>
        /// 单位管理员删除用户
        /// </summary>
        /// <param name="id">人员ID</param>
        /// <returns></returns>
        public HttpResponseMessage Delete(int id)
        {
            try
            {
                var user = UserHelper.GetCurrentUser();
                repository.DeletePerson(id, p => p.InstituteId == user.InstId);
                return ResponseWrapper.SuccessResponse("删除成功");
            }
            catch (Exception e)
            {
                return ResponseWrapper.ExceptionResponse(e);
            }
            ;
        }

        /// <summary>
        /// 批量添加人员
        /// </summary>
        /// <returns></returns>
        [Route("api/instperson/upload")]
        public HttpResponseMessage UploadPersonInfoFile()
        {
            try
            {
                var user = UserHelper.GetCurrentUser();
                repository.UploadPersonInfoFile(user.InstId);
                return ResponseWrapper.SuccessResponse("上传成功");
            }
            catch (Exception e)
            {
                return ResponseWrapper.ExceptionResponse(e);
            }

        }

        /// <summary>
        /// 单位管理员启用被禁的用户
        /// </summary>
        /// <param name="personId">人员ID</param>
        /// <returns></returns>
        [HttpPut, Route("api/instperson/revive/{personId}")]
        public HttpResponseMessage InstRevivePerson(int personId)
        {
            try
            {

                var user = UserHelper.GetCurrentUser();
                return ResponseWrapper.SuccessResponse(repository.RevivePerson(personId, p => p.InstituteId == user.InstId));
            }
            catch (Exception e)
            {
                return ResponseWrapper.ExceptionResponse(e);
            }

        }
    }
}
