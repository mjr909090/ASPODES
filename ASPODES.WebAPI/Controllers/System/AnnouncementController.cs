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
using ASPODES.WebAPI.Service;

namespace ASPODES.WebAPI.Controllers
{
    /// <summary>
    /// 公告
    /// </summary>
    public class AnnouncementController : ApiController
    {
        private AnnouncementRepository repository = new AnnouncementRepository();
        //private CreateNoticeService _noticeService;

        //public AnnouncementController(CreateNoticeService noticeService)
        //{
        //    this._noticeService = noticeService;
        //}
        /// <summary>
        /// 用户获取单位的公告的列表
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("api/Announcement/user/inst/{page}")]
        public HttpResponseMessage GetUserInstAnnoucementList(int page)
        {
            try
            {
                var parameters = Request.GetQueryNameValuePairs();
                DateTime startTime = DateTime.Now;
                if (!DateTime.TryParse(parameters.FirstOrDefault(p => p.Key == "StartTime").Value, out startTime))
                {
                    startTime = DateTime.Parse("2017-01-01");
                }
                DateTime endTime = DateTime.Now;
                if (!DateTime.TryParse(parameters.FirstOrDefault(p => p.Key == "EndTime").Value, out endTime))
                {
                    endTime = DateTime.Now;
                }

                var user = UserHelper.GetCurrentUser();
                Func<Announcement, bool> predicate = a => (a.PublishDate >= startTime)
                    && (a.PublishDate <= endTime)
                    && (a.InstituteId == user.InstId)
                    && a.Status == "C"
                    && (a.Type == "I");
                return ResponseWrapper.SuccessResponse(repository.GetAnnoucementList(predicate, page));
            }
            catch (Exception e)
            {
                return ResponseWrapper.ExceptionResponse(e);
            }
        }

        /// <summary>
        /// 用户获取院的公告的列表
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("api/Announcement/user/depart/{page}")]
        public HttpResponseMessage GetUserDepartAnnoucementList(int page)
        {
            try
            {
                var parameters = Request.GetQueryNameValuePairs();
                DateTime startTime = DateTime.Now;
                if (!DateTime.TryParse(parameters.FirstOrDefault(p => p.Key == "StartTime").Value, out startTime))
                {
                    startTime = DateTime.Parse("2017-01-01");
                }
                DateTime endTime = DateTime.Now;
                if (!DateTime.TryParse(parameters.FirstOrDefault(p => p.Key == "EndTime").Value, out endTime))
                {
                    endTime = DateTime.Now;
                }

                var user = UserHelper.GetCurrentUser();
                Func<Announcement, bool> predicate = a => (a.PublishDate >= startTime)
                    && (a.PublishDate <= endTime)
                    && a.Status == "C"
                    && (a.Type == "D");
                return ResponseWrapper.SuccessResponse(repository.GetAnnoucementList(predicate, page));
            }
            catch (Exception e)
            {
                return ResponseWrapper.ExceptionResponse(e);
            }


        }

        /// <summary>
        /// 单位管理员获取公告的列表
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Authorize(Roles = "单位管理员"), Route("api/Announcement/inst/{page}")]
        public HttpResponseMessage GetInstAnnoucementList(int page)
        {
            try
            {
                var parameters = Request.GetQueryNameValuePairs();
                DateTime startTime = DateTime.Now;
                if (!DateTime.TryParse(parameters.FirstOrDefault(p => p.Key == "StartTime").Value, out startTime))
                {
                    startTime = DateTime.Parse("2017-01-01");
                }
                DateTime endTime = DateTime.Now;
                if (!DateTime.TryParse(parameters.FirstOrDefault(p => p.Key == "EndTime").Value, out endTime))
                {
                    endTime = DateTime.Now;
                }

                var user = UserHelper.GetCurrentUser();
                Func<Announcement, bool> predicate = a => (a.PublishDate >= startTime)
                    && (a.PublishDate <= endTime)
                    && (a.InstituteId == user.InstId)
                    && a.Status == "C"
                    && (a.Type == "I");
                return ResponseWrapper.SuccessResponse(repository.GetAnnoucementList(predicate, page));
            }
            catch (Exception e)
            {
                return ResponseWrapper.ExceptionResponse(e);
            }


        }

        /// <summary>
        /// 院管理员获取公告的列表
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Authorize(Roles = "院管理员"), Route("api/Announcement/depart/{page}")]
        public HttpResponseMessage GetDepartAnnoucementList(int page)
        {
            try
            {
                var parameters = Request.GetQueryNameValuePairs();
                DateTime startTime = DateTime.Now;
                if (!DateTime.TryParse(parameters.FirstOrDefault(p => p.Key == "StartTime").Value, out startTime))
                {
                    startTime = DateTime.Parse("2017-01-01");
                }
                DateTime endTime = DateTime.Now;
                if (!DateTime.TryParse(parameters.FirstOrDefault(p => p.Key == "EndTime").Value, out endTime))
                {
                    endTime = DateTime.Now;
                }

                var user = UserHelper.GetCurrentUser();
                Func<Announcement, bool> predicate = a => (a.PublishDate >= startTime)
                    && (a.PublishDate <= endTime)
                    && (a.InstituteId == user.InstId)
                    && a.Status == "C"
                    && (a.Type == "D");
                return ResponseWrapper.SuccessResponse(repository.GetAnnoucementList(predicate, page));
            }
            catch (Exception e)
            {
                return ResponseWrapper.ExceptionResponse(e);
            }


        }

        /// <summary>
        /// 获取单位的公告详情
        /// </summary>
        /// <returns></returns>
        public HttpResponseMessage Get(int Id)
        {
            try
            {
                return ResponseWrapper.SuccessResponse(repository.GetAnnoucement(Id));
            }
            catch (Exception e)
            {
                return ResponseWrapper.ExceptionResponse(e);
            }
        }


        /// <summary>
        /// 单位管理员添加公告
        /// </summary>
        /// <param name="dto">公告信息</param>
        /// <returns></returns>
        [HttpPost]
        [Authorize(Roles = "单位管理员"), Route("api/Announcement/inst")]
        public HttpResponseMessage AddInstAnnouncement(AddAnnouncementDTO dto)
        {
            try
            {
                dto.Type = "I";
                var result = repository.AddAnnouncement(dto);
                return ResponseWrapper.SuccessResponse(result);
            }
            catch (Exception e)
            {
                return ResponseWrapper.ExceptionResponse(e);
            }

        }

        /// <summary>
        /// 院管理员添加公告
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpPost]
        [Authorize(Roles = "院管理员"), Route("api/Announcement/depart")]
        public HttpResponseMessage AddDepartAnnouncement(AddAnnouncementDTO dto)
        {
            try
            {
                dto.Type = "D";
                var result = repository.AddAnnouncement(dto);
                return ResponseWrapper.SuccessResponse(result);
            }
            catch (Exception e)
            {
                return ResponseWrapper.ExceptionResponse(e);
            }
        }

        /// <summary>
        /// 单位管理员更改公告信息
        /// </summary>
        /// <param name="announceDTO">更新的公告信息</param>
        /// <returns></returns>
        [HttpPut]
        [Authorize(Roles = "单位管理员"), Route("api/Announcement/inst")]
        public HttpResponseMessage UpdateInstAnnouncement(AddAnnouncementDTO announceDTO)
        {
            try
            {
                var user = UserHelper.GetCurrentUser();
                return ResponseWrapper.SuccessResponse(repository.UpdateAnnouncement(announceDTO, a => a.Type == "I" && a.InstituteId == user.InstId));
            }
            catch (Exception e)
            {
                return ResponseWrapper.ExceptionResponse(e);
            }

        }
        /// <summary>
        /// 院管理员更改公告信息
        /// </summary>
        /// <param name="announceDTO">更新的公告信息</param>
        /// <returns></returns>
        [HttpPut]
        [Authorize(Roles = "院管理员"), Route("api/Announcement/depart")]
        public HttpResponseMessage UpdateDepartAnnouncement(AddAnnouncementDTO announceDTO)
        {
            try
            {
                return ResponseWrapper.SuccessResponse(repository.UpdateAnnouncement(announceDTO, a => a.Type == "D"));
            }
            catch (Exception e)
            {
                return ResponseWrapper.ExceptionResponse(e);
            }
        }

        /// <summary>
        /// 单位管理员删除公告
        /// </summary>
        /// <param name="AnnouncementId"></param>
        /// <returns></returns>
        [HttpDelete]
        [Authorize(Roles = "单位管理员"), Route("api/Announcement/inst/{AnnouncementId}")]
        public HttpResponseMessage DeleteInstAnnouncement(int AnnouncementId)
        {
            try
            {
                var user = UserHelper.GetCurrentUser();
                repository.DeleteAnnouncement(AnnouncementId, a => a.Type == "I" && a.InstituteId == user.InstId);
                return ResponseWrapper.SuccessResponse("删除成功");
            }
            catch (Exception e)
            {
                return ResponseWrapper.ExceptionResponse(e);
            }
        }

        /// <summary>
        /// 院管理员删除公告
        /// </summary>
        /// <param name="AnnouncementId"></param>
        /// <returns></returns>
        [HttpDelete]
        [Authorize(Roles = "院管理员"), Route("api/Announcement/depart/{AnnouncementId}")]
        public HttpResponseMessage DeleteDepartAnnouncement(int AnnouncementId)
        {
            try
            {
                var user = UserHelper.GetCurrentUser();
                repository.DeleteAnnouncement(AnnouncementId, a => a.Type == "D" && a.InstituteId == user.InstId);
                return ResponseWrapper.SuccessResponse("删除成功");
            }
            catch (Exception e)
            {
                return ResponseWrapper.ExceptionResponse(e);
            }
        }
    }
}
