using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using ASPODES.Database;
using ASPODES.Model;
using ASPODES.DTO.System;
using ASPODES.WebAPI.Common;
using ASPODES.WebAPI.Models;
using AutoMapper;
using System.IO;
using ASPODES.Common;
using ASPODES.Common.Util;
using ASPODES.WebAPI.Security;
using System.Net.Http.Headers;
namespace ASPODES.WebAPI.Repository
{
    /// <summary>
    /// 公告附件操作类
    /// </summary>
    public class AnnouncementAttachmentRepository
    {


        /// <summary>
        /// 获取公告附件的文档列表
        /// </summary>
        /// <param name="predicate">检索条件</param>
        /// <returns></returns>
        public List<GetAnnouncementAttachmentDTO> GetAnnouncementAttachmentList(Func<AnnouncementAttachment, bool> predicate)
        {
            using (var ctx = new AspodesDB())
            {
                var anattachDTOs = ctx.AnnouncementAttachments
                      .Where(predicate)
                      .Select(Mapper.Map<GetAnnouncementAttachmentDTO>)
                      .ToList();
                return anattachDTOs;
            }
        }


        /// <summary>
        /// 上传公告附件
        /// </summary>
        /// <param name="id">公告ID</param>
        /// <returns></returns>
        public GetAnnouncementAttachmentDTO UploadAnnouncementAttachment(int? announcementId = null)
        {
            string randomString = StringExtensions.GetRandomString(6, true);
            //获取绝对路径
            DirectoryInfo dir = Directory.CreateDirectory(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, SystemConfig.UploadFilePathWin, randomString));
            //保存文件
            string saveName = FileHelper.Upload(HttpContext.Current, dir.FullName);

            //生成AnnouncementAttachment对象
            string path = "/" + Path.Combine(SystemConfig.UploadFilePathWeb, randomString, saveName).Replace(@"\", @"/");

            AnnouncementAttachment aa = new AnnouncementAttachment()
            {
                AnnouncementId = announcementId,
                Name = saveName,
                RelativeURL = path
            };

            //保存到数据库
            using (var ctx = new AspodesDB())
            {
                //if (!ctx.Announcements.Any(a => a.AnnouncementId == id && a.Status == "C"))
                //{
                //    throw new NotFoundException("未找到公告");
                //}
                var announcementattachments = ctx.AnnouncementAttachments.Add(aa);
                ctx.SaveChanges();
                return Mapper.Map<GetAnnouncementAttachmentDTO>(announcementattachments);
            }
        }


        /// <summary>
        /// 下载公告附件
        /// </summary>
        /// <param name="announcementAttachmentId">附件ID</param>
        /// <returns></returns>
        public HttpResponseMessage DownloadAnnouncementAttachment(int? announcementAttachmentId)
        {
            try
            {
                AnnouncementAttachment attach = null;
                using (var ctx = new AspodesDB())
                {
                    attach = ctx.AnnouncementAttachments.FirstOrDefault(aa => aa.AnnouncementAttachmentId == announcementAttachmentId);
                }
                return attach == null ? ResponseWrapper.ExceptionResponse(new NotFoundException()) 
                    : FileHelper.Download(HttpContext.Current, attach.RelativeURL, attach.Name);
            }
            catch (Exception e)
            {
                return ResponseWrapper.ExceptionResponse(e);
            }
        }


        /// <summary>
        /// 删除公告附件
        /// </summary>
        /// <param name="announcementAttachmentId">公告附件ID</param>
        /// <returns></returns>
        public void Delete(int announcementAttachmentId)
        {
            AnnouncementAttachment aa = null;
            string path = "";
            using (var ctx = new AspodesDB())
            {

                aa = ctx.AnnouncementAttachments.FirstOrDefault(an => an.AnnouncementAttachmentId == announcementAttachmentId);
                if (null == aa)
                    throw new NotFoundException("未找到公告");

                //获取绝对路径
                string relative = aa.RelativeURL.Replace("~", "").Replace("/", @"\");
                if (relative.StartsWith(@"\"))
                {
                    relative = relative.Substring(1);
                }
                path = Path.Combine(HttpRuntime.AppDomainAppPath, relative);
                ctx.AnnouncementAttachments.Remove(aa);
                ctx.SaveChanges();
            }
        }


    }

}