using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ASPODES.Model;
using ASPODES.Database;
using ASPODES.WebAPI.Common;
using ASPODES.Common.Util;

namespace ASPODES.WebAPI.Repository
{
    public class AnnualTaskDocRepository : IAnnualTaskDocRepository
    {
        private AspodesDB _ctx;

        public AnnualTaskDocRepository(AspodesDB ctx)
        {
            _ctx = ctx;
        }

        public AnnualTaskDoc Get(int id)
        {
            return _ctx.AnnualTaskDocs.FirstOrDefault(atd => atd.AnnualTaskDocId == id);
        }

        public IQueryable<AnnualTaskDoc> GetAnnualTaskDocList()
        {
            return _ctx.AnnualTaskDocs;
        }

        public AnnualTaskDoc AddAttachment(AnnualTaskDoc doc)
        {
            var task = _ctx.AnnualTasks.FirstOrDefault(at => at.AnnualTaskId == doc.AnnualTaskId);

            if (!task.Editable())
                throw new OtherException("申请书状态不允许上传该文档");

            var entity = _ctx.AnnualTaskDocs.Add(doc);
            _ctx.SaveChanges();
            return entity;

        }

        public AnnualTaskDoc AddBody(AnnualTaskDoc doc)
        {
            var task = _ctx.AnnualTasks.FirstOrDefault(at => at.AnnualTaskId == doc.AnnualTaskId);
            if (null == task) throw new NotFoundException("未找到年度任务");

            if (!task.Editable())
                throw new OtherException("申请书状态不允许上传该文档");

            var transaction = _ctx.Database.BeginTransaction();
            try
            {
                //task.Status = AnnualTaskStatus.INST_CHECK;
                var pre = _ctx.AnnualTaskDocs.Where(atd => atd.AnnualTaskId == doc.AnnualTaskId && atd.Type == doc.Type);
                _ctx.AnnualTaskDocs.RemoveRange(pre);
                var cur = _ctx.AnnualTaskDocs.Add(doc);
                _ctx.SaveChanges();
                transaction.Commit();
                return cur;
            }
            catch (Exception e)
            {
                transaction.Commit();
                throw e;
            }

        }

        public AnnualTaskDoc AddAnnualReport(AnnualTaskDoc doc)
        {
            var task = _ctx.AnnualTasks.Include("Project")
                .FirstOrDefault(at => at.AnnualTaskId == doc.AnnualTaskId);
            if (null == task) throw new NotFoundException("未找到年度任务");

            if (!task.Terminable())
                throw new OtherException("申请书状态不允许上传该文档");

            var pdfName = DateTime.Now.ToFileTime() + task.Project.Name;
            if (!PdfHelper.ConvertTaskPdf(task.AnnualTaskId.Value, doc.Name, pdfName))
            {
                throw new OtherException("生成PDF文档错误");
            }

            doc.RelativeURL = doc.RelativeURL.Replace( doc.Name, pdfName + ".pdf");
            doc.Name = pdfName + ".pdf";
            
            var pre = _ctx.AnnualTaskDocs.Where(atd => atd.AnnualTaskId == doc.AnnualTaskId && atd.Type == AnnualTaskDocType.ANNUAL_REPORT);
            _ctx.AnnualTaskDocs.RemoveRange(pre);

            var cur = _ctx.AnnualTaskDocs.Add(doc);

            task.Status = AnnualTaskStatus.INST_REVIEW_ANNUAL_REPORT;
            _ctx.SaveChanges();

            return cur;
        }

        public void Delete(int id)
        {
            var entity = _ctx.AnnualTaskDocs.Include("AnnualTask").FirstOrDefault(atd => atd.AnnualTaskDocId == id);
            if (null == entity) throw new NotFoundException("未找到要删除的文档");
            if (!entity.AnnualTask.Editable())
                throw new OtherException("申请书状态不允许删除文档");

            _ctx.AnnualTaskDocs.Remove(entity);
            _ctx.SaveChanges();
        }
    }
}