using ASPODES.DTO.System;
using ASPODES.Model;
using ASPODES.WebAPI.Common;
using ASPODES.WebAPI.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web;
using System.Web.Http;

namespace ASPODES.WebAPI.Controllers.System
{
    /// <summary>
    /// 获得各项待审核数量
    /// </summary>
    public class NumberController : ApiController
    {
        private ApplicationRepository _applicationrepository = new ApplicationRepository();
        private IProjectRepository _projectrepository;
        private IAnnualTaskRepository _annualtaskrepository;
        private ReviewAssignmentRepository _reviewAssignment;

        /// <summary>
        /// 构造函数
        /// </summary>
        public NumberController(IProjectRepository projectRepository, IAnnualTaskRepository annualtaskrepository, ReviewAssignmentRepository reviewAssignmentRepository)
        {
            _projectrepository = projectRepository;
            _annualtaskrepository = annualtaskrepository;
            _reviewAssignment = reviewAssignmentRepository;
        }


        /// <summary>
        /// 获取各项待审核数量
        /// </summary>
        /// <returns></returns>
        public HttpResponseMessage Get()
        {
            NumberListDTO numlist = new NumberListDTO();
            try
            {
                var user = UserHelper.GetCurrentUser();
                if(user.Roles[0] == "系统管理员" && user.Roles.Count() == 1) return ResponseWrapper.SuccessResponse("无");
                if (user.ProjectTypeIds != null && user.ProjectTypeIds.Count()!=0)
                {
                    //院待受理申请书数量
                    numlist.DepartAppNum = _applicationrepository.GetAcceptApplicationNumber(user.ProjectTypeIds);
                    //院待审核结题项目数量
                    numlist.DepartProjectNum = _projectrepository.GetProjectList(ProjectStatus.DEPART_REVIEW)
                                      .Where(p => user.ProjectTypeIds.Contains(p.ProjectTypeId)).Count();
                    //院待审核任务书数量
                    numlist.DepartATBookNum = _annualtaskrepository.GetAnnualTaskList(0, AnnualTaskStatus.DEPART_CHECK, SystemConfig.ApplicationStartYear)
                                         .Where(at => user.ProjectTypeIds.Contains(at.Project.ProjectTypeId)).Count();
                    //院待审核年度报告数量
                    numlist.DepartATTalkNum = _annualtaskrepository.GetAnnualTaskList(0, AnnualTaskStatus.DEPART_REVIEW_ANNUAL_REPORT, SystemConfig.ApplicationStartYear)
                                         .Where(at => user.ProjectTypeIds.Contains(at.Project.ProjectTypeId)).Count();
                }
                if(user.Roles.Contains("单位管理员"))
                {
                    //单位待审核申请书数量
                    numlist.IntAppNum = _applicationrepository.GetCheckApplicationNumber(user.InstId);

                    //单位待审核结题项目数量
                    numlist.IntProjectNum = _projectrepository.GetInstituteProjectList(user.InstId, ProjectStatus.INST_REVIEW).Count();

                    //单位待审核任务书数量
                    numlist.IntATBookNum = _annualtaskrepository.GetAnnualTaskList(user.InstId, AnnualTaskStatus.INST_CHECK, SystemConfig.ApplicationStartYear).Count();
                    //单位待审核年度报告数量
                    numlist.IntATTalkNum = _annualtaskrepository.GetAnnualTaskList(user.InstId, AnnualTaskStatus.INST_REVIEW_ANNUAL_REPORT, SystemConfig.ApplicationStartYear).Count();
                }
                if (user.Roles.Contains("专家"))
                {
                    //专家待审核申请书数量
                    numlist.CheckingAppNum = _reviewAssignment.GetReviewAssignmentCount(user.UserId);

                }
                //申请人获得被院不受理的申请书数量
                numlist.DepartRefuseAppNum = _applicationrepository.GetApplicationList(a => a.LeaderId == user.PersonId && a.Status == ApplicationStatus.REFUSE).Count();
                
                //申请人获得被单位驳回的申请书数量
                numlist.IntRejectAppNum = _applicationrepository.GetApplicationList(a => a.LeaderId == user.PersonId && a.Status == ApplicationStatus.REJECT).Count();
                
                //申请人获得被院驳回的结题项目数量
                numlist.DepartRejectProjectNum = _projectrepository.GetLeaderProjectList(user.PersonId, ProjectStatus.DEPART_REJECT).Count();
                
                //申请人获得被单位驳回的结题项目数量
                numlist.IntRejectProjectNum = _projectrepository.GetLeaderProjectList(user.PersonId, ProjectStatus.INST_REJECT).Count();

                //申请人获得被院驳回的任务书数量
                numlist.DepartRejectATBookNum = _annualtaskrepository.GetAnnualTaskList(user.PersonId).Where(at => at.Status == AnnualTaskStatus.DEPART_REJECT).Count();

                //申请人获得被单位驳回的任务书数量
                numlist.IntRejectProjectNum = _annualtaskrepository.GetAnnualTaskList(user.PersonId).Where(at => at.Status == AnnualTaskStatus.INST_REJECT).Count();

                //申请人获得被院驳回的年度报告数量
                numlist.DepartRejectProjectNum = _annualtaskrepository.GetAnnualTaskList(user.PersonId).Where(at => at.Status == AnnualTaskStatus.DEPART_REJECT_ANNUAL_REPORT).Count();

                //申请人获得被单位驳回的年度报告数量
                numlist.IntRejectProjectNum = _annualtaskrepository.GetAnnualTaskList(user.PersonId).Where(at => at.Status == AnnualTaskStatus.INST_REJECT_ANNUAL_REPORT).Count();

                return ResponseWrapper.SuccessResponse(numlist);
            }
            catch (Exception e)
            {
                return ResponseWrapper.ExceptionResponse(e);
            }
        }
    }
}