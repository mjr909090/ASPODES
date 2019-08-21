using ASPODES.Common;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Net.Http;
using ASPODES.WebAPI.Common;
using NPOI.SS.UserModel;
using ASPODES.Database;
using System.Text;
using ASPODES.Model;
using AutoMapper;
using Newtonsoft.Json;
using ASPODES.DTO.Consultation;
using System.Text.RegularExpressions;
using System.Web.Http.ModelBinding;
using System.ComponentModel.DataAnnotations;
using ASPODES.WebAPI.Service;

namespace ASPODES.WebAPI.Repository
{
    /// <summary>
    /// 咨询审议
    /// </summary>
    public class ConsultationRepository : IConsultationRepository
    {

        private AspodesDB _ctx;
        // 使用属性依赖注入service（万般无奈，等重构时再修改）
        //private CreateNoticeService _noticeService { get; set; }
        public ConsultationRepository(AspodesDB ctx)
        {
            _ctx = ctx;
        }

        /// <summary>
        /// 获取导入结果
        /// </summary>
        /// <param name="year"></param>
        /// <returns></returns>
        public List<Consultation> GetConsultaion( int year )
        {
            return _ctx.Consultations.Where(c => c.ImportTime.Year == year).ToList();
        }

        public HttpResponseMessage DownloadConsultationTemplate()
        {
            List<GetApplicationConsultationDTO> appConsultations = _ctx.Applications
                .Where(a => a.CurrentYear == SystemConfig.ApplicationStartYear && a.Status == ApplicationStatus.FINISH_REVIEW)
                .OrderBy( a=>a.TotalScore )
                .Select( a=> new GetApplicationConsultationDTO
                {
                    ProjectName = a.ProjectName,
                    ApplicationId = a.ApplicationId,
                    DelegateType = a.DeleageType == DelegateType.NORMAL ? "非定向":"定向",
                    ProjectTypeName = a.ProjectType.Name,
                    InstituteName = a.Institute.Name,
                    LeaderName = a.Leader.Name,
                    Period = a.Period.Value,
                    Budget = a.TotalBudget,
                    TotalScore = a.TotalScore,
                    CurrentYearBudget = a.FirstYearBudget
                })
                .OrderByDescending( ac=>ac.TotalScore )
                .ToList();


            var prjConsultations = _ctx.Projects
                .Where(p => p.Status == ProjectStatus.ACTIVE)
                .Where(p => p.EndDate.Year >= DateTime.Now.Year)
                .Where(p => !p.AnnualTasks.Any(at => at.Year == DateTime.Now.Year))
                .Select(p => new GetProjectConsultationDTO() 
                {
                    ProjectName = p.Name,
                    ProjectId = p.ProjectId,
                    DelegateType = p.DelegateType == DelegateType.NORMAL ? "非定向" : "定向",
                    ProjectTypeName = p.ProjectType.Name,
                    InstituteName = p.Institude.Name,
                    LeaderName = p.Leader.Name,
                    Period = p.Period.Value,
                    Budget = p.TotalBudget,
                    ArrivalBudget = p.AnnualTasks.Sum(at => at.CurrentBudget.Value),
                    ApplicationId = p.ApplicationId,
                    Year = p.AnnualTasks.Count() + 1,
                    CurrentYearBudget = 0.0
                })
                .ToList();

            foreach( var prj in prjConsultations )
            {
                var budget = _ctx.AnnualBudgets.FirstOrDefault(ab => ab.ApplicationId == prj.ApplicationId && ab.Year == prj.Year );
                if (null != budget) prj.CurrentYearBudget = budget.Amount.Value;
            }

            string path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, SystemConfig.ExportConsultationExcelTemplate);
           
            IWorkbook workbook = WorkbookFactory.Create(path);
            ICellStyle style = workbook.CreateCellStyle();

            style.Alignment = HorizontalAlignment.General;
            style.VerticalAlignment = VerticalAlignment.Center;

            

            ISheet appSheet = workbook.GetSheet("Application");
            ISheet prjSheet = workbook.GetSheet("Project");

            if (appConsultations.Count > 0 || prjConsultations.Count > 0 )
            {
                workbook.RemoveSheetAt(0);
                workbook.RemoveSheetAt(0);
            }

            int supportAmount = appConsultations.Count * 2 / 10;
            int storageAmount = appConsultations.Count * 4 / 10;
            var support = appConsultations.Take(supportAmount).Select(a => a.ApplicationId).ToList();
            var storage = appConsultations.Skip(supportAmount).Take(storageAmount).Select(a => a.ApplicationId).ToList();

            var projectTypes = _ctx.ProjectTypes.Select( pt=>pt.Name);
            #region 存放非定向的项目
            foreach ( string type in projectTypes)
            {
                var typeApp = appConsultations.Where(a => a.ProjectTypeName.Equals(type) && a.DelegateType.Equals("非定向"));//筛选出非定向的项目
                if (typeApp.Count() <= 0) continue;
                
                ISheet typeSheet = appSheet.CopySheet( type, true );

                int insertRow = 1;
                foreach( var app in typeApp )
                {
                    if (!app.TotalScore.HasValue) return ResponseWrapper.ExceptionResponse(new OtherException("请先手动计算总分后再导出数据"));
                    IRow row = typeSheet.CreateRow(insertRow);
                    row.CreateCell(0).SetCellValue(app.ProjectName);
                    row.CreateCell(1).SetCellValue(app.ApplicationId);
                    row.CreateCell(2).SetCellValue(app.DelegateType);
                    row.CreateCell(3).SetCellValue(app.InstituteName);
                    row.CreateCell(4).SetCellValue(app.LeaderName);
                    row.CreateCell(5).SetCellValue(app.Period);
                    row.CreateCell(6).SetCellValue(app.TotalScore.Value);
                    
                    row.CreateCell(7).SetCellValue(app.Budget.Value);
                    row.CreateCell(8).SetCellValue(app.CurrentYearBudget.Value);
                    if (support.Contains(app.ApplicationId))
                        row.CreateCell(9).SetCellValue("出库");
                    else if (storage.Contains(app.ApplicationId))
                        row.CreateCell(9).SetCellValue("入库");
                    else
                        row.CreateCell(9).SetCellValue("不资助");
                    insertRow++;
                }
                //typeSheet.SetColumnHidden(2, true);
            }
            #endregion
            #region 存放定向项目
            var directionalProject = appConsultations.Where(a => a.DelegateType.Equals("定向"));
            if(directionalProject.Count() > 0)
            {
                ISheet typeSheet = appSheet.CopySheet("定向委托项目", true);
                //对于定向项目，把初审评分修改为类别信息
                IRow headRow = typeSheet.GetRow(0);
                headRow.GetCell(6).SetCellValue("类别");

                int insertRow = 1;
                foreach (var app in directionalProject)
                {
                    IRow row = typeSheet.CreateRow(insertRow);
                    row.CreateCell(0).SetCellValue(app.ProjectName);
                    row.CreateCell(1).SetCellValue(app.ApplicationId);
                    row.CreateCell(2).SetCellValue(app.DelegateType);
                    row.CreateCell(3).SetCellValue(app.InstituteName);
                    row.CreateCell(4).SetCellValue(app.LeaderName);
                    row.CreateCell(5).SetCellValue(app.Period);
                    row.CreateCell(6).SetCellValue(app.ProjectTypeName);

                    row.CreateCell(7).SetCellValue(app.Budget.Value);
                    row.CreateCell(8).SetCellValue(app.CurrentYearBudget.Value);
                    row.CreateCell(9).SetCellValue("出库");
                    insertRow++;
                }
            }
            #endregion
            if ( prjConsultations.Count() > 0 )
            {
                ISheet projectSheet = prjSheet.CopySheet("在研项目", true);

                int insertRow = 1;
                foreach (var prj in prjConsultations)
                {
                    IRow row = projectSheet.CreateRow(insertRow);
                    row.CreateCell(0).SetCellValue( prj.ProjectName);
                    row.CreateCell(1).SetCellValue(prj.ProjectId);
                    row.CreateCell(2).SetCellValue(prj.ProjectTypeName);
                    row.CreateCell(3).SetCellValue(prj.InstituteName);
                    row.CreateCell(4).SetCellValue(prj.LeaderName);
                    row.CreateCell(5).SetCellValue(prj.Period);
                    row.CreateCell(6).SetCellValue(prj.Budget.Value);
                    row.CreateCell(7).SetCellValue(prj.ArrivalBudget.Value);
                    row.CreateCell(8).SetCellValue(prj.CurrentYearBudget.Value);
                    row.CreateCell(9).SetCellValue("持续资助");
                    insertRow++;
                }
            }
            

            string desName = string.Format("{0}_{1}.{2}", "Consultation", DateTime.Now.ToFileTime(), "xls");
            string desPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, SystemConfig.ConsultationPath, desName);

            var stream = File.OpenWrite(desPath);
            workbook.Write(stream);
            stream.Close();
            workbook.Close();
            return FileHelper.Download( HttpContext.Current, "\\" + SystemConfig.ConsultationPath + "\\" + desName , desName);
        }

        public List<Consultation> UploadConsultationResult()
        {
            IWorkbook workbook = null;
            StringBuilder responseMsg = new StringBuilder();

            //获取绝对路径
            string path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, SystemConfig.ConsultationPath);
            //保存文件
            string saveName = FileHelper.Upload(HttpContext.Current, path);
            //string saveName = "Consultation_131568322669228587.xls";

            //读取标题
            string applicationConfig = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Config", "ApplicationConsultation.json");
            string projectConfig = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Config", "ProjectConsultation.json");

            Dictionary<string, string> applicationTitleMap = JsonConvert.DeserializeObject<Dictionary<string, string>>(File.ReadAllText(applicationConfig));
            Dictionary<string, string> projectTitleMap = JsonConvert.DeserializeObject<Dictionary<string, string>>(File.ReadAllText(projectConfig));

            List<AddConsultationDTO> consultations = new List<AddConsultationDTO>();

            try
            {

                workbook = WorkbookFactory.Create(Path.Combine(path, saveName));

                foreach (ISheet sheet in workbook)
                {
                    //检查标题是否合法，记录标题的列下标
                    Dictionary<string, int> propertyIndex = null;
                    int typeTag = ParseTitle(applicationTitleMap, projectTitleMap, sheet, out propertyIndex);
                    sheet.RemoveRow(sheet.GetRow(0));
                    if( typeTag > 2 || typeTag < 1 )
                    {
                        responseMsg.Append(sheet.SheetName + "标题不合法");
                        continue;
                    }

                    //判断是申请书还是项目评审结果
                    Type type = typeTag == 1 ? typeof(AddApplicationConsultationDTO) : typeof(AddProjectConsultationDTO);

                    List<ValidationResult> validationsResults = new List<ValidationResult>();

                    foreach( IRow row in sheet )
                    {
                        //一行数据转化成一个consultation对象
                        var consultation = ParseConsultation(row, type, propertyIndex);

                        //检查实体合法性
                        var context = new ValidationContext(consultation, null, null);
                        validationsResults.Clear();
                        Validator.TryValidateObject(consultation, context, validationsResults, true);
                        foreach (var result in validationsResults)
                        {
                            responseMsg.Append(string.Format("{0}表{1}行：{2}", sheet.SheetName, row.RowNum, result.ErrorMessage));
                        }

                        if( validationsResults.Count() == 0 )consultations.Add(consultation);
                    }   
                }

                //检查申请书状态
                foreach (var acd in consultations.OfType<AddApplicationConsultationDTO>())
                {
                    var application = _ctx.Applications.FirstOrDefault(a => a.ApplicationId == acd.ApplicationId);
                    if (application == null || application.CurrentYear != SystemConfig.ApplicationStartYear || application.Status != ApplicationStatus.FINISH_REVIEW)
                    {
                        responseMsg.Append(string.Format("编号为{0}的申请书不符合条件", acd.ApplicationId));
                    }
                }

                //检查项目状态
                foreach (var pcd in consultations.OfType<AddProjectConsultationDTO>())
                {
                    var project = _ctx.Projects.FirstOrDefault(p => p.ProjectId == pcd.ProjectId);
                    if (project == null || project.Status != ProjectStatus.ACTIVE)
                    {
                        responseMsg.Append(string.Format("编号为{0}的项目不符合条件", pcd.ProjectId));
                    }
                }
            }
            catch (Exception e)
            {
                throw e;
            }
            finally
            {
                if (workbook != null) workbook.Close();
            }

            //有问题抛出异常
            if (responseMsg.Length > 0) throw new OtherException(responseMsg.ToString());

            //插入数据
            using (var transaction = _ctx.Database.BeginTransaction())
            {
                try
                {
                    //申请书评审意见
                    foreach (var item in consultations.OfType<AddApplicationConsultationDTO>())
                    {
                        var ac = (ApplicationConsultation)TypeConverter(item);

                        _ctx.Consultations.Add(ac);
                        
                        var application = _ctx.Applications.FirstOrDefault(a => a.ApplicationId == ac.ApplicationId);

                        switch( ac.Result )
                        {
                            case ApplicationConsultationResult.STORAGE:
                                application.Status = ApplicationStatus.STORAGE;

                                //院管理员导入咨询审议结果入库（临时使用，项目重构时修改）
                                //通知打点:发给普通用户
                                //_noticeService.AddNotice(
                                //    _noticeService.GetUserIdByApplicationId(application.ApplicationId), 24, 
                                //    new Dictionary<string, string> {
                                //        { "ApplicationName", application.ProjectName }
                                //    });
                                //通知打点:发给单位管理员
                                //_noticeService.AddNoticeList(
                                //    _noticeService.GetInstManagerIdsbyAppId(application.ApplicationId), 45,
                                //    new Dictionary<string, string> {
                                //        { "ApplicationName", application.ProjectName }
                                //    });
                                
                                break;
                            case ApplicationConsultationResult.SUPPORT:
                                var project = AddProject(application, ac);
                                AddAnnualTask(project, ac);
                                application.Status = ApplicationStatus.SUPPORT;

                                //院管理员导入咨询审议结果给予出库资助（临时使用，项目重构时修改）
                                //通知打点:发给普通用户
                                //_noticeService.AddNotice(
                                //    _noticeService.GetUserIdByApplicationId(application.ApplicationId), 23,
                                //    new Dictionary<string, string> {
                                //        { "ApplicationName", application.ProjectName }
                                //    });
                                //通知打点:发给单位管理员
                                //_noticeService.AddNoticeList(
                                //    _noticeService.GetInstManagerIdsbyAppId(application.ApplicationId), 44,
                                //    new Dictionary<string, string> {
                                //        { "ApplicationName", application.ProjectName }
                                //    });

                                //提醒项目负责人填写任务书（临时使用，项目重构时修改）
                                //通知打点:发给项目负责人
                                //_noticeService.AddNotice(
                                //    _noticeService.GetUserIdByApplicationId(application.ApplicationId), 25);

                                break;
                            case ApplicationConsultationResult.UNSUPPORT:
                                application.Status = ApplicationStatus.UNSUPPORT;

                                //院管理员导入咨询审议结果给不资助（临时使用，项目重构时修改）
                                //通知打点:发给普通用户
                                //_noticeService.AddNotice(
                                //    _noticeService.GetUserIdByApplicationId(application.ApplicationId), 22,
                                //    new Dictionary<string, string> {
                                //        { "ApplicationName", application.ProjectName }
                                //    });
                                //通知打点:发给单位管理员
                                //_noticeService.AddNoticeList(
                                //    _noticeService.GetInstManagerIdsbyAppId(application.ApplicationId), 43,
                                //    new Dictionary<string, string> {
                                //        { "ApplicationName", application.ProjectName }
                                //    });

                                break;
                            default:
                                throw new OtherException("评审结果状态错误");
                        }
                        
                    }

                    //项目评审意见
                    foreach( var item in consultations.OfType<AddProjectConsultationDTO>() )
                    {
                        var pc = (ProjectConsultation)TypeConverter(item);
                        _ctx.Consultations.Add(pc);

                        var project = _ctx.Projects.FirstOrDefault( p=>p.ProjectId == item.ProjectId);

                        switch (pc.Result)
                        {
                            case ProjectConsultationResult.CONTINUE:
                                AddAnnualTask(project, pc);
                                break;
                            case ProjectConsultationResult.SUSPEND:
                                project.Status = ProjectStatus.FINISH;
                                break;
                            default:
                                throw new OtherException("评审结果状态错误");
                        }
                    }

                    _ctx.SaveChanges();
                    transaction.Commit();
                    return null;
                }
                catch (Exception e )
                {
                    transaction.Rollback();
                    throw e;
                }
            }
        }

        

        private Project AddProject(Application application, Consultation consultation)
        {
            //Project project = Mapper.Map<Project>(application);
            var project = CreateProject(application, consultation);
            project = _ctx.Projects.Add(project);
            _ctx.SaveChanges();

            var members = _ctx.Members.Where(m => m.ApplicationId == project.ApplicationId);
            foreach (var member in members)
            {
                _ctx.ProjectMembers.Add(new ProjectMember { PersonId = member.PersonId, ProjectId = project.ProjectId, Task = member.Task });
            }
            _ctx.SaveChanges();

            return project;
        }

        private Project CreateProject(Application application, Consultation consultation)
        {
            Project project = Mapper.Map<Project>(application);
            var type = _ctx.ProjectTypes.FirstOrDefault(pt => pt.ProjectTypeId == project.ProjectTypeId);
            project.ProjectCode = string.Format("{0}_{1}_{2:D4}", SystemConfig.ApplicationStartYear, type.ProjectTypeId, ++type.Limit);
            project.StartDate = new DateTime(SystemConfig.ApplicationStartYear, 1, 1);
            project.EndDate = project.StartDate.AddYears(project.Period.Value);
            return project;
        }

        private AnnualTask AddAnnualTask(Project project, Consultation consultaion)
        {
            AnnualTask task = new AnnualTask
            {
                ProjectId = project.ProjectId,
                CurrentBudget = consultaion.CurrentYearBudget,
                EditTime = DateTime.Now,
                LeaderId = project.LeaderId,
                InstituteId = project.InstituteId,
                Year = SystemConfig.ApplicationStartYear,
                Name = string.Format("{0}({1})年度任务", project.Name, SystemConfig.ApplicationStartYear )
            };

            task = _ctx.AnnualTasks.Add(task);
            _ctx.SaveChanges();

            var jionInsts = _ctx.ProjectMembers
                .Where(pm => pm.ProjectId == project.ProjectId)
                .Select( pm=>pm.Person.InstituteId )
                .Distinct();

            foreach( var instId in jionInsts )
            {
                AnnualTaskInstBudget atis = new AnnualTaskInstBudget
                {
                    AnnualTaskId = task.AnnualTaskId,
                    InstituteId = instId,
                    Amount = 0
                };
                _ctx.AnnualTaskInstBudgets.Add(atis);
            }

            var subjects = _ctx.AccountingSubjects.ToList();
            foreach (var subject in subjects)
            {
                AnnualTaskBudgetItem item = new AnnualTaskBudgetItem
                {
                    SubjectId = subject.SubjectCode,
                    AnnualTaskId = task.AnnualTaskId,
                    Reason = "",
                    Amount = 0
                };
                _ctx.AnnualTaskBudgetItems.Add(item);
            }
            _ctx.SaveChanges();
            return task;
        }

        private int ParseTitle(Dictionary<string, string> applicationMap, Dictionary<string, string> projectMap, ISheet sheet, out Dictionary<string, int> propertyIndex)
        {
            IRow titleRow = sheet.GetRow(0);
            propertyIndex = new Dictionary<string, int>();
            Dictionary<string, int> titles = new Dictionary<string, int>();
            foreach (ICell cell in titleRow)
            {
                var columnName = Regex.Replace( cell.StringCellValue, @"\s", "");
                if( !titles.ContainsKey( columnName ))
                {
                    titles.Add(columnName, cell.ColumnIndex);
                }
            }

            if (applicationMap.Values.All(title => titles.Keys.Contains(title)))
            {
                foreach (var entry in applicationMap)
                {
                    int index = 0;
                    titles.TryGetValue(entry.Value, out index );
                    propertyIndex.Add(entry.Key, index);
                }
                return 1;
            }

            if (projectMap.Values.All(title => titles.Keys.Contains(title)))
            {
                foreach (var entry in projectMap)
                {
                    int index = 0;
                    titles.TryGetValue(entry.Value, out index);
                    propertyIndex.Add(entry.Key, index);
                }
                return 2;
            }
            return -1;
        }
         
        private AddConsultationDTO ParseConsultation(IRow row, Type type, Dictionary<string, int> propertyIndex)
        {
            StringBuilder errMsg = new StringBuilder();
            var constructor = type.GetConstructor(new Type[] { });
            AddConsultationDTO consultation = (AddConsultationDTO)constructor.Invoke(new object[] { });
            foreach (var entry in propertyIndex)
            {
                string content = null;
                var cell = row.GetCell(entry.Value);
                if (cell != null)
                {
                    if( cell.CellType == CellType.String )
                    {
                        content = cell.StringCellValue;
                    }
                    else if( cell.CellType == CellType.Numeric )
                    {
                        content = cell.NumericCellValue.ToString();
                    }
                }
                
                if (content == null || content.IsNullOrWhiteSpace())continue;
                
                var property = type.GetProperty(entry.Key);
                if (property.PropertyType.FullName.Contains("System.Int32"))
                {
                    int value = 0;
                    if (Int32.TryParse(content, out value))
                    {
                        property.SetValue(consultation, value);
                    }
                }
                else if (property.PropertyType.FullName.Contains("System.Double"))
                {
                    double value = 0;
                    if (Double.TryParse(content, out value))
                    {
                        property.SetValue(consultation, value);
                    }
                }
                else
                {
                    property.SetValue(consultation, content.Trim());
                }
            }
            return consultation;
        }

        public static Consultation TypeConverter( AddConsultationDTO addDTO )
        {

            if( addDTO is AddApplicationConsultationDTO )
            {
                AddApplicationConsultationDTO dto = (AddApplicationConsultationDTO)addDTO;
                return new ApplicationConsultation()
                {
                    ConsultationId = Guid.NewGuid().ToString(),
                    ApplicationId = dto.ApplicationId,
                    Period = dto.Period,
                    Budget = dto.TotalBudget,
                    CurrentYearBudget = dto.CurrentBudget,
                    DelegateType = (DelegateType)Enum.Parse(typeof(DelegateType), dto.DelegateType),
                    Result = (ApplicationConsultationResult)Enum.Parse(typeof(ApplicationConsultationResult), dto.Result, true)
                };
            }
            else if (addDTO is AddProjectConsultationDTO)
            {
                AddProjectConsultationDTO dto = (AddProjectConsultationDTO)addDTO;
                return new ProjectConsultation()
                {
                    ConsultationId = Guid.NewGuid().ToString(),
                    ProjectId = dto.ProjectId,
                    Period = dto.Period,
                    Budget = dto.TotalBudget,
                    CurrentYearBudget = dto.CurrentBudget,
                    DelegateType = (DelegateType)Enum.Parse(typeof(DelegateType), dto.DelegateType),
                    Result = (ProjectConsultationResult)Enum.Parse(typeof(ProjectConsultationResult), dto.Result, true),
                    ArrivalBudget = 0
                };
            }
            else
            {
                throw new Exception("参数类型错误");
            }
        }

        public static GetConsultationDTO TypeConverter( Consultation consultation )
        {
            var t = consultation.GetType().DeclaringType;
            var at = typeof(ApplicationConsultation).DeclaringType;
            if( consultation is ApplicationConsultation )
            {
                ApplicationConsultation ac = (ApplicationConsultation)consultation;
                return new GetApplicationConsultationDTO
                {
                    ApplicationId = ac.ApplicationId,
                    ProjectName = ac.Application.ProjectName,
                    Budget = ac.Budget,
                    CurrentYearBudget = ac.CurrentYearBudget,
                    DelegateType = ac.DelegateType == DelegateType.NORMAL ? "非定向":"定向",
                    InstituteName = ac.Application.Institute.Name,
                    LeaderName = ac.Application.Leader.Name,
                    Period = ac.Period.Value,
                    ProjectTypeName = ac.Application.ProjectType.Name,
                    Result = (int)ac.Result
                };
            }
            else if( consultation is ProjectConsultation )
            {
                ProjectConsultation pc = (ProjectConsultation)consultation;
                return new GetProjectConsultationDTO
                {
                    ProjectId = pc.ProjectId,
                    ProjectName = pc.Project.Name,
                    Budget = pc.Budget,
                    CurrentYearBudget = pc.CurrentYearBudget,
                    DelegateType = pc.DelegateType == DelegateType.NORMAL ? "非定向" : "定向",
                    InstituteName = pc.Project.Institude.Name,
                    LeaderName = pc.Project.Leader.Name,
                    Period = pc.Period.Value,
                    ProjectTypeName = pc.Project.ProjectType.Name,
                    Result = (int)pc.Result
                };
            }
            else
            {
                throw new OtherException("类型错误");
            }
        }
    
    }
}