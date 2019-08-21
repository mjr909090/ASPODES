using ASPODES.Database;
using ASPODES.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ASPODES.DTO.Project;
using ASPODES.DTO.AnnualTask;
using ASPODES.WebAPI.Common;
using ASPODES.Common.Util;

namespace ASPODES.WebAPI.Repository
{
    public class AnnualTaskRepository:IAnnualTaskRepository
    {
        private AspodesDB _context;
        /// <summary>
        /// 任务书操作类构造函数
        /// </summary>
        public AnnualTaskRepository(AspodesDB context)
        {
            _context = context;
        }

        public AnnualTask Get( int? annualTaskId )
        {
            AnnualTask task = _context.AnnualTasks.FirstOrDefault(at => at.AnnualTaskId == annualTaskId);
            if (null == task) throw new NotFoundException("没有找到任务书");
            return task;
        }

        public IQueryable<AnnualTask> GetAnnualTaskDetailList()
        {
            return _context.AnnualTasks
                .Include("AnnualTaskDocs")
                .Include("AnnualTaskBudgetItems")
                .Include("AnnualTaskInstBudgets")
                .Include("AnnualTaskInstBudgets.Institute")
                .OrderByDescending(at => at.EditTime);
        }

        /// <summary>
        /// 添加任务书
        /// </summary>
        /// <param name="annualTask">任务书实体</param>
        public AnnualTask AddAnnualTask(AnnualTask annualTask)
        {
            var addannualTask = _context.AnnualTasks.Add(annualTask);
            _context.SaveChanges();
            return addannualTask;
        }

        /// <summary>
        /// 查询任务书
        /// </summary>
        public IQueryable<AnnualTask> GetAnnualTaskList()
        {
            var getannualTask = _context.AnnualTasks.OrderByDescending( at=>at.EditTime );
            return getannualTask;
        }

        /// <summary>
        /// 根据负责人查询任务书
        /// </summary>
        public IQueryable<AnnualTask> GetAnnualTaskList(int leaderId)
        {
            var getannualTask = _context.AnnualTasks.Where(at => at.LeaderId == leaderId).OrderByDescending(at => at.EditTime);
            return getannualTask;
        }

        /// <summary>
        /// 查询任务书
        /// </summary>
        /// <param name="instId"></param>
        /// <param name="status"></param>
        /// <param name="year"></param>
        /// <returns></returns>
        public IQueryable<AnnualTask> GetAnnualTaskList( int instId, AnnualTaskStatus status, int year )
        {
            var getannualTask = _context.AnnualTasks
                .Where( at=>at.InstituteId == instId || instId == 0)
                .Where( at=>at.Status == status || status < 0 )
                .Where( at=>at.Year == year || year == 0 )
                .OrderByDescending(at => at.EditTime);
            return getannualTask;
        }

        public AnnualTask ChangeAnnualTaskStatus( AnnualTask task, AnnualTaskStatus from, AnnualTaskStatus to, string commnet = "" )
        {
            if (task.Status == from)
            {
                task.Status = to;
                task.Comment = commnet;
                _context.SaveChanges();
            }
            else
            {
                throw new OtherException("任务书状态不符合条件");
            }

            return task;
        }

        public AnnualTask SubmitAnnualTask(int id)
        {
            var task = _context.AnnualTasks.FirstOrDefault( at=>at.AnnualTaskId == id );
            if (task == null) throw new NotFoundException("未找到年度任务");
            if (!task.Editable()) throw new OtherException("任务书状态不符合条件");
            var body = _context.AnnualTaskDocs.FirstOrDefault( atd=>atd.AnnualTaskId == task.AnnualTaskId && atd.Type == AnnualTaskDocType.BODY );
            if( body == null )throw new NotFoundException("未找到年度任务正文");
            var pdfName = DateTime.Now.ToFileTime() + task.Project.Name;
            if( !PdfHelper.CreateTaskPdf( task.AnnualTaskId.Value, body.Name, pdfName , task.Year.Value ) )
            {
                throw new OtherException("生成PDF文件失败");
            }

            var pre = _context.AnnualTaskDocs.Where(atd => atd.Type == AnnualTaskDocType.PDF && atd.AnnualTaskId == task.AnnualTaskId);
            _context.AnnualTaskDocs.RemoveRange(pre);

            AnnualTaskDoc pdf = new AnnualTaskDoc()
            {
                AnnualTaskId = task.AnnualTaskId,
                Name = pdfName + ".pdf",
                RelativeURL = body.RelativeURL.Replace(body.Name, pdfName + ".pdf"),
                Type  = AnnualTaskDocType.PDF
            };

            _context.AnnualTaskDocs.Add(pdf);
            task.Status = AnnualTaskStatus.INST_CHECK;
            _context.SaveChanges();
            return task;
        }

        public void CreatePDF(int id)
        {
            var task = _context.AnnualTasks.FirstOrDefault(at => at.AnnualTaskId == id);
            if (task == null) throw new NotFoundException("未找到年度任务");
            //if (!task.Editable()) throw new OtherException("任务书状态不符合条件");
            var body = _context.AnnualTaskDocs.FirstOrDefault(atd => atd.AnnualTaskId == task.AnnualTaskId && atd.Type == AnnualTaskDocType.BODY);
            if (body == null) throw new NotFoundException("未找到年度任务正文");
            var pdfName = DateTime.Now.ToFileTime() + task.Project.Name;
            if (!PdfHelper.CreateTaskPdf(task.AnnualTaskId.Value, body.Name, pdfName, task.Year.Value))
            {
                throw new OtherException("生成PDF文件失败");
            }

            var pre = _context.AnnualTaskDocs.Where(atd => atd.Type == AnnualTaskDocType.PDF && atd.AnnualTaskId == task.AnnualTaskId);
            _context.AnnualTaskDocs.RemoveRange(pre);

            AnnualTaskDoc pdf = new AnnualTaskDoc()
            {
                AnnualTaskId = task.AnnualTaskId,
                Name = pdfName + ".pdf",
                RelativeURL = body.RelativeURL.Replace(body.Name, pdfName + ".pdf"),
                Type = AnnualTaskDocType.PDF
            };

            _context.AnnualTaskDocs.Add(pdf);
            //task.Status = AnnualTaskStatus.INST_CHECK;
            _context.SaveChanges();
            //return task;
        }

        /// <summary>
        /// 通过年度任务书ID获取项目ID
        /// </summary>
        /// <param name="annualTaskId"></param>
        /// <returns></returns>
        public string GetProjectIdByAnnualTask(int annualTaskId)
        {
            return _context.AnnualTasks.Find(annualTaskId).ProjectId;
        }

        /// <summary>
        /// 通过年度任务书ID获取项目所在单位ID
        /// </summary>
        /// <param name="annualTaskId"></param>
        /// <returns></returns>
        public int GetInstIdByAnnualTask(int annualTaskId)
        {
            return _context.AnnualTasks.Find(annualTaskId).InstituteId.Value;
        }

        /// <summary>
        /// 通过年度任务书ID获取项目负责人personId
        /// </summary>
        /// <param name="annualTaskId"></param>
        /// <returns></returns>
        public int GetPersonIdByAnnualTask(int annualTaskId)
        {
            return _context.AnnualTasks.Find(annualTaskId).LeaderId.Value;
        }
    }
}