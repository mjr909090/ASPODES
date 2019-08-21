using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Drawing;
using System.Security.Permissions;
using Aspose.Words;
using Aspose.Words.Saving;
using ASPODES.Database;
using ASPODES.Model;
using Novacode;

namespace ASPODES.Common.Util
{
    public class PdfHelper
    {
        private static string path = "View\\Upload\\";
        private static string AnnualTaskPath = "View\\Upload\\AnnualTask\\";
        private static string projectPath = "View\\Upload\\Project\\";

        public static bool CreatePdf(string applicationId ,string relativeURL, string pdfName,int startYear)
        {
            string localPath = HttpRuntime.AppDomainAppPath;
            string modelPath = Path.Combine(localPath, "DocTemplate\\ModelTitle.docx");//前两页模板
            string personPath = Path.Combine(localPath, "DocTemplate\\ModelPersonNew.docx");//单位人员模板
            //string otherPersonPath = Path.Combine(localPath, "DocTemplate\\ModelOtherPerson.docx");//其他单位人员模板
            string yearAnnualPath = Path.Combine(localPath, "DocTemplate\\ModelYearAmount.docx");//其他单位人员模板
            string allAnnualPath = Path.Combine(localPath, "DocTemplate\\ModelAllAmount.docx");//其他单位人员模板



            //string uploadPath = Path.Combine(localPath, path + applicationId + "\\" + uploadDocName);//上传文档
            string uploadPath = Path.Combine(localPath, relativeURL.Substring(1).Replace("/", @"\"));//上传文档

            string titlePath = Path.Combine(localPath, path + applicationId + "\\" + Guid.NewGuid() + ".docx");//前两页doc文档

            string lastPath = Path.Combine(localPath, "DocTemplate\\ModelLast.docx");//最后一页

            int n = 0;
            int year = 0;
            List<string> pathList = new List<string>();
            string tenpPath;

            if (File.Exists(modelPath) && File.Exists(uploadPath))
            {
                try
                {
                    using (var db = new AspodesDB())
                    {
                        Application app1 = db.Applications.FirstOrDefault(c => c.ApplicationId == applicationId);
                        if (app1!= null)
                        {

                            //生成文档前两页
                            using (DocX tempDoc = DocX.Load(modelPath))
                            {
                                tempDoc.AddCustomProperty(new CustomProperty("ProjectName", app1.ProjectName));
                                tempDoc.AddCustomProperty(new CustomProperty("InstituteName", app1.Institute.Name));
                                tempDoc.AddCustomProperty(new CustomProperty("LeaderName", app1.Leader.Name));
                                tempDoc.AddCustomProperty(new CustomProperty("Period", app1.Period.ToString() + "年"));
                                tempDoc.AddCustomProperty(new CustomProperty("TimeYear", DateTime.Now.Year));
                                tempDoc.AddCustomProperty(new CustomProperty("TimeMonth", DateTime.Now.Month));
                                tempDoc.AddCustomProperty(new CustomProperty("ProjectType", app1.ProjectType.Name + "：" + app1.SupportCategory.Name));
                                tempDoc.AddCustomProperty(new CustomProperty("PeriodStartYear", startYear));
                                tempDoc.AddCustomProperty(new CustomProperty("PeriodYear", (startYear + app1.Period - 1).ToString()));
                                tempDoc.AddCustomProperty(new CustomProperty("LeaderSex", app1.Leader.Male));
                                tempDoc.AddCustomProperty(new CustomProperty("LeaderTitle", app1.Leader.Title));
                                tempDoc.AddCustomProperty(new CustomProperty("LeaderEmail", app1.LeaderEmail));
                                tempDoc.AddCustomProperty(new CustomProperty("LeaderPhone", app1.LeaderPhone));
                                tempDoc.AddCustomProperty(new CustomProperty("ContactName", app1.Contact.Name));
                                tempDoc.AddCustomProperty(new CustomProperty("ContactPhone", app1.ContactPhone));
                                tempDoc.AddCustomProperty(new CustomProperty("ContactEmail", app1.ContactEmail));
                                tempDoc.AddCustomProperty(new CustomProperty("TotalBudget", app1.TotalBudget.ToString()));
                                tempDoc.AddCustomProperty(new CustomProperty("ProjectDetail", app1.AbstractContent));
                                tempDoc.AddCustomProperty(new CustomProperty("2017Budget", app1.FirstYearBudget.ToString()));

                                tempDoc.SaveAs(titlePath);

                                
                            }

                            pathList.Clear();

                            //人员文档
                            //获取单位人员
                            var m1 = db.Members.Where(c => c.ApplicationId == applicationId &&
                                                           c.Person.InstituteId == app1.InstituteId).OrderBy(c => c.PersonId).ToList();

                            //获取其他单位人员
                            var m2 = db.Members.Where(c => c.ApplicationId == applicationId &&
                                                           c.Person.InstituteId != app1.InstituteId).OrderBy(c => c.PersonId).ToList();

                            //获取当前用户信息
                            var userid = HttpContext.Current.User.Identity.Name;
                            var user = db.Users.FirstOrDefault(c => c.UserId == userid);
                            if (user==null)
                            {
                                return false;
                            }
                            
                            //获取项目负责人
                            Member userMembers = m1.Single(c => c.PersonId == user.PersonId);

                            //获取单位其他成员
                            m1 = m1.Where(c => c.PersonId != user.PersonId).OrderBy(c => c.Person.Name).ToList();
                            
                            using (DocX tempDoc = DocX.Load(personPath))
                            {
                                //填充项目负责人
                                tempDoc.AddCustomProperty(new CustomProperty("Name1", userMembers.Person.Name));
                                tempDoc.AddCustomProperty(new CustomProperty("Depart1", userMembers.Person.Institute.Name));
                                tempDoc.AddCustomProperty(new CustomProperty("IDCard1", userMembers.Person.IDCard));
                                tempDoc.AddCustomProperty(new CustomProperty("Sex1", userMembers.Person.Male));
                                tempDoc.AddCustomProperty(new CustomProperty("Major1", userMembers.Person.Major));
                                tempDoc.AddCustomProperty(new CustomProperty("Duty1", userMembers.Person.Duty));
                                tempDoc.AddCustomProperty(new CustomProperty("Task1", userMembers.Task));
                                tempDoc.AddCustomProperty(new CustomProperty("Phone1", userMembers.Person.Phone));

                                //获取当前人员表格
                                Table t = tempDoc.Tables[0];
                                Row userRow;

                                for (int i = 0; i < m1.Count; i++)
                                {
                                    userRow = t.InsertRow();
                                    userRow.Cells[1].Paragraphs.First().Append(m1[i].Person.Name).FontSize(10).Font(new FontFamily("SimHei")).Font(new FontFamily("Times New Roman")).Alignment = Alignment.center;
                                    userRow.Cells[2].Paragraphs.First().Append(m1[i].Person.Institute.Name).FontSize(10).Font(new FontFamily("SimHei")).Font(new FontFamily("Times New Roman")).Alignment = Alignment.center;
                                    userRow.Cells[3].Paragraphs.First().Append(m1[i].Person.IDCard).FontSize(10).Font(new FontFamily("SimHei")).Font(new FontFamily("Times New Roman")).Alignment = Alignment.center;
                                    userRow.Cells[4].Paragraphs.First().Append(m1[i].Person.Male).FontSize(10).Font(new FontFamily("SimHei")).Font(new FontFamily("Times New Roman")).Alignment = Alignment.center;
                                    userRow.Cells[5].Paragraphs.First().Append(m1[i].Person.Major).FontSize(10).Font(new FontFamily("SimHei")).Font(new FontFamily("Times New Roman")).Alignment = Alignment.center;
                                    userRow.Cells[6].Paragraphs.First().Append(m1[i].Person.Duty).FontSize(10).Font(new FontFamily("SimHei")).Font(new FontFamily("Times New Roman")).Alignment = Alignment.center;
                                    userRow.Cells[7].Paragraphs.First().Append(m1[i].Task).FontSize(10).Font(new FontFamily("SimHei")).Font(new FontFamily("Times New Roman")).Alignment = Alignment.center;
                                    userRow.Cells[8].Paragraphs.First().Append(m1[i].Person.Phone).FontSize(10).Font(new FontFamily("SimHei")).Font(new FontFamily("Times New Roman")).Alignment = Alignment.center;

                                    //水平居中

                                    for (int j = 1; j < 9; j++)
                                    {
                                        userRow.Cells[j].VerticalAlignment = VerticalAlignment.Center;
                                    }

                                    t.Rows.Add(userRow);
                                }

                                //本单位人员至少有3行，总行数最少为6行
                                while (t.Rows.Count<6)
                                {
                                    userRow = t.InsertRow();
                                    t.Rows.Add(userRow);
                                }

                                //填充单位信息并合并单元格
                                t.Rows[3].Cells[0].Paragraphs.First().Append("项目主持单位").Bold().FontSize(12).Alignment = Alignment.center;
                                t.Rows[3].Cells[0].VerticalAlignment = VerticalAlignment.Center;
                                t.MergeCellsInColumn(0, 3, t.Rows.Count-1);

                                //其他单位人员起始行
                                int otherRow = t.Rows.Count;

                                //填充其他单位人员
                                for (int i = 0; i < m2.Count; i++)
                                {
                                    userRow = t.InsertRow();
                                    userRow.Cells[1].Paragraphs.First().Append(m2[i].Person.Name).FontSize(10).Font(new FontFamily("SimHei")).Font(new FontFamily("Times New Roman")).Alignment = Alignment.center;
                                    userRow.Cells[2].Paragraphs.First().Append(m2[i].Person.Institute.Name).FontSize(10).Font(new FontFamily("SimHei")).Font(new FontFamily("Times New Roman")).Alignment = Alignment.center;
                                    userRow.Cells[3].Paragraphs.First().Append(m2[i].Person.IDCard).FontSize(10).Font(new FontFamily("SimHei")).Font(new FontFamily("Times New Roman")).Alignment = Alignment.center;
                                    userRow.Cells[4].Paragraphs.First().Append(m2[i].Person.Male).FontSize(10).Font(new FontFamily("SimHei")).Font(new FontFamily("Times New Roman")).Alignment = Alignment.center;
                                    userRow.Cells[5].Paragraphs.First().Append(m2[i].Person.Major).FontSize(10).Font(new FontFamily("SimHei")).Font(new FontFamily("Times New Roman")).Alignment = Alignment.center;
                                    userRow.Cells[6].Paragraphs.First().Append(m2[i].Person.Duty).FontSize(10).Font(new FontFamily("SimHei")).Font(new FontFamily("Times New Roman")).Alignment = Alignment.center;
                                    userRow.Cells[7].Paragraphs.First().Append(m2[i].Task).FontSize(10).Font(new FontFamily("SimHei")).Font(new FontFamily("Times New Roman")).Alignment = Alignment.center;
                                    userRow.Cells[8].Paragraphs.First().Append(m2[i].Person.Phone).FontSize(10).Font(new FontFamily("SimHei")).Font(new FontFamily("Times New Roman")).Alignment = Alignment.center;

                                    //水平居中
                                    for (int j = 1; j < 9; j++)
                                    {
                                        userRow.Cells[j].VerticalAlignment = VerticalAlignment.Center;
                                    }

                                    t.Rows.Add(userRow);
                                }

                                //其他单位人员至少有3行
                                for (int i = 0; i < 3-m2.Count; i++)
                                {
                                    userRow = t.InsertRow();
                                    t.Rows.Add(userRow);
                                }

                                //填充其他单位信息并合并单元格
                                t.Rows[otherRow].Cells[0].Paragraphs.First().Append("项目协作单位").Bold().FontSize(12).Alignment = Alignment.center;
                                t.Rows[otherRow].Cells[0].VerticalAlignment = VerticalAlignment.Center;
                                t.MergeCellsInColumn(0, otherRow, t.Rows.Count - 1);

                                //人员文档临时存储位置
                                tenpPath = Path.Combine(localPath, path + applicationId + "\\" + Guid.NewGuid() + ".docx");
                                tempDoc.SaveAs(tenpPath);
                                pathList.Add(tenpPath);
                            }
                            

                            //上传文档
                            pathList.Add(uploadPath);

                            //单位年度预算 第一年
                            using (DocX tempDoc = DocX.Load(yearAnnualPath))
                            {
                                IQueryable<AnnualBudgetItem> abilist = db.AnnualBudgetItems
                                    .Where(c => c.AnnualBudget.ApplicationId == applicationId &&
                                                c.AnnualBudget.Year == 1)//DateTime.Now.Year
                                    .OrderByDescending(c => c.AnnualBudget.AnnualBudgetId).ThenBy(c=>c.AnnualBudgetItemId)
                                    .Take(12);

                                tempDoc.AddCustomProperty(new CustomProperty("SubjectYear", startYear));
                                if (abilist.Any())
                                {
                                    n = 1;
                                    foreach (var temp in abilist)
                                    {
                                        tempDoc.AddCustomProperty(new CustomProperty("SubjectName" + n, temp.Subject.SubjectName));
                                        tempDoc.AddCustomProperty(new CustomProperty("Amount" + n, temp.Amount.ToString()));
                                        tempDoc.AddCustomProperty(new CustomProperty("Reason" + n, temp.Reason));

                                        n++;
                                    }
                                    tempDoc.AddCustomProperty(new CustomProperty("TotalAmount", abilist.First().AnnualBudget.Amount.ToString()));
                                }

                                tenpPath = Path.Combine(localPath, path + applicationId + "\\" + Guid.NewGuid() + ".docx");
                                pathList.Add(tenpPath);
                                tempDoc.SaveAs(tenpPath);
                            }

                            IQueryable<AnnualBudget> ablist =
                                db.AnnualBudgets
                                    .Where(c => c.ApplicationId == applicationId && c.Year != 1)//DateTime.Now.Year
                                    .OrderBy(c => c.Year);
                            if (ablist.Any())
                            {
                                foreach (var temp in ablist)
                                {
                                    using (DocX tempDoc = DocX.Load(yearAnnualPath))
                                    {
                                        IQueryable<AnnualBudgetItem> tempabilist =
                                            db.AnnualBudgetItems.Where(c => c.AnnualBudgetId == temp.AnnualBudgetId)
                                                .OrderBy(c => c.AnnualBudgetItemId);
                                        year = startYear + temp.Year.Value - 1;
                                        tempDoc.AddCustomProperty(new CustomProperty("SubjectYear", year));//
                                        if (tempabilist.Any())
                                        {
                                            n = 1;
                                            foreach (var tempitem in tempabilist)
                                            {
                                                tempDoc.AddCustomProperty(new CustomProperty("SubjectName" + n, tempitem.Subject.SubjectName));
                                                tempDoc.AddCustomProperty(new CustomProperty("Amount" + n, tempitem.Amount.ToString()));
                                                tempDoc.AddCustomProperty(new CustomProperty("Reason" + n, tempitem.Reason));

                                                n++;
                                            }
                                        }
                                        tempDoc.AddCustomProperty(new CustomProperty("TotalAmount", temp.Amount.ToString()));

                                        tenpPath = Path.Combine(localPath, path + applicationId + "\\" + Guid.NewGuid() + ".docx");
                                        pathList.Add(tenpPath);
                                        tempDoc.SaveAs(tenpPath);
                                    }
                                }
                            }

                            using (DocX tempDoc = DocX.Load(allAnnualPath))
                            {
                                //单位全部预算

                                tempDoc.AddCustomProperty(new CustomProperty("Year1", startYear));
                                tempDoc.AddCustomProperty(new CustomProperty("Year2", startYear + 1));
                                tempDoc.AddCustomProperty(new CustomProperty("Year3", startYear + 2));

                                InstBudget ib1 = db.InstBudgets
                                    .Where(c => c.ApplicationId == applicationId && c.InstituteId == app1.InstituteId)
                                    .FirstOrDefault();
                                if (ib1 != null)
                                {
                                    tempDoc.AddCustomProperty(new CustomProperty("Depart1", app1.Institute.Name));
                                    tempDoc.AddCustomProperty(new CustomProperty("TotalAmount1", ib1.Amount.ToString()));

                                    IQueryable<InstAnnualBudget> iablist =
                                        db.InstAnnualBudgets.Where(
                                            c => c.InstBudgetId==ib1.InstBudgetId);
                                    year = 1;//DateTime.Now.Year;
                                    if (iablist.FirstOrDefault(c => c.Year == year) != null)
                                    {
                                        tempDoc.AddCustomProperty(new CustomProperty("2017Amount1", iablist.FirstOrDefault(c => c.Year == year).Amount.ToString()));
                                    }

                                    year = 2;//DateTime.Now.AddYears(1).Year;
                                    if (iablist.FirstOrDefault(c => c.Year == year) != null)
                                    {
                                        tempDoc.AddCustomProperty(new CustomProperty("2018Amount1", iablist.FirstOrDefault(c => c.Year == year).Amount.ToString()));
                                    }

                                    year = 3;//DateTime.Now.AddYears(2).Year;
                                    if (iablist.FirstOrDefault(c => c.Year == year) != null)
                                    {
                                        tempDoc.AddCustomProperty(new CustomProperty("2019Amount1", iablist.FirstOrDefault(c => c.Year == year).Amount.ToString()));
                                    }
                                }

                                IQueryable<InstBudget> iblist =
                                    db.InstBudgets.Where(
                                            c => c.ApplicationId == applicationId && c.InstituteId != app1.InstituteId)
                                        .OrderBy(c => c.InstituteId)
                                        .Take(5);
                                if (iblist.Any())
                                {
                                    n = 2;
                                    foreach (var temp in iblist)
                                    {
                                        tempDoc.AddCustomProperty(new CustomProperty("Depart"+n, temp.Institute.Name));
                                        tempDoc.AddCustomProperty(new CustomProperty("TotalAmount"+n, temp.Amount.ToString()));
                                        IQueryable<InstAnnualBudget> tempiablist =
                                            db.InstAnnualBudgets.Where(c => c.InstBudgetId == temp.InstBudgetId);
                                        if (tempiablist.Any())
                                        {
                                            year = 1;
                                            if (tempiablist.FirstOrDefault(c => c.Year == year) != null)
                                            {
                                                tempDoc.AddCustomProperty(new CustomProperty("2017Amount" + n, tempiablist.FirstOrDefault(c => c.Year == year).Amount.ToString()));
                                            }

                                            year = 2;
                                            if (tempiablist.FirstOrDefault(c => c.Year == year) != null)
                                            {
                                                tempDoc.AddCustomProperty(new CustomProperty("2018Amount" + n, tempiablist.FirstOrDefault(c => c.Year == year).Amount.ToString()));
                                            }

                                            year = 3;
                                            if (tempiablist.FirstOrDefault(c => c.Year == year) != null)
                                            {
                                                tempDoc.AddCustomProperty(new CustomProperty("2019Amount" + n, tempiablist.FirstOrDefault(c => c.Year == year).Amount.ToString()));
                                            }
                                        }

                                        n++;
                                    }
                                }


                                tenpPath = Path.Combine(localPath, path + applicationId + "\\" + Guid.NewGuid() + ".docx");
                                pathList.Add(tenpPath);
                                tempDoc.SaveAs(tenpPath);
                            }


                            pathList.Add(lastPath);


                            //移除原文档
                            //if (File.Exists(Path.Combine(localPath, path + applicationId + "\\" + pdfName + ".docx")))
                            //{
                            //    File.Delete(Path.Combine(localPath, path + applicationId + "\\" + pdfName + ".docx"));
                            //}
                            //if (File.Exists(Path.Combine(localPath, path + applicationId + "\\" + pdfName + ".pdf")))
                            //{
                            //    File.Delete(Path.Combine(localPath, path + applicationId + "\\" + pdfName + ".pdf"));
                            //}


                            //合并文档
                            Document srcDoc = new Document(titlePath);
                            Document tempDocs;
                            foreach (var temp in pathList)
                            {
                                tempDocs = new Document(temp);
                                srcDoc.AppendDocument(tempDocs, ImportFormatMode.KeepSourceFormatting);
                            }

                            
                            srcDoc.Save(Path.Combine(localPath, path + applicationId + "\\" + pdfName+".docx"), Aspose.Words.SaveFormat.Docx);
                            srcDoc.Save(Path.Combine(localPath, path + applicationId + "\\" + pdfName + ".pdf"), Aspose.Words.SaveFormat.Pdf);
                            
                            pathList.Remove(uploadPath);
                            pathList.Remove(lastPath);
                            pathList.Add(titlePath);
                            foreach (var temp in pathList)
                            {
                                if (File.Exists(temp))
                                {
                                    File.Delete(temp);
                                }
                            }
                        }
                        else
                        {
                            return false;
                        }
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                    return false;
                }
                return true;
            }
            else
            {
                return false;
            }
            
        }

        /// <summary>
        /// 加密PDF
        /// </summary>
        /// <param name="applicationId">申请书ID</param>
        /// <param name="applicationPdfName">申请书生成的PDF文件名</param>
        /// <param name="pdfName">加密后的PDF文件名</param>
        /// <param name="password">密码</param>
        /// <returns></returns>
        public static bool EncryptPdf(string applicationId, string applicationPdfName, string pdfName, string password)
        {
            string localPath = HttpRuntime.AppDomainAppPath;
            string docPath = Path.Combine(localPath, path + applicationId + "\\" + applicationPdfName + ".docx");
            if (File.Exists(docPath))
            {
                try
                {
                    //加密文档
                    Document pdfDoc = new Document(docPath);
                    //Instantiate PDFSaveOptions to manage security attributes
                    Aspose.Words.Saving.PdfSaveOptions saveOption = new Aspose.Words.Saving.PdfSaveOptions();
                    //设置保存格式为PDF
                    saveOption.SaveFormat = Aspose.Words.SaveFormat.Pdf;
                    PdfEncryptionDetails encryptionDetails = new PdfEncryptionDetails(password, (HashHelper.IntoMd5(applicationId)).Substring(0, 10), PdfEncryptionAlgorithm.RC4_128);
                    encryptionDetails.Permissions = PdfPermissions.DisallowAll;
                    saveOption.EncryptionDetails = encryptionDetails;
                    pdfDoc.Save(Path.Combine(localPath, path + "Temp\\" + pdfName), saveOption);
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                    return false;
                }
                return true;
            }
            else
            {
                return false;
            }
        }



        public static bool CreateTaskPdf(int annualTaskId, string uploadDocName, string pdfName, int startYear)
        {
            string localPath = HttpRuntime.AppDomainAppPath;

            //合成文档内容列表
            List<string> pathList = new List<string>();

            //模板文档
            string modelPath = Path.Combine(localPath, "DocTemplate\\Task\\TaskModelTitle.docx");//前两页模板
            string personPath = Path.Combine(localPath, "DocTemplate\\Task\\TaskModelPerson.docx");//单位人员模板
            string yearAnnualPath = Path.Combine(localPath, "DocTemplate\\Task\\TaskModelYearAmount.docx");//其他单位人员模板
            string departAnnualPath = Path.Combine(localPath, "DocTemplate\\Task\\TaskModelDepartAmount.docx");//其他单位人员模板




            string uploadPath = Path.Combine(localPath, AnnualTaskPath + annualTaskId + "\\" + uploadDocName);//上传文档

            string endPath = Path.Combine(localPath, "DocTemplate\\Task\\TaskModelEnd.docx");


            string titlePath = Path.Combine(localPath, AnnualTaskPath + annualTaskId + "\\" + Guid.NewGuid() + ".docx");//前两页doc文档

            string temppath = string.Empty;

            if (File.Exists(modelPath) && File.Exists(uploadPath))
            {
                try
                {
                    using (var db = new AspodesDB())
                    {
                        AnnualTask task1 = db.AnnualTasks.Where(c => c.AnnualTaskId == annualTaskId).Include(c => c.Project).Include(c => c.Leader).Include(c=>c.Institute).FirstOrDefault();
                        if (task1 != null)
                        {
                            string part0Path = Path.Combine(localPath, path + task1.Project.ApplicationId + "\\part0.docx");//申请书文档0
                            string part1Path = Path.Combine(localPath, path + task1.Project.ApplicationId + "\\part1.docx");//申请书文档1
                            string part2Path = Path.Combine(localPath, path + task1.Project.ApplicationId + "\\part2.docx");//申请书文档2
                            string part3Path = Path.Combine(localPath, path + task1.Project.ApplicationId + "\\part3.docx");//申请书文档3
                            string part4Path = Path.Combine(localPath, path + task1.Project.ApplicationId + "\\part4.docx");//申请书文档4

                            if (File.Exists(part0Path)
                                && File.Exists(part1Path)
                                && File.Exists(part2Path)
                                && File.Exists(part3Path)
                                && File.Exists(part4Path))
                            {
                                Application app1 = db.Applications.First(c => c.ApplicationId == task1.Project.ApplicationId);

                                //更新前两页信息
                                using (DocX tempDoc = DocX.Load(modelPath))
                                {
                                    tempDoc.AddCustomProperty(new CustomProperty("TaskNo", task1.Project.ProjectCode));
                                    tempDoc.AddCustomProperty(new CustomProperty("TaskName", task1.Project.Name));
                                    tempDoc.AddCustomProperty(new CustomProperty("LeaderInstitute", task1.Institute.Name));
                                    tempDoc.AddCustomProperty(new CustomProperty("LeaderName", task1.Leader.Name));
                                    tempDoc.AddCustomProperty(new CustomProperty("TaskYear", task1.Year.ToString()));
                                    tempDoc.AddCustomProperty(new CustomProperty("EditYear", DateTime.Now.Year));
                                    tempDoc.AddCustomProperty(new CustomProperty("EditMonth", DateTime.Now.Month));
                                    tempDoc.AddCustomProperty(new CustomProperty("TaskStartEndYear", task1.Project.StartDate.Year + " - " + task1.Project.EndDate.Year));
                                    tempDoc.AddCustomProperty(new CustomProperty("ProjectType", task1.Project.ProjectType.Name));
                                    tempDoc.AddCustomProperty(new CustomProperty("LeaderMale", task1.Leader.Male));
                                    tempDoc.AddCustomProperty(new CustomProperty("LeaderZhicheng", task1.Leader.Major));
                                    tempDoc.AddCustomProperty(new CustomProperty("LeaderEmail", task1.Leader.Email));
                                    tempDoc.AddCustomProperty(new CustomProperty("LeaderPhone", task1.Leader.Phone));
                                    tempDoc.AddCustomProperty(new CustomProperty("ConnectName", task1.Institute.Contact.Person.Name));
                                    tempDoc.AddCustomProperty(new CustomProperty("ConnectPhone", task1.Institute.Contact.Person.Phone));
                                    tempDoc.AddCustomProperty(new CustomProperty("ConnectEmail", task1.Institute.Contact.Person.Email));
                                    tempDoc.AddCustomProperty(new CustomProperty("Allmoney", task1.Project.TotalBudget.ToString()));
                                    tempDoc.AddCustomProperty(new CustomProperty("Money", task1.CurrentBudget.ToString()));
                                    tempDoc.AddCustomProperty(new CustomProperty("Contect", app1.AbstractContent));

                                    tempDoc.SaveAs(titlePath);
                                }


                                pathList.Clear();


                                //人员文档
                                //获取单位人员
                                var m1 = db.ProjectMembers.Where(c => c.ProjectId == task1.ProjectId &&
                                                               c.Person.InstituteId == app1.InstituteId).OrderBy(c => c.PersonId).ToList();

                                //获取其他单位人员
                                var m2 = db.ProjectMembers.Where(c => c.ProjectId == task1.ProjectId &&
                                                               c.Person.InstituteId != app1.InstituteId).OrderBy(c => c.PersonId).ToList();

                                //获取当前用户信息
                                var userid = HttpContext.Current.User.Identity.Name;
                                var user = db.Users.FirstOrDefault(c => c.UserId == userid);
                                if (user == null)
                                {
                                    return false;
                                }

                                //测试数据
                                //user.PersonId = 2076;

                                //获取项目负责人
                                ProjectMember userMembers = m1.Single(c => c.PersonId == task1.LeaderId);

                                //获取单位其他成员
                                m1 = m1.Where(c => c.PersonId != user.PersonId).OrderBy(c => c.Person.Name).ToList();

                                using (DocX tempDoc = DocX.Load(personPath))
                                {
                                    //填充项目负责人
                                    tempDoc.AddCustomProperty(new CustomProperty("Name1", userMembers.Person.Name));
                                    tempDoc.AddCustomProperty(new CustomProperty("Depart1", userMembers.Person.Institute.Name));
                                    tempDoc.AddCustomProperty(new CustomProperty("IDCard1", userMembers.Person.IDCard));
                                    tempDoc.AddCustomProperty(new CustomProperty("Sex1", userMembers.Person.Male));
                                    tempDoc.AddCustomProperty(new CustomProperty("Major1", userMembers.Person.Major));
                                    tempDoc.AddCustomProperty(new CustomProperty("Duty1", userMembers.Person.Duty));
                                    tempDoc.AddCustomProperty(new CustomProperty("Task1", userMembers.Task));
                                    tempDoc.AddCustomProperty(new CustomProperty("Phone1", userMembers.Person.Phone));

                                    //获取当前人员表格
                                    Table t = tempDoc.Tables[0];
                                    Row userRow;

                                    for (int i = 0; i < m1.Count; i++)
                                    {
                                        userRow = t.InsertRow();
                                        userRow.Cells[1].Paragraphs.First().Append(m1[i].Person.Name).FontSize(10).Font(new FontFamily("SimHei")).Font(new FontFamily("Times New Roman")).Alignment = Alignment.center;
                                        userRow.Cells[2].Paragraphs.First().Append(m1[i].Person.Institute.Name).FontSize(10).Font(new FontFamily("SimHei")).Font(new FontFamily("Times New Roman")).Alignment = Alignment.center;
                                        userRow.Cells[3].Paragraphs.First().Append(m1[i].Person.IDCard).FontSize(10).Font(new FontFamily("SimHei")).Font(new FontFamily("Times New Roman")).Alignment = Alignment.center;
                                        userRow.Cells[4].Paragraphs.First().Append(m1[i].Person.Male).FontSize(10).Font(new FontFamily("SimHei")).Font(new FontFamily("Times New Roman")).Alignment = Alignment.center;
                                        userRow.Cells[5].Paragraphs.First().Append(m1[i].Person.Major).FontSize(10).Font(new FontFamily("SimHei")).Font(new FontFamily("Times New Roman")).Alignment = Alignment.center;
                                        userRow.Cells[6].Paragraphs.First().Append(m1[i].Person.Duty).FontSize(10).Font(new FontFamily("SimHei")).Font(new FontFamily("Times New Roman")).Alignment = Alignment.center;
                                        userRow.Cells[7].Paragraphs.First().Append(m1[i].Task).FontSize(10).Font(new FontFamily("SimHei")).Font(new FontFamily("Times New Roman")).Alignment = Alignment.center;
                                        userRow.Cells[8].Paragraphs.First().Append(m1[i].Person.Phone).FontSize(10).Font(new FontFamily("SimHei")).Font(new FontFamily("Times New Roman")).Alignment = Alignment.center;

                                        //水平居中

                                        for (int j = 1; j < 9; j++)
                                        {
                                            userRow.Cells[j].VerticalAlignment = VerticalAlignment.Center;
                                        }

                                        t.Rows.Add(userRow);
                                    }

                                    //本单位人员至少有3行，总行数最少为6行
                                    while (t.Rows.Count < 6)
                                    {
                                        userRow = t.InsertRow();
                                        t.Rows.Add(userRow);
                                    }

                                    //填充单位信息并合并单元格
                                    t.Rows[3].Cells[0].Paragraphs.First().Append("项目主持单位").Bold().FontSize(12).Alignment = Alignment.center;
                                    t.Rows[3].Cells[0].VerticalAlignment = VerticalAlignment.Center;
                                    t.MergeCellsInColumn(0, 3, t.Rows.Count - 1);

                                    //其他单位人员起始行
                                    int otherRow = t.Rows.Count;

                                    //填充其他单位人员
                                    for (int i = 0; i < m2.Count; i++)
                                    {
                                        userRow = t.InsertRow();
                                        userRow.Cells[1].Paragraphs.First().Append(m2[i].Person.Name).FontSize(10).Font(new FontFamily("SimHei")).Font(new FontFamily("Times New Roman")).Alignment = Alignment.center;
                                        userRow.Cells[2].Paragraphs.First().Append(m2[i].Person.Institute.Name).FontSize(10).Font(new FontFamily("SimHei")).Font(new FontFamily("Times New Roman")).Alignment = Alignment.center;
                                        userRow.Cells[3].Paragraphs.First().Append(m2[i].Person.IDCard).FontSize(10).Font(new FontFamily("SimHei")).Font(new FontFamily("Times New Roman")).Alignment = Alignment.center;
                                        userRow.Cells[4].Paragraphs.First().Append(m2[i].Person.Male).FontSize(10).Font(new FontFamily("SimHei")).Font(new FontFamily("Times New Roman")).Alignment = Alignment.center;
                                        userRow.Cells[5].Paragraphs.First().Append(m2[i].Person.Major).FontSize(10).Font(new FontFamily("SimHei")).Font(new FontFamily("Times New Roman")).Alignment = Alignment.center;
                                        userRow.Cells[6].Paragraphs.First().Append(m2[i].Person.Duty).FontSize(10).Font(new FontFamily("SimHei")).Font(new FontFamily("Times New Roman")).Alignment = Alignment.center;
                                        userRow.Cells[7].Paragraphs.First().Append(m2[i].Task).FontSize(10).Font(new FontFamily("SimHei")).Font(new FontFamily("Times New Roman")).Alignment = Alignment.center;
                                        userRow.Cells[8].Paragraphs.First().Append(m2[i].Person.Phone).FontSize(10).Font(new FontFamily("SimHei")).Font(new FontFamily("Times New Roman")).Alignment = Alignment.center;

                                        //水平居中
                                        for (int j = 1; j < 9; j++)
                                        {
                                            userRow.Cells[j].VerticalAlignment = VerticalAlignment.Center;
                                        }

                                        t.Rows.Add(userRow);
                                    }

                                    //其他单位人员至少有3行
                                    for (int i = 0; i < 3 - m2.Count; i++)
                                    {
                                        userRow = t.InsertRow();
                                        t.Rows.Add(userRow);
                                    }

                                    //填充其他单位信息并合并单元格
                                    t.Rows[otherRow].Cells[0].Paragraphs.First().Append("项目协作单位").Bold().FontSize(12).Alignment = Alignment.center;
                                    t.Rows[otherRow].Cells[0].VerticalAlignment = VerticalAlignment.Center;
                                    t.MergeCellsInColumn(0, otherRow, t.Rows.Count - 1);

                                    //人员文档临时存储位置
                                    temppath = Path.Combine(localPath, AnnualTaskPath + annualTaskId + "\\" + Guid.NewGuid() + ".docx");
                                    tempDoc.SaveAs(temppath);
                                    pathList.Add(temppath);
                                }

                                pathList.Add(part0Path);
                                pathList.Add(part1Path);
                                //pathList.Add(part2Path);
                                //pathList.Add(part3Path);

                                pathList.Add(uploadPath);


                                pathList.Add(part4Path);

                                

                                //单位年度预算
                                using (DocX tempDoc = DocX.Load(yearAnnualPath))
                                {
                                    List<AnnualTaskBudgetItem> abilist = db.AnnualTaskBudgetItems
                                        .Where(c => c.AnnualTaskId == task1.AnnualTaskId).Include(c=>c.Subject)//DateTime.Now.Year
                                        .OrderBy(c => c.SubjectId).ThenBy(c => c.AnnualTaskBudgetItemId).ToList();
                                    if (abilist.Any())
                                    {
                                        //第一条预算
                                        tempDoc.AddCustomProperty(new CustomProperty("Type1", abilist[0].Subject.SubjectName));
                                        tempDoc.AddCustomProperty(new CustomProperty("Money1", abilist[0].Amount.ToString()));
                                        tempDoc.AddCustomProperty(new CustomProperty("Detail1", abilist[0].Reason));

                                        Table t = tempDoc.Tables[0];
                                        Row userRow;
                                        if (abilist.Count>1)
                                        {
                                            //添佳全部预算
                                            for (int i = 1; i < abilist.Count; i++)
                                            {
                                                userRow = t.InsertRow();
                                                userRow.Cells[0].Paragraphs.First().Append(abilist[i].Subject.SubjectName).Bold().FontSize(14).Font(new FontFamily("SimHei")).Font(new FontFamily("Times New Roman")).Alignment = Alignment.center;
                                                userRow.Cells[1].Paragraphs.First().Append(abilist[i].Amount.ToString()).Bold().FontSize(14).Font(new FontFamily("SimHei")).Font(new FontFamily("Times New Roman")).Alignment = Alignment.center;
                                                userRow.Cells[2].Paragraphs.First().Append(abilist[i].Reason).Bold().FontSize(14).Font(new FontFamily("SimHei")).Font(new FontFamily("Times New Roman")).Alignment = Alignment.center;

                                                t.Rows.Add(userRow);
                                            }
                                        }
                                        //预算至少10行
                                        while (t.Rows.Count < 10)
                                        {
                                            userRow = t.InsertRow();
                                            userRow.Cells[0].Paragraphs.First().Append(" ").Bold().FontSize(14).Font(new FontFamily("SimHei")).Font(new FontFamily("Times New Roman")).Alignment = Alignment.center;
                                            userRow.Cells[1].Paragraphs.First().Append(" ").Bold().FontSize(14).Font(new FontFamily("SimHei")).Font(new FontFamily("Times New Roman")).Alignment = Alignment.center;
                                            userRow.Cells[2].Paragraphs.First().Append(" ").Bold().FontSize(14).Font(new FontFamily("SimHei")).Font(new FontFamily("Times New Roman")).Alignment = Alignment.center;
                                            userRow.MinHeight = 50;
                                            t.Rows.Add(userRow);
                                        }

                                        //汇总
                                        userRow = t.InsertRow();
                                        userRow.Cells[0].Paragraphs.First().Append("合计").Bold().FontSize(14).Font(new FontFamily("SimHei")).Font(new FontFamily("Times New Roman")).Alignment = Alignment.center;
                                        userRow.Cells[1].Paragraphs.First().Append(abilist.Sum(c => c.Amount).ToString()).Bold().FontSize(14).Font(new FontFamily("SimHei")).Font(new FontFamily("Times New Roman")).Alignment = Alignment.center;
                                        userRow.Cells[2].Paragraphs.First().Append("").Bold().FontSize(14).Font(new FontFamily("SimHei")).Font(new FontFamily("Times New Roman")).Alignment = Alignment.center;
                                        
                                        t.Rows.Add(userRow);
                                    }

                                    temppath = Path.Combine(localPath, AnnualTaskPath + annualTaskId + "\\" + Guid.NewGuid() + ".docx");
                                    pathList.Add(temppath);
                                    tempDoc.SaveAs(temppath);
                                }

                                //各单位预算
                                using (DocX tempDoc = DocX.Load(departAnnualPath))
                                {
                                    List<AnnualTaskInstBudget> atiblist = db.AnnualTaskInstBudgets
                                        .Where(c => c.AnnualTaskId == task1.AnnualTaskId).Include(c=>c.Institute).ToList();

                                    var leaderInst = atiblist.First(c => c.InstituteId == task1.InstituteId);

                                    //本单位预算
                                    tempDoc.AddCustomProperty(new CustomProperty("Depart1", leaderInst.Institute.Name));
                                    tempDoc.AddCustomProperty(new CustomProperty("moneyall1", task1.Project.TotalBudget.ToString()));
                                    tempDoc.AddCustomProperty(new CustomProperty("money1", leaderInst.Amount.ToString()));
                                    tempDoc.AddCustomProperty(new CustomProperty("MoneyYear1", task1.Year.ToString()));

                                    Table t = tempDoc.Tables[0];
                                    Row userRow;

                                    var otherInstlist = atiblist.Where(c => c.InstituteId != task1.InstituteId).OrderBy(c=>c.InstituteId)
                                        .ToList();
                                    foreach (var temp in otherInstlist)
                                    {
                                        userRow = t.InsertRow();
                                        userRow.Cells[0].Paragraphs.First().Append(temp.Institute.Name).Bold().FontSize(14).Font(new FontFamily("SimHei")).Font(new FontFamily("Times New Roman")).Alignment = Alignment.center;
                                        userRow.Cells[1].Paragraphs.First().Append(" ").Bold().FontSize(14).Font(new FontFamily("SimHei")).Font(new FontFamily("Times New Roman")).Alignment = Alignment.center;
                                        userRow.Cells[2].Paragraphs.First().Append(temp.Amount.ToString()).Bold().FontSize(14).Font(new FontFamily("SimHei")).Font(new FontFamily("Times New Roman")).Alignment = Alignment.center;

                                        t.Rows.Add(userRow);
                                    }
                                    //预算至少10行
                                    while (t.Rows.Count < 10)
                                    {
                                        userRow = t.InsertRow();
                                        userRow.Cells[0].Paragraphs.First().Append(" ").FontSize(14).Font(new FontFamily("SimHei")).Font(new FontFamily("Times New Roman")).Alignment = Alignment.center;
                                        userRow.Cells[1].Paragraphs.First().Append(" ").FontSize(14).Font(new FontFamily("SimHei")).Font(new FontFamily("Times New Roman")).Alignment = Alignment.center;
                                        userRow.Cells[2].Paragraphs.First().Append(" ").FontSize(14).Font(new FontFamily("SimHei")).Font(new FontFamily("Times New Roman")).Alignment = Alignment.center;
                                        userRow.MinHeight = 50;
                                        t.Rows.Add(userRow);
                                    }
                                    temppath = Path.Combine(localPath, AnnualTaskPath + annualTaskId + "\\" + Guid.NewGuid() + ".docx");
                                    pathList.Add(temppath);
                                    tempDoc.SaveAs(temppath);
                                }

                                pathList.Add(endPath);

                                //合并文档
                                Document srcDoc = new Document(titlePath);
                                Document tempDocs;
                                foreach (var temp in pathList)
                                {
                                    tempDocs = new Document(temp);
                                    srcDoc.AppendDocument(tempDocs, ImportFormatMode.KeepSourceFormatting);
                                }


                                srcDoc.Save(Path.Combine(localPath, AnnualTaskPath + annualTaskId + "\\" + pdfName + ".docx"), Aspose.Words.SaveFormat.Docx);
                                srcDoc.Save(Path.Combine(localPath, AnnualTaskPath + annualTaskId + "\\" + pdfName + ".pdf"), Aspose.Words.SaveFormat.Pdf);
                            }
                            else
                            {
                                return false;
                            }
                            
                        }
                        else
                        {
                            return false;
                        }
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                    return false;
                }
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// 转换任务文档为PDF
        /// </summary>
        /// <param name="annualTaskId">任务ID</param>
        /// <param name="uploadDocName">上传的文件</param>
        /// <param name="pdfName">转换的文件名</param>
        /// <param name="startYear"></param>
        /// <returns></returns>
        public static bool ConvertTaskPdf(int annualTaskId, string uploadDocName, string pdfName)
        {
            string localPath = HttpRuntime.AppDomainAppPath;
            string uploadPath = Path.Combine(localPath, AnnualTaskPath + annualTaskId + "\\" + uploadDocName);//上传文档
            if (File.Exists(uploadPath))
            {
                try
                {
                    Document srcDoc = new Document(uploadPath);
                    srcDoc.Save(Path.Combine(localPath, AnnualTaskPath + annualTaskId + "\\" + pdfName + ".docx"), Aspose.Words.SaveFormat.Docx);
                    srcDoc.Save(Path.Combine(localPath, AnnualTaskPath + annualTaskId + "\\" + pdfName + ".pdf"), Aspose.Words.SaveFormat.Pdf);
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                    return false; ;
                }
            }
            else
            {
                return false;
            }

            return true;
        }

        /// <summary>
        /// 转换任务书文档为PDF
        /// </summary>
        /// <param name="projectId"></param>
        /// <param name="uploadDocName"></param>
        /// <param name="pdfName"></param>
        /// <returns></returns>
        public static bool ConvertProjectPdf(string projectId, string uploadDocName, string pdfName)
        {
            string localPath = HttpRuntime.AppDomainAppPath;
            string uploadPath = Path.Combine(localPath, projectPath + projectId + "\\" + uploadDocName);//上传文档
            if (File.Exists(uploadPath))
            {
                try
                {
                    Document srcDoc = new Document(uploadPath);
                    srcDoc.Save(Path.Combine(localPath, projectPath + projectId + "\\" + pdfName + ".docx"), Aspose.Words.SaveFormat.Docx);
                    srcDoc.Save(Path.Combine(localPath, projectPath + projectId + "\\" + pdfName + ".pdf"), Aspose.Words.SaveFormat.Pdf);
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                    return false; ;
                }
            }
            else
            {
                return false;
            }

            return true;
        }


        /// <summary>
        /// 判断上传文档是否标准
        /// </summary>
        /// <param name="applicationId"></param>
        /// <param name="uploadDocName"></param>
        /// <returns></returns>
        public static bool Check(string applicationId, string relativeURL )
        {
            string localPath = HttpRuntime.AppDomainAppPath;
            //string uploadPath = Path.Combine(localPath, path + applicationId + "\\" + uploadDocName);//上传文档
            string uploadPath = Path.Combine(localPath, relativeURL.Substring(1).Replace("/", @"\"));//上传文档
            string savePath = string.Empty;

            
            if (File.Exists(uploadPath))
            {
                try
                {
                    Document document = new Document(uploadPath);
                    //int nn=document.Sections.Count;
                    var tables = document.GetChildNodes(NodeType.Table, true);
                    if (tables.Count >= 6 && document.Sections.Count==6)
                    {
                        //上传文档应包含6个表格
                        List<string> templist = new List<string>();

                        for (int i = 0; i < document.Sections.Count; i++)
                        {

                            string temppath = Path.Combine(localPath, path + applicationId + "\\" + Guid.NewGuid()+".docx");
                            document.Save(temppath);
                            templist.Add(temppath);

                            savePath = Path.Combine(localPath, path + applicationId + "\\part" + i);
                            saveNowDocx(temppath, i, savePath);
                        }

                        foreach (var temp in templist)
                        {
                            if (File.Exists(temp))
                            {
                                File.Delete(temp);
                            }
                        }
                    }
                    else
                    {
                        return false;
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                    return false;
                }
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// 复制文档内容到新文档
        /// </summary>
        /// <param name="srcDoc"></param>
        /// <param name="node"></param>
        /// <returns></returns>
        private static Document GenerateDocument(Document srcDoc, Node node)
        {
            // Create a blank document.
            Document dstDoc = new Document();
            // Remove the first paragraph from the empty document.
            dstDoc.FirstSection.Body.RemoveAllChildren();

            // Import each node from the list into the new document. Keep the original formatting of the node.
            NodeImporter importer = new NodeImporter(srcDoc, dstDoc, ImportFormatMode.KeepSourceFormatting);

            Node importNode = importer.ImportNode(node, true);
            dstDoc.FirstSection.Body.AppendChild(importNode);

            // Return the generated document.
            return dstDoc;
        }

        private static Document GenerateDocumentSection1(Document srcDoc, int SectionIndex)
        {
            // Create a blank document.
            Document dstDoc = new Document();
            // Remove the first paragraph from the empty document.
            dstDoc.FirstSection.Body.RemoveAllChildren();

            Aspose.Words.Section srcSection = srcDoc.Sections[SectionIndex];
            int nodeCount = srcSection.Body.ChildNodes.Count;
            // Import each node from the list into the new document. Keep the original formatting of the node.

            NodeImporter importer = new NodeImporter(srcDoc, dstDoc, ImportFormatMode.KeepSourceFormatting);
            for (int nodeIndex = 0; nodeIndex < nodeCount; nodeIndex++)
            {
                Node srcNode = srcSection.Body.ChildNodes[nodeIndex];
                Node newNode = importer.ImportNode(srcNode, true);
                dstDoc.FirstSection.Body.AppendChild(newNode);
            }

            // Return the generated document.
            return dstDoc;
        }

        private static void saveNowDocx(string soursepath, int sectionIndex ,string path)
        {
            Document srcDoc = new Document(soursepath);
            string docpath = path + ".docx";
            Aspose.Words.Section newSection = srcDoc.Sections[sectionIndex].Clone();
            
            srcDoc.Sections.Clear();
            srcDoc.Sections.Add(newSection);
            if (File.Exists(docpath))
            {
                File.Delete(docpath);
            }
            srcDoc.Save(docpath, SaveFormat.Docx);

            docpath = path + ".pdf";
            if (File.Exists(docpath))
            {
                File.Delete(docpath);
            }
            srcDoc.Save(docpath, SaveFormat.Pdf);
        }
    }
}
