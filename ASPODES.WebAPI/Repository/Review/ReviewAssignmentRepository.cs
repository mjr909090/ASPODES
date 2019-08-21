using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Net.Http;
using ASPODES.Database;
using ASPODES.WebAPI.Common;
using System.Data.Entity;
using ASPODES.Model;
using System.Data.Common;
using System.Data.SqlClient;
using System.Text;
using AutoMapper;
using ASPODES.DTO.Review;
using ASPODES.DTO;

namespace ASPODES.WebAPI.Repository
{
    /// <summary>
    /// 指派专家操作类
    /// </summary>
    public class ReviewAssignmentRepository
    {
        /// <summary>
        /// 受理申请书时指派专家
        /// </summary>
        /// <param name="applicationId"></param>
        /// <param name="privilege"></param>
        /// <returns></returns>
        public List<GetReviewAssignmentDTO> ApplicationExpertAssignment(string applicationId, Func<Model.Application, bool> privilege)
        {

            using (var ctx = new AspodesDB())
            {
                //申请书是否存在
                var application = ctx.Applications.FirstOrDefault(a => a.ApplicationId == applicationId);
                if (null == application) throw new NotFoundException();

                //验证权限
                if (!privilege(application)) throw new UnauthorizationException();

                //检查状态
                if (application.Status != Model.ApplicationStatus.ASSIGNMENT)
                    throw new OtherException("申请书状态错误");

                //取出申请书的研究子领域
                var applicationFields = ctx.ApplicationFields
                    .Where(af => af.ApplicationId == applicationId)
                    .Select(af => af.SubFieldId)
                    .ToList();

                //取出所有专家的UserId
                var expertIds = from authorize in ctx.Authorizes
                                where authorize.RoleId == 5
                                select authorize.UserId;

                //取出研究领域相关的专家UserId,UserId可能重复，不影响最后结果
                var relativeExpertIds = from expertField in ctx.ExpertFields
                                        where applicationFields.Contains(expertField.SubFieldId) && expertIds.Contains(expertField.UserId)
                                        select expertField.UserId;

                //取出不属于项目承担单位，已指派申请书数目未满，研究领域相关的的专家列表
                var candidateExpertIds = from user in ctx.Users
                                         where user.ReviewAmount < SystemConfig.ExpertReviewAmount && user.InstituteId != application.InstituteId && relativeExpertIds.Contains(user.UserId)
                                         orderby user.ReviewAmount
                                         select user.UserId;

                //需要指派的专家
                var assignmentExpertIds = candidateExpertIds.Take(SystemConfig.ApplicationExpertAmount).ToList();

                //添加专家指派
                var assigments = AddReviewAssignments(applicationId, ctx, assignmentExpertIds);
                application.Status = SystemConfig.ApplicationExpertAmount <= assigments.Count() ? ApplicationStatus.SEND_ASSIGNMENT : ApplicationStatus.MANUAL_ASSIGNMENT;
                ctx.SaveChanges();

                return assigments.Select(Mapper.Map<GetReviewAssignmentDTO>).ToList();
            }

        }

        /// <summary>
        /// 指派专家
        /// </summary>
        /// <param name="applicationId"></param>
        /// <param name="ctx"></param>
        /// <param name="expertList"></param>
        private static List<ReviewAssignment> AddReviewAssignments(string applicationId, AspodesDB ctx, IEnumerable<string> expertList)
        {
            List<ReviewAssignment> assignments = new List<ReviewAssignment>();
            foreach (var expert in expertList)
            {
                var user = ctx.Users.FirstOrDefault(u => u.UserId == expert);
                user.ReviewAmount += 1;
                ReviewAssignment assignment = new ReviewAssignment()
                {
                    ApplicationId = applicationId,
                    ExpertId = user.UserId,
                    Accept = false,
                    Year = SystemConfig.ApplicationStartYear,
                    Overdue = false,
                    Status = ReviewAssignmentStatus.NEW
                };
                var newAssignment = ctx.ReviewAssignments.Add(assignment);
                assignments.Add(newAssignment);
            }
            return assignments;
        }

        /// <summary>
        /// 集合中的申请书从专家集合中指派初审专家
        /// </summary>
        /// <param name="applicationIds"></param>
        /// <param name="expertIds"></param>
        /// <param name="privilege"></param>
        /// <returns></returns>
        public List<GetReviewAssignmentDTO> AddReviewAssignments(List<string> applicationIds, List<string> expertIds, Func<Model.Application, bool> privilege)
        {

            using (var ctx = new AspodesDB())
            {

                StringBuilder responseMsg = new StringBuilder();
                List<ReviewAssignment> total = new List<ReviewAssignment>();
                List<ReviewAssignment> assignments = new List<ReviewAssignment>();
                foreach (var applicationId in applicationIds)
                {

                    assignments.Clear();

                    var application = ctx.Applications.FirstOrDefault(a => a.ApplicationId == applicationId);
                    if (null == application)
                    {
                        responseMsg.Append(applicationId + "未找到申请书\n");
                        continue;
                    }

                    //检查权限
                    if (!privilege(application))
                    {
                        responseMsg.Append(applicationId + "没有权限为此申请书分配专家\n");
                        continue;
                    }

                    //检查申请书状态
                    if (application.Status != Model.ApplicationStatus.MANUAL_ASSIGNMENT)
                    {
                        responseMsg.Append(applicationId + "状态错误");
                        continue;
                    }

                    //选出申请书的研究子领域
                    var applicationFields = ctx.ApplicationFields
                        .Where(af => af.ApplicationId == applicationId)
                        .Select(af => af.SubFieldId)
                        .ToList();

                    //未满的专家数目
                    var amount = SystemConfig.ApplicationExpertAmount - ctx.ReviewAssignments.Count(ra => ra.ApplicationId == applicationId && !ra.Overdue.Value && ra.Status != ReviewAssignmentStatus.CHANGE && ra.Status != ReviewAssignmentStatus.REFUSE);
                    if (amount <= 0) continue;

                    //本年度已经指派给该申请书的专家
                    var applicationAssignedExpertIds = from assignment in ctx.ReviewAssignments
                                                       where !assignment.Overdue.Value && assignment.ApplicationId == application.ApplicationId
                                                       select assignment.ExpertId;

                    //选出能够指派给该申请书的专家，不能是本单位的，不能是已经指派的
                    var candidateExpertIds = from user in ctx.Users
                                             where user.InstituteId != application.InstituteId && !applicationAssignedExpertIds.Contains(user.UserId) && expertIds.Contains(user.UserId)
                                             orderby user.ReviewAmount
                                             select user.UserId;

                    //选出能够指派的研究领域相关的专家
                    var relativeExpertIds = from expertField in ctx.ExpertFields
                                            where applicationFields.Contains(expertField.SubFieldId) && candidateExpertIds.Contains(expertField.UserId)
                                            select expertField.UserId;
                    //选出指派候专家
                    var relativeReviewExpertIds = relativeExpertIds.Distinct().Take(amount);

                    //添加指派
                    assignments.AddRange(AddReviewAssignments(applicationId, ctx, relativeReviewExpertIds));
                    ctx.SaveChanges();

                    //错误，执行添加后relativeReviewExpertIds.Count()会有变化
                    //int randomExpertAmount = amount - relativeReviewExpertIds.Count(); //需要随机指派的专家数目
                    //var last = 0; //最后未满的专家数目

                    //如果不满添加领域不相关专家
                    if ( amount - assignments.Count() > 0)
                    {

                        var randomReviewExpert = candidateExpertIds.Except(applicationAssignedExpertIds).Take(amount - assignments.Count());
                        //last = randomExpertAmount - randomReviewExpert.Count();
                        assignments.AddRange(AddReviewAssignments(applicationId, ctx, randomReviewExpert));
                    }
                    application.Status = amount - assignments.Count() <= 0 ? ApplicationStatus.SEND_ASSIGNMENT : ApplicationStatus.MANUAL_ASSIGNMENT;
                    ctx.SaveChanges();
                    total.AddRange(assignments);
                }

                return total.Select(Mapper.Map<GetReviewAssignmentDTO>).ToList();
            }

        }

        /// <summary>
        /// 获取要参加评审的专家ID列表（要评审的申请书数量以后加上）
        /// </summary>
        /// <param name="appIdList"></param>
        /// <returns></returns>
        internal List<string> GetExpertIdList(List<string> appIdList)
        {
            using (var _context = new AspodesDB())
            {
                return (from ra in _context.ReviewAssignments
                where (appIdList).Contains(ra.ApplicationId)
                select ra.ExpertId).ToList();
            }
        }

        /// <summary>
        /// 更换初审专家,更换拒绝的初审专家
        /// </summary>
        /// <param name="applicationId"></param>
        /// <param name="oldExpertId"></param>
        /// <param name="newExpertId"></param>
        /// <param name="privilege"></param>
        /// <returns></returns>
        public GetReviewAssignmentDTO ChangeExpert(string applicationId, string oldExpertId, string newExpertId, Func<Model.Application, bool> privilege)
        {
            using (var ctx = new AspodesDB())
            {
                var application = ctx.Applications.FirstOrDefault(a => a.ApplicationId == applicationId);
                if (null == application) throw new NotFoundException("未找到申请书");

                if (!privilege(application)) throw new UnauthorizationException("您没有权限进行操作");

                if (!(application.Status == Model.ApplicationStatus.ASSIGNMENT || application.Status == Model.ApplicationStatus.MANUAL_ASSIGNMENT || application.Status == Model.ApplicationStatus.SEND_ASSIGNMENT))
                    throw new OtherException("不允许操作");

                if (oldExpertId.ToLower() != "empty")
                {
                    //var oldExpert = ctx.Users.FirstOrDefault(u => u.UserId == oldExpertId);
                    //if (UserHelper.UnnormalUser(oldExpert)) throw new NotFoundException("没有找到旧的专家");

                    var oldAssignment = ctx.ReviewAssignments.FirstOrDefault(ra => !ra.Overdue.Value && ra.ApplicationId == applicationId && ra.ExpertId == oldExpertId);
                    if (oldAssignment == null) throw new NotFoundException("没有找到旧的指派记录");
                    if( !(oldAssignment.Status == ReviewAssignmentStatus.NEW || oldAssignment.Status == ReviewAssignmentStatus.CHANGE))
                    {
                        throw new OtherException("专家指派的状态错误，不允许替换");
                    }
                    
                    oldAssignment.Status = ReviewAssignmentStatus.CHANGE;
                    oldAssignment.Expert.ReviewAmount -= 1;
                }

                var newExpert = ctx.Users.FirstOrDefault(u => u.UserId == newExpertId);
                if (UserHelper.UnnormalUser(newExpert)) throw new NotFoundException("没有找到新的专家");

                if (newExpert.InstituteId == application.InstituteId) throw new OtherException("不能指派给本单位的专家");

                if (ctx.ReviewAssignments.Any(ra => !ra.Overdue.Value && ra.ApplicationId == application.ApplicationId && ra.ExpertId == newExpert.UserId))
                    throw new OtherException("该专家已经是该申请书的评审人或者专家拒审");

                ReviewAssignment newAssignment = new ReviewAssignment
                {
                    ApplicationId = application.ApplicationId,
                    ExpertId = newExpert.UserId,
                    Accept = false,
                    Status = ReviewAssignmentStatus.NEW,
                    Overdue = false,
                    Year = SystemConfig.ApplicationStartYear
                };

                var updateassignment = ctx.ReviewAssignments.Add(newAssignment);
                newExpert.ReviewAmount += 1;
                ctx.SaveChanges();

                if (ctx.ReviewAssignments.Count(ra => 
                    !ra.Overdue.Value 
                    && ra.ApplicationId == applicationId
                    && ra.Status != ReviewAssignmentStatus.CHANGE
                    && ra.Status != ReviewAssignmentStatus.REFUSE) >= SystemConfig.ApplicationExpertAmount)
                {
                    application.Status = ApplicationStatus.SEND_ASSIGNMENT;
                }

                ctx.SaveChanges();
                return Mapper.Map<GetReviewAssignmentDTO>(updateassignment);
            }

        }

        /// <summary>
        /// 获取专家指派的列表
        /// </summary>
        /// <param name="predicate">选择条件</param>
        /// <returns></returns>
        public List<GetReviewAssignmentDTO> GetReviewAssignmentList(Func<ReviewAssignment, bool> predicate)
        {
            List<GetReviewAssignmentDTO> assignmentDTDs = null;
            using (var ctx = new AspodesDB())
            {
                assignmentDTDs = ctx.ReviewAssignments
                    .Where(predicate)
                    .Select(Mapper.Map<GetReviewAssignmentDTO>)
                    .ToList();
                return assignmentDTDs;
            }

        }

        /// <summary>
        /// 获取专家待评审申请书数量
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public int GetReviewAssignmentCount(string userId)
        {
            using (var ctx = new AspodesDB())
            {
                return ctx.ReviewAssignments
                    .Where(r => r.Overdue.Value == false && r.ExpertId == userId && r.Status == ReviewAssignmentStatus.WAITE_REVIEW)
                    .Count();
            }
        }

        /// <summary>
        /// 获取专家指派的列表,按研究子领域筛选
        /// </summary>
        /// <returns></returns>
        public PagingListDTO<GetApplicationReviwAssignmentDTO> GetPagingReviewAssignmentList(string fieldId, string subFieldId, Func<Application, bool> predicate, int page)
        {
            PagingListDTO<GetApplicationReviwAssignmentDTO> pagingList = new PagingListDTO<GetApplicationReviwAssignmentDTO>();
            var userInfo = UserHelper.GetCurrentUser();

            pagingList.NowPage = page <= 0 ? 1 : page;
            using (var ctx = new AspodesDB())
            {
                List<string> subFields = new List<string>();
                if (fieldId == "all")
                {
                    subFields.AddRange(ctx.SubFields.Select(sf => sf.SubFieldName).ToList());
                }
                else if (subFieldId == "all")
                {
                    subFields.AddRange(ctx.SubFields.Where(sf => sf.ParentName == fieldId).Select(sf => sf.SubFieldName).ToList());
                }
                else
                {
                    subFields.Add(subFieldId);
                }

                var applicationIds = from applicationField in ctx.ApplicationFields
                                     where subFields.Contains(applicationField.SubFieldId)
                                     select applicationField.ApplicationId;

                var applications = from application in ctx.Applications
                                   where applicationIds.Contains(application.ApplicationId) && userInfo.ProjectTypeIds.Contains(application.ProjectTypeId)
                                   select application;

                pagingList.TotalNum = applications.Count(predicate);
                pagingList.TotalPageNum = (pagingList.TotalNum + SystemConfig.ReviewAssignmentPageCount - 1) / SystemConfig.ReviewAssignmentPageCount;
                if (pagingList.TotalPageNum <= 0) pagingList.TotalPageNum = 1;

                pagingList.ItemDTOs = applications
                    .Where(predicate)
                    .Skip((pagingList.NowPage - 1) * SystemConfig.ReviewAssignmentPageCount)
                    .Take(SystemConfig.ReviewAssignmentPageCount)
                    .Select(Mapper.Map<GetApplicationReviwAssignmentDTO>)
                    .ToList();

                pagingList.NowNum = pagingList.ItemDTOs.Count();
                return pagingList;
            }
        }

        /// <summary>
        /// 发送专家指派
        /// </summary>
        /// <returns></returns>
        public void SendReviewAssignment()
        {
            using (var ctx = new AspodesDB())
            {
                var userInfo = UserHelper.GetCurrentUser();
                var applications = ctx.Applications
                    .Where(a => a.CurrentYear == SystemConfig.ApplicationStartYear
                        && a.Status == ApplicationStatus.SEND_ASSIGNMENT
                        && userInfo.ProjectTypeIds.Contains(a.ProjectTypeId));

                foreach (var application in applications)
                {
                    //获取今年的指派
                    var assignments = ctx.ReviewAssignments
                        .Where(ra => ra.ApplicationId == application.ApplicationId && !ra.Overdue.Value && ra.Status != ReviewAssignmentStatus.CHANGE)
                        .ToList();
                    //删除多余的专家指派
                    if (assignments.Count() > SystemConfig.ApplicationExpertAmount)
                    {
                        var extra = assignments.Skip(SystemConfig.ApplicationExpertAmount).Take(assignments.Count());
                        assignments.RemoveRange(SystemConfig.ApplicationExpertAmount, extra.Count());
                        ctx.ReviewAssignments.RemoveRange(extra);
                    }
                    //修改指派状态
                    foreach (var assignment in assignments)
                    {
                        if (assignment.Status == ReviewAssignmentStatus.NEW)
                        {
                            assignment.Status = ReviewAssignmentStatus.WAITE_REVIEW;
                        }
                    }
                    //修改申请书状态
                    application.Status = ApplicationStatus.REVIEW;
                }
                ctx.SaveChanges();
            }
        }

        /// <summary>
        /// 判断是否自动分配专家完毕，分配专家数量是在配置文件中配置的
        /// </summary>
        /// <param name="applicationId"></param>
        /// <returns></returns>
        public bool CompleteAssignment(string applicationId)
        {
            using (var ctx = new AspodesDB())
            {
                return ctx.ReviewAssignments
                    .Count(ra => ra.ApplicationId == applicationId) >= SystemConfig.ApplicationExpertAmount;
            }
        }
    }
}