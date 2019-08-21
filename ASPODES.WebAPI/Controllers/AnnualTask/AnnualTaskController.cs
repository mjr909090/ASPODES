using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.IO;
using ASPODES.WebAPI.Repository;
using AutoMapper;
using ASPODES.DTO;
using ASPODES.DTO.AnnualTask;
using ASPODES.Model;
using ASPODES.WebAPI.Common;
using ASPODES.WebAPI.Util;
using ASPODES.Common;
using System.Web;
using ASPODES.Common.Util;
using ASPODES.WebAPI.Service;

namespace ASPODES.WebAPI.Controllers
{
    /// <summary>
    /// 年度任务Controller
    /// </summary>
    public class AnnualTaskController : ApiController
    {
        private IAnnualTaskRepository _repository;
        //private CreateNoticeService _noticeService;
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="repository">年度任务仓储类</param>
        public AnnualTaskController(IAnnualTaskRepository repository)
            //CreateNoticeService noticeService)
        {
            _repository = repository;
            //this._noticeService = noticeService;
        }
        
        /// <summary>
        /// 获取年度任务的详细信息
        /// </summary>
        /// <param name="id">年度任务ID</param>
        /// <returns></returns>
        [Route("api/annualTask/{id}")]
        public HttpResponseMessage GetAnnualTask( int id )
        {
            try
            {
                var taskDTO = _repository.GetAnnualTaskDetailList()
                    .Where( at=>at.AnnualTaskId == id )
                    .Select( Mapper.Map<GetAnnualTaskDTO>)
                    .FirstOrDefault();

                if (taskDTO == null) throw new NotFoundException();

                return ResponseWrapper.SuccessResponse(taskDTO);
            }
            catch( Exception e)
            {
                return ResponseWrapper.ExceptionResponse(e);
            }
        }
        
        /// <summary>
        /// 获取年度任务的基本信息
        /// </summary>
        /// <param name="id">年度任务ID</param>
        /// <returns></returns>
        [Route("api/annualTask/{id}/basic")]
        public HttpResponseMessage GetAnnualTaskBasicInfo( int id )
        {
            try
            {
                //获取当前用户的个人信息
                //var user = UserHelper.GetCurrentUser();
                //获取要访问的资源
                var annualTask = _repository.Get(id);
                //验证当前用户是否有访问该资源的权限。只有项目负责人能够查看年度任务
                //if (user.PersonId != annualTask.LeaderId)
                //    throw new UnauthorizationException("您没有权限访问该资源");

                return ResponseWrapper.SuccessResponse(Mapper.Map<GetAnnualTaskBasicInfoDTO>(annualTask));
            }
            catch (Exception e)
            {
                return ResponseWrapper.ExceptionResponse(e);
            }
        }
        
        /// <summary>
        /// 获取项目的年度任务列表
        /// </summary>
        /// <param name="projectId"></param>
        /// <returns></returns>
        [Route("api/annualTask/{projectId}/tasks")]
        public HttpResponseMessage GetProjectAnnualTaskList( string projectId )
        {
            try
            {
                GetProjectAnnualTaskDTO projectTaskDTO = new GetProjectAnnualTaskDTO();
                var projectTasks = _repository
                    .GetAnnualTaskList()
                    .Where(at => at.ProjectId == projectId)
                    .Select( Mapper.Map<GetAnnualTaskVO> );

                projectTaskDTO.Previous = projectTasks.ToList();
                var current = projectTaskDTO.Previous.FirstOrDefault();
                if (current != null && current.Status < AnnualTaskStatus.FINISH) 
                {
                    projectTaskDTO.Previous.Remove(current);
                    projectTaskDTO.Current = current;
                }
                return ResponseWrapper.SuccessResponse(projectTaskDTO);
            }
            catch( Exception e )
            {
                return ResponseWrapper.ExceptionResponse(e);
            }
        }

        /// <summary>
        /// 保存年度任务
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPut,Route("api/annualTask/save/{id}")]
        public HttpResponseMessage SaveAnnualTask( int id ) 
        {
            try
            {
                var task = _repository.Get(id);
                if( task.Editable() )
                {
                    //生成PDF文档

                    _repository.ChangeAnnualTaskStatus(task, task.Status, AnnualTaskStatus.INST_CHECK);
                    return ResponseWrapper.SuccessResponse();
                }
                return ResponseWrapper.ExceptionResponse( new OtherException("状态不符合提交条件"));
            }
            catch( Exception e )
            {
                return ResponseWrapper.ExceptionResponse(e);
            }
            
        }

        /// <summary>
        /// 项目负责人提交任务书
        /// </summary>
        /// <param name="id">年度任务ID</param>
        /// <returns></returns>
        [HttpPut, Route("api/annualTask/submit/{Id}")]
        public HttpResponseMessage SubmitAnnualTask( int id )
        {
            try
            {
                var task = _repository.SubmitAnnualTask( id );
                var result = Mapper.Map<GetAnnualTaskBasicInfoDTO>(task);

                //用户提交任务书
                //通知打点:发给所在单位所有管理员
                //_noticeService.AddNoticeList(
                //    _noticeService.GetInstituteManagerIdList(task.InstituteId.Value), 46);
                //通知信打点:发给自己
                //_noticeService.AddNotice(
                //    _noticeService.GetUserIdByPersonId(task.LeaderId.Value), 26);

                return ResponseWrapper.SuccessResponse(result);
            }
            catch( Exception e )
            {
                return ResponseWrapper.WarningResponse(e);
            }
        }

        /// <summary>
        /// 生成年度任务的PDF文件
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPut, Route("api/annualTask/createPDF/{Id}")]
        public HttpResponseMessage CreateAnnualTaskPDF(int id)
        {
            try
            {
                _repository.CreatePDF(id);
                return ResponseWrapper.SuccessResponse();
            }
            catch (Exception e)
            {
                return ResponseWrapper.WarningResponse(e);
            }
        }


        /// <summary>
        /// 单位管理员审核任务书
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPut, Route("api/annualTask/instReview/{Id}")]
        public HttpResponseMessage InstReviewAnnualTask(int id, [FromUri] bool pass)
        {
            try
            {
                string comment = Request.Content.ReadAsStringAsync().Result;
                AnnualTask task = _repository.Get(id);
                task = _repository.ChangeAnnualTaskStatus(task, AnnualTaskStatus.INST_CHECK, pass ? AnnualTaskStatus.DEPART_CHECK: AnnualTaskStatus.INST_REJECT, comment);
                var result = Mapper.Map<GetAnnualTaskBasicInfoDTO>(task);

                ////单位管理员审核通过和不通过的情况
                //if (pass)
                //{
                //    //单位管理员审核任务书通过
                //    //通知打点:发给负责相关领域的院管理员
                //    //_noticeService.AddNoticeList(
                //    //    _noticeService.GetDepartManagerIdListByProjectTypeId(task.Project.ProjectTypeId.Value), 55);
                //    //通知信打点:发给用户
                //    _noticeService.AddNotice(
                //        _noticeService.GetUserIdByPersonId(task.LeaderId.Value), 27);
                //}
                //else
                //{
                //    //单位管理员审核任务书不通过
                //    //通知信打点:发给用户
                //    _noticeService.AddNotice(
                //        _noticeService.GetUserIdByPersonId(task.LeaderId.Value), 28);
                //}
                
                return ResponseWrapper.SuccessResponse(result);
            }
            catch (Exception e)
            {
                return ResponseWrapper.ExceptionResponse(e);
            }
        }

        /// <summary>
        /// 院管理员审核任务书
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPut, Route("api/annualTask/departReview/{Id}")]
        public HttpResponseMessage DepartReviewAnnualTask(int id, [FromUri] bool pass )
        {
            try
            {
                string comment = Request.Content.ReadAsStringAsync().Result;
                AnnualTask task = _repository.Get(id);
                task = _repository.ChangeAnnualTaskStatus(task, AnnualTaskStatus.DEPART_CHECK, pass ? AnnualTaskStatus.UPLOAD_ANNUAL_REPORT:AnnualTaskStatus.DEPART_REJECT, comment );
                var result = Mapper.Map<GetAnnualTaskBasicInfoDTO>(task);

                ////院管理员审核任务书通过和不通过的情况
                //if (pass)
                //{
                //    //院管理员审核任务书通过
                //    //通知打点:发给用户所在的单位管理员
                //    //_noticeService.AddNoticeList(
                //    //    _noticeService.GetInstituteManagerIdList(task.InstituteId.Value) , 47);
                //    //通知信打点:发给用户
                //    _noticeService.AddNotice(
                //        _noticeService.GetUserIdByPersonId(task.LeaderId.Value), 29);

                //    //院管理员审核任务书通过，通知其上传年度报告
                //    //通知打点:发给用户
                //    _noticeService.AddNotice(
                //        _noticeService.GetUserIdByPersonId(task.LeaderId.Value), 31);
                //}
                //else
                //{
                //    //院管理员审核任务书不通过
                //    //通知打点:发给用户所在的单位管理员
                //    //_noticeService.AddNoticeList(
                //    //    _noticeService.GetInstituteManagerIdList(task.InstituteId.Value), 48);
                //    //通知信打点:发给用户
                //    _noticeService.AddNotice(
                //        _noticeService.GetUserIdByPersonId(task.LeaderId.Value), 30);
                //}

                return ResponseWrapper.SuccessResponse(result);
            }
            catch (Exception e)
            {
                return ResponseWrapper.ExceptionResponse(e);
            }
        }

        /// <summary>
        /// 单位管理员审核年度报告
        /// </summary>
        /// <param name="id"></param>
        /// <param name="pass"></param>
        /// <returns></returns>
        [HttpPut, Route("api/annualTask/instReviewAnnualReport/{id}")]
        public HttpResponseMessage InstReviewAnnualReport( int id,  [FromUri]bool pass )
        {
            try
            {
                string comment = Request.Content.ReadAsStringAsync().Result;
                AnnualTask task = _repository.Get(id);
                task = _repository.ChangeAnnualTaskStatus(task, AnnualTaskStatus.INST_REVIEW_ANNUAL_REPORT, pass ? AnnualTaskStatus.DEPART_REVIEW_ANNUAL_REPORT : AnnualTaskStatus.INST_REJECT_ANNUAL_REPORT, pass ? "" : "单位管理员驳回原因：" + comment);
                var result = Mapper.Map<GetAnnualTaskBasicInfoDTO>(task);

                //if (pass)
                //{
                //    //单位管理员审核年度报告通过
                //    //通知打点:发给负责相关领域的院管理员
                //    //_noticeService.AddNoticeList(
                //    //    _noticeService.GetDepartManagerIdListByProjectTypeId(task.Project.ProjectTypeId.Value), 56);
                //    //通知信打点:发给用户
                //    _noticeService.AddNotice(
                //        _noticeService.GetUserIdByPersonId(task.LeaderId.Value), 33);
                //}
                //else
                //{
                //    //单位管理员审核年度报告不通过
                //    //通知信打点:发给用户
                //    _noticeService.AddNotice(
                //        _noticeService.GetUserIdByPersonId(task.LeaderId.Value), 34);
                //}

                return ResponseWrapper.SuccessResponse(result);
            }
            catch (Exception e)
            {
                return ResponseWrapper.ExceptionResponse(e);
            }
        }

        /// <summary>
        /// 院管理员审核年度报告
        /// </summary>
        /// <param name="id"></param>
        /// <param name="pass"></param>
        /// <returns></returns>
        [HttpPut, Route("api/annualTask/departReviewAnnualReport/{id}")]
        public HttpResponseMessage DepartReviewAnnualReport(int id, [FromUri]bool pass)
        {
            try
            {
                string comment = Request.Content.ReadAsStringAsync().Result;
                AnnualTask task = _repository.Get(id);
                task = _repository.ChangeAnnualTaskStatus(task, AnnualTaskStatus.DEPART_REVIEW_ANNUAL_REPORT, pass ? AnnualTaskStatus.FINISH : AnnualTaskStatus.DEPART_REJECT_ANNUAL_REPORT, pass ? "":"院管理员驳回原因：" + comment);
                var result = Mapper.Map<GetAnnualTaskBasicInfoDTO>(task);

                ////院管理员审核年度报告通过和不通过的情况
                //if (pass)
                //{
                //    //院管理员审核年度报告通过
                //    //通知打点:发给用户所在的单位管理员
                //    //_noticeService.AddNoticeList(
                //    //    _noticeService.GetInstituteManagerIdList(task.InstituteId.Value), 50);
                //    //通知信打点:发给用户
                //    _noticeService.AddNotice(
                //        _noticeService.GetUserIdByPersonId(task.LeaderId.Value), 35);
                //}
                //else
                //{
                //    //院管理员审核年度报告不通过
                //    //通知打点:发给用户所在的单位管理员
                //    //_noticeService.AddNoticeList(
                //    //    _noticeService.GetInstituteManagerIdList(task.InstituteId.Value), 51);
                //    //通知信打点:发给用户
                //    _noticeService.AddNotice(
                //        _noticeService.GetUserIdByPersonId(task.LeaderId.Value), 36);
                //}

                return ResponseWrapper.SuccessResponse(result);
            }
            catch (Exception e)
            {
                return ResponseWrapper.ExceptionResponse(e);
            }
        }

        /// <summary>
        /// 单位管理员获取待审核的年度任务列表
        /// </summary>
        /// <param name="page"></param>
        /// <returns></returns>
        [Route("api/annualTask/instAdmin/review")]
        public HttpResponseMessage GetInstReviewAnnualTask( [FromUri] AnnualTaskStatus status, [FromUri]int page )
        {
            try
            {
                page = page <= 0 ? 1 : page;
                var userInfo = UserHelper.GetCurrentUser();
                var reviewTaskDTOs = _repository
                    .GetAnnualTaskList(userInfo.InstId, status, SystemConfig.ApplicationStartYear )
                    .Select(Mapper.Map<GetAnnualTaskBasicInfoDTO>);

                var pagingList = PagingHelper.PagingWrapper(reviewTaskDTOs, page, SystemConfig.AnnualTaskPageCount); ;

                return ResponseWrapper.SuccessResponse(pagingList);
            }
            catch( Exception e )
            {
                return ResponseWrapper.ExceptionResponse(e);
            }
        }



        /// <summary>
        /// 院管理员获取待审核的年度任务列表
        /// </summary>
        /// <param name="page">页码</param>
        /// <returns></returns>
        [Route("api/annualTask/departAdmin/review")]
        public HttpResponseMessage GetDepartReviewAnnualTask([FromUri] AnnualTaskStatus status, [FromUri]int page)
        {
            try
            {
                page = page <= 0 ? 1 : page;

                var userInfo = UserHelper.GetCurrentUser();

                var reviewTaskDTOs = _repository
                    .GetAnnualTaskList(0, status, SystemConfig.ApplicationStartYear )
                    .Where(at => userInfo.ProjectTypeIds.Contains(at.Project.ProjectTypeId) )
                    .Select(Mapper.Map<GetAnnualTaskBasicInfoDTO>);

                var pagingList = PagingHelper.PagingWrapper(reviewTaskDTOs, page, SystemConfig.AnnualTaskPageCount); 

                return ResponseWrapper.SuccessResponse(pagingList);
            }
            catch (Exception e)
            {
                return ResponseWrapper.ExceptionResponse(e);
            }
        }

    }
}
