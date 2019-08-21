using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Configuration;
using Quartz;

namespace ASPODES.WebAPI.Jobs
{
    public class TimedTask:IJob
    {
        public void Execute(IJobExecutionContext context)
        {
            LogHelper.Info("开始定时任务");
            try
            {
                //Configuration config = System.Web.Configuration.WebConfigurationManager.OpenWebConfiguration(Path.Combine(HttpRuntime.AppDomainAppPath,"web.config"));
                Configuration config = WebConfigurationManager.OpenWebConfiguration("~/");
                AppSettingsSection appSection = (AppSettingsSection)config.GetSection("appSettings");
                appSection.Settings["JobKey"].Value = context.JobDetail.Key.Name;
                if (context.FireTimeUtc.HasValue)
                {
                    appSection.Settings["JobTime"].Value = context.FireTimeUtc.Value.LocalDateTime.ToString("yyyy-MM-dd HH:mm:ss.fff");
                }
                if (context.ScheduledFireTimeUtc.HasValue)
                {
                    appSection.Settings["JobScheduledTime"].Value = context.ScheduledFireTimeUtc.Value.LocalDateTime.ToString("yyyy-MM-dd HH:mm:ss.fff");
                }
                if (context.NextFireTimeUtc.HasValue)
                {
                    appSection.Settings["JobNextTime"].Value = context.NextFireTimeUtc.Value.LocalDateTime.ToString("yyyy-MM-dd HH:mm:ss.fff");
                }
                //appSection.Settings["JobCount"].Value = context.RefireCount.ToString();
                config.Save();
            }
            catch (Exception e)
            {
                LogHelper.Error(e.Message);
                LogHelper.Error(e.ToString());
            }
            
            LogHelper.Info("结束定时任务");
        }
    }
}
