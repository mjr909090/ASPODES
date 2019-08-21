using ASPODES.DTO;
using ASPODES.DTO.System;
using ASPODES.Model;
using ASPODES.WebAPI.Repository;
using ASPODES.WebAPI.Util;
using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ASPODES.WebAPI.Service
{
    /// <summary>
    /// 通知操作服务
    /// </summary>
    public class OperateNoticeService
    {
        private NoticeRepository _noticeRepository;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="noticeRepository"></param>
        public OperateNoticeService(NoticeRepository noticeRepository)
        {
            this._noticeRepository = noticeRepository;
        }

        /// <summary>
        /// 获得通知
        /// </summary>
        /// <param name="page">返回页数</param>
        ///// <param name="read">是否已读</param>
        ///// <param name="userType">用户类型</param>
        public PagingListDTO<NoticeDTO> GetNotice(int page)
        {
            List<NoticeDTO> list = new List<NoticeDTO>();
            string[] roles = UserHelper.GetCurrentUser().Roles;
            int instituteId = UserHelper.GetCurrentUser().InstId;
            string userId = UserHelper.GetCurrentUser().UserId;
            int personId = UserHelper.GetCurrentUser().PersonId;

            //添加普通用户的通知
            list.AddRange(Mapper.Map<List<NoticeDTO>>(_noticeRepository.GetUserNotice(personId)));

            if (roles.Contains("单位管理员"))
            {
                list.AddRange(Mapper.Map<List<NoticeDTO>>(_noticeRepository.GetInstNotice(instituteId)));
            }
            if (roles.Contains("院管理员"))
            {
                list.AddRange(Mapper.Map<List<NoticeDTO>>(_noticeRepository.GetDeptNotice(userId)));
            }
            if (roles.Contains("专家"))
            {
                list.AddRange(Mapper.Map<List<NoticeDTO>>(_noticeRepository.GetExpertNotice(userId)));
            }
            var paging = PagingHelper.PagingWrapper(list, page, SystemConfig.NoticePageCount);
            return paging;
            ////不对通知进行角色分类
            //if(userType == 0)
            //{
            //    if (read == 0)//查询未读的通知,小铃铛里面的通知也用这一块数据
            //    {
            //        if (roles.Contains("单位管理员"))
            //        {
            //            list.AddRange(Mapper.Map<List<NoticeDTO>>(_noticeRepository.GetInstNotice(instituteId, true)));
            //        }
            //        if (roles.Contains("院管理员"))
            //        {
            //            list.AddRange(Mapper.Map<List<NoticeDTO>>(_noticeRepository.GetDeptNotice(userId)));
            //        }
            //        if (roles.Contains("专家"))
            //        {
            //            list.AddRange(Mapper.Map<List<NoticeDTO>>(_noticeRepository.GetExpertNotice(userId)));
            //        }
            //        list.AddRange(_noticeRepository.GetNoticeByReceive(userId).Where(n => !n.Read).Select(Mapper.Map<NoticeDTO>).ToArray());
            //    }
            //    else if (read == 1)//查询已读的通知
            //    {
            //        list.AddRange(_noticeRepository.GetNoticeByReceive(userId).Where(n => n.Read).Select(Mapper.Map<NoticeDTO>).ToArray());
            //    }
            //    else//查询所有的通知
            //    {
            //        if (userType == 2 && roles.Contains("单位管理员"))
            //        {
            //            list.AddRange(Mapper.Map<List<NoticeDTO>>(_noticeRepository.GetInstNotice(instituteId)));
            //            list.AddRange(_noticeRepository.GetNoticeByReceive(userId).Where(n => n.NoticeTemplate.ReceiverType== ReceiverType.InstManager).Select(Mapper.Map<NoticeDTO>).ToArray());
            //        }
            //        if (userType == 3 && roles.Contains("院管理员"))
            //        {
            //            list.AddRange(Mapper.Map<List<NoticeDTO>>(_noticeRepository.GetDeptNotice(userId)));
            //            list.AddRange(_noticeRepository.GetNoticeByReceive(userId).Where(n => n.NoticeTemplate.ReceiverType == ReceiverType.DeptManager).Select(Mapper.Map<NoticeDTO>).ToArray());
            //        }
            //        if (roles.Contains("专家"))
            //        {
            //            list.AddRange(Mapper.Map<List<NoticeDTO>>(_noticeRepository.GetExpertNotice(userId)));
            //        }
            //        if(userType == 1)
            //        {
            //            list.AddRange(_noticeRepository.GetNoticeByReceive(userId).Where(n => n.NoticeTemplate.ReceiverType == ReceiverType.User).Select(Mapper.Map<NoticeDTO>).ToArray());
            //        }
            //        list.AddRange(_noticeRepository.GetNoticeByReceive(userId).Select(Mapper.Map<NoticeDTO>).ToArray());
            //    }
            //}
            ////按照角色对通知分类
            //else
            //{
            //    if (roles.Contains("单位管理员"))
            //    {
            //        list.AddRange(Mapper.Map<List<NoticeDTO>>(_noticeRepository.GetInstNotice(instituteId)));
            //    }
            //    if (roles.Contains("院管理员"))
            //    {
            //        list.AddRange(Mapper.Map<List<NoticeDTO>>(_noticeRepository.GetDeptNotice(userId)));
            //    }
            //    if (roles.Contains("专家"))
            //    {
            //        list.AddRange(Mapper.Map<List<NoticeDTO>>(_noticeRepository.GetExpertNotice(userId)));
            //    }
            //}
            //list.Sort((x,y)=> x.SendTime.CompareTo(y.SendTime));
            //var paging = PagingHelper.PagingWrapper(list, page, SystemConfig.NoticePageCount);
            //return paging;

        }

        /// <summary>
        /// 未读通知变为已读
        /// </summary>
        /// <param name="noticeId">通知的Id</param>
        //public NoticeDTO ReadNotice(int? noticeId)
        //{
        //    var notice = _noticeRepository.GetNoticeById(noticeId);
        //    if(!notice.Read)
        //    {
        //        notice.Read = true;
        //    }
        //    var updateNotice = Mapper.Map<NoticeDTO>(_noticeRepository.UpdateNotice(notice));
        //    return updateNotice;

        //}
    }
}