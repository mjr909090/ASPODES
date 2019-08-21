using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

using ASPODES.DTO.Inst_Person_User;
using ASPODES.WebAPI.Repository;
using ASPODES.WebAPI.Common;
using ASPODES.WebAPI.Service;

namespace ASPODES.WebAPI.Controllers
{
    /// <summary>
    /// 单位操作
    /// </summary>
    [ASPODES.WebAPI.Filter.ActionTrack]
    public class InstController : ApiController
    {
        private InstRepository repository = new InstRepository();
        //private CreateNoticeService _noticeService;

        //public InstController(CreateNoticeService noticeService)
        //{
        //    this._noticeService = noticeService;
        //}
        /// <summary>
        /// 根据单位ID获取单位信息
        /// </summary>
        /// <param name="id">单位ID</param>
        /// <returns></returns>
        public HttpResponseMessage Get(int id)
        {
            try
            {
                return ResponseWrapper.SuccessResponse(repository.GetOneInst(id));
            }
            catch (Exception e)
            {
                return ResponseWrapper.ExceptionResponse(e);
            }
        }

        /// <summary>
        /// 获取所有院管单位列表,简单信息列表，只有ID和单位名，用于显示下拉列表
        /// </summary>
        /// <returns></returns>
        public HttpResponseMessage Get()
        {
            try
            {
                return ResponseWrapper.SuccessResponse(repository.GetComboInstList(i => i.Status == "C" && i.Type != Model.InstituteType.PARTNER));
            }
            catch (Exception e)
            {
                return ResponseWrapper.ExceptionResponse(e);
            }
        }

        /// <summary>
        /// 获取所有外单位列表,简单信息列表，只有ID和单位名，用于显示下拉列表
        /// </summary>
        /// <returns></returns>
        [Route("api/inst/partners")]
        public HttpResponseMessage GetPartners()
        {
            try
            {
                return ResponseWrapper.SuccessResponse(repository.GetComboInstList(i => i.Status == "C" && i.Type == Model.InstituteType.PARTNER));
            }
            catch (Exception e)
            {
                return ResponseWrapper.ExceptionResponse(e);
            }
        }

        /// <summary>
        /// 获取除本单位外的单位列表,简单信息列表，只有ID和单位名，用于显示下拉列表
        /// </summary>
        /// <returns></returns>
        [Route("api/inst/others")]
        public HttpResponseMessage GetOtherInsts()
        {
            try
            {
                var userInfo = UserHelper.GetCurrentUser();
                return ResponseWrapper.SuccessResponse(repository.GetComboInstList(i => i.Status == "C" && i.InstituteId != userInfo.InstId ));
            }
            catch (Exception e)
            {
                return ResponseWrapper.ExceptionResponse(e);
            }
        }
        
        /// <summary>
        /// 获取单位详细信息列表
        /// </summary>
        /// <returns></returns>
        [Route("api/inst/list")]
        public HttpResponseMessage GetInstList()
        {
            try
            {
                return ResponseWrapper.SuccessResponse(repository.GetInstList(i => i.Status == "C"));
            }
            catch (Exception e)
            {
                return ResponseWrapper.ExceptionResponse(e);
            }
        }

        /// <summary>
        /// 导入单位信息
        /// </summary>
        /// <returns></returns>
        [Route("api/inst/upload"),Authorize(Roles="系统管理员")]
        public HttpResponseMessage UploadInstInfoFile()
        {
            return repository.UplodInstInfoFile();
        }

        /// <summary>
        /// 添加院外单位
        /// </summary>
        /// <param name="dto">单位信息</param>
        /// <returns></returns>
        [Route("api/inst/partners")]
        public HttpResponseMessage PostPartnersInst(AddInstituteDTO dto)
        {
            try
            {
                dto.Type = "0";
                dto.ContactId = null;
                return ResponseWrapper.SuccessResponse(repository.AddInstitute(dto));
            }
            catch (Exception e)
            {
                return ResponseWrapper.ExceptionResponse(e);
            }
        }

        /// <summary>
        /// 添加单位
        /// </summary>
        /// <param name="dto">单位信息</param>
        /// <returns></returns>
        [Authorize(Roles = "系统管理员")]
        public HttpResponseMessage Post(AddInstituteDTO dto)
        {
            try
            {
                return ResponseWrapper.SuccessResponse(repository.AddInstitute(dto));
            }
            catch (Exception e)
            {
                return ResponseWrapper.ExceptionResponse(e);
            }
        }

        /// <summary>
        /// 单位管理员更改单位信息
        /// </summary>
        /// <param name="instDTO">更新的单位信息</param>
        /// <returns></returns>
        [Authorize(Roles="单位管理员")]
        public HttpResponseMessage Put(AddInstituteDTO instDTO)
        {
            try
            {
                return ResponseWrapper.SuccessResponse(repository.UpdateInstitute(instDTO, i => i.InstituteId == instDTO.InstituteId));
            }
            catch (Exception e)
            {
                return ResponseWrapper.ExceptionResponse(e);
            }
        }

        /// <summary>
        /// 院管理员更改单位信息
        /// </summary>
        /// <param name="instDTO">更新的单位信息</param>
        /// <returns></returns>
        [HttpPut]
        [Route("api/inst/depart/update"),Authorize(Roles="系统管理员")]
        public HttpResponseMessage SystemUpdateInst(AddInstituteDTO instDTO)
        {
            try
            {
                var result = repository.UpdateInstitute(instDTO, i => true);

                //系统管理员修改单位信息
                //通知打点:单位管理员
                //_noticeService.AddNoticeList(
                //    _noticeService.GetInstituteManagerIdList(instDTO.InstituteId.Value), 117);

                return ResponseWrapper.SuccessResponse(result);
            }
            catch (Exception e)
            {
                return ResponseWrapper.ExceptionResponse(e);
            }
        }

        
        /// <summary>
        /// 获取当前用户所属的单位信息
        /// </summary>
        /// <returns></returns>
        [Route("api/inst/self")]
        public HttpResponseMessage GetSelfInst()
        {
            try
            {
                var userInfo = UserHelper.GetCurrentUser();
                return ResponseWrapper.SuccessResponse(repository.GetOneInst(userInfo.InstId));
            }
            catch (Exception e)
            {
                return ResponseWrapper.ExceptionResponse(e);
            }
            
        }

        /// <summary>
        /// 删除单位
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [Authorize(Roles="系统管理员")]
        public HttpResponseMessage Delete(int id)
        {
            try
            {
                repository.DeleteInst(id);
                return ResponseWrapper.SuccessResponse();
            }
            catch (Exception e)
            {
                return ResponseWrapper.ExceptionResponse(e);
            }
        }


        /// <summary>
        /// 验证单位是否存在
        /// </summary>
        /// <param name="Code">单位组织机构代码</param>
        /// <returns></returns>Validate
        //[Authorize(Roles="系统管理员")]  可以添加外单位
        [HttpPost,Route("api/inst/validatecode/{code}")]
        public HttpResponseMessage PostValidateCode(string code)
        {
            try
            {
                var InstituteList = repository.GetInstList(i => i.Code == code);
                if(InstituteList.Count() != 0)
                {
                    return ResponseWrapper.SuccessResponse("该组织机构代码已存在，若找不到该单位请确定该单位状态是否锁定");
                }
                return ResponseWrapper.SuccessResponse("该组织机构代码正确");

            }
            catch (Exception e)
            {
                return ResponseWrapper.ExceptionResponse(e);
            }
        }
    }
}
