using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using ASPODES.WebAPI.Common;
using ASPODES.DTO.AnnualTask;
using ASPODES.Model;
using ASPODES.WebAPI.Repository;

using AutoMapper;
using System.Web;
using System.IO;
using ASPODES.Common;
using ASPODES.WebAPI.Service;

namespace ASPODES.WebAPI.Controllers
{
    /// <summary>
    /// 年度任务文档控制器
    /// </summary>
    public class AnnualTaskDocController : ApiController
    {
        private IAnnualTaskDocRepository _repository;
        //private CreateNoticeService _noticeService;
        public AnnualTaskDocController(IAnnualTaskDocRepository repository)
            //CreateNoticeService noticeService)
        {
            _repository = repository;
            //_noticeService = noticeService;
        }

        /// <summary>
        /// 获取年度任务的文档列表
        /// </summary>
        /// <param name="annualTaskId">年度任务Id</param>
        /// <returns></returns>
        [Route("api/annualTaskDoc/{annualTaskId}/docs")]
        public HttpResponseMessage GetAnnualTaskDoc(int annualTaskId)
        {
            try
            {
                var docDTOs = _repository.GetAnnualTaskDocList()
                    .Where(atd => atd.AnnualTaskId == annualTaskId)
                    .Select(Mapper.Map<GetAnnualTaskDocDTO>)
                    .ToList();
                return ResponseWrapper.SuccessResponse(docDTOs);
            }
            catch (Exception e)
            {
                return ResponseWrapper.ExceptionResponse(e);
            }
        }
        /// <summary>
        /// 下载年度任务文档
        /// </summary>
        /// <param name="id">文档Id</param>
        /// <returns></returns>
        [HttpGet]
        public HttpResponseMessage Download(int id)
        {
            try
            {
                var doc = _repository.Get(id);
                if (doc == null) throw new NotFoundException();
                return FileHelper.Download(HttpContext.Current, doc.RelativeURL, doc.Name);
            }
            catch (Exception e)
            {
                return ResponseWrapper.ExceptionResponse(e);
            }
        }
        /// <summary>
        /// 下载年度任务文档
        /// </summary>
        /// <param name="annualTaskId">年度任务Id</param>
        /// <param name="docType">文档类型</param>
        /// <returns></returns>
        [HttpGet, Route("api/annualTaskDoc")]
        public HttpResponseMessage Download([FromUri] int annualTaskId, [FromUri]AnnualTaskDocType docType)
        {
            try
            {
                var doc = _repository.GetAnnualTaskDocList()
                     .FirstOrDefault(atd => atd.AnnualTaskId == annualTaskId && atd.Type == docType);

                if (doc == null) throw new NotFoundException();

                return FileHelper.Download(HttpContext.Current, doc.RelativeURL, doc.Name);
            }
            catch (Exception e)
            {
                return ResponseWrapper.ExceptionResponse(e);
            }
        }
        /// <summary>
        /// 上传年度任务文档
        /// </summary>
        /// <param name="annualTaskId">年度任务Id</param>
        /// <param name="docType">文档类型</param>
        /// <returns></returns>
        [HttpPost, Route("api/annualTaskDoc/upload")]
        public HttpResponseMessage Upload([FromUri] int annualTaskId, [FromUri]AnnualTaskDocType docType)
        {
            try
            {
                
                AnnualTaskDoc doc = reciveFile(annualTaskId, (AnnualTaskDocType)docType);

                if( docType == AnnualTaskDocType.BODY )
                {
                    doc = _repository.AddBody(doc);
                }
                else if (docType == AnnualTaskDocType.ANNUAL_REPORT )
                {
                    doc = _repository.AddAnnualReport(doc);

                    //用户上传年度报告
                    //通知打点:发给所在单位所有管理员
                    //_noticeService.AddNoticeList(
                    //    _noticeService.GetInstManagerListByAnnualTaskId(doc.AnnualTaskId.Value), 49);
                    //通知信打点:发给自己
                    //_noticeService.AddNotice(
                    //    _noticeService.GetUserIdByAnnualTaskId(doc.AnnualTaskId.Value), 32);

                }
                else
                {
                    doc = _repository.AddAttachment(doc);
                }
                return ResponseWrapper.SuccessResponse(doc);
            }
            catch (Exception e)
            {
                return ResponseWrapper.ExceptionResponse(e);
            }
        }
        /// <summary>
        /// 删除年度任务文档
        /// </summary>
        /// <param name="id">文档Id</param>
        /// <returns></returns>
        [HttpDelete, Route("api/annualTaskDoc/{id}")]
        public HttpResponseMessage Delete(int id)
        {
            try
            {
                _repository.Delete(id);
                return ResponseWrapper.SuccessResponse("删除成功");
            }
            catch (Exception e)
            {
                return ResponseWrapper.ExceptionResponse(e);
            }
        }

        private static AnnualTaskDoc reciveFile(int annualTaskId, AnnualTaskDocType docType)
        {
            var path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, SystemConfig.AnnualTaskPathWin, annualTaskId.ToString());
            //获取绝对路径
            DirectoryInfo dir = Directory.CreateDirectory(path);
            //保存文件
            string saveName = FileHelper.Upload(HttpContext.Current, dir.FullName);

            //生成AnnualTaskDoc对象
            string url = "/" + Path.Combine(SystemConfig.AnnualTaskPathWeb, annualTaskId.ToString(), saveName).Replace(@"\", @"/");

            AnnualTaskDoc doc = new AnnualTaskDoc()
            {
                RelativeURL = url,
                Type = docType,
                Name = saveName,
                AnnualTaskId = annualTaskId
            };
            return doc;
        }
    }
}
