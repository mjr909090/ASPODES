using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using ASPODES.Common.Util;
using ASPODES.DTO.System;
using ASPODES.WebAPI.Authorize;
using ASPODES.WebAPI.Filter;
using ASPODES.WebAPI.Repository;
using ASPODES.WebAPI.Common;

namespace ASPODES.WebAPI.Controllers
{
    /// <summary>
    /// 系统管理员设置系统参数
    /// </summary>
    [AspodesAuthorize(Roles = "系统管理员")]
    [ActionTrack]
    public class SettingController : ApiController
    {
        private SettingRepository repository = new SettingRepository();

        /// <summary>
        /// 获取提交申请书的日期参数设置
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("api/Setting/ApplicationSetting")]
        public HttpResponseMessage GetApplicationSetting()
        {
            try
            {
                return ResponseWrapper.SuccessResponse(repository.GetApplicationSetting());
            }
            catch(Exception e)
            {
                return ResponseWrapper.ExceptionResponse(e);
            }
            
        }

        /// <summary>
        /// 更新提交申请书的日期参数设置
        /// </summary>
        /// <param name="appSetting"></param>
        /// <returns></returns>
        [HttpPut]
        [Validation]
        [Route("api/Setting/ApplicationSetting")]
        public HttpResponseMessage UpdateApplicationSetting(GetApplicationSettingDTO appSetting)
        {
            try
            {
                repository.UpdateApplicationSetting(appSetting);
                return ResponseWrapper.SuccessResponse("操作成功！");
            }
            catch(Exception e)
            {
                return ResponseWrapper.ExceptionResponse(e);
            }
            
        }

        /// <summary>
        /// 获取院管理员未受理原因列表
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("api/Setting/TemplateReason/Depart")]
        public HttpResponseMessage GetDepartTemplateReason()
        {
            try
            {
                return ResponseWrapper.SuccessResponse(repository.GetTemplateReason(0));
            }
            catch (Exception e)
            {
                return ResponseWrapper.ExceptionResponse(e);
            }
        }

        /// <summary>
        /// 添加院管理员未受理原因
        /// </summary>
        /// <param name="templateReason"></param>
        /// <returns></returns>
        [HttpPost]
        [Validation]
        [Route("api/Setting/TemplateReason/Depart")]
        public HttpResponseMessage AddDepartTemplateReason(TemplateReasonDTO templateReason)
        {
            try
            {
                templateReason.ReasonId = -1;
                repository.AddOrUpdateTemplateReason(0, templateReason);
                return ResponseWrapper.SuccessResponse("添加成功！");
            }
            catch (Exception e)
            {
                return ResponseWrapper.ExceptionResponse(e);
            }

        }

        /// <summary>
        /// 修改院管理员未受理原因
        /// </summary>
        /// <param name="templateReason"></param>
        /// <returns></returns>
        [HttpPut]
        [Validation]
        [Route("api/Setting/TemplateReason/Depart")]
        public HttpResponseMessage UpdateDepartTemplateReason(TemplateReasonDTO templateReason)
        {
            try
            {
                repository.AddOrUpdateTemplateReason(0, templateReason);
                return ResponseWrapper.SuccessResponse("修改成功");
            }
            catch (Exception e)
            {
                return ResponseWrapper.ExceptionResponse(e);
            }
            
        }

        /// <summary>
        /// 删除院管理员未受理原因
        /// </summary>
        /// <param name="templateReason"></param>
        /// <returns></returns>
        [HttpDelete]
        [Validation]
        [Route("api/Setting/TemplateReason/Depart")]
        public HttpResponseMessage DeleteDepartTemplateReason([FromBody]TemplateReasonDTO templateReason)
        {
            try
            {
                repository.DeleteTemplateReason(0, templateReason);
                return ResponseWrapper.SuccessResponse("删除成功");
            }
            catch (Exception e)
            {
                return ResponseWrapper.ExceptionResponse(e);
            }
        }


        /// <summary>
        /// 获取单位管理员驳回原因列表
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("api/Setting/TemplateReason/Inst")]
        public HttpResponseMessage GetInstTemplateReason()
        {
            try
            {
                return ResponseWrapper.SuccessResponse(repository.GetTemplateReason(1));
            }
            catch (Exception e)
            {
                return ResponseWrapper.ExceptionResponse(e);
            }
        }

        /// <summary>
        /// 添加单位管理员驳回原因
        /// </summary>
        /// <param name="templateReason"></param>
        /// <returns></returns>
        [HttpPost]
        [Validation]
        [Route("api/Setting/TemplateReason/Inst")]
        public HttpResponseMessage AddInstTemplateReason(TemplateReasonDTO templateReason)
        {
            try
            {
                templateReason.ReasonId = -1;
                repository.AddOrUpdateTemplateReason(1, templateReason);
                return ResponseWrapper.SuccessResponse("添加成功");
            }
            catch (Exception e)
            {
                return ResponseWrapper.ExceptionResponse(e);
            }
        }

        /// <summary>
        /// 修改单位管理员驳回原因
        /// </summary>
        /// <param name="templateReason"></param>
        /// <returns></returns>
        [HttpPut]
        [Validation]
        [Route("api/Setting/TemplateReason/Inst")]
        public HttpResponseMessage UpdateInstTemplateReason(TemplateReasonDTO templateReason)
        {
            try
            {
                repository.AddOrUpdateTemplateReason(1, templateReason);
                return ResponseWrapper.SuccessResponse("修改成功");
            }
            catch (Exception e)
            {
                return ResponseWrapper.ExceptionResponse(e);
            }
        }

        /// <summary>
        /// 删除单位管理员驳回原因
        /// </summary>
        /// <param name="templateReason"></param>
        /// <returns></returns>
        [HttpDelete]
        [Validation]
        [Route("api/Setting/TemplateReason/Inst")]
        public HttpResponseMessage DeleteInstTemplateReason([FromBody]TemplateReasonDTO templateReason)
        {
            try
            {
                repository.DeleteTemplateReason(1, templateReason);
                return ResponseWrapper.SuccessResponse("删除成功");
            }
            catch (Exception e)
            {
                return ResponseWrapper.ExceptionResponse(e);
            }
        }

        /// <summary>
        /// 设置开启新的年份
        /// </summary>
        /// <param name="year"></param>
        /// <returns></returns>
        [HttpPut,Route("api/setting/startApplication/{year}")]
        public HttpResponseMessage StartApplication( int year )
        {
            try
            {
                repository.StartApplication(year);
                return ResponseWrapper.SuccessResponse("成功开启" + year +"年度申请");
            }
            catch(Exception e)
            {
                return ResponseWrapper.ExceptionResponse(e);
            }
        }


        //[HttpGet]
        //[AllowAnonymous]
        //[Route("api/Setting/test")]
        //public HttpResponseMessage test()
        //{
        //    if (PdfHelper.CreateTaskPdf(51, "dark.docx", "test", 2017))
        //    {
        //        return ResponseWrapper.SuccessResponse("create");
        //    }
        //    else
        //    {
        //        return ResponseWrapper.SuccessResponse("error");

        //    }

        //}

    }
}
