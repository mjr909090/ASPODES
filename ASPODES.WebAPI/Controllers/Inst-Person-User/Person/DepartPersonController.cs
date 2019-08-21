using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Net.Http;
using System.Web.Http;

using ASPODES.DTO.Inst_Person_User;
using ASPODES.WebAPI.Authorize;
using ASPODES.WebAPI.Repository;
using ASPODES.Common;
using ASPODES.Model;
using ASPODES.WebAPI.Common;

namespace ASPODES.WebAPI.Controllers
{
    /// <summary>
    /// 院管理员人员操作
    /// </summary>
    [Authorize(Roles="院管理员")]
    [ASPODES.WebAPI.Filter.ActionTrack]
    public class DepartPersonController : ApiController
    {
        private PersonRepository repository = new PersonRepository();

        /// <summary>
        /// 导入InstId单位的人员
        /// </summary>
        /// <param name="instId"></param>
        /// <returns></returns>
        [Route("api/departperson/upload/{instId}")]
        public HttpResponseMessage UploadInstPersonInfoFile( int instId )
        {
            try
            {
                repository.UploadPersonInfoFile( instId );
                return ResponseWrapper.SuccessResponse();
            }
            catch( Exception e)
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
                return ResponseWrapper.SuccessResponse(repository.AddPerson(personDTO, p => true));
            }
            catch (Exception e)
            {
                return ResponseWrapper.ExceptionResponse(e);
            }
        }

        /// <summary>
        /// 院管理员更新人员信息
        /// </summary>
        /// <param name="personDTO">人员信息</param>
        /// <returns></returns>
        public HttpResponseMessage Put(UpdatePersonDTO personDTO)
        {
            try
            {
                return ResponseWrapper.SuccessResponse(repository.UpdatePerson(personDTO, p => true));
            }
            catch (Exception e)
            {
                return ResponseWrapper.ExceptionResponse(e);
            }
        }

        /// <summary>
        /// 院管理员删除人员
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public HttpResponseMessage Delete(int id)
        {
            try
            {
                repository.DeletePerson(id, p => true);
                return ResponseWrapper.SuccessResponse();
            }
            catch (Exception e)
            {
                return ResponseWrapper.ExceptionResponse(e);
            }

        }

        /// <summary>
        /// 院管理员启用被禁的用户
        /// </summary>
        /// <param name="personId"></param>
        /// <returns></returns>
        [HttpPut,Route("api/departperson/revive/{personId}")]
        public HttpResponseMessage RevivePerson(int personId)
        {
            try
            {
                return ResponseWrapper.SuccessResponse(repository.RevivePerson(personId, p => true));
            }
            catch (Exception e)
            {
                return ResponseWrapper.ExceptionResponse(e);
            }
        }

        ///// <summary>
        ///// 获取单位管理员列表
        ///// </summary>
        ///// <returns></returns>
        //[Route("api/departperson/instadmins/{instId}/{page}")]
        //public HttpResponseMessage GetInstAdmins( int instId, int page)
        //{
        //    return Common.Response.Create(repository.GetPagingPersonListByRole(2, instId, page ));
        //}

        ///// <summary>
        ///// 设置用户为单位管理员
        ///// </summary>
        ///// <param name="userId">用户ID</param>
        ///// <returns></returns>
        //[HttpPut]
        //[Route("api/departperson/appoint/instadmin/{personId}")]
        //public HttpResponseMessage AppointInstAdmin( int personId )
        //{
        //    var content = PersonRepository.GrantRole(personId, 2, p => true);
        //    return Common.Response.Create(content);
        //}

        ///// <summary>
        ///// 取消用户为单位管理员
        ///// </summary>
        ///// <param name="userId">用户ID</param>
        ///// <returns></returns>
        //[HttpPut]
        //[Route("api/departperson/dismiss/instadmin/{personId}")]
        //public HttpResponseMessage DismissInstAdmin(int personId)
        //{
        //    var content = PersonRepository.RevokeRole(personId, 2, p => true);
        //    return Common.Response.Create(content);
        //}

        ///// <summary>
        ///// 设置单位联系人
        ///// </summary>
        ///// <param name="personId"></param>
        ///// <returns></returns>
        //[HttpPut, Route("api/departperson/instcontact/{personId}")]
        //public HttpResponseMessage SetInstContact(int personId)
        //{
        //    return repository.SetInstContact(personId);
        //}
    }
}
