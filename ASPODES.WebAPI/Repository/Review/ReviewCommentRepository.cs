using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Net;
using System.Web.Http;
using System.Net.Http;
using ASPODES.Model;
using ASPODES.DTO.Review;
using ASPODES.Database;
using AutoMapper;
using ASPODES.WebAPI.Common;
using System.Security.Principal;
using ASPODES.WebAPI;
using System.Text;
using ASPODES.DTO;
using ASPODES.Model;
using System.IO;
using NPOI.SS.UserModel;
using NPOI.SS.Util;
using ASPODES.Common;

namespace ASPODES.WebAPI.Repository
{
    /// <summary>
    /// 专家评审结果的操作类
    /// </summary>
    public class ReviewCommentRepository
    {
        /// <summary>
        /// 获取专家评审意见列表
        /// </summary>
        /// <param name="predicate">评审意见的查询条件</param>
        /// <param name="page">页码</param>
        /// <returns></returns>
        public PagingListDTO<GetReviewCommentDTO> GetPagingReviewCommentList(Func<ReviewAssignment, bool> predicate, int page)
        {
            PagingListDTO<GetReviewCommentDTO> pagingList = new PagingListDTO<GetReviewCommentDTO>();
            using (var ctx = new AspodesDB())
            {
                var assignmentIds = ctx.ReviewAssignments.Where(predicate).Select(ra => ra.ReviewAssignmentId);
                pagingList.TotalNum = ctx.ReviwerComments.Count(rc => assignmentIds.Contains(rc.ReviewAssignmentId));
                pagingList.TotalPageNum = (pagingList.TotalNum + SystemConfig.ReviewCommentPageCount - 1) / SystemConfig.ReviewCommentPageCount;
                if (pagingList.TotalPageNum <= 0) pagingList.TotalPageNum = 1;
                pagingList.NowPage = page <= 0 ? 1 : page;

                pagingList.ItemDTOs = ctx.ReviwerComments
                    .Where(rc => assignmentIds.Contains(rc.ReviewAssignmentId))
                    .Select(Mapper.Map<GetReviewCommentDTO>)
                    .OrderBy(rcd => rcd.ProjectName)
                    .OrderBy(rcd => rcd.ExpertName)
                    .Skip((pagingList.NowPage - 1) * SystemConfig.ReviewCommentPageCount)
                    .Take(SystemConfig.ReviewCommentPageCount)
                    .ToList();

                pagingList.NowNum = pagingList.ItemDTOs.Count();
                return pagingList;
            }
        }

        /// <summary>
        /// 获取申请书的评审意见列表
        /// </summary>
        /// <param name="predicate">申请书检索条件</param>
        /// <param name="year">评审年份</param>
        /// <param name="page">页码</param>
        /// <returns></returns>
        public PagingListDTO<GetApplicationReviewCommentDTO> GetPagingApplicationReviewCommentList(Func<Application, bool> predicate,int page)
        {
            PagingListDTO<GetApplicationReviewCommentDTO> pagingList = new PagingListDTO<GetApplicationReviewCommentDTO>();
            using (var ctx = new AspodesDB())
            {

                var applicationReviewComments = ctx.Applications.Include("ReviewComments")
                                    .Where(predicate)
                                    .Where(a => a.ReviewComments.Count() > 0)
                                    .Select(Mapper.Map<GetApplicationReviewCommentDTO>);

                pagingList.TotalNum = applicationReviewComments.Count();
                pagingList.TotalPageNum = (pagingList.TotalNum + SystemConfig.ApplicationPageCount - 1) / SystemConfig.ApplicationPageCount;
                if (pagingList.TotalPageNum <= 0) pagingList.TotalPageNum = 1;
                pagingList.NowPage = page <= 0 ? 1 : page;
                pagingList.ItemDTOs = applicationReviewComments
                    .OrderByDescending(arc => arc.TotalScore)
                    .Skip((pagingList.NowPage - 1) * SystemConfig.ApplicationPageCount)
                    .Take(SystemConfig.ApplicationPageCount)
                    .ToList();

                pagingList.NowNum = pagingList.ItemDTOs.Count();
                return pagingList;


            }
        }

        public GetApplicationReviewCommentDTO GetApplicationReviewComment(string applicationId)
        {
            using (var ctx = new AspodesDB())
            {
                var applicationReviewComments = ctx.Applications.Include("ReviewComments").FirstOrDefault(a => a.ApplicationId == applicationId);
                return Mapper.Map<GetApplicationReviewCommentDTO>(applicationReviewComments);
            }
        }
        /// <summary>
        /// 添加评审结果
        /// </summary>
        /// <param name="ReviewCommentDTO">评审结果信息</param>
        /// <returns>
        /// 添加成功，返回ResponseStatus.success
        /// 失败，返回ResponseStatus.unknown_error和错误信息
        /// </returns>
        public GetReviewCommentDTO AddOrUpdateReviewComment(AddReviewCommentDTO dto, Func<ReviewAssignment, bool> privilege)
        {
            using (var ctx = new AspodesDB())
            {
                //权限验证
                var assignment = ctx.ReviewAssignments.FirstOrDefault(ra => ra.ReviewAssignmentId == dto.ReviewAssignmentId);
                if (null == assignment) throw new NotFoundException();
                if (assignment.Overdue.Value) throw new OtherException("不允许执行操作");
                if (!privilege(assignment)) throw new UnauthorizationException();


                //补全信息
                var newComment = Mapper.Map<ReviewComment>(dto);
                newComment.ExpertId = assignment.ExpertId;
                newComment.ApplicationId = assignment.ApplicationId;
                newComment.Status = ReviwerCommentStatus.SAVE;
                newComment.Year = SystemConfig.ApplicationStartYear;
                newComment.ReviewCommentId = null;
                //添加或者更改
                var oldComment = ctx.ReviwerComments.FirstOrDefault(rc => rc.ReviewAssignmentId == dto.ReviewAssignmentId);
                var transaction = ctx.Database.BeginTransaction();
                try
                {
                    if (null == oldComment)
                    {
                        newComment.Status = ReviwerCommentStatus.SAVE;
                        ctx.ReviwerComments.Add(newComment);
                        assignment.Status = ReviewAssignmentStatus.ALREADY_REVIEW;
                        ctx.SaveChanges();

                        bool reviewFinish = ctx.ReviewAssignments
                            .Where(ra => ra.ApplicationId == assignment.ApplicationId && !ra.Overdue.Value && ra.Status != ReviewAssignmentStatus.CHANGE && ra.Status != ReviewAssignmentStatus.REFUSE)
                            .All(ra => ra.Status == Model.ReviewAssignmentStatus.ALREADY_REVIEW);
                        if (reviewFinish) assignment.Application.Status = Model.ApplicationStatus.FINISH_REVIEW;
                    }
                    else
                    {
                        //检查是否允许更改

                        //不允许更改的字段
                        newComment.ReviewCommentId = oldComment.ReviewCommentId;
                        newComment.Status = oldComment.Status;
                        newComment.ExpertId = oldComment.ExpertId;
                        newComment.ApplicationId = oldComment.ApplicationId;
                        //更新
                        ctx.Entry(oldComment).CurrentValues.SetValues(newComment);
                    }

                    ctx.SaveChanges();
                    transaction.Commit();
                }
                catch( Exception e )
                {
                    transaction.Rollback();
                    throw e;
                }
                
                return Mapper.Map<GetReviewCommentDTO>(newComment);
            }

        }


        /// <summary>
        /// 获取一条专家评审意见
        /// </summary>
        /// <param name="predicate">检索条件</param>
        /// <returns></returns>
        public GetReviewCommentDTO GetOneReviewComment(Func<ReviewComment, bool> predicate)
        {
            GetReviewCommentDTO commentDTO = null;
            using (var ctx = new AspodesDB())
            {
                var comment = ctx.ReviwerComments.FirstOrDefault(predicate);
                if (comment == null) throw new NotFoundException();
                commentDTO = Mapper.Map<GetReviewCommentDTO>(comment);
                return commentDTO;
            }

        }

        
        /// <summary>
        /// 导出初审结果
        /// </summary>
        /// <param name="predicate"></param>
        /// <param name="count"></param>
        /// <param name="userName"></param>
        /// <returns></returns>
        public HttpResponseMessage ExportReviewComment(Func<Application, bool> predicate, int count, string userName)
        {

            string relativePath = "";
            //string userName = UserHepler.GetCurrentUser().Name;
            FileStream writeStream = null;
            IWorkbook workbook = null;
            try
            {
                List<string> projectTypes = null;
                List<GetApplicationReviewCommentDTO> applications = new List<GetApplicationReviewCommentDTO>();
                using (var ctx = new AspodesDB())
                {
                    if (count <= 0) count = ctx.Applications.Count(predicate);
                    //筛选出需要导出的数据
                    applications = ctx.Applications.Include("ReviewComments")
                        .Where(predicate)
                        .Select(Mapper.Map<GetApplicationReviewCommentDTO>)
                        .Take(count)
                        .OrderByDescending(a => a.TotalScore)
                        .ToList();

                    projectTypes = ctx.ProjectTypes.Select(pt => pt.Name).ToList();
                }

                string path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, SystemConfig.ExportReviewCommentExcelTemplate);
                workbook = WorkbookFactory.Create(path);

                //设置单元格样式
                ICellStyle style = workbook.CreateCellStyle();
                style.Alignment = HorizontalAlignment.General;
                style.VerticalAlignment = VerticalAlignment.Center;

                //取出标题行
                ISheet titleSheet = workbook.GetSheetAt(0);
                IRow titleRow = titleSheet.GetRow(0);
                int columns = titleRow.Cells.Count;

                foreach (var type in projectTypes)
                {
                    //按项目类型取出数据
                    var typeApplications = applications
                        .Where(a => type.Equals(a.ProjectTypeName))
                        .OrderByDescending(a => a.TotalScore);

                    if (typeApplications.Count() == 0) continue;

                   
                    //应用单元格样式
                    ISheet sheet = workbook.CreateSheet(type);
                    for (int i = 0; i < columns; i++)
                    {
                        sheet.SetDefaultColumnStyle(i, style);
                    }

                    //复制标题行
                    IRow firstRow = sheet.CreateRow(0);
                    CopyRow(titleRow, firstRow);

                    //插入数据
                    int insertRowIndex = 1;
                    for (int i = 0; i < typeApplications.Count(); i++)
                    {
                        var applicationComments = typeApplications.ElementAt(i);
                        IRow row = sheet.CreateRow(insertRowIndex);
                        int areaRows = applicationComments.ReviewComments.Count();
                        if (areaRows <= 0) continue;
                        if (!applicationComments.TotalScore.HasValue) return ResponseWrapper.ExceptionResponse(new OtherException("请先手动计算总分后再导出数据"));
                        SetMergedRegion(row, applicationComments.ProjectName, new CellRangeAddress(insertRowIndex, insertRowIndex + areaRows - 1, 0, 0));
                        SetMergedRegion(row, applicationComments.ApplicationId, new CellRangeAddress(insertRowIndex, insertRowIndex + areaRows - 1, 1, 1));
                        SetMergedRegion(row, applicationComments.ProjectTypeName, new CellRangeAddress(insertRowIndex, insertRowIndex + areaRows - 1, 2, 2));
                        SetMergedRegion(row, applicationComments.InstituteName, new CellRangeAddress(insertRowIndex, insertRowIndex + areaRows - 1, 3, 3));
                        SetMergedRegion(row, applicationComments.LeaderName, new CellRangeAddress(insertRowIndex, insertRowIndex + areaRows - 1, 4, 4));
                        SetMergedRegion(row, applicationComments.TotalBudget.Value, new CellRangeAddress(insertRowIndex, insertRowIndex + areaRows - 1, 5, 5));
                        SetMergedRegion(row, applicationComments.FirstYearBudget.Value, new CellRangeAddress(insertRowIndex, insertRowIndex + areaRows - 1, 6, 6));
                        SetMergedRegion(row, applicationComments.Period.Value, new CellRangeAddress(insertRowIndex, insertRowIndex + areaRows - 1, 7, 7));
                        SetMergedRegion(row, applicationComments.TotalScore.Value, new CellRangeAddress(insertRowIndex, insertRowIndex + areaRows - 1, 8, 8));
                        SetMergedRegion(row, (i + 1), new CellRangeAddress(insertRowIndex, insertRowIndex + areaRows - 1, 9, 9));

                        for (int j = 0; j < areaRows; j++)
                        {
                            var comment = applicationComments.ReviewComments.ElementAt(j);
                            IRow commentRow = sheet.GetRow(insertRowIndex + j);
                            if (commentRow == null) commentRow = sheet.CreateRow(insertRowIndex + j);
                            SetCell(commentRow, "第" + (j + 1) + "位专家", 10);
                            SetCell(commentRow, comment.Level, 11);
                            SetCell(commentRow, comment.Imburse, 12);
                            SetCell(commentRow, comment.Comment, 13);
                        }

                        insertRowIndex += areaRows;
                    }
                    //sheet.SetColumnHidden(2, false);
                }

                //删除标题Sheet页
                if( workbook.NumberOfSheets > 1 )workbook.RemoveSheetAt(0);

                //保存数据
                string filename = userName + DateTime.Now.ToFileTime() + ".xls";
                relativePath = Path.Combine(SystemConfig.ExportReviewCommentPath, filename);
                string savePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, SystemConfig.ExportReviewCommentPath, filename);
                writeStream = File.Create(savePath);
                workbook.Write(writeStream);
                writeStream.Flush();
            }
            catch (Exception e)
            {
                return ResponseWrapper.ExceptionResponse(e);
            }
            finally
            {
                if (null != writeStream)
                {
                    writeStream.Close();
                    writeStream.Dispose();
                }
                if (null != workbook) workbook.Close();
            }

            return FileHelper.Download(HttpContext.Current, "\\" + relativePath, SystemConfig.ApplicationStartYear + ".xls");

        }


        /// <summary>
        /// 计算所有申请书的初审总分
        /// </summary>
        public static void ReviewCommentTotalScore()
        {
            try
            {
                using (var ctx = new AspodesDB())
                {
                    var applications = ctx.Applications.Where(
                        a => a.CurrentYear == SystemConfig.ApplicationStartYear //本年度的
                        //&& a.Status == ApplicationStatus.FINISH_REVIEW//评审完结的申请书
                        );
                    foreach (var a in applications)
                    {
                        var comments = ctx.ReviwerComments.Where(rc => rc.ApplicationId == a.ApplicationId && rc.Year == SystemConfig.ApplicationStartYear);
                        a.TotalScore = CalculationReviewCommentTotalScore(comments.ToList());
                    }
                    ctx.SaveChanges();
                }
            }
            catch (Exception e)
            {
                //写日志
            }
        }

        /// <summary>
        /// 计算一份申请书的总分
        /// </summary>
        /// <param name="comments">申请书评审意见集合</param>
        /// <returns></returns>
        public static double CalculationReviewCommentTotalScore(ICollection<ReviewComment> comments)
        {
            int total = 0;
            try
            {
                foreach (var c in comments)
                {
                    //total += (100 - (c.Level.ElementAt(0) - 'A') * 15) * 60;
                    //total += (100 - (c.Imburse.ElementAt(0) - 'A') * 15) * 40;
                    total += c.Score.HasValue ? c.Score.Value : 0;
                }
                //total = total / (comments.Count() * 100);
                total /= comments.Count();
            }
            catch (Exception e)
            {
                return -1;
            }
            return total;
        }


        private void SetMergedRegion(IRow row, string value, CellRangeAddress address)
        {
            ICell cell = row.CreateCell(address.FirstColumn);
            cell.SetCellValue(value);
            row.Sheet.AddMergedRegion(address);
        }

        private void SetMergedRegion(IRow row, double value, CellRangeAddress address)
        {
            ICell cell = row.CreateCell(address.FirstColumn);
            cell.SetCellValue(value);
            row.Sheet.AddMergedRegion(address);
        }

        private void SetCell(IRow row, string value, int columIndex)
        {
            ICell cell = row.CreateCell(columIndex);
            cell.SetCellValue(value);
        }

        private void CopyRow(IRow source, IRow dest)
        {
            foreach (var cell in source)
            {
                var copy = dest.CreateCell(cell.ColumnIndex);
                copy.SetCellValue(cell.StringCellValue);
            }
        }

        /// <summary>
        /// 通过初审评论ID获取申请书ID
        /// </summary>
        /// <param name="assignmentCommentId"></param>
        /// <returns></returns>
        public string GetAppId(int assignmentCommentId)
        {
            using (var _context = new AspodesDB())
            {
                return _context.ReviwerComments
                    .FirstOrDefault(rc => rc.ReviewAssignmentId == assignmentCommentId)
                    .ApplicationId;
            }
                
        }

        /// <summary>
        /// 获取申请书当前已被多少专家评审完
        /// </summary>
        /// <param name="appId"></param>
        /// <returns></returns>
        public int getReviewedNum(string appId)
        {
            using (var _context = new AspodesDB())
            {
                return _context.ReviwerComments
                    .Where(rc => rc.ApplicationId == appId 
                    && rc.Status != ReviwerCommentStatus.NULL).Count();
            }
        }
        
    }


}