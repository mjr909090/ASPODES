using ASPODES.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using System.Data.Entity.Migrations;

using System.Net;
using System.Net.Http;

using ASPODES.DTO.Application;
using ASPODES.DTO.Inst_Person_User;
using ASPODES.WebAPI.Common;
using AutoMapper;
using ASPODES.Model;
using ASPODES.Database;
using System.Data.Entity;
using ASPODES.WebAPI;
using ASPODES.Common.Util;
using System.Security.Principal;
using ASPODES.WebAPI.Security;
using ASPODES.DTO.Category;
using ASPODES.DTO.Review;


namespace ASPODES.WebAPI.Repository
{
    /// <summary>
    /// 申请书操作类
    /// </summary>
    public class ApplicationRepository
    {
        //还不能用依赖注入
        //private AspodesDB _context;
        //public ApplicationRepository(AspodesDB context)
        //{
        //    this._context = context;
        //}
        /// <summary>
        /// 获取一份申请书的基本信息
        /// </summary>
        /// <param name="id">申请书ID</param>
        /// <returns>
        /// 成功，返回ResponseStatus.success和申请书基本信息
        /// 没有找到数据，返回ResponseStatus.parameter_error,
        /// 出错，返回ResponseStatus.unknown_error和错误信息
        /// 没有权限访问，返回ResponseStatus.unauthorize
        /// </returns>
        public GetApplicationDTO GetOneApplication(string id)
        {
            GetApplicationDTO appDTO = null;

            using (var ctx = new AspodesDB())
            {
                var app = ctx.Applications.FirstOrDefault(a => a.ApplicationId == id);
                if (app == null)
                    throw new NotFoundException("未找到申请书");
                appDTO = Mapper.Map<GetApplicationDTO>(app);
                var pdf = ctx.ApplicationDocs.FirstOrDefault(ad => ad.ApplicationId == appDTO.ApplicationId && ad.Type == ApplicationDocType.PDF);
                if (pdf != null) appDTO.PDF = pdf.RelativeURL.Replace("~", "");
            }

            return appDTO;
        }

        /// <summary>
        /// 删除申请书草稿
        /// </summary>
        /// <param name="applicationId">申请书ID</param>
        /// <returns></returns>
        public void Delete(string applicationId, Func<Application, bool> privilege)
        {

            using (var ctx = new AspodesDB())
            {
                var application = ctx.Applications.FirstOrDefault(a => a.ApplicationId == applicationId);
                if (null == application)
                    throw new NotFoundException("未找到申请书");
                if (!privilege(application))
                    throw new UnauthorizedAccessException("您没有权限删除该申请书");
                if (application.Status >= ApplicationStatus.CHECK)
                    throw new OtherException("该申请书不允许删除");
                ctx.Applications.Remove(application);
                ctx.SaveChanges();
            }
        }


        /// <summary>
        /// 获取申请项目的参与单位,包括主持单位和协作单位
        /// </summary>
        /// <param name="applicationId">申请书ID</param>
        /// <returns>
        /// 成功,返回ResponseStatus.success和参与单位列表
        /// 没有找申请书,返回ResponseStatus.parameter_error,
        /// 出错,返回ResponseStatus.unknown_error和错误信息
        /// </returns>
        public List<GetComboInstDTO> GetParticipateInst(string applicationId)
        {
            List<GetComboInstDTO> insts = new List<GetComboInstDTO>();

            using (var ctx = new AspodesDB())
            {
                var application = ctx.Applications.FirstOrDefault(a => a.ApplicationId == applicationId);
                if (application == null) throw new NotFoundException("未找到申请书");

                //获取成员的人员信息列表
                var personsQuery = from m in ctx.Members
                                   where m.ApplicationId == applicationId
                                   select m.Person;

                //获取参加单位列表
                var instsQuery = from person in personsQuery select person.Institute;

                //删除重复的单位
                var distinctInstsQuery = from inst in instsQuery
                                         group inst by inst.InstituteId into instGroup
                                         select instGroup.FirstOrDefault();

                //删除主持单位
                //var jionInst = distinctInstsQuery.Where(i => i.InstituteId != application.InstituteId);
                return distinctInstsQuery.Select(Mapper.Map<GetComboInstDTO>).ToList();
            }
        }

        /// <summary>
        /// 获取申请书列表
        /// </summary>
        /// <param name="predicate"></param>
        /// <returns></returns>
        public List<GetApplicationDTO> GetApplicationList(Func<Application, bool> predicate)
        {
            List<GetApplicationDTO> applicationDTOs = new List<GetApplicationDTO>();

            using (var ctx = new AspodesDB())
            {
                var applications = ctx.Applications
                    .Where(predicate)
                    .OrderByDescending(a => a.EditTime)
                    .Select(Mapper.Map<GetApplicationDTO>);

                //如果不需要PDF预览功能，删除下面的循环
                foreach (var applicationDTO in applications)
                {
                    var pdf = ctx.ApplicationDocs.FirstOrDefault(ad => ad.ApplicationId == applicationDTO.ApplicationId && ad.Type == ApplicationDocType.PDF);
                    if (pdf != null) applicationDTO.PDF = pdf.RelativeURL.Replace("~", "");
                    applicationDTOs.Add(applicationDTO);
                }
            }

            return applicationDTOs;
        }

        /// <summary>
        /// 获取分页的申请书列表
        /// </summary>
        /// <param name="predicate"></param>
        /// <param name="page"></param>
        /// <returns></returns>
        public PagingListDTO<GetApplicationDTO> GetPagingApplicationList(Func<Application, bool> predicate, int page)
        {
            PagingListDTO<GetApplicationDTO> pagingList = new PagingListDTO<GetApplicationDTO>();

            using (var ctx = new AspodesDB())
            {
                pagingList.TotalNum = ctx.Applications.Where(predicate).Count();
                pagingList.TotalPageNum = (pagingList.TotalNum + SystemConfig.ApplicationPageCount - 1) / SystemConfig.ApplicationPageCount;
                if (pagingList.TotalNum <= 0) pagingList.TotalPageNum = 1;

                pagingList.NowPage = page <= 0 ? 1 : page;

                var applications = ctx.Applications
                    .Where(predicate)
                    .OrderByDescending(a => a.EditTime)
                    .Skip((pagingList.NowPage - 1) * SystemConfig.ApplicationPageCount)
                    .Take(SystemConfig.ApplicationPageCount);

                //如果不需要PDF预览功能，删除下面的循环
                foreach (var application in applications)
                {
                    var applicationDTO = Mapper.Map<GetApplicationDTO>(application);
                    var pdf = ctx.ApplicationDocs.FirstOrDefault(ad => ad.ApplicationId == application.ApplicationId && ad.Type == ApplicationDocType.PDF);
                    if (pdf != null) applicationDTO.PDF = pdf.RelativeURL.Replace("~", "");
                    pagingList.ItemDTOs.Add(applicationDTO);
                }

                pagingList.NowNum = pagingList.ItemDTOs.Count();
            }

            return pagingList;
        }



        /// <summary>
        /// 修改申请书基本信息
        /// </summary>
        /// <param name="applicationDTO"></param>
        /// <param name="privilege"></param>
        /// <returns></returns>
        public GetApplicationDTO UpdateApplication(AddApplicationDTO applicationDTO, Func<Application, CurrentUser, bool> privilege)
        {

            using (var ctx = new AspodesDB())
            {
                var oldValue = ctx.Applications.FirstOrDefault(a => a.ApplicationId == applicationDTO.ApplicationId);
                if (null == oldValue)
                    throw new NotFoundException("未找到申请书");

                if (!privilege(oldValue, UserHelper.GetCurrentUser()))
                    throw new UnauthorizationException();

                var newValue = Mapper.Map<Application>(applicationDTO);

                //不允许在Update中修改的属性值
                newValue.LeaderId = oldValue.LeaderId;
                newValue.YearCreated = oldValue.YearCreated;
                newValue.CurrentYear = oldValue.CurrentYear;
                newValue.AppliyTimes = oldValue.AppliyTimes;
                newValue.AppliyTimes = oldValue.AppliyTimes;
                newValue.Status = oldValue.Status;
                newValue.InstituteId = oldValue.InstituteId;
                newValue.ContactId = oldValue.ContactId;
                newValue.ContactEmail = oldValue.ContactEmail;
                newValue.ContactPhone = oldValue.ContactPhone;
                newValue.LeaderEmail = oldValue.LeaderEmail;
                newValue.LeaderPhone = oldValue.LeaderPhone;

                if (oldValue.Period != newValue.Period)
                {
                    //删除年度预算
                    var annualBudgets = from annualBudget in ctx.AnnualBudgets
                                        where annualBudget.ApplicationId == oldValue.ApplicationId
                                        select annualBudget;
                    ctx.AnnualBudgets.RemoveRange(annualBudgets.ToList());
                    //删除单位预算
                    var instBudgets = from instBudget in ctx.InstBudgets
                                      where instBudget.ApplicationId == oldValue.ApplicationId
                                      select instBudget;
                    ctx.InstBudgets.RemoveRange(instBudgets.ToList());
                }
                //更新编辑时间
                newValue.EditTime = DateTime.Now;

                ctx.Entry(oldValue).CurrentValues.SetValues(newValue);
                ctx.SaveChanges();
                return Mapper.Map<GetApplicationDTO>(newValue);
            }
        }

        /// <summary>
        /// 获取属于某种状态的申请书Id列表
        /// </summary>
        /// <param name="status"></param>
        /// <returns></returns>
        internal List<string> GetAppIdListByStatus(ApplicationStatus status)
        {
            using (var _context = new AspodesDB())
            {
                return _context.Applications
                    .Where(a => a.Status == status)
                    .Where(a => a.CurrentYear == SystemConfig.ApplicationStartYear)
                    .Select(a => a.ApplicationId).ToList();
            }
        }

        /// <summary>
        /// 添加申请书
        /// </summary>
        /// <param name="applicationDTO"></param>
        /// <returns></returns>
        public GetApplicationDTO AddApplication(AddApplicationDTO applicationDTO)
        {

            using (var ctx = new AspodesDB())
            {
                var user = UserHelper.GetCurrentUser();

                Person leader = ctx.Persons.FirstOrDefault(p => p.Status == "C" && p.PersonId == user.PersonId);
                if (leader == null)
                    throw new NotFoundException("未找到负责人");
                var contactUser = ctx.Users.FirstOrDefault(u => u.UserId == leader.Institute.ContactId);
                Person contact = ctx.Persons.FirstOrDefault(p => p.Status == "C" && p.PersonId == contactUser.PersonId);
                if (contact == null)
                    throw new NotFoundException("未找到单位联系人");
                var app = Mapper.Map<Application>(applicationDTO);

                //补全申请书信息
                app.Status = ApplicationStatus.NEW_ONE;
                app.YearCreated = DateTime.Now.Year;
                app.CurrentYear = SystemConfig.ApplicationStartYear;
                app.AppliyTimes = 1;
                app.EditTime = DateTime.Now;

                app.LeaderId = leader.PersonId;
                app.LeaderPhone = leader.Phone;
                app.LeaderEmail = leader.Email;
                if (contact != null)
                {
                    app.ContactId = contact.PersonId;
                    app.ContactPhone = contact.Phone;
                    app.ContactEmail = contact.Email;
                }

                app.InstituteId = leader.InstituteId;

                app = ctx.Applications.Add(app);

                //将负责人添加进申请书成员
                Member leaderMember = new Member()
                {
                    ApplicationId = app.ApplicationId,
                    PersonId = leader.PersonId,
                    Task = "负责人"
                };
                ctx.Members.Add(leaderMember);
                ctx.SaveChanges();
                return Mapper.Map<GetApplicationDTO>(app);
            }
        }

        /// <summary>
        /// 添加或者更改申请书
        /// </summary>
        /// <param name="applicationDTO"></param>
        /// <returns></returns>
        public GetApplicationDTO AddOrUpdateApplication(AddApplicationDTO applicationDTO, Func<Application, CurrentUser, bool> privilege)
        {

            using (var ctx = new AspodesDB())
            {
                if (!ctx.Applications.Any(a => a.ApplicationId == applicationDTO.ApplicationId))
                {
                    return AddApplication(applicationDTO);
                }
                else
                {
                    var userInfo = UserHelper.GetCurrentUser();
                    return UpdateApplication(applicationDTO, privilege);
                }
            }
        }

        /// <summary>
        /// 保存申请书，主要操作是生成申请书PDF文档，将申请书的状态修改为ApplicationStatus.NEW
        /// </summary>
        /// <param name="applicationId">申请书ID</param>
        /// <returns>
        /// 成功,返回ResponseStatus.success，
        /// 没有找申请书,返回ResponseStatus.parameter_error,
        /// 出错,返回ResponseStatus.unknown_error和错误信息
        /// </returns>
        public GetApplicationDTO SaveApplication(string applicationId, Func<Application, CurrentUser, bool> privilege)
        {
            using (var ctx = new AspodesDB())
            {
                //取出申请书信息
                Application application = ctx.Applications.FirstOrDefault(a => a.ApplicationId == applicationId);
                if (null == application)
                    throw new NotFoundException("未找到申请书");

                if (!privilege(application, UserHelper.GetCurrentUser()))
                    throw new UnauthorizationException();

                //生成申请书PDF文档
                var body = ctx.ApplicationDocs.FirstOrDefault(ad => ad.ApplicationId == application.ApplicationId && ad.Type == ApplicationDocType.BODY);
                var oldPdf = ctx.ApplicationDocs.FirstOrDefault(ad => ad.ApplicationId == application.ApplicationId && ad.Type == ApplicationDocType.PDF);
                if (null != oldPdf)
                {
                    ApplicationDocRepository.Delete(oldPdf.ApplicationDocId.Value, (a, u) => true);
                }

                var pdfName = body.Name.Replace(".docx", "") + "PDF";
                bool createPDF = PdfHelper.CreatePdf(application.ApplicationId, body.RelativeURL, pdfName, SystemConfig.ApplicationStartYear);
                if (!createPDF)
                    throw new OtherException("PDF生成失败");
                //添加PDF文档记录
                ApplicationDoc pdf = new ApplicationDoc()
                {
                    Name = pdfName + ".pdf",
                    RelativeURL = body.RelativeURL.Replace(body.Name, pdfName + ".pdf"),
                    Type = ApplicationDocType.PDF,
                    ApplicationId = applicationId
                };
                ctx.ApplicationDocs.Add(pdf);
                //修改申请书状态
                application.Status = ApplicationStatus.NEW;
                ctx.SaveChanges();

                return Mapper.Map<GetApplicationDTO>(application);
            }
        }

        /// <summary>
        /// 提交申请书，先检查申请书的完整性，如通过检查将申请书状态修改为ApplicationStatus.CHECK，否则返回不允许此操作
        /// </summary>
        /// <param name="applicationId">申请书ID</param>
        /// <returns>
        /// 成功,返回ResponseStatus.success，
        /// 没有找申请书,返回ResponseStatus.parameter_error,
        /// 出错,返回ResponseStatus.unknown_error和错误信息
        /// </returns>
        public void SubmitApplication(string applicationId, Func<Application, bool> privilege)
        {

            using (var ctx = new AspodesDB())
            {
                //获取申请书信息
                var application = ctx.Applications.FirstOrDefault(a => a.ApplicationId == applicationId);
                if (null == application) throw new NotFoundException("未找到申请书");
                if (!privilege(application)) throw new UnauthorizationException();

                //检查申请书填写的完整性
                if (!ApplicationIntegrityConstraint(application))
                    throw new OtherException("申请书填写不完整");

                if (application.Status != ApplicationStatus.NEW)
                {
                    throw new OtherException("申请书状态错误");
                }

                application.Status = ApplicationStatus.CHECK;
                ctx.SaveChanges();
            }

        }

        public List<GetReviewAssignmentDTO> AcceptApplication( string applicationId, Func<Application,bool> privilege )
        {
            var deleagetype = DelegateType.DIRECTIONAL;
            using (var ctx = new AspodesDB())
            {
                var application = ctx.Applications.FirstOrDefault(a => a.ApplicationId == applicationId && a.Status == ApplicationStatus.ACCEPT );
                if (null == application) throw new NotFoundException("未找到申请书或者申请书状态不符合条件");
                if (!privilege(application)) throw new UnauthorizationException("当前用没有受理该申请书的权限");
                //定向委托项目直接跳过评审
                if(application.DeleageType == DelegateType.DIRECTIONAL)
                {
                    application.Status = ApplicationStatus.FINISH_REVIEW;
                }
                else
                {
                    deleagetype = DelegateType.NORMAL;
                    application.Status = SystemConfig.AutoAssignmentExpert ? ApplicationStatus.ASSIGNMENT : ApplicationStatus.MANUAL_ASSIGNMENT;
                }

                
                ctx.SaveChanges();
            }

            if( SystemConfig.AutoAssignmentExpert && deleagetype != DelegateType.DIRECTIONAL)
            {
                return new ReviewAssignmentRepository().ApplicationExpertAssignment(applicationId, a=>true);
            }
            
            return new List<GetReviewAssignmentDTO>();
        }
        /// <summary>
        /// 申请书完整性检查
        /// </summary >
        /// <param name="app"></param>
        /// <returns></returns>
        public bool ApplicationIntegrityConstraint(Application app)
        {
            using (var ctx = new AspodesDB())
            {
                //后续添加
                if (ctx.ApplicationFields.Count(a => a.ApplicationId == app.ApplicationId) != SystemConfig.ApplicationFieldAmount)
                    throw new OtherException("申请书研究领域数目错误");
            }
            return true;
        }

        /// <summary>
        /// 判断是否已经存在一封申请书
        /// </summary>
        /// <returns></returns>
        public bool IsResubmitted()
        {
            using (var ctx = new AspodesDB())
            {
                int leaderId = UserHelper.GetCurrentUser().PersonId;
                var submittedApplication = ctx.Applications.FirstOrDefault(a => a.LeaderId == leaderId && (a.Status == ApplicationStatus.CHECK || a.Status == ApplicationStatus.ACCEPT));
                if (submittedApplication != null) return true;
                return false;
            }
        }

        /// <summary>
        /// 更改申请书状态
        /// </summary>
        /// <param name="applicationId">申请书ID</param>
        /// <param name="from">修改前的状态</param>
        /// <param name="to">修改后的状态</param>
        /// <param name="privilege">修改权限</param>
        /// <param name="logDTO">操作日志</param>
        /// <returns></returns>
        public void ChangeApplicationStatus(string applicationId, ApplicationStatus from, ApplicationStatus to, Func<Application, bool> privilege, AddApplicationLogDTO logDTO = null)
        {
            using (var ctx = new AspodesDB())
            {
                //取出申请书
                var application = ctx.Applications.FirstOrDefault(a => a.ApplicationId == applicationId);
                if (null == application) throw new NotFoundException("未找到申请书");
                //验证权限
                if (!privilege(application)) throw new OtherException("您没有权限或操作时间有误");
                //修改状态
                if (application.Status != from) throw new OtherException("申请书状态错误");

                application.Status = to;

                //添加日志
                if (logDTO != null)
                {
                    ApplicationLogRepository.AddApplicationLog(ctx, logDTO);
                }

                ctx.SaveChanges();
            }
        }


        /// <summary>
        /// 获得院待受理申请书数量
        /// </summary>
        /// <param></param>
        public int GetAcceptApplicationNumber(int?[] projectTypes)
        {
            using (var ctx = new AspodesDB())
            {
                var number = (from p in ctx.Applications
                              where p.Status == ApplicationStatus.ACCEPT 
                              && (projectTypes).Contains(p.ProjectTypeId)
                              && p.CurrentYear == SystemConfig.ApplicationStartYear
                              select p.ApplicationId).Count();
                return number;
            }
        }

        /// <summary>
        /// 获得单位待审核申请书数量
        /// </summary>
        /// <param></param>
        public int GetCheckApplicationNumber(int instituteId)
        {
            using (var ctx = new AspodesDB())
            {
                var applicationNumber = ctx.Applications.Where(a => a.Status == ApplicationStatus.CHECK && a.InstituteId == instituteId).Count();
                return applicationNumber;
            }
        }

        /// <summary>
        /// 通过applicationId获取申请书所在的领域的Id
        /// </summary>
        /// <param name="applicationId"></param>
        /// <returns></returns>
        public int GetProjectTypeId(string applicationId)
        {
            using (var _context = new AspodesDB())
            {
                var projectTypeId = (from application in _context.Applications
                                     where application.ApplicationId == applicationId
                                     select application.ProjectTypeId).FirstOrDefault();
                return projectTypeId.Value;
            }  
        }

        /// <summary>
        /// 通过ID获取申请书名称
        /// </summary>
        /// <param name="applicationId"></param>
        /// <returns></returns>
        public string GetApplicationNameById(string applicationId)
        {
            using (var _context = new AspodesDB())
            {
                return _context.Applications.Find(applicationId).ProjectName.ToString();
            }
        }

        /// <summary>
        /// 判断是否存在相同领域的等待分配专家的申请书，疑问：如何判断是当年的？？？
        /// </summary>
        /// <param name="applicationId"></param>
        /// <returns></returns>
        public bool ExistUnAssignmentApplication(string applicationId)
        {
            using (var _context = new AspodesDB())
            {
                int type = _context.Applications.Find(applicationId).ProjectTypeId.Value;
                var application = _context.Applications
                        .Where(a =>
                            (a.Status == ApplicationStatus.ASSIGNMENT || a.Status == ApplicationStatus.MANUAL_ASSIGNMENT)
                            && a.ProjectTypeId == type
                            && a.CurrentYear == SystemConfig.ApplicationStartYear)
                        .FirstOrDefault();
                return application != null;
            }
        }

        //----------------------专家-----------------------------------

        /// <summary>
        /// 专家获取需要评审的申请书
        /// </summary>
        /// <param name="userId"></param>
        public List<GetApplicationDTO> ExpertGetReviewApplication(string userId)
        {
            using (var ctx = new AspodesDB())
            {
                var assignments = from assignment in ctx.ReviewAssignments
                                  where assignment.Status == ReviewAssignmentStatus.ACCEPT && assignment.ExpertId == userId && !assignment.Overdue.Value
                                  select assignment.ApplicationId;
                var applicationIds = assignments.ToList();
                return GetApplicationList(a => applicationIds.Contains(a.ApplicationId));
            }
        }

        public ApplicationStepOneLeftDTO GetStepOneLeft(string id)
        {
            ApplicationStepOneLeftDTO stepOneLeft = null;
            using (var ctx = new AspodesDB())
            {
                Application application = ctx.Applications.FirstOrDefault(a => a.ApplicationId == id);
                if (null != application)
                {
                    stepOneLeft = Mapper.Map<ApplicationStepOneLeftDTO>(application);
                }
                else
                {
                    stepOneLeft = new ApplicationStepOneLeftDTO();
                    stepOneLeft.ApplicationId = id;
                }

                stepOneLeft.ProjectTypes = ctx.ProjectTypes.Select(Mapper.Map<GetProjectTypeDTO>).ToList();
                if (!stepOneLeft.ProjectTypeId.HasValue)
                {
                    ProjectType type = ctx.ProjectTypes.FirstOrDefault();
                    stepOneLeft.ProjectTypeName = type.Name;
                    stepOneLeft.ProjectTypeId = type.ProjectTypeId;
                }

                stepOneLeft.SupportCategorys = ctx.SupportCategorys
                    .Where(sc => sc.ProjectTypeId == stepOneLeft.ProjectTypeId)
                    .Select(Mapper.Map<GetSupportCategoryDTO>)
                    .ToList();
                if (!stepOneLeft.SupportCategoryId.HasValue)
                {
                    SupportCategory supportCategory = ctx.SupportCategorys.FirstOrDefault();
                    stepOneLeft.SupportCategoryId = supportCategory.SupportCategoryId;
                    stepOneLeft.SupportCategoryName = supportCategory.Name;
                }

                stepOneLeft.Fields = ctx.Fields.ToList();
                List<ApplicationField> applicationFields = ctx.ApplicationFields.Where(af => af.ApplicationId == id).ToList();

                if (applicationFields.Count < SystemConfig.ApplicationFieldAmount)
                {
                    SubField subField = ctx.SubFields.FirstOrDefault(sf => sf.SubFieldName != null);
                    while (applicationFields.Count < SystemConfig.ApplicationFieldAmount)
                    {
                        applicationFields.Add(new ApplicationField { SubFieldId = subField.SubFieldName });
                    }
                }

                foreach (var applicationField in applicationFields)
                {
                    SubField subField = ctx.SubFields.FirstOrDefault(sf => sf.SubFieldName == applicationField.SubFieldId);
                    var applicationFieldVO = Mapper.Map<ApplicationFieldVO>(applicationField);
                    applicationFieldVO.FieldId = subField.ParentName;
                    applicationFieldVO.SubFields = ctx.SubFields
                        .Where(sf => sf.ParentName == subField.ParentName)
                        .Select(Mapper.Map<GetSubFieldDTO>)
                        .ToList();
                    stepOneLeft.ApplicationFields.Add(applicationFieldVO);
                }
            }
            return stepOneLeft;
        }

        public void CreateApplicationPDF(string applicationId)
        {
            using (var ctx = new AspodesDB())
            {
                //取出申请书信息
                Application application = ctx.Applications.FirstOrDefault(a => a.ApplicationId == applicationId);
                if (null == application)
                    throw new NotFoundException("未找到申请书");

                //生成申请书PDF文档
                var body = ctx.ApplicationDocs.FirstOrDefault(ad => ad.ApplicationId == application.ApplicationId && ad.Type == ApplicationDocType.BODY);
                var oldPdf = ctx.ApplicationDocs.FirstOrDefault(ad => ad.ApplicationId == application.ApplicationId && ad.Type == ApplicationDocType.PDF);
                if (null != oldPdf)
                {
                    ApplicationDocRepository.Delete(oldPdf.ApplicationDocId.Value, (a, u) => true);
                }

                var pdfName = body.Name.Replace(".docx", "") + "PDF";
                bool createPDF = PdfHelper.CreatePdf(application.ApplicationId, body.Name, pdfName, SystemConfig.ApplicationStartYear);
                if (!createPDF)
                    throw new OtherException("PDF生成失败");
                //添加PDF文档记录
                ApplicationDoc pdf = new ApplicationDoc()
                {
                    Name = pdfName + ".pdf",
                    RelativeURL = body.RelativeURL.Replace(body.Name, pdfName + ".pdf"),
                    Type = ApplicationDocType.PDF,
                    ApplicationId = applicationId
                };
                ctx.ApplicationDocs.Add(pdf);
               
                ctx.SaveChanges();
            }
        }

        /// <summary>
        /// 申请书综合查询(不分页)
        /// </summary>
        /// <param name="dto"></param>
        public List<GetApplicationInquiriesDTO> ApplicationInquiries(ApplicationInquiriesDTO dto)
        {
            using (var ctx = new AspodesDB())
            {
                IQueryable<Application>query = ctx.Applications;
                //var query = ctx.Applications;
                if (dto.ProjectName != null && !"".Equals(dto.ProjectName)) query.Where(a => a.ProjectName.Contains(dto.ProjectName));
                if (dto.Period != null && dto.Period.Length != 0) query.Where(a => dto.Period.Contains(a.Period.Value));
                if (dto.ProjectType != null && dto.ProjectType.Length != 0) query.Where(a => dto.Period.Contains(a.ProjectTypeId.Value));
                if (dto.Institute != null && dto.Institute.Length != 0) query.Where(a => dto.Period.Contains(a.InstituteId.Value));
                if (dto.LeaderName != null && !"".Equals(dto.LeaderName)) query.Where(a => a.Leader.Name.Contains(dto.LeaderName));
                if (dto.StartYearCreated != null) query.Where(a => a.YearCreated >= dto.StartYearCreated);
                if (dto.EndYearCreated != null) query.Where(a => a.YearCreated <= dto.EndYearCreated);
                if (dto.StartTotalBudget != null) query.Where(a => a.TotalBudget >= dto.StartTotalBudget);
                if (dto.EndTotalBudget != null) query.Where(a => a.TotalBudget <= dto.EndTotalBudget);
                if (dto.EndTotalBudget != null) query.Where(a => a.TotalBudget <= dto.EndTotalBudget);
                if (dto.Status != null && dto.Status.Length != 0) query.Where(a => dto.Status.Contains((int)a.Status));
                if (dto.DelegateType != null) query.Where(a => (int)a.DeleageType == dto.DelegateType);
                if (dto.StartTotalScore != null) query.Where(a => a.TotalScore != null && a.TotalScore >= dto.StartTotalScore);
                if (dto.EndTotalScore != null) query.Where(a => a.TotalScore != null && a.TotalScore <= dto.EndTotalScore);
                
                return query.Select(Mapper.Map<GetApplicationInquiriesDTO>).ToList();
            }
        }

        /// <summary>
        /// 申请书综合查询(分页)
        /// </summary>
        /// <param name="dto"></param>
        /// <param name="page"></param>
        public PagingListDTO<GetApplicationInquiriesDTO> ApplicationInquiriesByPage(ApplicationInquiriesDTO dto, int page)
        {
            PagingListDTO<GetApplicationInquiriesDTO> pagingList = new PagingListDTO<GetApplicationInquiriesDTO>();
            using (var ctx = new AspodesDB())
            {
                IQueryable<Application> query = ctx.Applications;
                //var query = ctx.Applications;
                if (dto.ProjectName != null && !"".Equals(dto.ProjectName))
                    query = query.Where(a => a.ProjectName.Contains(dto.ProjectName));
                if (dto.Period != null && dto.Period.Length != 0) query = query.Where(a => dto.Period.Contains(a.Period.Value));
                if (dto.ProjectType != null && dto.ProjectType.Length != 0) query = query.Where(a => dto.ProjectType.Contains(a.ProjectTypeId.Value));
                if (dto.Institute != null && dto.Institute.Length != 0) query = query.Where(a => dto.Institute.Contains(a.InstituteId.Value));
                if (dto.LeaderName != null && !"".Equals(dto.LeaderName)) query = query.Where(a => a.Leader.Name.Contains(dto.LeaderName));
                if (dto.StartYearCreated != null) query = query.Where(a => a.YearCreated >= dto.StartYearCreated);
                if (dto.EndYearCreated != null) query = query.Where(a => a.YearCreated <= dto.EndYearCreated);
                if (dto.StartTotalBudget != null) query = query.Where(a => a.TotalBudget >= dto.StartTotalBudget);
                if (dto.EndTotalBudget != null) query = query.Where(a => a.TotalBudget <= dto.EndTotalBudget);
                if (dto.EndTotalBudget != null) query = query.Where(a => a.TotalBudget <= dto.EndTotalBudget);
                if (dto.Status != null && dto.Status.Length != 0) query = query.Where(a => dto.Status.Contains((int)a.Status));
                if (dto.DelegateType != null) query = query.Where(a => (int)a.DeleageType == dto.DelegateType);
                if (dto.StartTotalScore != null) query = query.Where(a => a.TotalScore != null && a.TotalScore >= dto.StartTotalScore);
                if (dto.EndTotalScore != null) query = query.Where(a => a.TotalScore != null && a.TotalScore <= dto.EndTotalScore);

                pagingList.TotalNum = query.Count();
                pagingList.TotalPageNum = (pagingList.TotalNum + SystemConfig.ApplicationPageCount - 1) / SystemConfig.ApplicationPageCount;
                if (pagingList.TotalNum <= 0) pagingList.TotalPageNum = 1;
                pagingList.NowPage = page <= 0 ? 1 : page;

                pagingList.ItemDTOs = query.OrderByDescending(a => a.EditTime)
                    .Skip((pagingList.NowPage - 1) * SystemConfig.ApplicationPageCount)
                    .Take(SystemConfig.ApplicationPageCount)
                    .Select(Mapper.Map<GetApplicationInquiriesDTO>)
                    .ToList();
                pagingList.NowNum = pagingList.ItemDTOs.Count();

                return pagingList;
            }
        }
    }
}