using ASPODES.Common;
using ASPODES.Common.Util;
using ASPODES.DTO.Project;
using ASPODES.Model;
using ASPODES.WebAPI.Common;
using ASPODES.WebAPI.Filter;
using ASPODES.WebAPI.Repository;
using ASPODES.WebAPI.Security;
using AutoMapper;
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
    /// 项目文档处理
    /// </summary>
    [ActionTrack]
    public class ProjectDocController : ApiController
    {
        private IProjectDocRepository _projectdocRepository;

        /// <summary>
        /// 项目文档处理构造函数
        /// </summary>
        public ProjectDocController(IProjectDocRepository projectdocrepository)
        {
            _projectdocRepository = projectdocrepository;
        }

        /// <summary>
        /// 获取项目的文档列表
        /// </summary>
        /// <param name="projectId">项目ID</param>
        /// <returns></returns>
        [HttpGet]
        [Route("api/projectdoc/{projectId}")]
        public HttpResponseMessage GetActiveProjectDoc(string projectId)
        {
            try
            {
                var projectDocDTO = _projectdocRepository.GetProjectDocList(projectId)
                    .Select(Mapper.Map<GetProjectDocDTO>)
                    .ToList();

                return ResponseWrapper.SuccessResponse(projectDocDTO);
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
        public HttpResponseMessage Post()
        {
            try
            {
                var projectId = HttpContext.Current.Request.Params["ProjectId"];
                ProjectDocType docType = (ProjectDocType)int.Parse(HttpContext.Current.Request.Params["Type"]);

                //获取绝对路径
                DirectoryInfo dir = Directory.CreateDirectory(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, SystemConfig.ProjectPathWin, projectId));
                //保存文件
                string saveName = FileHelper.Upload(HttpContext.Current, dir.FullName);

                //生成ProjectDoc对象
                string path = "/" + Path.Combine(SystemConfig.ProjectPathWeb, projectId, saveName).Replace(@"\", @"/");

                ProjectDoc doc = new ProjectDoc()
                {
                    RelativeURL = path,
                    Type = (ProjectDocType)docType,
                    Name = saveName,
                    ProjectId = projectId 
                };

                switch( docType )
                {
                    case ProjectDocType.FINISH_REPORT:
                        _projectdocRepository.UploadFinishReport(doc);
                        break;
                    case ProjectDocType.OTHER:
                        _projectdocRepository.UploadProjectDoc(doc);
                        break;
                    default:
                        throw new OtherException("文档类型错误");
                }

                //保存到数据库
                return ResponseWrapper.SuccessResponse();
            }
            catch (Exception e)
            {
                return ResponseWrapper.ExceptionResponse(e);
            }
        }

        /// <summary>
        /// 根据项目文档的ID下载文档
        /// </summary>
        /// <param name="projectDocId">文档ID</param>
        /// <returns></returns>
        [HttpGet]
        [Route("api/projectdoc/download/{projectDocId}")]
        public HttpResponseMessage Download(int projectDocId)
        {
            try
            {
                return _projectdocRepository.DownloadProjectDoc(projectDocId);
            }
            catch (Exception e)
            {
                return ResponseWrapper.ExceptionResponse(e);
            }

        }

        /// <summary>
        /// 根据项目ID下载结题报告
        /// </summary>
        /// <param name="projectId">项目ID</param>
        /// <returns></returns>
        [HttpGet]
        [Route("api/projectdoc/download/endreport/{projectId}")]
        public HttpResponseMessage DownloadEndReport(string projectId)
        {
            try
            {
                var projectDocId = _projectdocRepository.GetProjectDocList(projectId)
                                  .Where(pd => pd.Type == ProjectDocType.FINISH_REPORT)
                                  .Select(pd =>pd.ProjectDocId)
                                  .FirstOrDefault();
                return _projectdocRepository.DownloadProjectDoc(projectDocId);
            }
            catch (Exception e)
            {
                return ResponseWrapper.ExceptionResponse(e);
            }

        }

        /// <summary>
        /// 删除项目文档
        /// </summary>
        /// <param name="projectDocId">文档id</param>
        /// <returns></returns>
        [Route("api/projectdoc/delete/{projectDocId}")]
        public HttpResponseMessage Delete(int projectDocId)
        {
            try
            {
                //删除返回文档路径
                string relative = _projectdocRepository.Delete(projectDocId).RelativeURL.Replace("~", "").Replace("/", @"\");
                //获取绝对路径
                if (relative.StartsWith(@"\"))
                {
                    relative = relative.Substring(1);
                }
                String path = Path.Combine(HttpRuntime.AppDomainAppPath, relative);
                File.Delete(path);
                return ResponseWrapper.SuccessResponse();
            }
            catch (Exception e)
            {
                return ResponseWrapper.ExceptionResponse(e);
            }

        }
    }
}