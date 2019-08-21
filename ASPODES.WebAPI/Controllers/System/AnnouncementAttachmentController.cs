using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using ASPODES.Database;
using ASPODES.WebAPI.Authorize;
using ASPODES.WebAPI.Common;
using ASPODES.WebAPI.Filter;
using ASPODES.WebAPI.Repository;
using ASPODES.DTO.System;
using ASPODES.Model;
using ASPODES.WebAPI.Security;
namespace ASPODES.WebAPI.Controllers

{
    /// <summary>
    /// 公告附件
    /// </summary>
    public class AnnouncementAttachmentController : ApiController
    {
        private AnnouncementAttachmentRepository repository = new AnnouncementAttachmentRepository();
        private Privilege privilege = new Privilege();
        /// <summary>
        /// 获取公告附件的文档列表
        /// </summary>
        /// <param name="id">公告ID</param>
        /// <returns></returns>
        public HttpResponseMessage Get(int id)
        {
            try
            {
                return ResponseWrapper.SuccessResponse(repository.GetAnnouncementAttachmentList(aa => aa.AnnouncementId == id));
            }
            catch (Exception e)
            {
                return ResponseWrapper.ExceptionResponse(e);
            }
        }

        /// <summary>
        /// 根据公告附件的ID下载附件
        /// </summary>
        /// <param name="announcementAttachmentId">公告附件ID</param>
        /// <returns></returns>
        [HttpGet]
        [Route("api/announcementAttachment/download/{announcementAttachmentId}")]
        public HttpResponseMessage Download(int announcementAttachmentId)
        {
            return repository.DownloadAnnouncementAttachment(announcementAttachmentId);
        }



        /// <summary>
        /// 上传公告附件
        /// </summary>
        /// <returns></returns>

        public HttpResponseMessage Post()
        {
            try
            {
                var result = repository.UploadAnnouncementAttachment();
                return ResponseWrapper.SuccessResponse(result);
            }
            catch (Exception e)
            {
                return ResponseWrapper.ExceptionResponse(e);
            }
        }

        /// <summary>
        /// 更新时添加公告ID
        /// </summary>
        /// <returns></returns>
        public HttpResponseMessage Post(int id)
        {
            try
            {
                var result = repository.UploadAnnouncementAttachment(id);
                return ResponseWrapper.SuccessResponse(result);
            }
            catch (Exception e)
            {
                return ResponseWrapper.ExceptionResponse(e);
            }
        }

        /// <summary>
        /// 删除公告附件
        /// </summary>
        /// <param name="id">公告附件</param>
        /// <returns></returns>
        public HttpResponseMessage Delete(int id)
        {
            try
            {
                repository.Delete(id);
                return ResponseWrapper.SuccessResponse("删除成功");
            }
            catch (Exception e)
            {
                return ResponseWrapper.ExceptionResponse(e);
            }
        }

    }
}
