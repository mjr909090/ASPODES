using ASPODES.DTO;
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

namespace ASPODES.WebAPI.Repository
{
    /// <summary>
    /// 公告
    /// </summary>
    public class AnnouncementRepository
    {
        /// <summary>
        /// 获取公告详情
        /// </summary>
        /// <param name="announcementId">公告Id</param>
        /// <returns></returns>
        public GetAnnouncementDTO GetAnnoucement(int announcementId)
        {
            using (var ctx = new AspodesDB())
            {
                var annoucement = ctx.Announcements
                    .Include("Publisher").Include("AnnouncementAttachments")
                    .Select( Mapper.Map<GetAnnouncementDTO> )
                    .FirstOrDefault(a => a.AnnouncementId == announcementId);

                if (annoucement == null)
                {
                    throw new NotFoundException("未找到公告");
                }
                /*
                announcementDTO = Mapper.Map<GetAnnouncementDTO>(annoucement);
                announcementDTO.PublisherName = ctx.Users.FirstOrDefault(u => u.UserId == announcementDTO.PublisherId).Name;
                announcementDTO.Attachments = ctx.AnnouncementAttachments
                    .Where(aa => aa.AnnouncementId.Value == announcementId)
                    .Select(Mapper.Map<GetAnnouncementAttachmentDTO>)
                    .ToList();
                 * */
                return annoucement;
            }

        }

        /// <summary>
        /// 获取公告列表
        /// </summary>
        /// <param name="predicate">选择条件</param>
        /// <returns></returns>
        public PagingListDTO<GetAnnouncementComboDTO> GetAnnoucementList(Func<Announcement, bool> predicate, int page)
        {
            PagingListDTO<GetAnnouncementComboDTO> pagingList = new PagingListDTO<GetAnnouncementComboDTO>();
            page = page <= 0 ? 1 : page;
            using (var ctx = new AspodesDB())
            {
                pagingList.TotalNum = ctx.Announcements.Where(predicate).Count();
                pagingList.TotalPageNum = (pagingList.TotalNum + SystemConfig.AnnouncementPageCount - 1) / SystemConfig.AnnouncementPageCount;
                if (pagingList.TotalPageNum <= 0) pagingList.TotalPageNum = 1;
                pagingList.NowPage = page;

                pagingList.ItemDTOs = ctx.Announcements
                    .Where(predicate)
                    .OrderByDescending(a => a.PublishDate)
                    .Skip((page - 1) * SystemConfig.AnnouncementPageCount)
                    .Take(SystemConfig.AnnouncementPageCount)
                    .Select(Mapper.Map<GetAnnouncementComboDTO>)
                    .ToList();

                pagingList.NowNum = pagingList.ItemDTOs.Count();

            }
            return pagingList;
        }

        /// <summary>
        /// 添加公告、修改公告
        /// </summary>
        /// <param name="announceDTO">公告信息</param>
        /// <returns>
        /// 添加成功，返回ResponseStatus.success
        /// 失败，返回ResponseStatus.unknown_error和错误信息
        /// </returns>
        public GetAnnouncementDTO AddAnnouncement(AddAnnouncementDTO announceDTO)
        {
            Announcement saveValue = null;
            using (var ctx = new AspodesDB())
            {
                var oldValue = ctx.Announcements.FirstOrDefault(a => a.AnnouncementId == announceDTO.AnnouncementId);

                if (oldValue == null)
                {
                    Announcement announce = Mapper.Map<Announcement>(announceDTO);
                    var user = UserHelper.GetCurrentUser();
                    announce.InstituteId = user.InstId;
                    announce.PublisherId = user.UserId;
                    saveValue = ctx.Announcements.Add(announce);
                    ctx.SaveChanges();
                    foreach (int attachmentId in announceDTO.Attachments)
                    {
                        ctx.AnnouncementAttachments.Find(attachmentId).AnnouncementId = saveValue.AnnouncementId;
                    }
                }
                else
                {
                    saveValue = Mapper.Map<Announcement>(announceDTO);
                    //不允许在Update时更改的值
                    saveValue.Status = oldValue.Status;
                    saveValue.AnnouncementId = oldValue.AnnouncementId;
                    saveValue.InstituteId = oldValue.InstituteId;
                    saveValue.PublisherId = oldValue.PublisherId;
                    saveValue.PublishDate = oldValue.PublishDate;
                    ctx.Entry(oldValue).CurrentValues.SetValues(saveValue);
                    ctx.SaveChanges();
                    foreach (int attachmentId in announceDTO.Attachments)
                    {
                        ctx.AnnouncementAttachments.Find(attachmentId).AnnouncementId = saveValue.AnnouncementId;
                    }
                }
                ctx.SaveChanges();
                return Mapper.Map<GetAnnouncementDTO>(saveValue);
            }
        }

        /// <summary>
        /// 更改公告信息
        /// </summary>
        /// <param name="announceDTO">更新的公告信息</param>
        /// <returns></returns>
        public GetAnnouncementDTO UpdateAnnouncement(AddAnnouncementDTO announceDTO, Func<Announcement, bool> privilege)
        {
            var newValue = Mapper.Map<Announcement>(announceDTO);
            using (var ctx = new AspodesDB())
            {
                var oldValue = ctx.Announcements.FirstOrDefault(a => a.AnnouncementId == newValue.AnnouncementId);
                if (oldValue == null)
                    throw new NotFoundException("未找到公告");
                if (!privilege(oldValue))
                    throw new UnauthorizedAccessException();


                //不允许在Update时更改的值
                newValue.Status = oldValue.Status;
                newValue.InstituteId = oldValue.InstituteId;
                newValue.PublisherId = oldValue.PublisherId;
                newValue.PublishDate = oldValue.PublishDate;
                ctx.Entry(oldValue).CurrentValues.SetValues(newValue);
                ctx.SaveChanges();
                return Mapper.Map<GetAnnouncementDTO>(newValue);
            }
        }

        /// <summary>
        /// 删除公告
        /// </summary>
        /// <param name="id">公告ID</param>
        /// <param name="privilege">权限</param>
        /// <returns></returns>
        public void DeleteAnnouncement(int id, Func<Announcement, bool> privilege)
        {
            using (var ctx = new AspodesDB())
            {
                var announcement = ctx.Announcements.FirstOrDefault(i => i.AnnouncementId == id);
                if (null == announcement) throw new NotFoundException("未找到公告");
                if (!privilege(announcement))
                    throw new UnauthorizedAccessException();
                announcement.Status = "D";
                ctx.SaveChanges();
            }
        }

        
    }
}
