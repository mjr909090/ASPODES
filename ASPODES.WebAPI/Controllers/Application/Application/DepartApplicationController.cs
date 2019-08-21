using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Net.Http;
using System.Web.Http;
using ASPODES.DTO.Application;
using ASPODES.WebAPI.Repository;
using ASPODES.Model;
using ASPODES.WebAPI.Authorize;
using ASPODES.WebAPI.Filter;
using ASPODES.WebAPI.Common;
using ASPODES.WebAPI.Service;

namespace ASPODES.WebAPI.Controllers
{
    /// <summary>
    /// 院管理员申请书操作
    /// </summary>
    //[AspodesAuthorize(Roles="院管理员")]
    [Authorize(Roles="院管理员")]
    [ActionTrack]
    public class DepartApplicationController : ApiController
    {
        public ApplicationRepository repository = new ApplicationRepository();
        private ExportExcelRepository _excelExportRepository;

        public DepartApplicationController(ExportExcelRepository excelExportRepository)
        {
            this._excelExportRepository = excelExportRepository;
        }
        //private CreateNoticeService _noticeService;

        //public DepartApplicationController(CreateNoticeService noticeService)
        //{
        //    this._noticeService = noticeService;
        //}

        /// <summary>
        /// 获取全院的既往申请书,按单位、分类、分页
        /// </summary>
        /// <returns></returns>
        [Route("api/departapplication/previous/{instituteId}/{projectTypeId}/{page}")]
        public HttpResponseMessage GetPreviousApplications(int instituteId, int projectTypeId,int page)
        {
            try
            {
                var userInfo = UserHelper.GetCurrentUser();
                Func<Application, bool> predicate = a => a.CurrentYear < SystemConfig.ApplicationStartYear
                                                         && (userInfo.ProjectTypeIds.Contains(a.ProjectTypeId))
                                                         && (instituteId == 0 || a.InstituteId == instituteId)
                                                         && ((projectTypeId == 0) || a.ProjectTypeId == projectTypeId);
                return ResponseWrapper.SuccessResponse(repository.GetPagingApplicationList(predicate, page));
            }
            catch (Exception e)
            {
                return ResponseWrapper.ExceptionResponse(e);
            }
            
        }

        /// <summary>
        /// 院管理员导出既往申请书
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Route("api/deptapplication/previous")]
        public HttpResponseMessage ExportDeptpreviousApplications(DeptExportAppDTO dto)
        {
            try
            {
                var userInfo = UserHelper.GetCurrentUser();
                Func<Application, bool> predicate = a => ((dto.Year == 0 && a.CurrentYear< SystemConfig.ApplicationStartYear )|| a.CurrentYear == dto.Year)  //某一年的
                                                         && (userInfo.ProjectTypeIds.Contains(a.ProjectTypeId))
                                                         && ((dto.InstituteId == 0) || (a.InstituteId == dto.InstituteId))
                                                         && (dto.CategoryId == 0 || a.ProjectTypeId == dto.CategoryId);
                List<GetApplicationDTO> applicationDTOs = repository.GetApplicationList(predicate);

                string[][] strs = new string[applicationDTOs.Count + 1][];
                strs[0] = new string[6];
                int titleIndex = 0;
                if (dto.Order != null && !"".Equals(dto.Order)) strs[0][titleIndex++] = dto.Order;
                if (dto.Name != null && !"".Equals(dto.Name)) strs[0][titleIndex++] = dto.Name;
                if (dto.Deleage != null && !"".Equals(dto.Deleage)) strs[0][titleIndex++] = dto.Deleage;
                if (dto.Category != null && !"".Equals(dto.Category)) strs[0][titleIndex++] = dto.Category;
                if (dto.Institute != null && !"".Equals(dto.Institute)) strs[0][titleIndex++] = dto.Institute;
                if (dto.Leader != null && !"".Equals(dto.Leader)) strs[0][titleIndex++] = dto.Leader;
                //if (dto.Status != null && !"".Equals(dto.Status)) strs[0][titleIndex++] = dto.Status;

                int i = 1;
                foreach (GetApplicationDTO item in applicationDTOs)
                {
                    strs[i] = new string[6];
                    int j = 0;
                    if (dto.Order != null && !"".Equals(dto.Order)) strs[i][j++] = i.ToString();
                    if (dto.Name != null && !"".Equals(dto.Name)) strs[i][j++] = item.ProjectName;
                    if (dto.Deleage != null && !"".Equals(dto.Deleage)) strs[i][j++] = item.DeleageType == DelegateType.DIRECTIONAL ? "委托" : "非委托";
                    if (dto.Category != null && !"".Equals(dto.Category)) strs[i][j++] = item.SupportCategoryName;
                    if (dto.Institute != null && !"".Equals(dto.Institute)) strs[0][titleIndex++] = dto.Institute;
                    if (dto.Leader != null && !"".Equals(dto.Leader)) strs[i][j++] = item.LeaderName;
                    //if (dto.Status != null && !"".Equals(dto.Status)) UserHelper.ApplicationStatusTransform.TryGetValue(item.Status, out strs[i][j++]);
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
        /// 获取全院状态为 等待院管理员受理 的申请书,按单位、分类、分页
        /// </summary>
        /// <returns></returns>
        [Route("api/departapplication/accept/{instituteId}/{projectTypeId}/{page}")]
        public HttpResponseMessage GetAcceptApplications(int instituteId, int projectTypeId, int page)
        {
            try
            {
                var userInfo = UserHelper.GetCurrentUser();
                Func<Application, bool> predicate = a => a.CurrentYear == SystemConfig.ApplicationStartYear
                                                         && (userInfo.ProjectTypeIds.Contains(a.ProjectTypeId))
                                                         && a.Status == ApplicationStatus.ACCEPT
                                                         && ((instituteId == 0) || (a.InstituteId == instituteId))
                                                         && ((projectTypeId == 0) || (a.ProjectTypeId == projectTypeId));
                return ResponseWrapper.SuccessResponse(repository.GetPagingApplicationList(predicate, page));
            }
            catch (Exception e)
            {
                return ResponseWrapper.ExceptionResponse(e);
            }
            
        }

        /// <summary>
        /// 院管理员导出待受理申请书
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Route("api/deptapplication/export")]
        public HttpResponseMessage ExportDeptCheckApplications(DeptExportAppDTO dto)
        {
            try
            {
                var userInfo = UserHelper.GetCurrentUser();
                Func<Application, bool> predicate = a => a.CurrentYear == SystemConfig.ApplicationStartYear //本年度的
                                                         && (userInfo.ProjectTypeIds.Contains(a.ProjectTypeId))
                                                         && a.Status == ApplicationStatus.ACCEPT
                                                         && ((dto.InstituteId == 0) || (a.InstituteId == dto.InstituteId))
                                                         && (dto.CategoryId == 0 || a.ProjectTypeId == dto.CategoryId);
                List<GetApplicationDTO> applicationDTOs = repository.GetApplicationList(predicate);

                string[][] strs = new string[applicationDTOs.Count + 1][];
                strs[0] = new string[6];
                int titleIndex = 0;
                if (dto.Order != null && !"".Equals(dto.Order)) strs[0][titleIndex++] = dto.Order;
                if (dto.Name != null && !"".Equals(dto.Name)) strs[0][titleIndex++] = dto.Name;
                if (dto.Deleage != null && !"".Equals(dto.Deleage)) strs[0][titleIndex++] = dto.Deleage;
                if (dto.Category != null && !"".Equals(dto.Category)) strs[0][titleIndex++] = dto.Category;
                if (dto.Institute != null && !"".Equals(dto.Institute)) strs[0][titleIndex++] = dto.Institute;
                if (dto.Leader != null && !"".Equals(dto.Leader)) strs[0][titleIndex++] = dto.Leader;
                //if (dto.Status != null && !"".Equals(dto.Status)) strs[0][titleIndex++] = dto.Status;

                int i = 1;
                foreach (GetApplicationDTO item in applicationDTOs)
                {
                    strs[i] = new string[6];
                    int j = 0;
                    if (dto.Order != null && !"".Equals(dto.Order)) strs[i][j++] = i.ToString();
                    if (dto.Name != null && !"".Equals(dto.Name)) strs[i][j++] = item.ProjectName;
                    if (dto.Deleage != null && !"".Equals(dto.Deleage)) strs[i][j++] = item.DeleageType == DelegateType.DIRECTIONAL ? "委托" : "非委托";
                    if (dto.Category != null && !"".Equals(dto.Category)) strs[i][j++] = item.SupportCategoryName;
                    if (dto.Institute != null && !"".Equals(dto.Institute)) strs[0][titleIndex++] = dto.Institute;
                    if (dto.Leader != null && !"".Equals(dto.Leader)) strs[i][j++] = item.LeaderName;
                    //if (dto.Status != null && !"".Equals(dto.Status)) UserHelper.ApplicationStatusTransform.TryGetValue(item.Status, out strs[i][j++]);
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
        /// 获取全院状态为 院不受理 的申请书,按单位、分类、分页
        /// </summary>
        /// <returns></returns>
        [Route("api/departapplication/refuse/{instituteId}/{projectTypeId}/{page}")]
        public HttpResponseMessage GetRefuseApplications(int instituteId = 0, int projectTypeId = 0, int page = 1)
        {
            try
            {
                var userInfo = UserHelper.GetCurrentUser();
                Func<Application, bool> predicate = a => a.CurrentYear == SystemConfig.ApplicationStartYear
                                                       && (userInfo.ProjectTypeIds.Contains(a.ProjectTypeId))
                                                       && a.Status == ApplicationStatus.REFUSE
                                                       && ((instituteId == 0) || (a.InstituteId == instituteId))
                                                       && ((projectTypeId == 0) || (a.ProjectTypeId == projectTypeId));
                return ResponseWrapper.SuccessResponse(repository.GetPagingApplicationList(predicate, page));
            }
            catch (Exception e)
            {
                return ResponseWrapper.ExceptionResponse(e);
            }
            
        }

        /// <summary>
        /// 获取全院状态为 已受理 的申请书,按单位、分类、分页
        /// </summary>
        /// <returns></returns>
        [Route("api/departapplication/evaluate/{instituteId}/{projectTypeId}/{page}")]
        public HttpResponseMessage GetHandInApplications(int instituteId, int projectTypeId, int page)
        {
            try
            {
                var userInfo = UserHelper.GetCurrentUser();
                Func<Application, bool> predicate = a => a.CurrentYear == SystemConfig.ApplicationStartYear
                                                       && (userInfo.ProjectTypeIds.Contains(a.ProjectTypeId))
                                                       && a.Status >= ApplicationStatus.ASSIGNMENT
                                                       && ((instituteId == 0) || (a.InstituteId == instituteId))
                                                       && ((projectTypeId == 0) || (a.ProjectTypeId == projectTypeId));
                return ResponseWrapper.SuccessResponse(repository.GetPagingApplicationList(predicate, page));
            }
            catch (Exception e)
            {
                return ResponseWrapper.ExceptionResponse(e);
            }
            
        }

        /// <summary>
        /// 院管理员导出已受理申请书
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Route("api/deptapplication/accept")]
        public HttpResponseMessage ExportDeptacceptApplications(DeptExportAppDTO dto)
        {
            try
            {
                var userInfo = UserHelper.GetCurrentUser();
                Func<Application, bool> predicate = a => a.CurrentYear == SystemConfig.ApplicationStartYear
                                                         && (userInfo.ProjectTypeIds.Contains(a.ProjectTypeId))
                                                         && a.Status >= ApplicationStatus.ASSIGNMENT
                                                         && ((dto.InstituteId == 0) || (a.InstituteId == dto.InstituteId))
                                                         && (dto.CategoryId == 0 || a.ProjectTypeId == dto.CategoryId);
                List<GetApplicationDTO> applicationDTOs = repository.GetApplicationList(predicate);

                string[][] strs = new string[applicationDTOs.Count + 1][];
                strs[0] = new string[6];
                int titleIndex = 0;
                if (dto.Order != null && !"".Equals(dto.Order)) strs[0][titleIndex++] = dto.Order;
                if (dto.Name != null && !"".Equals(dto.Name)) strs[0][titleIndex++] = dto.Name;
                if (dto.Deleage != null && !"".Equals(dto.Deleage)) strs[0][titleIndex++] = dto.Deleage;
                if (dto.Category != null && !"".Equals(dto.Category)) strs[0][titleIndex++] = dto.Category;
                if (dto.Institute != null && !"".Equals(dto.Institute)) strs[0][titleIndex++] = dto.Institute;
                if (dto.Leader != null && !"".Equals(dto.Leader)) strs[0][titleIndex++] = dto.Leader;
                //if (dto.Status != null && !"".Equals(dto.Status)) strs[0][titleIndex++] = dto.Status;

                int i = 1;
                foreach (GetApplicationDTO item in applicationDTOs)
                {
                    strs[i] = new string[6];
                    int j = 0;
                    if (dto.Order != null && !"".Equals(dto.Order)) strs[i][j++] = i.ToString();
                    if (dto.Name != null && !"".Equals(dto.Name)) strs[i][j++] = item.ProjectName;
                    if (dto.Deleage != null && !"".Equals(dto.Deleage)) strs[i][j++] = item.DeleageType == DelegateType.DIRECTIONAL ? "委托" : "非委托";
                    if (dto.Category != null && !"".Equals(dto.Category)) strs[i][j++] = item.SupportCategoryName;
                    if (dto.Institute != null && !"".Equals(dto.Institute)) strs[0][titleIndex++] = dto.Institute;
                    if (dto.Leader != null && !"".Equals(dto.Leader)) strs[i][j++] = item.LeaderName;
                    //if (dto.Status != null && !"".Equals(dto.Status)) UserHelper.ApplicationStatusTransform.TryGetValue(item.Status, out strs[i][j++]);
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
        /// 受理申请书
        /// </summary>
        /// <param name="applicationId">申请书ID</param>
        /// <returns></returns>
        [HttpPut]
        [Route("api/departapplication/accept/{applicationId}")]
        public HttpResponseMessage HandInApplication(string applicationId)
        {
            try
            {
                //repository.ChangeApplicationStatus(applicationId, ApplicationStatus.ACCEPT, ApplicationStatus.ASSIGNMENT, a => true);
                Object response = repository.AcceptApplication(applicationId, a => true);

                //院管理员受理申请书
                //通知打点:发给提交申请书的用户
                //_noticeService.AddNotice(
                //    _noticeService.GetUserIdByApplicationId(applicationId), 6);
                //通知打点:发给审核通过申请书的单位
                //_noticeService.AddNoticeList(
                //    _noticeService.GetInstManagerIdsbyAppId(applicationId), 11);

                //系统设置使用手动分配专家
                //if ( !SystemConfig.AutoAssignmentExpert)
                //{
                //    //院管理员受理申请书,并系统设置手动指派
                //    //通知打点:院管理员
                //    _noticeService.AddNoticeList(
                //        _noticeService.GetDepartManagerIdListByApplicationId(applicationId), 16);
                //}

                return ResponseWrapper.SuccessResponse(response);
            }
            catch (Exception e)
            {
                return ResponseWrapper.ExceptionResponse(e);
            }
        }

        /// <summary>
        /// 不受理申请书
        /// </summary>
        /// <param name="logDTO">不受理操作日志</param>
        /// <returns></returns>
        [HttpPut]
        [Route("api/departapplication/refuse")]
        public HttpResponseMessage RejectApplication(AddApplicationLogDTO logDTO )
        {
            try
            {
                var userInfo = UserHelper.GetCurrentUser();
                logDTO.Operation = ActionType.REFUSE_APPLICATION;
                logDTO.UserId = userInfo.UserId;
                repository.ChangeApplicationStatus(logDTO.ApplicationId, ApplicationStatus.ACCEPT, ApplicationStatus.REFUSE, a => true, logDTO);

                //院管理员不受理申请书
                //通知打点:发给提交申请书的用户
                //_noticeService.AddNotice(
                //    _noticeService.GetUserIdByApplicationId(logDTO.ApplicationId), 5);
                //通知打点:发给审核通过申请书的单位
                //_noticeService.AddNoticeList(
                //    _noticeService.GetInstManagerIdsbyAppId(logDTO.ApplicationId), 10);

                return ResponseWrapper.SuccessResponse();
            }
            catch (Exception e)
            {
                return ResponseWrapper.ExceptionResponse(e);
            }
            
        }

        /// <summary>
        /// 导出申请书综合查询列表
        /// </summary>
        /// <param name="appInquiriesDTO">不受理操作日志</param>
        /// <returns></returns>
        [HttpPost]
        [Route("api/departapplication/ApplicationInquiries")]
        public HttpResponseMessage ApplicationInquiries(ApplicationInquiriesDTO appInquiriesDTO)
        {
            try
            {
                var appList = repository.ApplicationInquiries(appInquiriesDTO);
                string[][] strs = new string[appList.Count + 1][];
                strs[0] = new string[18];
                for(int titleIndex = 0; titleIndex < UserHelper.ApplicationExportTableHeader.Count(); titleIndex++)
                {
                    UserHelper.ApplicationExportTableHeader.TryGetValue(titleIndex, out strs[0][titleIndex]);
                }
                int i = 1;
                foreach (GetApplicationInquiriesDTO item in appList)
                {
                    strs[i] = new string[18];
                    int j = 0;
                    strs[i][j++] = item.ApplicationId;
                    strs[i][j++] = item.ProjectName;
                    strs[i][j++] = item.Period;
                    strs[i][j++] = item.ProjectTypeName;
                    strs[i][j++] = item.SupportCategoryName;
                    strs[i][j++] = item.InstituteName;
                    strs[i][j++] = item.LeaderName;
                    strs[i][j++] = item.LeaderPhone;
                    strs[i][j++] = item.LeaderEmail;
                    strs[i][j++] = item.ContactPhone;
                    strs[i][j++] = item.ContactEmail;
                    strs[i][j++] = item.TotalBudget.ToString();
                    strs[i][j++] = item.FirstYearBudget.ToString();
                    strs[i][j++] = item.YearCreated.ToString();
                    strs[i][j++] = item.CurrentYear.ToString();
                    strs[i][j++] = item.AbstractContent;
                    strs[i][j++] = item.DeleageType == DelegateType.DIRECTIONAL ? "定向项目":"非定向项目";
                    UserHelper.ApplicationStatusTransform.TryGetValue(item.Status, out strs[i][j++]);
                    i++;
                }
                return _excelExportRepository.DownloadExportExcel(strs, "申请书综合查询");
            }
            catch (Exception e)
            {
                return ResponseWrapper.ExceptionResponse(e);
            }

        }
        
        /// <summary>
        /// 申请书综合查询（分页）
        /// </summary>
        /// <param name="appInquiriesDTO">不受理操作日志</param>
        /// <param name="page"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("api/departapplication/ApplicationInquiries/{page}")]
        public HttpResponseMessage ApplicationInquiries(ApplicationInquiriesDTO appInquiriesDTO,int page)
        {
            try
            {
                var appList = repository.ApplicationInquiriesByPage(appInquiriesDTO, page);
                return ResponseWrapper.SuccessResponse(appList);
            }
            catch (Exception e)
            {
                return ResponseWrapper.ExceptionResponse(e);
            }

        }

    }
}
