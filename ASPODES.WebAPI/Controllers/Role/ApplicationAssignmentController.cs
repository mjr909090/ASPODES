using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

using ASPODES.WebAPI.Repository;
using ASPODES.DTO.Role;
using System.Web;
using System.IO;
using ASPODES.WebAPI.Authorize;
using ASPODES.WebAPI.Common;
using ASPODES.WebAPI.Filter;
using Newtonsoft.Json;
using ASPODES.WebAPI.Service;

namespace ASPODES.WebAPI.Controllers.Role
{

    /// <summary>
    /// 系统管理员分配院管理员所管理项目分类
    /// </summary>
    [AspodesAuthorize(Roles="系统管理员")]
    [ActionTrack]
    public class ApplicationAssignmentController : ApiController
    {
        private ApplicationAssignmentRepository repository = new ApplicationAssignmentRepository();
        //private CreateNoticeService _noticeService;

        //public ApplicationAssignmentController(CreateNoticeService noticeService)
        //{
        //    this._noticeService = noticeService;
        //}
        // /// <summary>
        // /// 添加申请书分配
        // /// </summary>
        // /// <param name="dto"申请书分配信息</param>
        // /// <returns></returns>
        // public HttpResponseMessage Post(AddApplicationAssignmentDTO dto)
        // {
        //     return repository.AddApplicationAssignment(dto);
        // }

        // /// <summary>
        // /// 根据申請書ID查看已分配的申请书情況
        // /// </summary>
        // /// <param name="id">分配的申請書ID</param>
        // /// <returns></returns>
        //public HttpResponseMessage Get(int Id)
        // {
        //     return repository.GetApplicationAssignment(Id);
        // }

        // /// <summary>
        // /// 删除已分配的申请书
        // /// </summary>
        ///// <param name="id">申请书Id</param>
        // /// <returns></returns>
        // public HttpResponseMessage Delete(int Id)
        // {
        //     return repository.DeleteApplicationAssignment(Id);
        // }

        /// <summary>
        /// 获取院管理员所负责的申请书分类
        /// </summary>
        /// <param name="Id">personid</param>
        /// <returns></returns>
        [HttpGet]
        [Route("api/applicationassignments/{id}")]
        [Validation]
        public HttpResponseMessage GetAssignments(int id)
        {
            //var request = HttpContext.Current.Request;
            //var id = request.Params["id"];
            //if (null == id || string.IsNullOrEmpty(id))
            //{
            //    return Response.CreateParameterErrorResponse("id不能为空！");
            //}
            try
            {
                return ResponseWrapper.SuccessResponse(repository.GetUserApplicationAssignment(id));
            }
            catch(Exception e)
            {
                return ResponseWrapper.ExceptionResponse(e);
            }
        }


        /// <summary>
        /// 更新院管理员所管理的项目分类信息
        /// </summary>
        /// <param name="id">personid</param>
        /// <param name="AssignmentList"></param>
        /// <returns></returns>
        [HttpPut]
        [Route("api/applicationassignments/{id}")]
        [Validation]
        public HttpResponseMessage PostAssignments(int id,List<ApplicationAssignmentDTO> AssignmentList)
        {
            //var request = HttpContext.Current.Request;
            //var id = request.Params["id"];
            //if (null == id || string.IsNullOrEmpty(id))
            //{
            //    return Response.CreateParameterErrorResponse("id不能为空！");
            //}
            try
            {
                var result = repository.UpdateUserApplicationAssignment(id, AssignmentList);

                //系统管理员更改某院管理员的分管类型
                //通知打点:发给用户
                //_noticeService.AddNotice(
                //    _noticeService.GetUserIdByPersonId(id), 110);

                return ResponseWrapper.SuccessResponse(result);
            }
            catch (Exception e)
            {
                 return ResponseWrapper.ExceptionResponse(e);
            }
        }
    }
}
