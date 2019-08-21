using ASPODES.Common;
using ASPODES.Database;
using ASPODES.DTO.Project;
using ASPODES.Model;
using ASPODES.WebAPI.Common;
using ASPODES.WebAPI.Security;
using AutoMapper;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Net.Http;
using ASPODES.Common.Util;

namespace ASPODES.WebAPI.Repository
{
    /// <summary>
    /// 项目文档操作类
    /// </summary>
    public class ProjectDocRepository : IProjectDocRepository
    {
        private AspodesDB _context;
        /// <summary>
        /// 项目文档操作类构造函数
        /// </summary>
        public ProjectDocRepository(AspodesDB context)
        {
            _context = context;
        }

        /// <summary>
        /// 根据项目文档的ID下载文档
        /// </summary>
        /// <param name="projectDocId">文档ID</param>
        /// <returns>
        /// 成功，返回ResponseStatus.success和文件流
        /// 未找到文档，返回ResponseStatus.parameter_error
        /// 失败，返回ResponseStatus.unkown_error，和错误信息
        /// </returns>
        public HttpResponseMessage DownloadProjectDoc(int? projectDocId)
        {
            var doc = _context.ProjectDocs.FirstOrDefault(ad => ad.ProjectDocId == projectDocId);
            if (doc == null) throw new NotFoundException("未找到文档");
            return FileHelper.Download(HttpContext.Current, doc.RelativeURL, doc.Name);
        }

        /// <summary>
        /// 获取项目文档列表
        /// </summary>
        /// <param name="projectId">项目ID</param>
        /// <returns></returns>
        public IQueryable<ProjectDoc> GetProjectDocList(String projectId)
        {
            var query = _context.ProjectDocs
                        .Where(p => p.ProjectId == projectId);
            return query;
        }

        /// <summary>
        /// 上传文档
        /// </summary>
        /// <param name="privilege">权限验证</param>
        /// <returns>
        /// 成功，返回ResponseStatus.success
        /// 失败，返回ResponseStatus.unkown_error，和错误信息
        /// </returns>
        public void UploadProjectDoc(ProjectDoc doc)
        {
            _context.ProjectDocs.Add(doc);
            _context.SaveChanges();

        }

        /// <summary>
        /// 删除文档
        /// </summary>
        /// <param name="projectDocId">文档ID</param>
        /// <returns></returns>
        public ProjectDoc Delete(int projectDocId)
        {
            var doc = _context.ProjectDocs.FirstOrDefault(ad => ad.ProjectDocId == projectDocId);
            /*if (null == doc)
                throw new NotFoundException("未找到文档");
            var project = _context.Projects.FirstOrDefault(a => a.ProjectId == doc.ProjectId);

            if (!Privilege.UserEditProject(project, UserHelper.GetCurrentUser()))
            {
                throw new UnauthorizationException();
            }*/

            
            var removedoc = _context.ProjectDocs.Remove(doc);
            _context.SaveChanges();
            return removedoc;
        }

        public ProjectDoc UploadFinishReport(ProjectDoc doc)
        {
            var project = _context.Projects.FirstOrDefault( p=>p.ProjectId == doc.ProjectId );
            if (null == project) throw new NotFoundException("未找到项目");

            if (!project.Terminable()) throw new OtherException("项目状态不允许上传该文档");

            var pdfName = DateTime.Now.ToFileTime() + project.Name;
            if( !PdfHelper.ConvertProjectPdf( project.ProjectId, doc.Name, pdfName ))
            {
                throw new OtherException("生成PDF文档失败");
            }

            doc.RelativeURL = doc.RelativeURL.Replace(doc.Name, pdfName + ".pdf");
            doc.Name = pdfName + ".pdf";

            var pre = _context.ProjectDocs.Where(pd => pd.Type == ProjectDocType.FINISH_REPORT && pd.ProjectId == project.ProjectId);
            _context.ProjectDocs.RemoveRange(pre);

            _context.ProjectDocs.Add(doc);
            //project.Status = ProjectStatus.INST_REVIEW;

            _context.SaveChanges();

            return doc;
        }
    }
}