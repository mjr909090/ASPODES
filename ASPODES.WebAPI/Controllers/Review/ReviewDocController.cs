using ASPODES.Common;
using ASPODES.WebAPI.Authorize;
using ASPODES.WebAPI.Common;
using ASPODES.WebAPI.Filter;
using ASPODES.WebAPI.Security;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Web;
using System.Web.Http;

namespace ASPODES.WebAPI.Controllers
{
    /// <summary>
    /// 评分说明文档处理
    /// </summary>
    [AspodesAuthorize]
    [ActionTrack]
    public class ReviewDocController : ApiController
    {
        /// <summary>
        /// 根据类别下载评分说明
        /// </summary>
        /// <returns></returns>
        [Authorize(Roles = "专家")]
        [HttpGet]
        [Route("api/reviewDoc/download")]
        public HttpResponseMessage Download(string Type)
        {
            try
            {
                string path = Path.Combine(SystemConfig.UploadFileReviewPathWeb, Type);
                return FileHelper.DownloadReview(HttpContext.Current, path);
            }
            catch (Exception e)
            {
                return ResponseWrapper.ExceptionResponse(new NotFoundException("文件还没有上传或其他参数错误"));
            }

        }

        /// <summary>
        /// 上传文档
        /// </summary>
        /// <returns></returns>
        [Authorize(Roles = "系统管理员")]
        [HttpPost]
        [Route("api/ReviewDoc")]
        public HttpResponseMessage UploadReview()
        {
            try
            {
                string fileName = reciveFile();
                return ResponseWrapper.SuccessResponse(fileName);
            }
            catch (Exception e)
            {
                return ResponseWrapper.ExceptionResponse(e);
            }
        }

        /// <summary>
        /// 获取评分说明文件列表
        /// </summary>
        /// <returns></returns>
        [Authorize(Roles = "系统管理员,专家")]
        [HttpGet]
        [Route("api/ReviewDocList")]
        public HttpResponseMessage GetReviewDocList()
        {
            try
            {
                Dictionary<string, string> dict = FileHelper.GetReviewDocList(HttpContext.Current, SystemConfig.UploadFileReviewPathWeb);
                return ResponseWrapper.SuccessResponse(dict);
            }
            catch (Exception e)
            {
                return ResponseWrapper.ExceptionResponse(e);
            }
        }

        private static string reciveFile()
        {
            var type = HttpContext.Current.Request.Params["Type"];

            //获取绝对路径
            string p = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, SystemConfig.UploadFileReview, type);
            DirectoryInfo dir = Directory.CreateDirectory(p);

            //保存文件
            string saveName = FileHelper.UploadReview(HttpContext.Current, dir.FullName);
            
            return saveName;
        }
    }
}