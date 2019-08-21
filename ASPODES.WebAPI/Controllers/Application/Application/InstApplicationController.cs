using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Net.Http;
using System.Web.Http;
using ASPODES.WebAPI.Repository;
using ASPODES.Model;
using ASPODES.DTO.Application;
using ASPODES.WebAPI.Common;
using System.Security.Principal;
using ASPODES.WebAPI.Authorize;
using ASPODES.WebAPI.Filter;
using ASPODES.WebAPI.Service;

namespace ASPODES.WebAPI.Controllers
{
    /// <summary>
    /// 单位管理员对申请书操作
    /// </summary>
    [AspodesAuthorize(Roles = "单位管理员")]
    [ActionTrack]
    public class InstApplicationController : ApiController
    {
        //private CreateNoticeService _noticeService;
        private ApplicationRepository repository = new ApplicationRepository();
        private ExportExcelRepository _excelExportRepository;

        public InstApplicationController( ExportExcelRepository excelExportRepository)
        {
            this._excelExportRepository = excelExportRepository;
        }
        /// <summary>
        /// 构造函数，获取CreateNoticeService
        /// </summary>
        /// <param name="noticeService"></param>
        //public InstApplicationController(CreateNoticeService noticeService)
        //{
        //    this._noticeService = noticeService;
        //}

        /// <summary>
        /// 获取状态为 等待单位审核 的单位申请书列表,按分类，按页码
        /// </summary>
        /// <returns></returns>
        [Route("api/instapplication/check/{projectTypeId}/{page}")]
        public HttpResponseMessage GetInstCheckApplications(int projectTypeId, int page)
        {
            try
            {
                var userInfo = UserHelper.GetCurrentUser();
                Func<Application, bool> predicate = a => a.CurrentYear == SystemConfig.ApplicationStartYear //本年度的
                                                         && a.InstituteId == userInfo.InstId//本单位的
                                                         && a.Status == ApplicationStatus.CHECK
                                                         && (projectTypeId == 0 || a.ProjectTypeId == projectTypeId);
                return ResponseWrapper.SuccessResponse(repository.GetPagingApplicationList(predicate, page));
            }
            catch (Exception e)
            {
                return ResponseWrapper.ExceptionResponse(e);
            }
            
        }

        /// <summary>
        /// 单位管理员导出待受理申请书
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Route("api/instapplication/export")]
        public HttpResponseMessage ExportInstCheckApplications(InstExportAppDTO dto)
        {
            try
            {
                var userInfo = UserHelper.GetCurrentUser();
                Func<Application, bool> predicate = a => a.CurrentYear == SystemConfig.ApplicationStartYear //本年度的
                                                         && a.InstituteId == userInfo.InstId//本单位的
                                                         && a.Status == ApplicationStatus.CHECK
                                                         && (dto.CategoryId == 0 || a.ProjectTypeId == dto.CategoryId);
                List<GetApplicationDTO> applicationDTOs = repository.GetApplicationList(predicate);

                string[][] strs = new string[applicationDTOs.Count+1][];
                strs[0] = new string[6];
                int titleIndex = 0;
                if (dto.Order != null && !"".Equals(dto.Order)) strs[0][titleIndex++] = dto.Order;
                if (dto.Name != null && !"".Equals(dto.Name)) strs[0][titleIndex++] = dto.Name;
                if (dto.Deleage != null && !"".Equals(dto.Deleage)) strs[0][titleIndex++] = dto.Deleage;
                if (dto.Category != null && !"".Equals(dto.Category)) strs[0][titleIndex++] = dto.Category;
                if (dto.Leader != null && !"".Equals(dto.Leader)) strs[0][titleIndex++] = dto.Leader;
                if (dto.Status != null && !"".Equals(dto.Status)) strs[0][titleIndex++] = dto.Status;

                int i = 1;
                foreach(GetApplicationDTO item in applicationDTOs)
                {
                    strs[i] = new string[6];
                    int j = 0;
                    if (dto.Order != null && !"".Equals(dto.Order)) strs[i][j++] = i.ToString();
                    if (dto.Name != null && !"".Equals(dto.Name)) strs[i][j++] = item.ProjectName;
                    if (dto.Deleage != null && !"".Equals(dto.Deleage)) strs[i][j++] = item.DeleageType == DelegateType.DIRECTIONAL? "委托":"非委托";
                    if (dto.Category != null && !"".Equals(dto.Category)) strs[i][j++] = item.SupportCategoryName;
                    if (dto.Leader != null && !"".Equals(dto.Leader)) strs[i][j++] = item.LeaderName;
                    if (dto.Status != null && !"".Equals(dto.Status)) UserHelper.ApplicationStatusTransform.TryGetValue(item.Status, out strs[i][j++]);
                    i++;

                }
                return _excelExportRepository.DownloadExportExcel(strs, "待受理申请书");
            }
            catch (Exception e)
            {
                return ResponseWrapper.ExceptionResponse(e);
            }

        }


        /// <summary>
        /// 获取状态为 等待院管理员受理 的单位申请书列表,按分类，按页码
        /// </summary>
        /// <returns></returns>
        [Route("api/instapplication/accept/{projectTypeId}/{page}")]
        public HttpResponseMessage GetInstAcceptApplications(int projectTypeId, int page)
        {
            try
            {
                var userInfo = UserHelper.GetCurrentUser();
                Func<Application, bool> predicate = a => a.CurrentYear == SystemConfig.ApplicationStartYear//本年度的
                                                        && a.InstituteId == userInfo.InstId//本单位的
                                                        && a.Status == ApplicationStatus.ACCEPT
                                                        && (projectTypeId == 0 || a.ProjectTypeId == projectTypeId);
                return ResponseWrapper.SuccessResponse(repository.GetPagingApplicationList(predicate, page));
            }
            catch (Exception e)
            {
                return ResponseWrapper.ExceptionResponse(e);
            }
             
        }

        /// <summary>
        /// 获取本年度单位管理员已经提交的申请书列表
        /// </summary>
        /// <returns></returns>
        [Route("api/instapplication/submit/{projectTypeId}/{page}")]
        public HttpResponseMessage GetInstSubmittedApplications( int projectTypeId, int page )
        {
            try
            {
                var userInfo = UserHelper.GetCurrentUser();
                Func<Model.Application, bool> predicate = a =>
                                                          a.InstituteId == userInfo.InstId//本单位的
                                                          && a.CurrentYear == SystemConfig.ApplicationStartYear  //本年度的
                                                          && a.Status >= ApplicationStatus.ACCEPT //已经提交的
                                                          && (projectTypeId == 0 || a.ProjectTypeId == projectTypeId);
                return ResponseWrapper.SuccessResponse(repository.GetPagingApplicationList(predicate, page));
            }
            catch (Exception e)
            {
                return ResponseWrapper.ExceptionResponse(e);
            }

        }

        /// <summary>
        /// 单位管理员导出已受理申请书
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Route("api/instapplication/exportAccept")]
        public HttpResponseMessage ExportInstAcceptApplications(InstExportAppDTO dto)
        {
            try
            {
                var userInfo = UserHelper.GetCurrentUser();
                Func<Application, bool> predicate = a => a.CurrentYear == SystemConfig.ApplicationStartYear //本年度的
                                                         && a.InstituteId == userInfo.InstId//本单位的
                                                         && a.Status >= ApplicationStatus.ACCEPT
                                                         && (dto.CategoryId == 0 || a.ProjectTypeId == dto.CategoryId);
                List<GetApplicationDTO> applicationDTOs = repository.GetApplicationList(predicate);

                string[][] strs = new string[applicationDTOs.Count + 1][];
                strs[0] = new string[6];
                int titleIndex = 0;
                if (dto.Order != null && !"".Equals(dto.Order)) strs[0][titleIndex++] = dto.Order;
                if (dto.Name != null && !"".Equals(dto.Name)) strs[0][titleIndex++] = dto.Name;
                if (dto.Deleage != null && !"".Equals(dto.Deleage)) strs[0][titleIndex++] = dto.Deleage;
                if (dto.Category != null && !"".Equals(dto.Category)) strs[0][titleIndex++] = dto.Category;
                if (dto.Leader != null && !"".Equals(dto.Leader)) strs[0][titleIndex++] = dto.Leader;
                if (dto.Status != null && !"".Equals(dto.Status)) strs[0][titleIndex++] = dto.Status;

                int i = 1;
                foreach (GetApplicationDTO item in applicationDTOs)
                {
                    strs[i] = new string[6];
                    int j = 0;
                    if (dto.Order != null && !"".Equals(dto.Order)) strs[i][j++] = i.ToString();
                    if (dto.Name != null && !"".Equals(dto.Name)) strs[i][j++] = item.ProjectName;
                    if (dto.Deleage != null && !"".Equals(dto.Deleage)) strs[i][j++] = item.DeleageType == DelegateType.DIRECTIONAL ? "委托" : "非委托";
                    if (dto.Category != null && !"".Equals(dto.Category)) strs[i][j++] = item.SupportCategoryName;
                    if (dto.Leader != null && !"".Equals(dto.Leader)) strs[i][j++] = item.LeaderName;
                    if (dto.Status != null && !"".Equals(dto.Status)) UserHelper.ApplicationStatusTransform.TryGetValue(item.Status, out strs[i][j++]);
                    i++;

                }
                return _excelExportRepository.DownloadExportExcel(strs, "已受理申请书");
            }
            catch (Exception e)
            {
                return ResponseWrapper.ExceptionResponse(e);
            }
        }

        /// <summary>
        /// 获取状态为 被单位管理员驳回 的单位申请书列表,按分类，按页码
        /// </summary>
        /// <returns></returns>
        [Route("api/instapplication/reject/{projectTypeId}/{page}")]
        public HttpResponseMessage GetInstRejectApplications(int projectTypeId, int page)
        {
            try
            {
                var userInfo = UserHelper.GetCurrentUser();
                Func<Model.Application, bool> predicate = a =>
                                                          a.InstituteId == userInfo.InstId//本单位的
                                                          && a.CurrentYear == SystemConfig.ApplicationStartYear  //本年度的
                                                          && a.Status == ApplicationStatus.REJECT //已经驳回的
                                                          && (projectTypeId == 0 || a.ProjectTypeId == projectTypeId);
                return ResponseWrapper.SuccessResponse(repository.GetPagingApplicationList(predicate, page));
            }
            catch (Exception e)
            {
                return ResponseWrapper.ExceptionResponse(e);
            }
            
        }

        /// <summary>
        /// 获取既往的单位申请书列表,按分类，按页码
        /// </summary>
        /// <returns></returns>
        [Route("api/instapplication/previous/{projectTypeId}/{page}")]
        public HttpResponseMessage GetInstPreviousApplications(int projectTypeId, int page)
        {
            try
            {
                var userInfo = UserHelper.GetCurrentUser();
                Func<Model.Application, bool> predicate = a =>
                                                      a.InstituteId == userInfo.InstId//本单位的
                                                      && a.CurrentYear < SystemConfig.ApplicationStartYear  //往年的
                                                      && (projectTypeId == 0 || a.ProjectTypeId == projectTypeId);
                return ResponseWrapper.SuccessResponse(repository.GetPagingApplicationList(predicate, page));
            }
            catch (Exception e)
            {
                return ResponseWrapper.ExceptionResponse(e);
            }
        }

        /// <summary>
        /// 单位管理员导出既往申请书
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Route("api/instapplication/Previous")]
        public HttpResponseMessage ExportInstPreviousApplications(InstExportAppDTO dto)
        {
            try
            {
                var userInfo = UserHelper.GetCurrentUser();
                Func<Application, bool> predicate = a => a.InstituteId == userInfo.InstId//本单位的
                                                         && ((dto.Year == 0 && a.CurrentYear < SystemConfig.ApplicationStartYear) || a.CurrentYear == dto.Year)  //某一年的
                                                         && (dto.CategoryId == 0 || a.ProjectTypeId == dto.CategoryId);
                List<GetApplicationDTO> applicationDTOs = repository.GetApplicationList(predicate);

                string[][] strs = new string[applicationDTOs.Count + 1][];
                strs[0] = new string[6];
                int titleIndex = 0;
                if (dto.Order != null && !"".Equals(dto.Order)) strs[0][titleIndex++] = dto.Order;
                if (dto.Name != null && !"".Equals(dto.Name)) strs[0][titleIndex++] = dto.Name;
                if (dto.Deleage != null && !"".Equals(dto.Deleage)) strs[0][titleIndex++] = dto.Deleage;
                if (dto.Category != null && !"".Equals(dto.Category)) strs[0][titleIndex++] = dto.Category;
                if (dto.Leader != null && !"".Equals(dto.Leader)) strs[0][titleIndex++] = dto.Leader;
                if (dto.Status != null && !"".Equals(dto.Status)) strs[0][titleIndex++] = dto.Status;

                int i = 1;
                foreach (GetApplicationDTO item in applicationDTOs)
                {
                    strs[i] = new string[6];
                    int j = 0;
                    if (dto.Order != null && !"".Equals(dto.Order)) strs[i][j++] = i.ToString();
                    if (dto.Name != null && !"".Equals(dto.Name)) strs[i][j++] = item.ProjectName;
                    if (dto.Deleage != null && !"".Equals(dto.Deleage)) strs[i][j++] = item.DeleageType == DelegateType.DIRECTIONAL ? "委托" : "非委托";
                    if (dto.Category != null && !"".Equals(dto.Category)) strs[i][j++] = item.SupportCategoryName;
                    if (dto.Leader != null && !"".Equals(dto.Leader)) strs[i][j++] = item.LeaderName;
                    if (dto.Status != null && !"".Equals(dto.Status)) UserHelper.ApplicationStatusTransform.TryGetValue(item.Status, out strs[i][j++]);
                    i++;

                }
                return _excelExportRepository.DownloadExportExcel(strs, "既往申请书");
            }
            catch (Exception e)
            {
                return ResponseWrapper.ExceptionResponse(e);
            }
        }


        /// <summary>
        /// 单位管理员审核通过申请书并提交给院管理员
        /// </summary>
        /// <param name="applicationId">申请书ID</param>
        /// <returns></returns>
        [HttpPut]
        [Route("api/instapplication/handin/{applicationId}")]
        public HttpResponseMessage HandInApplication(string applicationId)
        {
            try
            {
                var userInfo = UserHelper.GetCurrentUser();
                repository.ChangeApplicationStatus(applicationId, ApplicationStatus.CHECK, ApplicationStatus.ACCEPT, a => a.InstituteId == userInfo.InstId);

                //单位管理员审核申请书通过
                //通知打点:发给申请书提交者
                //_noticeService.AddNotice(_noticeService.GetUserIdByApplicationId(applicationId), 2);
                //通知打点:发给相关院管理员
                //_noticeService.AddNoticeList(
                //    _noticeService.GetDepartManagerIdListByApplicationId(applicationId), 14);

                return ResponseWrapper.SuccessResponse();
            }
            catch (Exception e)
            {
                return ResponseWrapper.ExceptionResponse(e);
            }
        }

        /// <summary>
        /// 单位管理员审核并驳回申请书
        /// </summary>
        /// <param name="logDTO">驳回申请书日志</param>
        /// <returns></returns>
        [HttpPut]
        [Route("api/instapplication/reject")]
        public HttpResponseMessage RejectApplication(AddApplicationLogDTO logDTO)
        {
            try
            {
                var userInfo = UserHelper.GetCurrentUser();
                logDTO.Operation = ActionType.REJECT_APPLICATION;
                logDTO.UserId = userInfo.UserId;
                repository.ChangeApplicationStatus(logDTO.ApplicationId, ApplicationStatus.CHECK, ApplicationStatus.REJECT, a => a.InstituteId == userInfo.InstId, logDTO);

                //单位管理员审核申请书驳回
                //通知打点:发给单位普通用户
                //_noticeService.AddNotice(_noticeService.GetUserIdByApplicationId(logDTO.ApplicationId), 3);

                return ResponseWrapper.SuccessResponse();
            }
            catch (Exception e)
            {
                return ResponseWrapper.ExceptionResponse(e);
            }
            
        }

        /// <summary>
        /// 单位管理员撤回提交至院的申请书
        /// </summary>
        /// <param name="applicationId">申请书ID</param>
        /// <returns></returns>
        [HttpPut]
        [Route("api/instapplication/cancel/{applicationId}")]
        public HttpResponseMessage CancelApplication(string applicationId)
        {
            try
            {
                var userInfo = UserHelper.GetCurrentUser();
                repository.ChangeApplicationStatus(applicationId, ApplicationStatus.ACCEPT, ApplicationStatus.CANCEL, a => a.InstituteId == userInfo.InstId);
                
                //单位管理员撤回审核通过的申请书
                //通知打点:发给申请书提交者
                //_noticeService.AddNotice(
                //    _noticeService.GetUserIdByApplicationId(applicationId), 4);
                //通知打点:发给相关院管理员(需要占位符替换)
                //_noticeService.AddNoticeList(
                //    _noticeService.GetDepartManagerIdListByApplicationId(applicationId), 15, 
                //    new Dictionary<string, string>
                //    { {"DepartmentName", _noticeService.GetInstituteNameById(userInfo.InstId)},
                //      { "ApplicationName", _noticeService.GetApplicationNameById(applicationId)} });

                return ResponseWrapper.SuccessResponse();
            }
            catch (Exception e)
            {
                return ResponseWrapper.ExceptionResponse(e);
            }
            
        }
    }
}
