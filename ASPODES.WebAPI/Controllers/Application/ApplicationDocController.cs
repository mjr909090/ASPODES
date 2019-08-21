using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using ASPODES.Common;
using ASPODES.WebAPI.Repository;

using ASPODES.WebAPI.Common;
using ASPODES.DTO.Application;
using System.Net;
using ASPODES.WebAPI.Authorize;
using ASPODES.WebAPI.Filter;
using ASPODES.WebAPI.Security;
using ASPODES.Model;
using System.IO;

namespace ASPODES.WebAPI.Controllers
{
    /// <summary>
    /// 申请书文档处理
    /// </summary>
    [AspodesAuthorize]
    [ActionTrack]
    public class ApplicationDocController : ApiController
    {

        private ApplicationDocRepository repository = new ApplicationDocRepository();
        private Privilege privilege = new Privilege();
        
        /// <summary>
        /// 获取申请书除PDF外的文档列表
        /// </summary>
        /// <param name="id">申请书ID</param>
        /// <returns></returns>
        [Authorize(Roles="用户")]
        public HttpResponseMessage Get( string id )
        {
            try
            {
                return ResponseWrapper.SuccessResponse(repository.GetApplicationDocList(ad => ad.ApplicationId == id && ad.Type != Model.ApplicationDocType.PDF));
            }
            catch (Exception e)
            {
                return ResponseWrapper.ExceptionResponse(e);
            }
        }

        /// <summary>
        /// 根据申请书文档的ID下载文档
        /// </summary>
        /// <param name="applicationDocId">文档ID</param>
        /// <returns></returns>
        [HttpGet]
        [Route("api/applicationdoc/download/{applicationDocId}")]
        public HttpResponseMessage Download(int applicationDocId )
        {
            try
            {
                return repository.DownloadApplicationDoc(applicationDocId);
            }
            catch (Exception e)
            {
                return ResponseWrapper.ExceptionResponse(e);
            }
            
        }

        /// <summary>
        /// 下载申请书的PDF文档
        /// </summary>
        /// <param name="applicationId">申请书ID</param>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpGet]
        [Route("api/applicationdoc/download/pdf/{applicationId}")]
        public HttpResponseMessage DownloadApplicationPDFDoc(string applicationId)
        {
            try
            {
                return repository.DownloadApplicationPDFDoc(applicationId);
            }
            catch (Exception e)
            {
                return ResponseWrapper.ExceptionResponse(e);
            }
            
        }

        /// <summary>
        /// 专家打包下载自己的申请书
        /// </summary>
        /// <returns></returns>
        [Route("api/applicationdoc/expert/downloadPackage"), Authorize(Roles = "专家")]
        [HttpGet]
        public HttpResponseMessage ExpertDownloadApplicationPackage()
        {
            try
            {
                var userInfo = UserHelper.GetCurrentUser();
                string packageName = "Review" + userInfo.PersonId + ".zip";
                return repository.ExpertDownloadApplicationPackage(userInfo.UserId, packageName);
            }
            catch (Exception e)
            {
                return ResponseWrapper.ExceptionResponse(e);
            }
        }

        /// <summary>
        /// 单位管理员打包下载待评审申请书
        /// </summary>
        /// <returns></returns>
        [Route("api/applicationdoc/inst/downloadPackage/{categoryId}"), Authorize(Roles = "单位管理员")]
        [HttpGet]
        public HttpResponseMessage InstDownloadApplicationPackage(int categoryId)
        {
            try
            {
                var userInfo = UserHelper.GetCurrentUser();
                string packageName = "InstChecking" + userInfo.PersonId + ".zip";
                return repository.InstDownloadApplicationPackage(userInfo.InstId, categoryId, ApplicationStatus.CHECK, packageName);
            }
            catch (Exception e)
            {
                return ResponseWrapper.ExceptionResponse(e);
            }
        }

        /// <summary>
        /// 院管理员打包下载待评审申请书
        /// </summary>
        /// <returns></returns>
        [Route("api/applicationdoc/inst/downloadPackage/{categoryId}/{instId}"), Authorize(Roles = "单位管理员")]
        [HttpGet]
        public HttpResponseMessage DeptDownloadApplicationPackage(int categoryId, int instId)
        {
            try
            {
                var userInfo = UserHelper.GetCurrentUser();
                string packageName = "DeptChecking" + userInfo.PersonId + ".zip";
                return repository.DeptDownloadApplicationPackage(instId, categoryId, userInfo.ProjectTypeIds, ApplicationStatus.ACCEPT, packageName);
            }
            catch (Exception e)
            {
                return ResponseWrapper.ExceptionResponse(e);
            }
        }

        /// <summary>
        /// 专家下载加密的PDF文件
        /// </summary>
        /// <param name="applicationId"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("api/applicationdoc/download/passwordpdf/{applicationId}")]
        public HttpResponseMessage DownloadPasswordPDFDoc( string applicationId )
        {
            try
            {
                var userInfo = UserHelper.GetCurrentUser();
                return repository.DownloadPasswordPDF(userInfo.UserId, applicationId, UserHelper.GetReviewPassword());
            }
            catch (Exception e)
            {
                return ResponseWrapper.ExceptionResponse(e);
            }
            
        }
        
        /// <summary>
        /// 上传文档
        /// </summary>
        /// <returns></returns>
        [Authorize(Roles="用户")]
        public HttpResponseMessage Post()
        {
            try
            {
                var doc = reciveFile();
                switch( doc.Type )
                {
                    case ApplicationDocType.BODY:
                        repository.AddApplicationBody(doc, privilege.UserEditApplication);
                        break;
                    case ApplicationDocType.ATTACHMENT:
                        repository.AddApplicationAttachment(doc, privilege.UserEditApplication);
                        break;
                    default:
                        throw new OtherException("文档类型错误");
                }
                return ResponseWrapper.SuccessResponse();
            }
            catch (Exception e)
            {
                return ResponseWrapper.ExceptionResponse(e);
            }
        }

        /// <summary>
        /// 删除申请书文档
        /// </summary>
        /// <param name="applicationDocId"></param>
        /// <returns></returns>
        [Authorize(Roles="用户")]
        public HttpResponseMessage Delete( int id )
        {
            try
            {
                ApplicationDocRepository.Delete(id, privilege.UserEditApplication);
                return ResponseWrapper.SuccessResponse();
            }
            catch (Exception e)
            {
                return ResponseWrapper.ExceptionResponse(e);
            }
            
        }

        private static ApplicationDoc reciveFile()
        {
            var applicationId = HttpContext.Current.Request.Params["ApplicationId"];
            int docType = int.Parse(HttpContext.Current.Request.Params["Type"]);

            //获取绝对路径
            string p = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, SystemConfig.UploadFilePathWin, applicationId);
            DirectoryInfo dir = Directory.CreateDirectory(p);

            //保存文件
            string saveName = FileHelper.Upload(HttpContext.Current, dir.FullName);

            //生成ApplicationDoc对象
            string path = "/" + Path.Combine(SystemConfig.UploadFilePathWeb, applicationId, saveName).Replace(@"\", @"/");

            return  new ApplicationDoc()
            {
                RelativeURL = path,
                Type = (ApplicationDocType)docType,
                Name = saveName,
                ApplicationId = applicationId
            };
        }
    }
}