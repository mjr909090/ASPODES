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
using ASPODES.DTO.System;
using ASPODES.WebAPI.Common;
using ASPODES.WebAPI.Models;
using AutoMapper;
using ASPODES.Model;
using System.Data.SqlClient;

namespace ASPODES.WebAPI.Repository
{
    public class NoticeRepository
    {
        private AspodesDB _context;
        public NoticeRepository(AspodesDB context)
        {
            this._context = context;
        }

        /// <summary>
        /// 添加通知列表List
        /// </summary>
        /// <param name="notices"></param>
        public void SaveNoticeList(List<Notice> notices)
        {
            if (notices == null) return;
            foreach (Notice n in notices)
            {
                _context.Notices.Add(n);
            }
            _context.SaveChanges();
        }

        /// <summary>
        /// 根据收件人获得通知
        /// </summary>
        /// <param name="id">user表的id</param>
        public IOrderedQueryable<Notice> GetNoticeByReceive(string id)
        {
            return _context.Notices.Where(n => n.ReceiveId == id).OrderBy(n => n.Read);
        }

        /// <summary>
        /// 根据通知Id获得通知
        /// </summary>
        /// <param name="id">user表的id</param>
        public Notice GetNoticeById(int? id)
        {
            return _context.Notices.FirstOrDefault(n => n.NoticeID == id);
        }

        /// <summary>
        /// 添加单个通知
        /// </summary>
        /// <param name="notice"></param>
        public void SaveNotice(Notice notice)
        {
            _context.Notices.Add(notice);
            _context.SaveChanges();
        }

        ///// <summary>
        ///// 更新通知为已读
        ///// </summary>
        ///// <param name="notice"></param>
        //public Notice UpdateNotice(Notice notice)
        //{
        //    var invalid = _context.Notices.FirstOrDefault(n => n.NoticeID == notice.NoticeID);
        //    _context.Entry<Notice>(invalid).CurrentValues.SetValues(notice);
        //    _context.SaveChanges();
        //    return invalid;
        //}

        /// <summary>
        /// 获取普通户的通知
        /// </summary>
        /// <param name="leaderId"></param>
        /// <returns></returns>
        public SpNotice[] GetUserNotice(int leaderId)
        {
            SqlParameter[] param = new SqlParameter[]
            {
                new SqlParameter("@leaderId", leaderId),
                new SqlParameter("@currentYear", SystemConfig.ApplicationStartYear)
            };
            //if(important)
            //{
            //    return _context.Database.SqlQuery<SpNotice>
            //    ("dbo.spInstImportantNotice @instituteId", param).ToArray();
            //}
            return _context.Database.SqlQuery<SpNotice>
            ("dbo.spUserNotice @leaderId,@currentYear", param).ToArray();
        }

        /// <summary>
        /// 获取单位管理员的通知信息
        /// </summary>
        /// <param name="instituteId">单位ID</param>
        /// <param name="important">是否是重要的通知信息</param>
        /// <returns></returns>
        public SpNotice[] GetInstNotice(int instituteId, bool important=false)
        {
            SqlParameter[] param = new SqlParameter[]
            {
                new SqlParameter("@instituteId", instituteId),
                new SqlParameter("@currentYear", SystemConfig.ApplicationStartYear)
            };
            //if(important)
            //{
            //    return _context.Database.SqlQuery<SpNotice>
            //    ("dbo.spInstImportantNotice @instituteId", param).ToArray();
            //}
                return _context.Database.SqlQuery<SpNotice>
                ("dbo.spInstNotice @instituteId,@currentYear", param).ToArray();
        }

        /// <summary>
        /// 获取院管理员的通知信息
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public SpNotice[] GetDeptNotice(string userId)
        {
            SqlParameter[] param = new SqlParameter[]
            {
                new SqlParameter("@userId", userId),
                new SqlParameter("@expertNumber", SystemConfig.ExpertReviewAmount),
                new SqlParameter("@currentYear", SystemConfig.ApplicationStartYear)
            };

            var statistics = _context.Database.SqlQuery<SpNotice>
                ("dbo.spDeptNotice @userId,@expertNumber,@currentYear", param);
            return statistics.ToArray();
        }

        /// <summary>
        /// 获取专家的通知信息
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public SpNotice[] GetExpertNotice(string userId)
        {
            SqlParameter[] param = new SqlParameter[]
            {
                new SqlParameter("@expertId", userId),
                new SqlParameter("@currentYear", SystemConfig.ApplicationStartYear)
            };

            var statistics = _context.Database.SqlQuery<SpNotice>
                ("dbo.spExpertNotice @expertId,@currentYear", param);
            return statistics.ToArray();
        }

        /// <summary>
        /// 获取通知
        /// </summary>
        /// <param name="id">通知的id</param>
        /// <returns></returns>
        //public GetNoticeDTO GetNotice(int id)
        //{
        //    GetNoticeDTO noticeDTO = new GetNoticeDTO();
        //    var userid = HttpContext.Current.User.Identity.Name;

        //    using (var db = new AspodesDB())
        //    {
        //        noticeDTO.NoticeTotalNum = db.Notices.Count(c => c.ReceiveId == userid);
        //        noticeDTO.NoticePage = id;
        //        noticeDTO.NoticePageNum = (int)Math.Ceiling((double)noticeDTO.NoticeTotalNum /
        //                                               (double)SystemSettings.NoticeCount());
        //        var noticelist = db.Notices.Where(c => c.ReceiveId == userid).OrderByDescending(c => c.SendTime).Skip((id - 1) * SystemSettings.NoticeCount()).Take(SystemSettings.NoticeCount());

        //        noticeDTO.NoticeList = new List<NoticeDTO>();
        //        foreach (var temp in noticelist)
        //        {
        //            noticeDTO.NoticeList.Add(Mapper.Map<NoticeDTO>(temp));
        //            temp.Read = true;
        //        }
        //        db.SaveChanges();


        //        noticeDTO.NoticeNum = noticeDTO.NoticeList.Count;
        //        return noticeDTO;
        //    }
        //}


        ///// <summary>
        ///// 获取未读消息总数及最近五条消息
        ///// </summary>
        ///// <returns></returns>
        //public NoticeDropdownListDTO GetDropDownNotice()
        //{
        //    NoticeDropdownListDTO NoticeList = new NoticeDropdownListDTO();
        //    var userid = HttpContext.Current.User.Identity.Name;

        //    using (var db = new AspodesDB())
        //    {
        //        NoticeList.UnReadNum = db.Notices.Count(c => c.ReceiveId == userid && c.Read == false);

        //        NoticeList.Notices = db.Notices.Where(c => c.ReceiveId == userid).OrderByDescending(c => c.SendTime).Take(5).Select(Mapper.Map<NoticeDTO>).ToList();
        //        return NoticeList;
        //    }

        //}
    }
}
