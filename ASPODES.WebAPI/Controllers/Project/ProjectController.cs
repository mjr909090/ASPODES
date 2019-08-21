using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using ASPODES.WebAPI.Repository;
using ASPODES.WebAPI.Common;
using ASPODES.Model;
using ASPODES.DTO.Project;
using AutoMapper;
using ASPODES.WebAPI.Util;
using ASPODES.WebAPI.Filter;
using System.Web.Http;
using ASPODES.WebAPI.Service;

namespace ASPODES.WebAPI.Controllers.Project
{
    /// <summary>
    /// 项目操作
    /// </summary>
    [ActionTrack]
    public class ProjectController : ApiController
    {
        private IProjectRepository _repository;
        private ExportExcelRepository _excelExportRepository;
        //private CreateNoticeService _noticeService;

        public ProjectController(IProjectRepository projectRepository, ExportExcelRepository excelExportRepository)
        {
            _repository = projectRepository;
            this._excelExportRepository = excelExportRepository;
        }

        /// <summary>
        /// 个人获得主持项目的信息
        /// 在研为详细信息，非在研为简略信息
        /// </summary>
        /// <param name="Status">项目状态</param>
        [Route("api/project/leader")]
        public HttpResponseMessage GetLeaderProject([FromUri]ProjectStatus Status)
        {
            try
            {
                var LeaderId = UserHelper.GetCurrentUser().PersonId;
                if (Status == ProjectStatus.ACTIVE)
                {
                    var ProjectDTO = _repository.GetLeaderProjectList(LeaderId, Status)
                                     .Select(Mapper.Map<GetProjectDTO>).ToList();
                    
                    return ResponseWrapper.SuccessResponse(ProjectDTO);
                }
                else
                {
                    var ProjectDTO = _repository.GetLeaderProjectList(LeaderId, Status)
                                     .Select(Mapper.Map</*项目是否一次性取出数据GetComboProjectDTO*/GetProjectDTO>).ToList();
                    return ResponseWrapper.SuccessResponse(ProjectDTO);
                }
 
            }
            catch (Exception e)
            {
                return ResponseWrapper.ExceptionResponse(e);
            }
        }

        /// <summary>
        /// 负责人确认提交项目结题
        /// </summary>
        /// <param name="ProjectId">项目Id</param>
        [HttpGet]
        [Route("api/project/leader/IsSubmitProjectFinish/{ProjectId}")]
        public HttpResponseMessage IsSubmitProjectFinish(string ProjectId)
        {
            try
            {

                var projectDTO = Mapper.Map<GetProjectDTO>(_repository.GetProjectDetailList(ProjectId));
                if(projectDTO.Docs.Count(d => d.Type == ProjectDocType.FINISH_REPORT) == 0)
                {
                    projectDTO.IsSubmit = false;
                }
                else if(projectDTO.AnnualTasks.Count(a => a.Status == AnnualTaskStatus.FINISH) != projectDTO.Period)
                {
                    projectDTO.IsSubmit = false;
                }
                else
                {
                    projectDTO.IsSubmit = true;
                }
                return ResponseWrapper.SuccessResponse(projectDTO);

            }
            catch (Exception e)
            {
                return ResponseWrapper.ExceptionResponse(e);
            }
        }

        /// <summary>
        /// 负责人提交项目结题
        /// </summary>
        /// <param name="ProjectId">项目Id</param>
        [HttpGet]
        [Route("api/project/leader/SubmitProjectFinish/{ProjectId}")]
        public HttpResponseMessage SubmitProjectFinish(string ProjectId)
        {
            try
            {
                var project = _repository.GetProjectDetailList(ProjectId);
                if (project.Docs.Count(d => d.Type == ProjectDocType.FINISH_REPORT) > 0 
                    && (project.Status == ProjectStatus.ACTIVE || project.Status == ProjectStatus.INST_REJECT || project.Status == ProjectStatus.DEPART_REJECT))//添加状态
                {
                    project.Status = ProjectStatus.INST_REVIEW;
                    _repository.Update(project);

                    //项目负责人上传结题报告
                    //通知打点:发给所在单位所有管理员
                    //_noticeService.AddNoticeList(
                    //    _noticeService.GetInstituteManagerIdList(project.InstituteId.Value), 52);
                    //通知打点:发给自己
                    //_noticeService.AddNotice(
                    //    _noticeService.GetUserIdByPersonId(project.LeaderId.Value), 38);

                }
                else
                {
                    throw new ModelValidException(project.Docs.Count(d => d.Type == ProjectDocType.FINISH_REPORT) > 0 ? "该项目为非在研状态项目":"结题文档未上传");
                }
               
                return ResponseWrapper.SuccessResponse("提交成功");

            }
            catch (Exception e)
            {
                return ResponseWrapper.ExceptionResponse(e);
            }
        }


        /// <summary>
        /// 单位获得主持项目的基本信息
        /// </summary>
        /// <param name="Status">项目状态0-本单位在研项目，1-已结题项目</param>
        /// <param name="page">分页</param>
        [Authorize(Roles = "单位管理员")]
        [Route("api/project/institute")]
        public HttpResponseMessage GetInstituteComboProject([FromUri]ProjectStatus Status, [FromUri]int page)
        {
            try
            {
                var InstituteId = UserHelper.GetCurrentUser().InstId;
                var comboProjectDTO = _repository.GetInstituteProjectList(InstituteId, Status)
                                      .Select(Mapper.Map</*项目是否一次性取出数据GetComboProjectDTO*/GetProjectDTO>);

                var pageCpmboProject = PagingHelper.PagingWrapper(comboProjectDTO, page, SystemConfig.ApplicationPageCount);

                return ResponseWrapper.SuccessResponse(pageCpmboProject);
            }
            catch (Exception e)
            {
                return ResponseWrapper.ExceptionResponse(e);
            }
        }

        /// <summary>
        /// 个人获得参与的项目的基本信息
        /// </summary>
        /// <param name="Status">项目状态</param>
        [Route("api/project/participant/person")]
        public HttpResponseMessage GetPersonJoinComboProjectList([FromUri]ProjectStatus Status)
        {
            try
            {
                int personId = UserHelper.GetCurrentUser().PersonId;
                var comboProjectDTO = _repository.GetPersonJoinComboProjectList(personId, Status)
                                     .Select(Mapper.Map</*项目是否一次性取出数据GetComboProjectDTO*/GetProjectDTO>).ToList();

                return ResponseWrapper.SuccessResponse(comboProjectDTO); 
            }
            catch (Exception e)
            {
                return ResponseWrapper.ExceptionResponse(e);
            }
        }

        /// <summary>
        /// 单位获得参与项目的基本信息
        /// </summary>
        /// <param name="Status">项目状态</param>
        /// <param name="page">分页</param>
        [Authorize(Roles = "单位管理员")]
        [Route("api/project/participant/institute")]
        public HttpResponseMessage GetInstituteJoinComboProjectList([FromUri]ProjectStatus Status, [FromUri]int page)
        {
            try
            {
                int instituteId = UserHelper.GetCurrentUser().InstId;
                var comboProjectDTO = _repository.GetInstituteJoinComboProjectList(instituteId, Status)
                                      .Select(Mapper.Map</*项目是否一次性取出数据GetComboProjectDTO*/GetProjectDTO>).Distinct();

                var pageCpmboProject = PagingHelper.PagingWrapper(comboProjectDTO, page, SystemConfig.ApplicationPageCount);

                return ResponseWrapper.SuccessResponse(pageCpmboProject);
            }
            catch (Exception e)
            {
                return ResponseWrapper.ExceptionResponse(e);
            }
        }

        /// <summary>
        /// 单位审核结题项目
        /// </summary>
        /// <param name="ProjectId">项目Id</param>
        /// <param name="Pass">项目是否通过</param>
        /// <param name="Comment">审核意见</param>
        [HttpPost]
        [Authorize(Roles = "单位管理员")]
        [Route("api/project/institute/Auditing/{ProjectId}")]
        public HttpResponseMessage InstituteAuditingProjectFinish(string ProjectId, [FromUri]bool Pass)
        {
            //获得审核意见
            string Comment = Request.Content.ReadAsStringAsync().Result;
            try
            {
                var project = _repository.GetProjectDetailList(ProjectId);
                if (project.Status == ProjectStatus.INST_REVIEW)
                {
                    if (Pass)
                    {   //更新项目状态
                        project.Status = ProjectStatus.DEPART_REVIEW;
                        _repository.Update(project);

                        //单位管理员审核结题报告通过
                        //通知打点:发给负责相关领域的院管理员
                        //_noticeService.AddNoticeList(
                        //    _noticeService.GetDepartManagerIdListByProjectTypeId(project.ProjectTypeId.Value), 57);
                        //通知打点:发给用户
                        //_noticeService.AddNotice(
                        //    _noticeService.GetUserIdByPersonId(project.LeaderId.Value), 39,
                        //    new Dictionary<string, string>
                        //    { {"ApplicationName", project.Name } });

                    }
                    else
                    {   //更新项目状态，存放评审意见
                        project.Status = ProjectStatus.INST_REJECT;
                        project.Comment = Comment;
                        _repository.Update(project);

                        //单位管理员审核年度报告不通过
                        //通知打点:发给用户
                        //_noticeService.AddNotice(
                        //    _noticeService.GetUserIdByPersonId(project.LeaderId.Value), 40,
                        //    new Dictionary<string, string>
                        //    { {"ApplicationName", project.Name } });

                    }
                }
                else
                {
                    throw new ModelValidException("项目状态错误");
                }
                  
                return ResponseWrapper.SuccessResponse("审核完成");
            }
            catch (Exception e)
            {
                return ResponseWrapper.ExceptionResponse(e);
            }
        }

        /// <summary>
        /// 获取单位项目列表
        /// </summary>
        /// <param name="projectType"></param>
        /// <param name="year"></param>
        /// <param name="status"></param>
        /// <param name="page"></param>
        /// <returns></returns>
        [Route("api/project/inst")]
        public HttpResponseMessage GetInstProjectList(int projectType, int year, int status, int page)
        {
            try
            {
                var user = UserHelper.GetCurrentUser();
                var pt = projectType == 0 ? null : new int?[] { projectType };
                var projects = _repository
                    .GetProjectList( user.InstId, year, status, pt )
                    .Select( Mapper.Map<GetProjectDTO>);
                var paging = PagingHelper.PagingWrapper<GetProjectDTO>(projects, page, SystemConfig.ProjectPageCount);
                return ResponseWrapper.SuccessResponse(paging);
            }
            catch( Exception e)
            {
                return ResponseWrapper.ExceptionResponse(e);
            }
        }

        /// <summary>
        /// 单位管理员导出本单位的项目
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Route("api/instProject/export")]
        public HttpResponseMessage ExportInstCheckApplications(InstExportProjectDTO dto)
        {
            try
            {
                var user = UserHelper.GetCurrentUser();
                var pt = dto.CategoryId == 0 ? null : new int?[] { dto.CategoryId };
                var projects = _repository
                    .GetProjectList(user.InstId, dto.Year, (int)dto.Status, pt)
                    .Select(Mapper.Map<GetProjectDTO>);

                string[][] strs = new string[projects.Count() + 1][];
                strs[0] = new string[7];
                int titleIndex = 0;
                if (dto.Order != null && !"".Equals(dto.Order)) strs[0][titleIndex++] = dto.Order;
                if (dto.Name != null && !"".Equals(dto.Name)) strs[0][titleIndex++] = dto.Name;
                if (dto.Deleage != null && !"".Equals(dto.Deleage)) strs[0][titleIndex++] = dto.Deleage;
                if (dto.Category != null && !"".Equals(dto.Category)) strs[0][titleIndex++] = dto.Category;
                //if (dto.Institute != null && !"".Equals(dto.Institute)) strs[0][titleIndex++] = dto.Institute;
                if (dto.Leader != null && !"".Equals(dto.Leader)) strs[0][titleIndex++] = dto.Leader;
                if (dto.period != null && !"".Equals(dto.Status)) strs[0][titleIndex++] = dto.period;
                if (dto.StartAndStopTime != null && !"".Equals(dto.StartAndStopTime)) strs[0][titleIndex++] = dto.StartAndStopTime;

                int i = 1;
                foreach (var item in projects)
                {
                    strs[i] = new string[7];
                    int j = 0;
                    if (dto.Order != null && !"".Equals(dto.Order)) strs[i][j++] = i.ToString();
                    if (dto.Name != null && !"".Equals(dto.Name)) strs[i][j++] = item.Name;
                    if (dto.Deleage != null && !"".Equals(dto.Deleage)) strs[i][j++] = item.DelegateType;
                    if (dto.Category != null && !"".Equals(dto.Category)) strs[i][j++] = item.ProjectTypeName;
                    if (dto.Leader != null && !"".Equals(dto.Leader)) strs[i][j++] = item.Leader.UserName;
                    //if (dto.Institute != null && !"".Equals(dto.Institute)) strs[i][j++] = "本单位";
                    if (dto.period != null && !"".Equals(dto.period)) strs[i][j++] = item.Period.ToString();
                    if (dto.StartAndStopTime != null && !"".Equals(dto.StartAndStopTime)) strs[i][j++] = item.StartDate + "-" + item.EndDate;
                    i++;
                }
                return _excelExportRepository.DownloadExportExcel(strs, "项目导出");
            }
            catch (Exception e)
            {
                return ResponseWrapper.ExceptionResponse(e);
            }

        }

        /// <summary>
        /// 获取院项目列表
        /// </summary>
        /// <param name="projectType"></param>
        /// <param name="inst"></param>
        /// <param name="year"></param>
        /// <param name="status"></param>
        /// <param name="page"></param>
        /// <returns></returns>
        [Route("api/project/depart")]
        public HttpResponseMessage GetDepartProjectList(int projectType, int inst, int year, int status, int page)
        {
            try
            {
                var user = UserHelper.GetCurrentUser();
                var pt = projectType == 0 ? user.ProjectTypeIds : new int?[] { projectType };
                var projects = _repository
                    .GetProjectList(inst, year, status, pt)
                    .Select(Mapper.Map<GetProjectDTO>);
                var paging = PagingHelper.PagingWrapper<GetProjectDTO>(projects, page, SystemConfig.ProjectPageCount);
                return ResponseWrapper.SuccessResponse(paging);
            }
            catch (Exception e)
            {
                return ResponseWrapper.ExceptionResponse(e);
            }
        }

        /// <summary>
        /// 院管理员导出项目
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Route("api/deptProject/export")]
        public HttpResponseMessage ExportDeptCheckApplications(InstExportProjectDTO dto)
        {
            try
            {
                var user = UserHelper.GetCurrentUser();
                var pt = dto.CategoryId == 0 ? null : new int?[] { dto.CategoryId };
                var projects = _repository
                    .GetProjectList(dto.InstituteId, dto.Year, (int)dto.Status, pt)
                    .Select(Mapper.Map<GetProjectDTO>);

                string[][] strs = new string[projects.Count() + 1][];
                strs[0] = new string[7];
                int titleIndex = 0;
                if (dto.Order != null && !"".Equals(dto.Order)) strs[0][titleIndex++] = dto.Order;
                if (dto.Name != null && !"".Equals(dto.Name)) strs[0][titleIndex++] = dto.Name;
                if (dto.Deleage != null && !"".Equals(dto.Deleage)) strs[0][titleIndex++] = dto.Deleage;
                if (dto.Category != null && !"".Equals(dto.Category)) strs[0][titleIndex++] = dto.Category;
                //if (dto.Institute != null && !"".Equals(dto.Institute)) strs[0][titleIndex++] = dto.Institute;
                if (dto.Leader != null && !"".Equals(dto.Leader)) strs[0][titleIndex++] = dto.Leader;
                if (dto.period != null && !"".Equals(dto.Status)) strs[0][titleIndex++] = dto.period;
                if (dto.StartAndStopTime != null && !"".Equals(dto.StartAndStopTime)) strs[0][titleIndex++] = dto.StartAndStopTime;

                int i = 1;
                foreach (var item in projects)
                {
                    strs[i] = new string[7];
                    int j = 0;
                    if (dto.Order != null && !"".Equals(dto.Order)) strs[i][j++] = i.ToString();
                    if (dto.Name != null && !"".Equals(dto.Name)) strs[i][j++] = item.Name;
                    if (dto.Deleage != null && !"".Equals(dto.Deleage)) strs[i][j++] = item.DelegateType;
                    if (dto.Category != null && !"".Equals(dto.Category)) strs[i][j++] = item.ProjectTypeName;
                    if (dto.Leader != null && !"".Equals(dto.Leader)) strs[i][j++] = item.Leader.UserName;
                    //if (dto.Institute != null && !"".Equals(dto.Institute)) strs[i][j++] = item.;
                    if (dto.period != null && !"".Equals(dto.period)) strs[i][j++] = item.Period.ToString();
                    if (dto.StartAndStopTime != null && !"".Equals(dto.StartAndStopTime)) strs[i][j++] = item.StartDate + "-" + item.EndDate;
                    i++;
                }
                return _excelExportRepository.DownloadExportExcel(strs, "项目导出");
            }
            catch (Exception e)
            {
                return ResponseWrapper.ExceptionResponse(e);
            }

        }

        /// <summary>
        /// 院管理员项目的信息
        /// </summary>
        /// <param name="Status">项目的状态</param>
        /// <param name="page">分页</param>
        [Authorize(Roles = "院管理员")]
        [Route("api/project/college")]
        public HttpResponseMessage GetComboProjectList([FromUri]ProjectStatus Status, [FromUri]int page)
        {
            try
            {
                var user = UserHelper.GetCurrentUser();
                var comboProjectDTO = _repository.GetProjectList(Status).Where(p => user.ProjectTypeIds.Contains(p.ProjectTypeId))
                                      .Select(Mapper.Map</*项目是否一次性取出数据GetComboProjectDTO*/GetProjectDTO>);

                var pageCpmboProject = PagingHelper.PagingWrapper(comboProjectDTO, page, SystemConfig.ApplicationPageCount);

                return ResponseWrapper.SuccessResponse(pageCpmboProject);
            }
            catch (Exception e)
            {
                return ResponseWrapper.ExceptionResponse(e);
            }
        }

        /// <summary>
        /// 院审核结题项目
        /// </summary>
        /// <param name="ProjectId">项目Id</param>
        /// <param name="Pass">项目是否通过</param>
        /// <param name="Comment">审核意见</param>
        [HttpPost]
        [Authorize(Roles = "院管理员")]
        [Route("api/project/college/Auditing/{ProjectId}")]
        public HttpResponseMessage CollegeAuditingProjectFinish(string ProjectId, [FromUri]bool Pass)
        {
            //获得审核意见
            string Comment = Request.Content.ReadAsStringAsync().Result;
            try
            {
                var project = _repository.GetProjectDetailList(ProjectId);
                if (project.Status == ProjectStatus.DEPART_REVIEW)
                {
                    if (Pass)
                    {   //更新项目状态
                        project.Status = ProjectStatus.FINISH;
                        _repository.Update(project);

                        //院管理员审核结题报告通过
                        //通知打点:发给用户所在的单位管理员
                        //_noticeService.AddNoticeList(
                        //    _noticeService.GetInstituteManagerIdList(project.InstituteId.Value), 53);
                        //通知打点:发给用户
                        //_noticeService.AddNotice(
                        //    _noticeService.GetUserIdByPersonId(project.LeaderId.Value), 41,
                        //    new Dictionary<string, string>
                        //    { {"ApplicationName", project.Name } });

                    }
                    else
                    {   //更新项目状态，存放评审意见
                        project.Status = ProjectStatus.DEPART_REJECT;
                        project.Comment = Comment;
                        _repository.Update(project);

                        //院管理员审核结题报告不通过
                        //通知打点:发给用户所在的单位管理员
                        //_noticeService.AddNoticeList(
                        //    _noticeService.GetInstituteManagerIdList(project.InstituteId.Value), 54);
                        //通知打点:发给用户
                        //_noticeService.AddNotice(
                        //    _noticeService.GetUserIdByPersonId(project.LeaderId.Value), 42,
                        //    new Dictionary<string, string>
                        //    { {"ApplicationName", project.Name } });

                    }
                }
                else
                {
                    throw new ModelValidException("项目状态错误");
                }

                return ResponseWrapper.SuccessResponse("审核完成");
            }
            catch (Exception e)
            {
                return ResponseWrapper.ExceptionResponse(e);
            }
        }



        /// <summary>
        /// 项目Id获得项目详细信息（包括成员、文档、任务书）
        /// </summary>
        /// <param name="ProjectId">项目的id</param>
        [Route("api/project/detail/{ProjectId}")]
        public HttpResponseMessage GetProjectDetail(string ProjectId)
        {
            try
            {
                var projectDTO = Mapper.Map<GetProjectDTO>(_repository.GetProjectDetailList(ProjectId));
                return ResponseWrapper.SuccessResponse(projectDTO);
            }
            catch (Exception e)
            {
                return ResponseWrapper.ExceptionResponse(e);
            }
        }

        /// <summary>
        /// 项目Id获得项目参与单位
        /// </summary>
        /// <param name="projectId">项目的id</param>
        [Route("api/project/{projectId}/jionInst")]
        public HttpResponseMessage GetJionInsts( string projectId )
        {
            try
            {
                return ResponseWrapper.SuccessResponse(_repository.GetJionInsts(projectId));
            }
            catch (Exception e)
            {
                return ResponseWrapper.ExceptionResponse(e);
            }
        }
    }
}
