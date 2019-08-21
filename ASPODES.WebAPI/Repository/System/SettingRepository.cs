using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Configuration;
using ASPODES.Database;
using ASPODES.DTO.System;
using ASPODES.Model;
using ASPODES.WebAPI.Common;
using AutoMapper;
using System.IO;

namespace ASPODES.WebAPI.Repository
{
    public class SettingRepository
    {
        /// <summary>
        /// 获取申请书提交日期参数
        /// </summary>
        /// <returns></returns>
        public GetApplicationSettingDTO GetApplicationSetting()
        {
            //GetApplicationSettingDTO applicationSetting=Mapper.Map<GetApplicationSettingDTO>(SystemConfig.)
            GetApplicationSettingDTO applicationSetting = new GetApplicationSettingDTO();
            applicationSetting.ApplicationSubmitBeginTime = SystemConfig.ApplicationSubmitBeginTime.Date;
            applicationSetting.ApplicationSubmitDeadline = SystemConfig.ApplicationSubmitDeadline;
            applicationSetting.ApplicationVerifyDeadline = SystemConfig.ApplicationVerifyDeadline;
            applicationSetting.ApplicationExpertDeadline = SystemConfig.ApplicationExpertDeadline;
            applicationSetting.ApplicationStartYear = SystemConfig.ApplicationStartYear;

            return applicationSetting;
        }

        /// <summary>
        /// 修改申请书提交日期参数
        /// </summary>
        /// <returns></returns>
        public SysSettingHistory UpdateApplicationSetting(GetApplicationSettingDTO appSetting)
        {
            //验证提交日期合法
            if ((appSetting.ApplicationSubmitBeginTime < appSetting.ApplicationSubmitDeadline)
                && (appSetting.ApplicationSubmitDeadline <= appSetting.ApplicationVerifyDeadline)
                && (appSetting.ApplicationVerifyDeadline < appSetting.ApplicationExpertDeadline)
                && (DateTime.Now.Year <= appSetting.ApplicationStartYear && appSetting.ApplicationStartYear <= DateTime.Now.AddYears(3).Year)
                )
            {
                Configuration config = WebConfigurationManager.OpenWebConfiguration("~/");
                AppSettingsSection appSection = (AppSettingsSection)config.GetSection("appSettings");

                appSetting.ApplicationSubmitBeginTime =
                    Convert.ToDateTime(appSetting.ApplicationSubmitBeginTime.ToString("yyyy-MM-dd") + " 00:00:00");
                appSection.Settings["ApplicationSubmitBeginTime"].Value = appSetting.ApplicationSubmitBeginTime.ToString("yyyy-MM-dd HH:mm:ss");

                appSetting.ApplicationSubmitDeadline =
                    Convert.ToDateTime(appSetting.ApplicationSubmitDeadline.ToString("yyyy-MM-dd") + " 23:59:59");
                appSection.Settings["ApplicationSubmitDeadline"].Value = appSetting.ApplicationSubmitDeadline.ToString("yyyy-MM-dd HH:mm:ss");

                appSetting.ApplicationVerifyDeadline =
                    Convert.ToDateTime(appSetting.ApplicationVerifyDeadline.ToString("yyyy-MM-dd") + " 23:59:59");
                appSection.Settings["ApplicationVerifyDeadline"].Value = appSetting.ApplicationVerifyDeadline.ToString("yyyy-MM-dd HH:mm:ss");

                appSetting.ApplicationExpertDeadline =
                    Convert.ToDateTime(appSetting.ApplicationExpertDeadline.ToString("yyyy-MM-dd") + " 23:59:59");
                appSection.Settings["ApplicationExpertDeadline"].Value = appSetting.ApplicationExpertDeadline.ToString("yyyy-MM-dd HH:mm:ss");

                appSection.Settings["ApplicationStartYear"].Value = appSetting.ApplicationStartYear.ToString();

                config.Save();

                using (var db = new AspodesDB())
                {
                    SysSettingHistory setings = new SysSettingHistory();
                    setings.ApplicationSubmitBeginTime = SystemConfig.ApplicationSubmitBeginTime;
                    setings.ApplicationSubmitDeadline = SystemConfig.ApplicationSubmitDeadline;
                    setings.ApplicationVerifyDeadline = SystemConfig.ApplicationVerifyDeadline;
                    setings.ApplicationExpertDeadline = SystemConfig.ApplicationExpertDeadline;
                    setings.ApplicationStartYear = SystemConfig.ApplicationStartYear;

                    setings.UpdateTime = DateTime.Now;
                    setings.UpdateId = HttpContext.Current.User.Identity.Name;
                    setings.UpdateIp = HttpContext.Current.Request.UserHostAddress;

                    var aa = db.SysSettingHistorys.Add(setings);

                    db.SaveChanges();
                    return aa;
                }

            }
            else
            {
                throw new ModelValidException("日期参数不符合规定");

            }
        }

        /// <summary>
        /// 获取驳回、未受理原因
        /// </summary>
        /// <param name="type">1为未受理、0为驳回</param>
        /// <returns></returns>
        public List<TemplateReasonDTO> GetTemplateReason(int type)
        {
            List<TemplateReasonDTO> templateReasonList = new List<TemplateReasonDTO>();
            using (var db = new AspodesDB())
            {
                templateReasonList = db.TemplateReasons.Where(c => c.Type == type)
                    .OrderBy(c => c.Content)
                    .Select(Mapper.Map<TemplateReasonDTO>)
                    .ToList();
                return templateReasonList;
            }
        }

        /// <summary>
        /// 添加或修改驳回、未受理原因
        /// </summary>
        /// <param name="type">1为未受理、0为驳回</param>
        /// <param name="templateReason">原因</param>
        /// <returns></returns>
        public GetTemplateReasonDTO AddOrUpdateTemplateReason(int type, TemplateReasonDTO templateReason)
        {
            using (var db = new AspodesDB())
            {
                TemplateReason tempReason;
                if (templateReason.ReasonId == -1)
                {
                    //新增
                    tempReason = new TemplateReason();
                }
                else
                {
                    //修改
                    tempReason = db.TemplateReasons
                        .Where(c => c.Type == type && c.ReasonId == templateReason.ReasonId)
                        .FirstOrDefault();
                }

                if (null == tempReason)
                {
                    throw new ModelValidException("参数错误，找不到对应的ID");

                }

                tempReason.Type = type;
                tempReason.Content = templateReason.ReasonContent;
                tempReason.EditTime = DateTime.Now;
                tempReason.EditorId = HttpContext.Current.User.Identity.Name;

                if (templateReason.ReasonId == -1)
                {
                    db.TemplateReasons.Add(tempReason);
                }

                db.SaveChanges();
                return Mapper.Map<GetTemplateReasonDTO>(tempReason);
            }
        }

        /// <summary>
        /// 删除驳回、未受理原因
        /// </summary>
        /// <param name="type">1为未受理、0为驳回</param>
        /// <param name="templateReason">原因</param>
        /// <returns></returns>
        public void DeleteTemplateReason(int type, TemplateReasonDTO templateReason)
        {
            using (var db = new AspodesDB())
            {
                var tempReason = db.TemplateReasons
                    .Where(c => c.Type == type && c.ReasonId == templateReason.ReasonId)
                    .FirstOrDefault();

                if (null == tempReason)
                {
                    throw new ModelValidException("参数错误，找不到对应的ID");
                }

                db.TemplateReasons.Remove(tempReason);

                db.SaveChanges();
            }

        }

        /// <summary>
        /// 开启第year年度的申请
        /// </summary>
        /// <param name="year">申请年度</param>
        public void StartApplication( int year )
        {
            if (year <= 0 || SystemConfig.ApplicationStartYear >= year)
                throw new OtherException("请年度不合法或者已经使用");

            using( var db  = new AspodesDB() )
            {
                using( var transaction = db.Database.BeginTransaction() )
                {
                    try
                    {
                        //添加修改记录
                        SysSettingHistory setings = new SysSettingHistory();
                        setings.ApplicationStartYear = SystemConfig.ApplicationStartYear;
                        setings.UpdateTime = DateTime.Now;
                        setings.UpdateId = HttpContext.Current.User.Identity.Name;
                        setings.UpdateIp = HttpContext.Current.Request.UserHostAddress;
                        db.SysSettingHistorys.Add(setings);

                        //重置一些字段
                        string resetProjectTypeLimit = "update ProjectTypes set Limit=0;";
                        string resetExpertReviewAmount = "update [User] set ReviewAmount=0;";
                        string setRevewAssignmentOverdue = "update ReviewAssignment set Overdue=1 where Overdue = 0;";

                        db.Database.ExecuteSqlCommand(resetProjectTypeLimit);
                        db.Database.ExecuteSqlCommand(resetExpertReviewAmount);
                        db.Database.ExecuteSqlCommand(setRevewAssignmentOverdue);

                        //修改配置文件
                        Configuration config = WebConfigurationManager.OpenWebConfiguration("~/");
                        AppSettingsSection appSection = (AppSettingsSection)config.GetSection("appSettings");
                        appSection.Settings["ApplicationStartYear"].Value = year.ToString();
                        config.Save();

                        //删除临时文件
                        //删除压缩的临时文件
                        DirectoryInfo subdir = new DirectoryInfo(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, SystemConfig.DeleteZipFileAddress));
                        if(subdir.Exists)
                        {
                            subdir.Delete(true);
                            subdir.Create();
                        }
                        subdir = new DirectoryInfo(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, SystemConfig.ExportExcel));
                        if (subdir.Exists)
                        {
                            subdir.Delete(true);
                            subdir.Create();
                        }

                        transaction.Commit();
                    }
                    catch( Exception e )
                    {
                        transaction.Rollback();
                        throw e;
                    }
                }
            }
        }
    }
}
