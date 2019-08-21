using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using System.Net;
using System.Net.Http;
using AutoMapper;

using System.IO;

using ASPODES.WebAPI.Common;
using ASPODES.Database;
using ASPODES.Model;
using ASPODES.DTO.Application;
using System.Net.Http.Headers;

using System.Web.Http;

using ASPODES.Common;
using ASPODES.Common.Util;
using ASPODES.WebAPI.Security;
using System.IO.Compression;
using ICSharpCode.SharpZipLib.Zip;
using ICSharpCode.SharpZipLib.Checksums;
using System;
using System.IO;

namespace ASPODES.WebAPI.Repository
{
    /// <summary>
    /// 申请书文档操作类
    /// </summary>
    public class ApplicationDocRepository
    {

        /// <summary>
        /// 获取申请书的文档列表
        /// </summary>
        /// <param name="applicationId">申请书ID</param>
        /// <returns></returns>
        public List<GetApplicationDocDTO> GetApplicationDocList(Func<ApplicationDoc, bool> predicate)
        {
            using (var ctx = new AspodesDB())
            {
                var docDTOs = ctx.ApplicationDocs
                    .Where(predicate)
                    .Select(Mapper.Map<GetApplicationDocDTO>)
                    .ToList();
                return docDTOs;
            }
        }

        public void AddApplicationBody(ApplicationDoc doc, Func<Application, CurrentUser, bool> privilege)
        {
            if (!PdfHelper.Check(doc.ApplicationId, doc.RelativeURL))
                throw new OtherException("申请书正文格式错误");

            //保存到数据库
            using (var ctx = new AspodesDB())
            {
                var application = ctx.Applications.FirstOrDefault(a => a.ApplicationId == doc.ApplicationId);
                if (null == application) throw new NotFoundException("未找到申请书");

                if (!privilege(application, UserHelper.GetCurrentUser()))
                {
                    throw new UnauthorizationException();
                }

                var pre = ctx.ApplicationDocs.Where(ad => ad.ApplicationId == doc.ApplicationId && ad.Type == doc.Type);

                //申请书正文只有一份，附件可以有多份
                ctx.ApplicationDocs.Add(doc);
                ctx.ApplicationDocs.RemoveRange(pre);
                ctx.SaveChanges();
            }
        }
        public void AddApplicationAttachment( ApplicationDoc doc, Func<Application, CurrentUser, bool> privilege)
        {
            using (var ctx = new AspodesDB())
            {
                var application = ctx.Applications.FirstOrDefault(a => a.ApplicationId == doc.ApplicationId);
                if (null == application) throw new NotFoundException("未找到申请书");

                if (!privilege(application, UserHelper.GetCurrentUser()))
                {
                    throw new UnauthorizationException();
                }
                ctx.ApplicationDocs.Add(doc);
                ctx.SaveChanges();
            }
        }

        /// <summary>
        /// 根据申请书文档的ID下载文档
        /// </summary>
        /// <param name="applicationDocId">文档ID</param>
        /// <returns>
        /// 成功，返回ResponseStatus.success和文件流
        /// 未找到文档，返回ResponseStatus.parameter_error
        /// 失败，返回ResponseStatus.unkown_error，和错误信息
        /// </returns>
        public HttpResponseMessage DownloadApplicationDoc(int? applicationDocId)
        {
            ApplicationDoc doc = null;
            using (var ctx = new AspodesDB())
            {
                doc = ctx.ApplicationDocs.FirstOrDefault(ad => ad.ApplicationDocId == applicationDocId);
            }
            if (doc == null) throw new NotFoundException("未找到需要下载的文档");
            return FileHelper.Download(HttpContext.Current, doc.RelativeURL, doc.Name);
        }


        /// <summary>
        /// 下载申请书的PDF文档
        /// </summary>
        /// <param name="applicationId"></param>
        /// <returns></returns>
        public HttpResponseMessage DownloadApplicationPDFDoc(string applicationId)
        {
            ApplicationDoc pdf = null;
            using (var ctx = new AspodesDB())
            {
                pdf = ctx.ApplicationDocs.FirstOrDefault(ad => ad.ApplicationId == applicationId && ad.Type == ApplicationDocType.PDF);
            }
            if (pdf == null) throw new NotFoundException("未找到需要下载的文档");
            return FileHelper.Download(HttpContext.Current, pdf.RelativeURL, pdf.Name);
        }

        /// <summary>
        /// 删除文档
        /// </summary>
        /// <param name="applicationDocId">文档ID</param>
        /// <returns></returns>
        public static void Delete(int applicationDocId, Func<Application, CurrentUser, bool> privilege)
        {

            using (var ctx = new AspodesDB())
            {
                var doc = ctx.ApplicationDocs.FirstOrDefault(ad => ad.ApplicationDocId == applicationDocId);
                if (null == doc)
                    throw new NotFoundException("未找到文档");

                var application = ctx.Applications.FirstOrDefault(a => a.ApplicationId == doc.ApplicationId);
                if (!privilege(application, UserHelper.GetCurrentUser()))
                {
                    throw new UnauthorizationException();
                }

                ctx.ApplicationDocs.Remove(doc);
                ctx.SaveChanges();
            }
        }

        /// <summary>
        /// 下载加密的申请书PDF文件
        /// </summary>
        /// <param name="expertId"></param>
        /// <param name="applicationId"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        public HttpResponseMessage DownloadPasswordPDF(string expertId, string applicationId, string password)
        {
            using (var ctx = new AspodesDB())
            {
                var application = ctx.Applications.FirstOrDefault(a => a.ApplicationId == applicationId && a.CurrentYear == SystemConfig.ApplicationStartYear && a.Status == ApplicationStatus.REVIEW);
                if (application == null)
                {
                    throw new OtherException("申请书不在专家评审期间");
                }

                if (!ctx.ReviewAssignments.Any(ra => !ra.Overdue.Value
                    && ra.ApplicationId == applicationId
                    && ra.ExpertId == expertId
                    && (ra.Status == ReviewAssignmentStatus.ALREADY_REVIEW || ra.Status == ReviewAssignmentStatus.WAITE_REVIEW)))
                {
                    throw new UnauthorizationException();
                }

                var pdf = ctx.ApplicationDocs.FirstOrDefault(ad => ad.ApplicationId == applicationId && ad.Type == ApplicationDocType.PDF);
                if (null == pdf) throw new NotFoundException();
                var pdfName = pdf.Name.Replace(".pdf", "");
                var pwdPdfName = pdf.ApplicationId + ".pdf";
                if (!PdfHelper.EncryptPdf(applicationId, pdfName, pwdPdfName, password))
                {
                    throw new UnknownException("加密的PDF生成失败");
                }

                var path = "/" + SystemConfig.UploadFilePathWeb + "/Temp/" + pwdPdfName;

                return FileHelper.Download(HttpContext.Current, path, application.ProjectName);
            }
        }

        /// <summary>
        /// 专家下载打包后的申请书的PDF文档
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="packageName"></param>
        /// <param name="predicate"></param>
        /// <returns></returns>
        public HttpResponseMessage ExpertDownloadApplicationPackage(string userId, string packageName)
        {
            ApplicationDoc[] PDFs = null;
            using (var ctx = new AspodesDB())
            {
                PDFs = (from ad in ctx.ApplicationDocs
                                   from ra in ctx.ReviewAssignments
                                   where !ra.Overdue.Value //未过期的
                                       && ra.ExpertId == userId //指派给他的
                                       && ra.Status == ReviewAssignmentStatus.WAITE_REVIEW //状态筛选
                                       && ra.ApplicationId == ad.ApplicationId
                                       && ad.Type == ApplicationDocType.PDF //格式为pdf
                                   select ad).ToArray();
            }
            string[] applicationDocs = new string[PDFs.Length];
            string password = UserHelper.GetReviewPassword();
            for (int i=0;i<PDFs.Length;i++)
            {
                var pdfName = PDFs[i].Name.Replace(".pdf", "");
                var pwdPdfName = PDFs[i].ApplicationId + ".pdf";
                if (!PdfHelper.EncryptPdf(PDFs[i].ApplicationId, pdfName, pwdPdfName, password))
                {
                    throw new UnknownException("加密的PDF生成失败");
                }
                var path = "/" + SystemConfig.UploadFilePathWeb + "/Temp/" + pwdPdfName;
                applicationDocs[i] = path;
            }
            UserHelper.ZipFiles(applicationDocs, SystemConfig.ZipFileAddress, packageName);
            return FileHelper.Download(HttpContext.Current, SystemConfig.ZipFileAddress + packageName, packageName);
        }

        /// <summary>
        /// 单位管理员和院管理员下载打包后的申请书的PDF文档
        /// </summary>
        /// <param name="instId"></param>
        /// <param name="projectTypeId"></param>
        /// <param name="status"></param>
        /// <param name="packageName"></param>
        /// <returns></returns>
        public HttpResponseMessage InstDownloadApplicationPackage(int instId, int projectTypeId, ApplicationStatus status, string packageName)
        {
            string[] applicationDocs = null;
            using (var ctx = new AspodesDB())
            {
                applicationDocs = (from ad in ctx.ApplicationDocs
                                   from a in ctx.Applications
                                   where a.CurrentYear == SystemConfig.ApplicationStartYear //本年度的
                                    && (a.InstituteId==0 || a.InstituteId == instId)//单位的
                                    && a.Status == status
                                    && (projectTypeId == 0 || a.ProjectTypeId == projectTypeId)
                                    && ad.ApplicationId == a.ApplicationId
                                    && ad.Type == ApplicationDocType.PDF //格式为pdf
                                   select ad.RelativeURL).ToArray();
            }
            if(applicationDocs == null || applicationDocs.Length == 0)
            {
                return ResponseWrapper.SuccessResponse("没有要导出的数据");
            }
            UserHelper.ZipFiles(applicationDocs, SystemConfig.ZipFileAddress, packageName);
            return FileHelper.Download(HttpContext.Current, SystemConfig.ZipFileAddress + packageName, packageName);
        }

        /// <summary>
        /// 院管理员和院管理员下载打包后的申请书的PDF文档
        /// </summary>
        /// <param name="instId"></param>
        /// <param name="projectTypeId"></param>
        /// <param name="ProjectTypeIds"></param>
        /// <param name="status"></param>
        /// <param name="packageName"></param>
        /// <returns></returns>
        public HttpResponseMessage DeptDownloadApplicationPackage(int instId, int projectTypeId, int?[] ProjectTypeIds, ApplicationStatus status, string packageName)
        {
            string[] applicationDocs = null;
            using (var ctx = new AspodesDB())
            {
                applicationDocs = (from ad in ctx.ApplicationDocs
                                   from a in ctx.Applications
                                   where a.CurrentYear == SystemConfig.ApplicationStartYear //本年度的
                                    && (instId == 0 || a.InstituteId == instId)//单位的
                                    && a.Status == status
                                    && ((projectTypeId == 0 && ProjectTypeIds.Contains(a.ProjectTypeId)) || (ProjectTypeIds.Contains(a.ProjectTypeId)&&(projectTypeId==a.ProjectTypeId)))
                                    && ad.ApplicationId == a.ApplicationId
                                    && ad.Type == ApplicationDocType.PDF //格式为pdf
                                   select ad.RelativeURL).ToArray();
            }
            if (applicationDocs == null || applicationDocs.Length == 0)
            {
                return ResponseWrapper.SuccessResponse("没有要导出的数据");
            }
            UserHelper.ZipFiles(applicationDocs, SystemConfig.ZipFileAddress, packageName);
            return FileHelper.Download(HttpContext.Current, SystemConfig.ZipFileAddress + packageName, packageName);
        }
    }

}